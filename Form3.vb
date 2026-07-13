Imports MySql.Data.MySqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.draw
Imports System.IO
Imports System.Diagnostics
Imports System.Collections.Generic
Imports Microsoft.VisualBasic

Public Class Form3
    Dim id As Integer

    Sub RefreshData()
        LoadInventory()
        Dashboard()
    End Sub


    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen

        RefreshData()
        CheckLowStock()
        CheckAdminAccess()
    End Sub

    Sub CheckAdminAccess()

        If Form2.LoggedInRole = "Admin" Then

            ComboBox1.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = True
            Button12.Enabled = True
            Button13.Enabled = True
            Button14.Enabled = True
            report.Enabled = True

        Else

            ComboBox1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button12.Enabled = False
            Button13.Enabled = False
            Button14.Enabled = False
            report.Enabled = False

        End If

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

            Dim cmd1 As New MySqlCommand(
            "SELECT COUNT(*) FROM inventory", conn)
            Label2.Text = cmd1.ExecuteScalar().ToString()

            Dim cmd2 As New MySqlCommand(
            "SELECT COUNT(*) FROM inventory WHERE status='In Stock'", conn)
            Label3.Text = cmd2.ExecuteScalar().ToString()

            Dim cmd3 As New MySqlCommand(
            "SELECT COUNT(*) FROM inventory WHERE status='Low Stock'", conn)
            Label4.Text = cmd3.ExecuteScalar().ToString()

            If Form2.LoggedInRole = "Admin" Then

                Dim cmd4 As New MySqlCommand(
                "SELECT IFNULL(SUM(total_amount),0) FROM transactions", conn)

                Label5.Text = "₱" & cmd4.ExecuteScalar().ToString()

            Else

                Label5.Text = "ADMIN ONLY"

            End If

        Catch ex As Exception

            If Form2.LoggedInRole = "Admin" Then
                MessageBox.Show(ex.Message)
            End If

        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
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

    Private ReportAccent As New BaseColor(150, 20, 75)
    Private ReportGray As New BaseColor(90, 90, 90)
    Private ReportRowAlt As New BaseColor(245, 245, 247)
    Private ReportBorder As New BaseColor(215, 215, 215)

    Private Function LoadReportFonts() As Dictionary(Of String, iTextSharp.text.Font)
        Dim fontsFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.Fonts)

        Dim baseRegular As BaseFont = BaseFont.CreateFont(Path.Combine(fontsFolder, "arial.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
        Dim baseBold As BaseFont = BaseFont.CreateFont(Path.Combine(fontsFolder, "arialbd.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
        Dim baseItalic As BaseFont = BaseFont.CreateFont(Path.Combine(fontsFolder, "ariali.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED)

        Dim black As BaseColor = BaseColor.BLACK
        Dim white As BaseColor = BaseColor.WHITE

        Dim f As New Dictionary(Of String, iTextSharp.text.Font)
        f("company") = New iTextSharp.text.Font(baseBold, 20, iTextSharp.text.Font.NORMAL, black)
        f("tagline") = New iTextSharp.text.Font(baseItalic, 9, iTextSharp.text.Font.NORMAL, ReportGray)
        f("reportTitle") = New iTextSharp.text.Font(baseBold, 14, iTextSharp.text.Font.NORMAL, ReportAccent)
        f("meta") = New iTextSharp.text.Font(baseRegular, 9, iTextSharp.text.Font.NORMAL, ReportGray)
        f("summaryLabel") = New iTextSharp.text.Font(baseBold, 8.5F, iTextSharp.text.Font.NORMAL, white)
        f("summaryValue") = New iTextSharp.text.Font(baseBold, 13, iTextSharp.text.Font.NORMAL, white)
        f("tableHeader") = New iTextSharp.text.Font(baseBold, 9.5F, iTextSharp.text.Font.NORMAL, white)
        f("tableCell") = New iTextSharp.text.Font(baseRegular, 9, iTextSharp.text.Font.NORMAL, black)
        f("totalLabel") = New iTextSharp.text.Font(baseBold, 11, iTextSharp.text.Font.NORMAL, white)
        f("totalValue") = New iTextSharp.text.Font(baseBold, 13, iTextSharp.text.Font.NORMAL, white)
        f("footer") = New iTextSharp.text.Font(baseRegular, 8.5F, iTextSharp.text.Font.NORMAL, ReportGray)
        f("footerItalic") = New iTextSharp.text.Font(baseItalic, 8, iTextSharp.text.Font.NORMAL, ReportGray)
        f("normal") = New iTextSharp.text.Font(baseRegular, 9, iTextSharp.text.Font.NORMAL, black)
        Return f
    End Function

    Private Sub AddReportLetterhead(doc As Document, f As Dictionary(Of String, iTextSharp.text.Font), reportTitle As String)

        Dim company As New Paragraph("SCOOPIFY CREAMERY", f("company"))
        company.Alignment = Element.ALIGN_CENTER
        doc.Add(company)

        Dim tagline As New Paragraph("Sweet Moments, One Scoop at a Time", f("tagline"))
        tagline.Alignment = Element.ALIGN_CENTER
        tagline.SpacingAfter = 6
        doc.Add(tagline)

        Dim rule As New LineSeparator(1.2F, 100, ReportAccent, Element.ALIGN_CENTER, -2)
        doc.Add(New Chunk(rule))

        Dim title As New Paragraph(reportTitle, f("reportTitle"))
        title.Alignment = Element.ALIGN_CENTER
        title.SpacingBefore = 12
        title.SpacingAfter = 6
        doc.Add(title)

        Dim roleText As String = "Staff"
        Try
            roleText = Form2.LoggedInRole
        Catch
        End Try

        Dim metaTable As New PdfPTable(2)
        metaTable.WidthPercentage = 100
        metaTable.SetWidths(New Single() {1, 1})
        metaTable.SpacingBefore = 4
        metaTable.SpacingAfter = 12

        AddPlainCell(metaTable, "Generated on: " & DateTime.Now.ToString("MMMM dd, yyyy   hh:mm tt"), f("meta"), Element.ALIGN_LEFT)
        AddPlainCell(metaTable, "Generated by: " & roleText, f("meta"), Element.ALIGN_RIGHT)
        doc.Add(metaTable)
    End Sub

    Private Sub AddReportFooter(doc As Document, f As Dictionary(Of String, iTextSharp.text.Font), summaryLine As String)

        doc.Add(New Paragraph(" ", f("normal")) With {.SpacingBefore = 10})

        Dim rule As New LineSeparator(0.8F, 100, ReportAccent, Element.ALIGN_CENTER, -2)
        doc.Add(New Chunk(rule))

        Dim summary As New Paragraph(summaryLine, f("footer"))
        summary.SpacingBefore = 6
        doc.Add(summary)

        Dim confidential As New Paragraph("This is a system-generated report from the Scoopify Inventory Management System. For internal administrative use only.", f("footerItalic"))
        confidential.SpacingBefore = 3
        doc.Add(confidential)

        Dim printed As New Paragraph("Printed: " & DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), f("footerItalic"))
        printed.SpacingBefore = 2
        doc.Add(printed)
    End Sub

    Private Sub AddSummaryRow(doc As Document, f As Dictionary(Of String, iTextSharp.text.Font), labels As String(), values As String())
        Dim table As New PdfPTable(labels.Length)
        table.WidthPercentage = 100
        table.SpacingBefore = 2
        table.SpacingAfter = 10

        For i = 0 To labels.Length - 1
            Dim cell As New PdfPCell()
            cell.BackgroundColor = ReportAccent
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER
            cell.PaddingTop = 8
            cell.PaddingBottom = 8
            cell.HorizontalAlignment = Element.ALIGN_CENTER

            If i > 0 Then
                cell.BorderWidthLeft = 0.75F
                cell.BorderColorLeft = BaseColor.WHITE
            End If

            Dim lbl As New Paragraph(labels(i), f("summaryLabel"))
            lbl.Alignment = Element.ALIGN_CENTER
            cell.AddElement(lbl)

            Dim valPara As New Paragraph(values(i), f("summaryValue"))
            valPara.Alignment = Element.ALIGN_CENTER
            valPara.SpacingBefore = 2
            cell.AddElement(valPara)

            table.AddCell(cell)
        Next
        doc.Add(table)
    End Sub

    Private Sub AddHeaderCell(table As PdfPTable, text As String, font As iTextSharp.text.Font)
        Dim cell As New PdfPCell(New Phrase(text, font))
        cell.BackgroundColor = ReportAccent
        cell.HorizontalAlignment = Element.ALIGN_CENTER
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Padding = 6
        cell.Border = iTextSharp.text.Rectangle.NO_BORDER
        table.AddCell(cell)
    End Sub

    Private Sub AddBodyCell(table As PdfPTable, text As String, font As iTextSharp.text.Font, bg As BaseColor, alignment As Integer)
        Dim cell As New PdfPCell(New Phrase(text, font))
        cell.BackgroundColor = bg
        cell.HorizontalAlignment = alignment
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Padding = 5
        cell.BorderColor = ReportBorder
        cell.BorderWidth = 0.5F
        table.AddCell(cell)
    End Sub

    Private Sub AddPlainCell(table As PdfPTable, text As String, font As iTextSharp.text.Font, alignment As Integer)
        Dim cell As New PdfPCell(New Phrase(text, font))
        cell.Border = iTextSharp.text.Rectangle.NO_BORDER
        cell.HorizontalAlignment = alignment
        table.AddCell(cell)
    End Sub

    Private Sub report_Click(sender As Object, e As EventArgs) Handles report.Click
        GenerateInventoryReportPdf()
    End Sub

    Private Sub GenerateInventoryReportPdf()
        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Reports")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim filePath As String = Path.Combine(folderPath, "Inventory_Report_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".pdf")

            Dim doc As New Document(PageSize.A4, 36, 36, 40, 40)
            PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
            doc.Open()

            Dim f As Dictionary(Of String, iTextSharp.text.Font) = LoadReportFonts()

            AddReportLetterhead(doc, f, "INVENTORY STATUS REPORT")

            Dim items As New List(Of String())
            Dim totalItems As Integer = 0
            Dim inStock As Integer = 0
            Dim lowStock As Integer = 0
            Dim outStock As Integer = 0

            conn.Open()
            Dim cmd As New MySqlCommand("SELECT item_name, category, current_stock, reorder_level, status FROM inventory ORDER BY item_name", conn)
            Dim reader = cmd.ExecuteReader()
            While reader.Read()
                Dim status As String = reader("status").ToString()
                items.Add({reader("item_name").ToString(), reader("category").ToString(), reader("current_stock").ToString(), reader("reorder_level").ToString(), status})
                totalItems += 1
                Select Case status
                    Case "In Stock" : inStock += 1
                    Case "Low Stock" : lowStock += 1
                    Case "Out of Stock" : outStock += 1
                End Select
            End While
            reader.Close()
            conn.Close()

            AddSummaryRow(doc, f,
                New String() {"TOTAL ITEMS", "IN STOCK", "LOW STOCK", "OUT OF STOCK"},
                New String() {totalItems.ToString(), inStock.ToString(), lowStock.ToString(), outStock.ToString()})

            Dim table As New PdfPTable(5)
            table.WidthPercentage = 100
            table.SetWidths(New Single() {3, 2, 1.3F, 1.5F, 1.5F})

            For Each h As String In New String() {"Item Name", "Category", "Stock", "Reorder Lvl", "Status"}
                AddHeaderCell(table, h, f("tableHeader"))
            Next

            Dim rowIdx As Integer = 0
            For Each row As String() In items
                Dim bg As BaseColor = If(rowIdx Mod 2 = 0, BaseColor.WHITE, ReportRowAlt)
                AddBodyCell(table, row(0), f("tableCell"), bg, Element.ALIGN_LEFT)
                AddBodyCell(table, row(1), f("tableCell"), bg, Element.ALIGN_LEFT)
                AddBodyCell(table, row(2), f("tableCell"), bg, Element.ALIGN_CENTER)
                AddBodyCell(table, row(3), f("tableCell"), bg, Element.ALIGN_CENTER)
                AddBodyCell(table, row(4), f("tableCell"), bg, Element.ALIGN_CENTER)
                rowIdx += 1
            Next
            doc.Add(table)

            AddReportFooter(doc, f, "Total Inventory Items Listed: " & totalItems)

            doc.Close()

            MessageBox.Show("Inventory report generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Process.Start("explorer.exe", folderPath)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
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

        GenerateTransactionReportPdf()
    End Sub

    Private Sub GenerateTransactionReportPdf()
        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Reports")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim filePath As String = Path.Combine(folderPath, "Transaction_Report_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".pdf")

            Dim doc As New Document(PageSize.A4, 36, 36, 40, 40)
            PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
            doc.Open()

            Dim f As Dictionary(Of String, iTextSharp.text.Font) = LoadReportFonts()
            AddReportLetterhead(doc, f, "TRANSACTION REPORT")

            Dim table As New PdfPTable(4)
            table.WidthPercentage = 100
            table.SetWidths(New Single() {1, 2, 1.5F, 1.5F})

            For Each h As String In New String() {"ID", "Date", "Total Amount", "Payment Method"}
                AddHeaderCell(table, h, f("tableHeader"))
            Next

            Dim totalAmount As Decimal = 0
            Dim rowIdx As Integer = 0

            conn.Open()
            Dim cmd As New MySqlCommand("
                SELECT transaction_id, transaction_date, total_amount, payment_method
                FROM transactions
                ORDER BY transaction_date DESC", conn)

            Dim reader = cmd.ExecuteReader()
            While reader.Read()
                Dim bg As BaseColor = If(rowIdx Mod 2 = 0, BaseColor.WHITE, ReportRowAlt)
                Dim amt As Decimal = CDec(reader("total_amount"))
                totalAmount += amt

                AddBodyCell(table, reader("transaction_id").ToString(), f("tableCell"), bg, Element.ALIGN_CENTER)
                AddBodyCell(table, CDate(reader("transaction_date")).ToString("MMM dd, yyyy"), f("tableCell"), bg, Element.ALIGN_CENTER)
                AddBodyCell(table, "₱" & amt.ToString("0.00"), f("tableCell"), bg, Element.ALIGN_RIGHT)
                AddBodyCell(table, reader("payment_method").ToString(), f("tableCell"), bg, Element.ALIGN_CENTER)

                rowIdx += 1
            End While
            reader.Close()
            conn.Close()

            doc.Add(table)

            Dim totalTable As New PdfPTable(2)
            totalTable.WidthPercentage = 100
            totalTable.SetWidths(New Single() {3, 1})
            totalTable.SpacingBefore = 6

            Dim lblCell As New PdfPCell(New Phrase("TOTAL SALES", f("totalLabel")))
            lblCell.BackgroundColor = ReportAccent
            lblCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            lblCell.PaddingTop = 8
            lblCell.PaddingBottom = 8
            lblCell.PaddingLeft = 6
            lblCell.VerticalAlignment = Element.ALIGN_MIDDLE
            totalTable.AddCell(lblCell)

            Dim valCell As New PdfPCell(New Phrase("₱" & totalAmount.ToString("0.00"), f("totalValue")))
            valCell.BackgroundColor = ReportAccent
            valCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            valCell.HorizontalAlignment = Element.ALIGN_RIGHT
            valCell.PaddingTop = 8
            valCell.PaddingBottom = 8
            valCell.PaddingRight = 6
            valCell.VerticalAlignment = Element.ALIGN_MIDDLE
            totalTable.AddCell(valCell)

            doc.Add(totalTable)

            AddReportFooter(doc, f, "Total Transactions Listed: " & rowIdx)

            doc.Close()

            MessageBox.Show("Transaction Report Generated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Process.Start("explorer.exe", folderPath)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
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
        GenerateExpenseReportPdf()
    End Sub

    Private Sub GenerateExpenseReportPdf()
        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Reports")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim filePath As String = Path.Combine(folderPath, "Expense_Report_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".pdf")

            Dim doc As New Document(PageSize.A4, 36, 36, 40, 40)
            PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
            doc.Open()

            Dim f As Dictionary(Of String, iTextSharp.text.Font) = LoadReportFonts()

            AddReportLetterhead(doc, f, "EXPENSE REPORT")

            Dim table As New PdfPTable(4)
            table.WidthPercentage = 100
            table.SetWidths(New Single() {1, 3.5F, 1.5F, 1.5F})

            For Each h As String In New String() {"ID", "Description", "Amount", "Date"}
                AddHeaderCell(table, h, f("tableHeader"))
            Next

            Dim totalAmount As Decimal = 0
            Dim rowIdx As Integer = 0

            conn.Open()
            Dim cmd As New MySqlCommand("
                SELECT expense_id, description, amount, expense_date
                FROM expenses
                ORDER BY expense_date DESC", conn)

            Dim reader = cmd.ExecuteReader()
            While reader.Read()
                Dim bg As BaseColor = If(rowIdx Mod 2 = 0, BaseColor.WHITE, ReportRowAlt)
                Dim amt As Decimal = CDec(reader("amount"))
                totalAmount += amt

                AddBodyCell(table, reader("expense_id").ToString(), f("tableCell"), bg, Element.ALIGN_CENTER)
                AddBodyCell(table, reader("description").ToString(), f("tableCell"), bg, Element.ALIGN_LEFT)
                AddBodyCell(table, "₱" & amt.ToString("0.00"), f("tableCell"), bg, Element.ALIGN_RIGHT)
                AddBodyCell(table, CDate(reader("expense_date")).ToString("MMM dd, yyyy"), f("tableCell"), bg, Element.ALIGN_CENTER)

                rowIdx += 1
            End While
            reader.Close()
            conn.Close()

            doc.Add(table)

            Dim totalTable As New PdfPTable(2)
            totalTable.WidthPercentage = 100
            totalTable.SetWidths(New Single() {3, 1})
            totalTable.SpacingBefore = 6

            Dim lblCell As New PdfPCell(New Phrase("TOTAL EXPENSES", f("totalLabel")))
            lblCell.BackgroundColor = ReportAccent
            lblCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            lblCell.PaddingTop = 8
            lblCell.PaddingBottom = 8
            lblCell.PaddingLeft = 6
            lblCell.VerticalAlignment = Element.ALIGN_MIDDLE
            totalTable.AddCell(lblCell)

            Dim valCell As New PdfPCell(New Phrase("₱" & totalAmount.ToString("0.00"), f("totalValue")))
            valCell.BackgroundColor = ReportAccent
            valCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            valCell.HorizontalAlignment = Element.ALIGN_RIGHT
            valCell.PaddingTop = 8
            valCell.PaddingBottom = 8
            valCell.PaddingRight = 6
            valCell.VerticalAlignment = Element.ALIGN_MIDDLE
            totalTable.AddCell(valCell)

            doc.Add(totalTable)

            AddReportFooter(doc, f, "Total Expense Entries: " & rowIdx)

            doc.Close()

            MessageBox.Show("Expense Report Generated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                GenerateInventoryReportPdf()
            Case "2"
                GenerateExpenseReportPdf()
            Case "3"
                GenerateTransactionReportPdf()
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