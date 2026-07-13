Imports MySql.Data.MySqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.draw
Imports System.IO
Imports System.Diagnostics
Imports System.Threading.Tasks
Imports System.Drawing.Drawing2D

Public Class Form12

    Public Sub RefreshCart()
        CheckedListBox1.Items.Clear()
        For Each item In CartManager.CartItems
            CheckedListBox1.Items.Add(item.ItemName & " x" & item.Quantity & " - ₱" & item.Subtotal.ToString("0.00"))
        Next
        UpdateOrderSummary()
    End Sub

    Public Sub UpdateOrderSummary()
        RichTextBox1.Text =
        "========== ORDER SUMMARY ==========" & vbCrLf &
        "Items in Cart: " & CartManager.CartItems.Count & vbCrLf & vbCrLf &
        "TOTAL: ₱" & CartManager.GetTotal().ToString("0.00")
    End Sub

    Private Sub Form12_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen

        RefreshCart()
    End Sub
    Private Sub GenerateReceipt(total As Decimal,
                                 paymentMethod As String,
                                 amountTendered As Decimal,
                                 change As Decimal)

        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Receipts")

            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim fileName As String = "Receipt_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".pdf"
            Dim filePath As String = Path.Combine(folderPath, fileName)

            Dim pageSize As New iTextSharp.text.Rectangle(226, 750)
            Dim doc As New Document(pageSize, 18, 18, 20, 20)

            PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
            doc.Open()

            Dim hotPink As New BaseColor(236, 64, 122)
            Dim darkPink As New BaseColor(173, 20, 87)
            Dim lightPink As New BaseColor(252, 228, 236)
            Dim white As BaseColor = BaseColor.WHITE
            Dim black As BaseColor = BaseColor.BLACK
            Dim gray As New BaseColor(120, 120, 120)

            Dim fontPathRegular As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf")
            Dim fontPathBold As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialbd.ttf")
            Dim fontPathItalic As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ariali.ttf")

            Dim baseRegular As BaseFont = BaseFont.CreateFont(fontPathRegular, BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
            Dim baseBold As BaseFont = BaseFont.CreateFont(fontPathBold, BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
            Dim baseItalic As BaseFont = BaseFont.CreateFont(fontPathItalic, BaseFont.IDENTITY_H, BaseFont.EMBEDDED)

            Dim titleFont As New iTextSharp.text.Font(baseBold, 17, iTextSharp.text.Font.NORMAL, white)
            Dim taglineFont As New iTextSharp.text.Font(baseItalic, 8, iTextSharp.text.Font.NORMAL, white)
            Dim headerFont As New iTextSharp.text.Font(baseBold, 10.5F, iTextSharp.text.Font.NORMAL, darkPink)
            Dim labelFont As New iTextSharp.text.Font(baseRegular, 8.5F, iTextSharp.text.Font.NORMAL, gray)
            Dim normalFont As New iTextSharp.text.Font(baseRegular, 9, iTextSharp.text.Font.NORMAL, black)
            Dim itemNameFont As New iTextSharp.text.Font(baseBold, 9, iTextSharp.text.Font.NORMAL, black)
            Dim itemSubFont As New iTextSharp.text.Font(baseRegular, 8, iTextSharp.text.Font.NORMAL, gray)
            Dim totalLabelFont As New iTextSharp.text.Font(baseBold, 12, iTextSharp.text.Font.NORMAL, white)
            Dim totalValueFont As New iTextSharp.text.Font(baseBold, 14, iTextSharp.text.Font.NORMAL, white)
            Dim footerBoldFont As New iTextSharp.text.Font(baseBold, 10, iTextSharp.text.Font.NORMAL, darkPink)
            Dim footerSmallFont As New iTextSharp.text.Font(baseItalic, 7.5F, iTextSharp.text.Font.NORMAL, gray)

            Dim headerTable As New PdfPTable(1)
            headerTable.WidthPercentage = 100
            Dim headerCell As New PdfPCell()
            headerCell.BackgroundColor = hotPink
            headerCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            headerCell.PaddingTop = 12
            headerCell.PaddingBottom = 10
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER

            Dim titlePara As New Paragraph("SCOOPIFY CREAMERY", titleFont)
            titlePara.Alignment = Element.ALIGN_CENTER
            headerCell.AddElement(titlePara)

            Dim taglinePara As New Paragraph("Sweet Moments, One Scoop at a Time", taglineFont)
            taglinePara.Alignment = Element.ALIGN_CENTER
            taglinePara.SpacingBefore = 3
            headerCell.AddElement(taglinePara)

            headerTable.AddCell(headerCell)
            doc.Add(headerTable)

            doc.Add(New Paragraph(" ", normalFont) With {.SpacingAfter = 2})

            ' ---------- RECEIPT META INFO ----------
            Dim metaTable As New PdfPTable(2)
            metaTable.WidthPercentage = 100
            metaTable.SetWidths(New Single() {1, 1})

            AddPlainCell(metaTable, "Receipt No.", labelFont, Element.ALIGN_LEFT)
            AddPlainCell(metaTable, DateTime.Now.ToString("MMM dd, yyyy"), labelFont, Element.ALIGN_RIGHT)
            AddPlainCell(metaTable, DateTime.Now.ToString("yyyyMMddHHmmss"), normalFont, Element.ALIGN_LEFT)
            AddPlainCell(metaTable, DateTime.Now.ToString("hh:mm tt"), labelFont, Element.ALIGN_RIGHT)
            doc.Add(metaTable)

            doc.Add(DashedSeparator(darkPink))

            Dim itemsHeading As New Paragraph("ITEMS PURCHASED", headerFont)
            itemsHeading.SpacingBefore = 6
            itemsHeading.SpacingAfter = 4
            doc.Add(itemsHeading)

            Dim itemsTable As New PdfPTable(2)
            itemsTable.WidthPercentage = 100
            itemsTable.SetWidths(New Single() {3, 1.3F})

            Dim rowIndex As Integer = 0
            For Each item In CartManager.CartItems
                Dim rowBg As BaseColor = If(rowIndex Mod 2 = 0, white, lightPink)

                Dim nameCell As New PdfPCell()
                nameCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                nameCell.BackgroundColor = rowBg
                nameCell.PaddingTop = 4
                nameCell.PaddingBottom = 4
                nameCell.PaddingLeft = 4
                nameCell.AddElement(New Paragraph(item.ItemName, itemNameFont))
                nameCell.AddElement(New Paragraph("Qty: " & item.Quantity & "  x  ₱" & item.Price.ToString("0.00"), itemSubFont))
                itemsTable.AddCell(nameCell)

                Dim subCell As New PdfPCell(New Phrase("₱" & item.Subtotal.ToString("0.00"), itemNameFont))
                subCell.Border = iTextSharp.text.Rectangle.NO_BORDER
                subCell.BackgroundColor = rowBg
                subCell.HorizontalAlignment = Element.ALIGN_RIGHT
                subCell.VerticalAlignment = Element.ALIGN_MIDDLE
                subCell.PaddingRight = 4
                itemsTable.AddCell(subCell)

                rowIndex += 1
            Next
            doc.Add(itemsTable)

            doc.Add(DashedSeparator(darkPink))

            Dim totalTable As New PdfPTable(2)
            totalTable.WidthPercentage = 100
            totalTable.SetWidths(New Single() {1, 1})
            totalTable.SpacingBefore = 6

            Dim totalLabelCell As New PdfPCell(New Phrase("TOTAL", totalLabelFont))
            totalLabelCell.BackgroundColor = darkPink
            totalLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            totalLabelCell.PaddingTop = 8
            totalLabelCell.PaddingBottom = 8
            totalLabelCell.PaddingLeft = 6
            totalLabelCell.VerticalAlignment = Element.ALIGN_MIDDLE
            totalTable.AddCell(totalLabelCell)

            Dim totalValueCell As New PdfPCell(New Phrase("₱" & total.ToString("0.00"), totalValueFont))
            totalValueCell.BackgroundColor = darkPink
            totalValueCell.Border = iTextSharp.text.Rectangle.NO_BORDER
            totalValueCell.HorizontalAlignment = Element.ALIGN_RIGHT
            totalValueCell.PaddingTop = 8
            totalValueCell.PaddingBottom = 8
            totalValueCell.PaddingRight = 6
            totalValueCell.VerticalAlignment = Element.ALIGN_MIDDLE
            totalTable.AddCell(totalValueCell)

            doc.Add(totalTable)

            doc.Add(New Paragraph(" ", normalFont) With {.SpacingAfter = 2})

            Dim payTable As New PdfPTable(2)
            payTable.WidthPercentage = 100
            payTable.SetWidths(New Single() {1.3F, 1})

            AddPlainCell(payTable, "Payment Method", labelFont, Element.ALIGN_LEFT)
            AddPlainCell(payTable, paymentMethod.ToUpper(), normalFont, Element.ALIGN_RIGHT)
            AddPlainCell(payTable, "Amount Tendered", labelFont, Element.ALIGN_LEFT)
            AddPlainCell(payTable, "₱" & amountTendered.ToString("0.00"), normalFont, Element.ALIGN_RIGHT)
            AddPlainCell(payTable, "Change", labelFont, Element.ALIGN_LEFT)
            AddPlainCell(payTable, "₱" & change.ToString("0.00"), normalFont, Element.ALIGN_RIGHT)
            doc.Add(payTable)

            doc.Add(DashedSeparator(darkPink))

            Dim thanks As New Paragraph("Thank you for choosing Scoopify!", footerBoldFont)
            thanks.Alignment = Element.ALIGN_CENTER
            thanks.SpacingBefore = 8
            doc.Add(thanks)

            Dim footerNote As New Paragraph("We hope your day becomes sweeter with every scoop.", footerSmallFont)
            footerNote.Alignment = Element.ALIGN_CENTER
            footerNote.SpacingBefore = 3
            footerNote.SpacingAfter = 6
            doc.Add(footerNote)

            doc.Close()

            Process.Start(New ProcessStartInfo(filePath) With {.UseShellExecute = True})

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Receipt Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub AddPlainCell(table As PdfPTable, text As String, font As iTextSharp.text.Font, alignment As Integer)
        Dim cell As New PdfPCell(New Phrase(text, font))
        cell.Border = iTextSharp.text.Rectangle.NO_BORDER
        cell.HorizontalAlignment = alignment
        cell.PaddingTop = 1
        cell.PaddingBottom = 1
        table.AddCell(cell)
    End Sub

    Private Function DashedSeparator(color As BaseColor) As Chunk
        Dim line As New DottedLineSeparator()
        line.LineColor = color
        line.Gap = 3
        Return New Chunk(line)
    End Function

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged

        If CheckedListBox1.SelectedIndex = -1 Then Exit Sub
        Dim index As Integer = CheckedListBox1.SelectedIndex
        Dim result As DialogResult

        result = MessageBox.Show(
        "Remove this item from cart?",
        "Remove Order",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            CartManager.CartItems.RemoveAt(index)
            RefreshCart()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If CheckedListBox1.Items.Count = 0 Then

            MessageBox.Show(
                "Cart is already empty.",
                "Cart",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

            Exit Sub

        End If

        Dim answer As DialogResult

        answer = MessageBox.Show(
            "Clear all items from cart?",
            "Clear Cart",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If answer = DialogResult.Yes Then

            CartManager.ClearCart()
            RefreshCart()

            MessageBox.Show(
                "Cart cleared successfully!",
                "Cart",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        End If

    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If CartManager.CartItems.Count = 0 Then
            MessageBox.Show("Your cart is empty.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim total As Decimal = CartManager.GetTotal()

        Dim amountText As String = InputBox(
        "Total Amount: ₱" & total.ToString("0.00") & vbCrLf & vbCrLf & "Enter Amount Tendered:",
        "Cash Payment")

        Dim amountTendered As Decimal
        If Not Decimal.TryParse(amountText, amountTendered) Then
            MessageBox.Show("Invalid amount entered.", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If amountTendered < total Then
            MessageBox.Show("Insufficient amount.", "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim change As Decimal = amountTendered - total

        SaveOrderToDatabase(total, amountTendered, change)

        MessageBox.Show(
    "Order placed successfully!" & vbCrLf &
    "Change: ₱" & change.ToString("0.00"),
    "Order Complete",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information)

        Await ShowGeneratingReceiptAsync()
        GenerateReceipt(total, "Cash", amountTendered, change)

        CartManager.ClearCart()
        RefreshCart()

    End Sub
    Private Async Function ShowGeneratingReceiptAsync() As Task

        Dim loadingForm As New Form()
        loadingForm.FormBorderStyle = FormBorderStyle.None
        loadingForm.StartPosition = FormStartPosition.CenterScreen
        loadingForm.Size = New Size(320, 130)
        loadingForm.BackColor = Color.White
        loadingForm.TopMost = True
        loadingForm.ShowInTaskbar = False

        Dim radius As Integer = 18
        Dim path As New GraphicsPath()
        Dim rect As New System.Drawing.Rectangle(0, 0, loadingForm.Width, loadingForm.Height)
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90)
        path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90)
        path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90)
        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90)
        path.CloseFigure()
        loadingForm.Region = New Region(path)

        AddHandler loadingForm.Paint, Sub(s As Object, pe As PaintEventArgs)
                                          Using pen As New Pen(Color.FromArgb(236, 64, 122), 2)
                                              pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                                              Dim borderRect As New System.Drawing.Rectangle(1, 1, loadingForm.Width - 3, loadingForm.Height - 3)
                                              pe.Graphics.DrawPath(pen, GetRoundedPath(borderRect, radius))
                                          End Using
                                      End Sub

        Dim iconLabel As New Label()
        iconLabel.Text = "SCOOPIFY"
        iconLabel.Font = New System.Drawing.Font("Segoe UI", 9, FontStyle.Bold)
        iconLabel.ForeColor = Color.FromArgb(173, 20, 87)
        iconLabel.TextAlign = ContentAlignment.MiddleCenter
        iconLabel.Dock = DockStyle.Top
        iconLabel.Height = 30
        iconLabel.Top = 12

        Dim mainLabel As New Label()
        mainLabel.Text = "Generating your receipt..."
        mainLabel.Font = New System.Drawing.Font("Segoe UI", 11, FontStyle.Bold)
        mainLabel.ForeColor = Color.FromArgb(236, 64, 122)
        mainLabel.TextAlign = ContentAlignment.MiddleCenter
        mainLabel.Dock = DockStyle.Top
        mainLabel.Height = 35

        Dim progressBar As New ProgressBar()
        progressBar.Style = ProgressBarStyle.Marquee
        progressBar.MarqueeAnimationSpeed = 25
        progressBar.Dock = DockStyle.Top
        progressBar.Height = 10
        progressBar.Width = 220
        progressBar.Margin = New Padding(20)

        Dim contentPanel As New Panel()
        contentPanel.Dock = DockStyle.Fill
        contentPanel.Padding = New Padding(20, 15, 20, 15)

        Dim barWrapper As New Panel()
        barWrapper.Dock = DockStyle.Top
        barWrapper.Height = 20
        progressBar.Left = 30
        progressBar.Top = 4
        progressBar.Width = loadingForm.Width - 60
        barWrapper.Controls.Add(progressBar)

        contentPanel.Controls.Add(barWrapper)
        contentPanel.Controls.Add(mainLabel)
        contentPanel.Controls.Add(iconLabel)

        loadingForm.Controls.Add(contentPanel)

        loadingForm.Show()
        loadingForm.Refresh()

        Await Task.Delay(1600)

        loadingForm.Close()
        loadingForm.Dispose()

    End Function

    Private Function GetRoundedPath(rect As System.Drawing.Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90)
        path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90)
        path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90)
        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    Private Sub SaveOrderToDatabase(total As Decimal, cashReceived As Decimal, changeAmount As Decimal)
        Dim connString As String = "server=localhost;database=Scoopify_Creamery;user=root;password=BugfixMaster#22;"

        Using conn As New MySqlConnection(connString)
            conn.Open()

            Dim cmdTrans As New MySqlCommand(
            "INSERT INTO transactions
            (customer_id, employee_id, total_amount, payment_method, cash_received, change_amount)
            VALUES
            (1, 3, @total, 'Cash', @cash, @change)", conn)

            cmdTrans.Parameters.AddWithValue("@total", total)
            cmdTrans.Parameters.AddWithValue("@cash", cashReceived)
            cmdTrans.Parameters.AddWithValue("@change", changeAmount)
            cmdTrans.ExecuteNonQuery()

            Dim transactionID As Integer = CInt(cmdTrans.LastInsertedId)

            For Each item In CartManager.CartItems
                Dim cmdDetail As New MySqlCommand(
                "INSERT INTO transaction_details (transaction_id, product_id, quantity, unit_price, subtotal)
                 VALUES (@tid, @pid, @qty, @price, @subtotal)", conn)
                cmdDetail.Parameters.AddWithValue("@tid", transactionID)
                cmdDetail.Parameters.AddWithValue("@pid", item.ProductID)
                cmdDetail.Parameters.AddWithValue("@qty", item.Quantity)
                cmdDetail.Parameters.AddWithValue("@price", item.Price)
                cmdDetail.Parameters.AddWithValue("@subtotal", item.Subtotal)
                cmdDetail.ExecuteNonQuery()
            Next
        End Using
    End Sub

    Private Sub Form12_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Me.Visible Then
            RefreshCart()
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Form4.Show()
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

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form11.Show()
        Me.Hide()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form6.Show()
        Me.Hide()
    End Sub
End Class