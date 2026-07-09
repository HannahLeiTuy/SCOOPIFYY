Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Diagnostics
Public Class Form12
    Private Sub Form12_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateOrderSummary()
    End Sub

    Public Sub UpdateOrderSummary()

        Dim total As Decimal = 0

        For Each item As String In CheckedListBox1.Items

            If item.Contains("₱") Then

                Dim parts() As String = item.Split("₱"c)

                If parts.Length > 1 Then

                    Dim priceText As String = parts(1).Trim()
                    Dim price As Decimal

                    If Decimal.TryParse(priceText, price) Then
                        total += price
                    End If

                End If

            End If

        Next

        RichTextBox1.Text =
        "========== ORDER SUMMARY ==========" & vbCrLf &
        "Items in Cart: " & CheckedListBox1.Items.Count & vbCrLf &
        vbCrLf &
        "TOTAL: ₱" & total.ToString("0.00")

    End Sub
    Private Sub GenerateReceipt(total As Decimal,
                            paymentMethod As String,
                            amountTendered As Decimal,
                            change As Decimal)

        Try

            Dim folderPath As String =
            Path.Combine(Application.StartupPath, "Receipts")

            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim fileName As String =
            "Receipt_" &
            DateTime.Now.ToString("yyyyMMdd_HHmmss") &
            ".pdf"

            Dim filePath As String =
            Path.Combine(folderPath, fileName)

            Dim doc As New Document()

            PdfWriter.GetInstance(doc,
                              New FileStream(filePath,
                                             FileMode.Create))

            doc.Open()

            Dim titleFont As Font =
    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22)

            Dim headerFont As Font =
    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)

            Dim normalFont As Font =
    FontFactory.GetFont(FontFactory.HELVETICA, 12)

            Dim smallFont As Font =
    FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10)

            Dim title As New Paragraph(
    "🍦 SCOOPIFY CREAMERY 🍦",
    titleFont)

            title.Alignment = Element.ALIGN_CENTER

            doc.Add(title)

            doc.Add(New Paragraph(
    "Sweet Moments, One Scoop at a Time",
    smallFont) With {
    .Alignment = Element.ALIGN_CENTER
})

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(
    "════════════════════════════",
    normalFont))

            doc.Add(New Paragraph(
    "OFFICIAL RECEIPT",
    headerFont))

            doc.Add(New Paragraph(
    "Date: " &
    DateTime.Now.ToString(
    "MMMM dd, yyyy hh:mm tt"),
    normalFont))

            doc.Add(New Paragraph(
    "Receipt No: " &
    DateTime.Now.ToString(
    "yyyyMMddHHmmss"),
    normalFont))

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(
    "🛒 ITEMS ORDERED",
    headerFont))

            doc.Add(New Paragraph(" "))

            For Each item As String In CheckedListBox1.Items
                doc.Add(New Paragraph(
        "• " & item,
        normalFont))
            Next

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(
    "════════════════════════════",
    normalFont))

            doc.Add(New Paragraph(
    "TOTAL AMOUNT: ₱" &
    total.ToString("0.00"),
    headerFont))

            doc.Add(New Paragraph(
    "PAYMENT METHOD: " &
    paymentMethod.ToUpper(),
    normalFont))

            doc.Add(New Paragraph(
    "AMOUNT TENDERED: ₱" &
    amountTendered.ToString("0.00"),
    normalFont))

            doc.Add(New Paragraph(
    "CHANGE: ₱" &
    change.ToString("0.00"),
    normalFont))

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(
    "💜 Thank you for choosing Scoopify!",
    headerFont))

            doc.Add(New Paragraph(
    "We hope to serve you again soon 🍨",
    smallFont))

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(
    "════════════════════════════",
    normalFont))

            doc.Close()
            MessageBox.Show(
            "Receipt generated successfully!",
            "Receipt",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

            Process.Start("explorer.exe", folderPath)

        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged

        If CheckedListBox1.SelectedIndex = -1 Then Exit Sub

        Dim result As DialogResult

        result = MessageBox.Show(
        "Remove this item from cart?",
        "Remove Order",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question)

        If result = DialogResult.Yes Then

            Dim selectedItem As String =
            CheckedListBox1.SelectedItem.ToString()

            CheckedListBox1.Items.Remove(selectedItem)

            UpdateOrderSummary()

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

            CheckedListBox1.Items.Clear()

            UpdateOrderSummary()

            MessageBox.Show(
                "Cart cleared successfully!",
                "Cart",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If CheckedListBox1.Items.Count = 0 Then

            MessageBox.Show(
            "Your cart is empty.",
            "Checkout",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)

            Exit Sub

        End If

        Dim total As Decimal = 0

        For Each item As String In CheckedListBox1.Items

            If item.Contains("₱") Then

                Dim parts() As String = item.Split("₱"c)

                If parts.Length > 1 Then

                    Dim priceText As String = parts(1).Trim()
                    Dim value As Decimal

                    If Decimal.TryParse(priceText, value) Then
                        total += value
                    End If

                End If

            End If

        Next
        Dim paymentMethod As String = "Cash"

        Dim amountText As String

        amountText = InputBox(
        "Total Amount: ₱" & total.ToString("0.00") &
        vbCrLf & vbCrLf &
        "Enter Amount Tendered:",
        "Cash Payment")

        Dim amountTendered As Decimal

        If Not Decimal.TryParse(amountText, amountTendered) Then

            MessageBox.Show(
            "Invalid amount entered.",
            "Payment Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)

            Exit Sub

        End If

        If amountTendered < total Then

            MessageBox.Show(
            "Insufficient amount.",
            "Payment Failed",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)

            Exit Sub

        End If

        Dim change As Decimal = amountTendered - total

        RichTextBox1.Text =
        "========== RECEIPT ==========" & vbCrLf &
        "Items Ordered: " & CheckedListBox1.Items.Count & vbCrLf &
        "Payment Method: CASH" & vbCrLf &
        "-----------------------------" & vbCrLf &
        "TOTAL: ₱" & total.ToString("0.00") & vbCrLf &
        "AMOUNT TENDERED: ₱" & amountTendered.ToString("0.00") & vbCrLf &
        "CHANGE: ₱" & change.ToString("0.00") & vbCrLf &
        vbCrLf &
        "Thank you for choosing Scoopify!"

        MessageBox.Show(
        "Order placed successfully!" & vbCrLf &
        "Change: ₱" & change.ToString("0.00"),
        "Order Complete",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information)

        GenerateReceipt(
        total,
        paymentMethod,
        amountTendered,
        change)

        CheckedListBox1.Items.Clear()
        UpdateOrderSummary()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim doc As New Document()

        Dim savePath As String =
            Path.Combine(Application.StartupPath, "Receipt.pdf")

        PdfWriter.GetInstance(doc,
                              New FileStream(savePath,
                                             FileMode.Create))

        doc.Open()

        doc.Add(New Paragraph("SCOOPIFY CREAMERY"))
        doc.Add(New Paragraph("-------------------------"))
        doc.Add(New Paragraph("OFFICIAL RECEIPT"))
        doc.Add(New Paragraph(""))
        doc.Add(New Paragraph("Date: " & Date.Now.ToString()))
        doc.Add(New Paragraph(""))
        doc.Add(New Paragraph("Cookies and Cream x1"))
        doc.Add(New Paragraph("Mango Sundae x2"))
        doc.Add(New Paragraph(""))
        doc.Add(New Paragraph("TOTAL: ₱275"))
        doc.Add(New Paragraph("PAYMENT METHOD: CASH"))

        doc.Close()

        MessageBox.Show("Receipt generated successfully!")
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