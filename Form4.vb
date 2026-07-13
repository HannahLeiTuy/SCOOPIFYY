Imports MySql.Data.MySqlClient

Public Class Form4

    Dim connString As String =
    "server=localhost;database=Scoopify_Creamery;user=root;password=BugfixMaster#22;"

    Dim total As Decimal = 0
    Private OrderList As New List(Of OrderItem)

    Public Class OrderItem
        Public ProductID As Integer
        Public ItemName As String
        Public Quantity As Integer
        Public Price As Decimal
    End Class

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen

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

    Private Sub LoadCategory(box As CheckedListBox,
                             conn As MySqlConnection,
                             category As String)

        box.Items.Clear()

        Dim sql As String =
            "SELECT item_name
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

            Dim sql As String =
                "SELECT product_id
                 FROM products
                 WHERE item_name=@name"

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

        Dim qty As Integer = NumericUpDown1.Value

        AddCheckedItemsToCart(CheckedListBox1, qty)
        AddCheckedItemsToCart(CheckedListBox2, qty)
        AddCheckedItemsToCart(CheckedListBox3, qty)
        AddCheckedItemsToCart(CheckedListBox4, qty)

        Label1.Text = "Total: ₱" & total.ToString("0.00")

    End Sub

    Private Sub AddCheckedItemsToCart(box As CheckedListBox, qty As Integer)

        For Each obj In box.CheckedItems

            Dim name As String = obj.ToString()
            Dim price As Decimal = GetPrice(name)
            Dim pid As Integer = GetProductID(name)

            CartManager.AddItem(pid, name, price, qty)

            total += price * qty

            ListBox2.Items.Add(
                name &
                " x" &
                qty &
                " - ₱" &
                (price * qty).ToString("0.00"))

        Next

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

    Private Sub ClearOrder()

        ListBox2.Items.Clear()

        total = 0

        Label1.Text = "Total: ₱0"

        NumericUpDown1.Value = 1

        OrderList.Clear()

        ClearSelection()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If ListBox2.Items.Count = 0 Then
            MessageBox.Show("Your order is empty!")
            Exit Sub
        End If

        MessageBox.Show("🛒 Added to cart!")

        Form12.RefreshCart()
        Form12.Show()

        Me.Hide()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ClearOrder()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ShowCategory(CheckedListBox1)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ShowCategory(CheckedListBox2)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        ShowCategory(CheckedListBox3)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        ShowCategory(CheckedListBox4)
    End Sub

    Private Sub ShowCategory(target As CheckedListBox)

        CheckedListBox1.Visible = False
        CheckedListBox2.Visible = False
        CheckedListBox3.Visible = False
        CheckedListBox4.Visible = False

        target.Visible = True

        For Each box As CheckedListBox In {
            CheckedListBox1,
            CheckedListBox2,
            CheckedListBox3,
            CheckedListBox4
        }

            For i As Integer = 0 To box.Items.Count - 1
                box.SetItemChecked(i, False)
            Next

        Next

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form6.Show()
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

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Form12.RefreshCart()
        Form12.Show()
        Me.Hide()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form10.Show()
        Me.Hide()
    End Sub

End Class