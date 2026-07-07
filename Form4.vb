Imports MySql.Data.MySqlClient

Public Class Form4
    Dim connString As String = "server=localhost;database=Scoopify_Creamery;user=root;password=Hannah_lei07;"
    Dim total As Decimal = 0
    Private OrderList As New List(Of OrderItem)

    Public Class OrderItem
        Public ProductID As Integer
        Public ItemName As String
        Public Quantity As Integer
        Public Price As Decimal
    End Class

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadProducts()
        NumericUpDown1.Minimum = 1
        NumericUpDown1.Maximum = 20
        NumericUpDown1.Value = 1
        Label1.Text = "Total: ₱0"
    End Sub

    Private Sub LoadProducts()
        Using conn As New MySqlConnection(connString)
            conn.Open()
            LoadCategory(CheckedListBox1, conn, "Flavor")
            LoadCategory(CheckedListBox2, conn, "Container")
            LoadCategory(CheckedListBox3, conn, "Topping")
            LoadCategory(CheckedListBox4, conn, "Syrup")
        End Using
    End Sub

    Private Sub AddItems(box As CheckedListBox, qty As Integer)
        For Each item In box.CheckedItems
            Dim p As New OrderItem
            p.ProductID = GetProductID(item.ToString())
            p.ItemName = item.ToString()
            p.Quantity = qty
            p.Price = GetPrice(item.ToString())
            OrderList.Add(p)
        Next
    End Sub

    Private Sub SaveItems(conn As MySqlConnection, transactionID As Integer)
        For Each item In OrderList
            Dim sql As String = "INSERT INTO transaction_details (transaction_id, product_id, quantity, unit_price, subtotal)
        VALUES (@tid,@pid,@qty,@price,@subtotal)"

            Dim cmd As New MySqlCommand(sql, conn)

            cmd.Parameters.AddWithValue("@tid", transactionID)
            cmd.Parameters.AddWithValue("@pid", item.ProductID)
            cmd.Parameters.AddWithValue("@qty", item.Quantity)
            cmd.Parameters.AddWithValue("@price", item.Price)
            cmd.Parameters.AddWithValue("@subtotal",
                                    item.Price * item.Quantity)
            cmd.ExecuteNonQuery()
        Next
    End Sub

    Private Sub LoadCategory(box As CheckedListBox, conn As MySqlConnection, category As String)
        box.Items.Clear()
        Dim sql As String = "SELECT item_name
     FROM products
     WHERE category=@category
     AND status='Available'"
        Dim cmd As New MySqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("@category", category)
        Dim reader = cmd.ExecuteReader()
        While reader.Read()
            box.Items.Add(reader("item_name").ToString())
        End While
        reader.Close()
    End Sub

    Private Sub FillListFromDB(box As CheckedListBox, conn As MySqlConnection, query As String)
        Dim cmd As New MySqlCommand(query, conn)
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            box.Items.Add(reader("item_name").ToString())
        End While

        reader.Close()
    End Sub

    Private Function GetPrice(itemName As String) As Decimal
        Dim price As Decimal = 0
        Using conn As New MySqlConnection(connString)
            conn.Open()
            Dim sql As String =
        "SELECT price
         FROM products
         WHERE item_name=@name"
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@name", itemName)
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                price = Convert.ToDecimal(result)
            End If
        End Using
        Return price
    End Function

    Private Function GetProductID(itemName As String) As Integer
        Dim id As Integer = 0
        Using conn As New MySqlConnection(connString)
            conn.Open()
            Dim sql As String = "SELECT product_id FROM products WHERE item_name=@name"
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@name", itemName)
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                id = Convert.ToInt32(result)
            End If
        End Using
        Return id
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim price As Decimal = 0
        Dim description As String = ""
        Dim qty As Integer = NumericUpDown1.Value

        For Each item In CheckedListBox1.CheckedItems
            price += GetPrice(item.ToString())
            description &= item.ToString() & ", "
        Next

        For Each item In CheckedListBox2.CheckedItems
            price += GetPrice(item.ToString())
            description &= item.ToString() & ", "
        Next

        For Each item In CheckedListBox3.CheckedItems
            price += GetPrice(item.ToString())
            description &= item.ToString() & ", "
        Next

        For Each item In CheckedListBox4.CheckedItems
            price += GetPrice(item.ToString())
            description &= item.ToString() & ", "
        Next

        If description = "" Then
            MessageBox.Show("Please select an item.")
            Exit Sub
        End If

        description = description.TrimEnd(","c, " "c)
        ListBox2.Items.Add(description & " | Qty: " & qty & " | Total: ₱" & price * qty)
        total += price * qty
        Label1.Text = "Total: ₱" & total

        AddItems(CheckedListBox1, qty)
        AddItems(CheckedListBox2, qty)
        AddItems(CheckedListBox3, qty)
        AddItems(CheckedListBox4, qty)
    End Sub

    Private Sub ClearSelection()
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, False)
        Next
        For i As Integer = 0 To CheckedListBox2.Items.Count - 1
            CheckedListBox2.SetItemChecked(i, False)
        Next
        For i As Integer = 0 To CheckedListBox3.Items.Count - 1
            CheckedListBox3.SetItemChecked(i, False)
        Next
        For i As Integer = 0 To CheckedListBox4.Items.Count - 1
            CheckedListBox4.SetItemChecked(i, False)
        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox2.Items.Count = 0 Then
            MessageBox.Show("Your order is empty!")
            Exit Sub
        End If

        Using conn As New MySqlConnection(connString)
            conn.Open()

            Dim cmdTrans As New MySqlCommand(
            "INSERT INTO transactions (customer_id, employee_id, total_amount, payment_method)
             VALUES (1, 3, @total, 'Cash')", conn)
            cmdTrans.Parameters.AddWithValue("@total", total)
            cmdTrans.ExecuteNonQuery()
            Dim transactionID As Integer = cmdTrans.LastInsertedId
            SaveItems(conn, transactionID)
        End Using
        MessageBox.Show(
        "Flavor: " & CheckedListBox1.CheckedItems.Count & vbCrLf &
        "Container: " & CheckedListBox2.CheckedItems.Count & vbCrLf &
        "Topping: " & CheckedListBox3.CheckedItems.Count & vbCrLf &
        "Syrup: " & CheckedListBox4.CheckedItems.Count)
        Dim message = "Order confirmed!" &
                        vbCrLf
        message &= "" & vbCrLf & "Total: ₱ " & total
        MessageBox.Show(message)
        Button3_Click(sender, e)

        ClearOrder()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ClearOrder()
    End Sub

    Private Sub ClearOrder()
        ListBox2.Items.Clear()
        total = 0
        Label1.Text = "Total: ₱0"
        NumericUpDown1.Value = 1
        OrderList.Clear()
        ClearSelection()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        CheckedListBox1.Visible = True
        CheckedListBox2.Visible = False
        CheckedListBox3.Visible = False
        CheckedListBox4.Visible = False

        For Each box As CheckedListBox In New CheckedListBox() {
    CheckedListBox1, CheckedListBox2, CheckedListBox3, CheckedListBox4}

            For i As Integer = 0 To box.Items.Count - 1
                box.SetItemChecked(i, False)
            Next
        Next
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        CheckedListBox1.Visible = False
        CheckedListBox2.Visible = True
        CheckedListBox3.Visible = False
        CheckedListBox4.Visible = False

        For Each box As CheckedListBox In New CheckedListBox() {
    CheckedListBox1, CheckedListBox2, CheckedListBox3, CheckedListBox4}

            For i As Integer = 0 To box.Items.Count - 1
                box.SetItemChecked(i, False)
            Next
        Next
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        CheckedListBox1.Visible = False
        CheckedListBox2.Visible = False
        CheckedListBox3.Visible = True
        CheckedListBox4.Visible = False

        For Each box As CheckedListBox In New CheckedListBox() {
    CheckedListBox1, CheckedListBox2, CheckedListBox3, CheckedListBox4}

            For i As Integer = 0 To box.Items.Count - 1
                box.SetItemChecked(i, False)
            Next
        Next
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        CheckedListBox1.Visible = False
        CheckedListBox2.Visible = False
        CheckedListBox3.Visible = False
        CheckedListBox4.Visible = True

        For Each box As CheckedListBox In New CheckedListBox() {
    CheckedListBox1, CheckedListBox2, CheckedListBox3, CheckedListBox4}

            For i As Integer = 0 To box.Items.Count - 1
                box.SetItemChecked(i, False)
            Next
        Next
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form3.Show()
        Me.Hide()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form9.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form5.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

    End Sub
End Class