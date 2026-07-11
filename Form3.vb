Imports MySql.Data.MySqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Diagnostics
Imports Microsoft.VisualBasic

Public Class Form3
    Dim id As Integer

    Sub RefreshData()
        LoadInventory()
        Dashboard()
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshData()
        CheckLowStock()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If DataGridView1.Columns.Contains("inventory_id") Then
            id = CInt(DataGridView1.CurrentRow.Cells("inventory_id").Value)
            TextBox1.Text = DataGridView1.CurrentRow.Cells("item_name").Value.ToString()
            ComboBox1.Text = DataGridView1.CurrentRow.Cells("category").Value.ToString()
            TextBox2.Text = DataGridView1.CurrentRow.Cells("current_stock").Value.ToString()
            TextBox3.Text = DataGridView1.CurrentRow.Cells("reorder_level").Value.ToString()
            ShowSuggestion()
        End If
    End Sub

    Sub LoadGrid(query As String, Optional paramName As String = Nothing, Optional paramValue As Object = Nothing)
        Try
            conn.Open()
            Dim da As New MySqlDataAdapter(query, conn)
            If paramName IsNot Nothing Then
                da.SelectCommand.Parameters.AddWithValue(paramName, paramValue)
            End If
            Dim dt As New DataTable
            da.Fill(dt)
            DataGridView1.DataSource = dt
            HighlightStocks()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub LoadInventory()
        LoadGrid("
            SELECT inventory_id, item_name, category, current_stock, reorder_level, status
            FROM inventory
            ORDER BY inventory_id")

        If DataGridView1.Columns.Contains("inventory_id") Then
            DataGridView1.Columns(0).Visible = False
            DataGridView1.Columns(1).HeaderText = "Item Name"
            DataGridView1.Columns(2).HeaderText = "Category"
            DataGridView1.Columns(3).HeaderText = "Current Stock"
            DataGridView1.Columns(4).HeaderText = "Reorder Level"
            DataGridView1.Columns(5).HeaderText = "Status"
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False
        End If
    End Sub

    Sub SearchInventory()
        LoadGrid("
            SELECT inventory_id, item_name, category, current_stock, reorder_level, status
            FROM inventory
            WHERE item_name LIKE @search", "@search", "%" & TextBox4.Text & "%")
    End Sub

    Sub Dashboard()
        Try
            conn.Open()
            Dim cmd1 As New MySqlCommand("SELECT COUNT(*) FROM inventory", conn)
            Label2.Text = cmd1.ExecuteScalar().ToString()
            Dim cmd2 As New MySqlCommand("SELECT COUNT(*) FROM inventory WHERE status='In Stock'", conn)
            Label3.Text = cmd2.ExecuteScalar().ToString()
            Dim cmd3 As New MySqlCommand("SELECT COUNT(*) FROM inventory WHERE status='Low Stock'", conn)
            Label4.Text = cmd3.ExecuteScalar().ToString()
            Dim cmd4 As New MySqlCommand("SELECT IFNULL(SUM(total_amount),0) FROM transactions", conn)
            Label5.Text = "₱" & cmd4.ExecuteScalar().ToString()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub HighlightStocks()
        If DataGridView1.Columns.Contains("status") = False Then Exit Sub
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.IsNewRow Then Continue For
            Dim status As String = row.Cells("status").Value.ToString()

            Select Case status
                Case "In Stock"
                    row.DefaultCellStyle.BackColor = Color.LightGreen
                    row.DefaultCellStyle.ForeColor = Color.Black
                Case "Low Stock"
                    row.DefaultCellStyle.BackColor = Color.Yellow
                    row.DefaultCellStyle.ForeColor = Color.Black
                Case "Out of Stock"
                    row.DefaultCellStyle.BackColor = Color.Red
                    row.DefaultCellStyle.ForeColor = Color.White
            End Select
        Next
    End Sub

    Sub ShowSuggestion()
        Dim current As Integer = CInt(TextBox2.Text)
        Dim suggested As Integer = 20 - current
        If suggested < 0 Then suggested = 0

        MessageBox.Show("Suggested Order : " & suggested & vbCrLf & TextBox1.Text)
    End Sub

    Private Sub report_Click(sender As Object, e As EventArgs) Handles report.Click
        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Reports")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim filePath As String = Path.Combine(folderPath, "Inventory_Report_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".pdf")

            Dim doc As New Document(PageSize.A4, 30, 30, 30, 30)
            PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
            doc.Open()

            Dim titleFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22)
            Dim headerFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)
            Dim normalFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 11)

            Dim title As New Paragraph("🍦 SCOOPIFY CREAMERY 🍦", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            doc.Add(title)

            Dim subtitle As New Paragraph("Inventory Report", headerFont)
            subtitle.Alignment = Element.ALIGN_CENTER
            doc.Add(subtitle)

            doc.Add(New Paragraph("Generated on: " & DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt"), normalFont))
            doc.Add(New Paragraph(" "))

            Dim pdfTable As New PdfPTable(5)
            pdfTable.WidthPercentage = 100
            pdfTable.SetWidths({3, 2, 2, 2, 2})
            pdfTable.AddCell("Item Name")
            pdfTable.AddCell("Category")
            pdfTable.AddCell("Stock")
            pdfTable.AddCell("Reorder Level")
            pdfTable.AddCell("Status")

            For Each row As DataGridViewRow In DataGridView1.Rows
                If Not row.IsNewRow Then
                    pdfTable.AddCell(row.Cells(1).Value.ToString())
                    pdfTable.AddCell(row.Cells(2).Value.ToString())
                    pdfTable.AddCell(row.Cells(3).Value.ToString())
                    pdfTable.AddCell(row.Cells(4).Value.ToString())
                    pdfTable.AddCell(row.Cells(5).Value.ToString())
                End If
            Next

            doc.Add(pdfTable)
            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph("Total Inventory Items: " & (DataGridView1.Rows.Count - 1), normalFont))
            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph("Generated by Scoopify Inventory Management System", normalFont))
            doc.Close()

            MessageBox.Show("Inventory report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Process.Start("explorer.exe", folderPath)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If id = 0 Then
            MessageBox.Show("Please select an item first.")
            Exit Sub
        End If

        If Not Integer.TryParse(TextBox2.Text, Nothing) Then
            MessageBox.Show("Current Stock must be a number.")
            Exit Sub
        End If

        If Not Integer.TryParse(TextBox3.Text, Nothing) Then
            MessageBox.Show("Reorder Level must be a number.")
            Exit Sub
        End If

        Try
            conn.Open()
            Dim cmd As New MySqlCommand("UPDATE inventory SET item_name=@a, category=@b, current_stock=@c, reorder_level=@d WHERE inventory_id=@id", conn)
            cmd.Parameters.AddWithValue("@a", TextBox1.Text)
            cmd.Parameters.AddWithValue("@b", ComboBox1.Text)
            cmd.Parameters.AddWithValue("@c", TextBox2.Text)
            cmd.Parameters.AddWithValue("@d", TextBox3.Text)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Item Updated Successfully!")
            RefreshData()
            Button4.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        id = 0
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        ComboBox1.SelectedIndex = -1
        TextBox1.Focus()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If TextBox1.Text.Trim() = "" Or ComboBox1.Text.Trim() = "" Or TextBox2.Text.Trim() = "" Or TextBox3.Text.Trim() = "" Then
            MessageBox.Show("Please complete all fields.")
            Exit Sub
        End If

        If Not Integer.TryParse(TextBox2.Text, Nothing) Then
            MessageBox.Show("Current Stock must be a number.")
            Exit Sub
        End If

        If Not Integer.TryParse(TextBox3.Text, Nothing) Then
            MessageBox.Show("Reorder Level must be a number.")
            Exit Sub
        End If

        Try
            conn.Open()
            Dim check As New MySqlCommand("SELECT COUNT(*) FROM inventory WHERE item_name=@name", conn)
            check.Parameters.AddWithValue("@name", TextBox1.Text)
            If check.ExecuteScalar() > 0 Then
                MessageBox.Show("Item already exists.")
                Exit Sub
            End If

            Dim cmd As New MySqlCommand("INSERT INTO inventory(item_name,category,current_stock,reorder_level) VALUES(@a,@b,@c,@d)", conn)
            cmd.Parameters.AddWithValue("@a", TextBox1.Text)
            cmd.Parameters.AddWithValue("@b", ComboBox1.Text)
            cmd.Parameters.AddWithValue("@c", TextBox2.Text)
            cmd.Parameters.AddWithValue("@d", TextBox3.Text)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Item Added Successfully!")
            RefreshData()
            Button4.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If id = 0 Then
            MessageBox.Show("Please select an item first.")
            Exit Sub
        End If

        If MessageBox.Show("Delete this item?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Try
                conn.Open()
                Dim cmd As New MySqlCommand("DELETE FROM inventory WHERE inventory_id=@id", conn)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Item Deleted!")
                RefreshData()
                Button4.PerformClick()

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                If conn.State = ConnectionState.Open Then conn.Close()
            End Try
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        SearchInventory()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        LoadGrid("
            SELECT item_name, category, description, price, status
            FROM products
            ORDER BY item_name")
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        If ComboBox2.Text.Trim() = "" Then
            MessageBox.Show("Please select a category first.")
            Exit Sub
        End If

        LoadGrid("SELECT * FROM inventory WHERE category=@cat", "@cat", ComboBox2.Text.Trim())
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        LoadGrid("SELECT * FROM inventory WHERE status='Low Stock' OR status='Out of Stock'")
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form13.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        LoadGrid("
            SELECT flavor_mood.mood_name, products.item_name, flavor_mood.flavor_description, flavor_mood.price
            FROM flavor_mood
            INNER JOIN products ON flavor_mood.product_id = products.product_id")
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        LoadGrid("
            SELECT transaction_id, transaction_date, total_amount, payment_method
            FROM transactions
            ORDER BY transaction_date DESC")
    End Sub

    Sub CheckLowStock()
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("
                SELECT item_name, current_stock
                FROM inventory
                WHERE current_stock<=reorder_level", conn)

            Dim reader = cmd.ExecuteReader()
            Dim msg As String = ""

            While reader.Read()
                msg &= reader("item_name").ToString & " (" & reader("current_stock").ToString & ")" & vbCrLf
            End While

            reader.Close()

            If msg <> "" Then
                MessageBox.Show("LOW STOCK WARNING!" & vbCrLf & vbCrLf & msg, "Inventory Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If id = 0 Then
            MessageBox.Show("Please select an item first.")
            Exit Sub
        End If

        Try
            Dim quantity As Integer
            Dim unitPrice As Decimal
            Dim total As Decimal
            Dim tendered As Decimal
            Dim change As Decimal
            Dim supplier As String

            If Not Integer.TryParse(InputBox("Enter Quantity to Restock"), quantity) Then
                MessageBox.Show("Invalid Quantity.")
                Exit Sub
            End If

            supplier = InputBox("Enter Supplier Name")
            If supplier = "" Then
                MessageBox.Show("Supplier is required.")
                Exit Sub
            End If

            If Not Decimal.TryParse(InputBox("Enter Cost per Item"), unitPrice) Then
                MessageBox.Show("Invalid Price.")
                Exit Sub
            End If

            total = quantity * unitPrice

            MessageBox.Show(
                "Quantity : " & quantity & vbCrLf &
                "Cost per Item : ₱" & unitPrice.ToString("0.00") & vbCrLf & vbCrLf &
                "TOTAL TO PAY : ₱" & total.ToString("0.00"))

            If Not Decimal.TryParse(InputBox("Amount Tendered"), tendered) Then
                MessageBox.Show("Invalid Amount.")
                Exit Sub
            End If

            If tendered < total Then
                MessageBox.Show("Insufficient Amount!")
                Exit Sub
            End If

            change = tendered - total

            conn.Open()

            Dim cmd As New MySqlCommand("UPDATE inventory SET current_stock = current_stock + @qty WHERE inventory_id=@id", conn)
            cmd.Parameters.AddWithValue("@qty", quantity)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()

            Dim cmd2 As New MySqlCommand("
                INSERT INTO restock_history (inventory_id,employee_id,quantity_added,supplier,total_cost)
                VALUES (@inv,@emp,@qty,@sup,@cost)", conn)
            cmd2.Parameters.AddWithValue("@inv", id)
            cmd2.Parameters.AddWithValue("@emp", 4)
            cmd2.Parameters.AddWithValue("@qty", quantity)
            cmd2.Parameters.AddWithValue("@sup", supplier)
            cmd2.Parameters.AddWithValue("@cost", total)
            cmd2.ExecuteNonQuery()

            Dim cmd3 As New MySqlCommand("
                INSERT INTO expenses (employee_id,description,amount,expense_date)
                VALUES (@emp,@desc,@amount,CURDATE())", conn)
            cmd3.Parameters.AddWithValue("@emp", 4)
            cmd3.Parameters.AddWithValue("@desc", "Restocked " & TextBox1.Text)
            cmd3.Parameters.AddWithValue("@amount", total)
            cmd3.ExecuteNonQuery()

            MessageBox.Show(
                "========== RESTOCK RECEIPT ==========" & vbCrLf & vbCrLf &
                "Item : " & TextBox1.Text & vbCrLf &
                "Supplier : " & supplier & vbCrLf &
                "Quantity : " & quantity & vbCrLf &
                "Cost per Item : ₱" & unitPrice.ToString("0.00") & vbCrLf &
                "------------------------------" & vbCrLf &
                "TOTAL COST : ₱" & total.ToString("0.00") & vbCrLf &
                "Amount Tendered : ₱" & tendered.ToString("0.00") & vbCrLf &
                "CHANGE : ₱" & change.ToString("0.00") & vbCrLf & vbCrLf &
                "✔ Restock Successful!",
                "Restock")
            RefreshData()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Expense_Click(sender As Object, e As EventArgs) Handles Expense.Click
        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Reports")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim filePath As String = Path.Combine(folderPath, "Expense_Report_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".pdf")

            Dim doc As New Document(PageSize.A4)
            PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
            doc.Open()

            doc.Add(New Paragraph("SCOOPIFY CREAMERY"))
            doc.Add(New Paragraph("Expense Report"))
            doc.Add(New Paragraph(Date.Now.ToString()))
            doc.Add(New Paragraph(" "))

            Dim table As New PdfPTable(4)
            table.AddCell("Expense ID")
            table.AddCell("Description")
            table.AddCell("Amount")
            table.AddCell("Expense Date")

            conn.Open()
            Dim cmd As New MySqlCommand("
                SELECT expense_id, description, amount, expense_date
                FROM expenses
                ORDER BY expense_date DESC", conn)

            Dim reader = cmd.ExecuteReader()
            While reader.Read()
                table.AddCell(reader("expense_id").ToString())
                table.AddCell(reader("description").ToString())
                table.AddCell("₱" & reader("amount").ToString())
                table.AddCell(CDate(reader("expense_date")).ToShortDateString())
            End While
            reader.Close()

            doc.Add(table)
            doc.Close()

            MessageBox.Show("Expense Report Generated!")
            Process.Start("explorer.exe", folderPath)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Dim choice As String
        choice = InputBox("1 - Inventory Report" & vbCrLf & "2 - Expense Report" & vbCrLf & "3 - Transaction Report")

        Select Case choice
            Case "1"
                report.PerformClick()
            Case "2"
                Expense.PerformClick()
            Case "3"
                Button12.PerformClick()
            Case Else
                MessageBox.Show("Invalid Choice")
        End Select
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Form4.Show()
        Me.Hide()
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Form9.Show()
        Me.Hide()
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Form5.Show()
        Me.Hide()
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Form11.Show()
        Me.Hide()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Form6.Show()
        Me.Hide()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        SearchInventory()
    End Sub
End Class