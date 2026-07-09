Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Diagnostics

Public Class Form8
    Dim connString As String = "server=localhost;database=Scoopify_Creamery;user=root;password=BugfixMaster#22;"
    Dim QuestionNo As Integer = 0
    Dim Tropical As Integer = 0
    Dim Caramel As Integer = 0
    Dim Cookies As Integer = 0
    Dim SelectedFlavor As String = ""
    Dim SelectedContainer As String = ""
    Dim SelectedTopping As String = ""
    Dim SelectedSyrup As String = ""
    Dim DisplayName As String = ""

    Private Sub GenerateQuizPDF()

        Dim total As Decimal = GetQuizTotal()

        Dim savePath As String =
        Application.StartupPath & "\DessertPersonalityReport.pdf"

        Dim doc As New iTextSharp.text.Document(
        iTextSharp.text.PageSize.A4,
        40,
        40,
        50,
        50)

        Dim writer As iTextSharp.text.pdf.PdfWriter =
        iTextSharp.text.pdf.PdfWriter.GetInstance(
            doc,
            New System.IO.FileStream(
                savePath,
                System.IO.FileMode.Create))

        doc.Open()

        Dim titleFont As New iTextSharp.text.Font(
        iTextSharp.text.Font.FontFamily.HELVETICA,
        24,
        iTextSharp.text.Font.BOLD)

        Dim headerFont As New iTextSharp.text.Font(
        iTextSharp.text.Font.FontFamily.HELVETICA,
        16,
        iTextSharp.text.Font.BOLD)

        Dim normalFont As New iTextSharp.text.Font(
        iTextSharp.text.Font.FontFamily.HELVETICA,
        12,
        iTextSharp.text.Font.NORMAL)

        Dim title As New iTextSharp.text.Paragraph(
        "SCOOPIFY CREAMERY" &
        vbCrLf &
        "Dessert Personality Quiz Report",
        titleFont)

        title.Alignment = iTextSharp.text.Element.ALIGN_CENTER

        doc.Add(title)

        doc.Add(New iTextSharp.text.Paragraph(" "))
        doc.Add(New iTextSharp.text.Paragraph(
        "Date Generated: " &
        DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt"),
        normalFont))

        doc.Add(New iTextSharp.text.Paragraph(" "))

        doc.Add(New iTextSharp.text.Paragraph(
        "Recommended Dessert",
        headerFont))

        doc.Add(New iTextSharp.text.Paragraph(
        DisplayName,
        normalFont))

        doc.Add(New iTextSharp.text.Paragraph(" "))

        Dim table As New iTextSharp.text.pdf.PdfPTable(2)

        table.WidthPercentage = 100

        table.AddCell("Flavor")
        table.AddCell(SelectedFlavor)

        table.AddCell("Container")
        table.AddCell(SelectedContainer)

        table.AddCell("Topping")
        table.AddCell(SelectedTopping)

        table.AddCell("Syrup")
        table.AddCell(SelectedSyrup)

        table.AddCell("Total Amount")
        table.AddCell("₱" & total.ToString("0.00"))

        doc.Add(table)

        doc.Add(New iTextSharp.text.Paragraph(" "))
        doc.Add(New iTextSharp.text.Paragraph(
        "══════════════════════════════",
        normalFont))

        doc.Add(New iTextSharp.text.Paragraph(
        "Thank you for choosing Scoopify Creamery!",
        headerFont))

        doc.Add(New iTextSharp.text.Paragraph(
        "Your personalized dessert recommendation was generated based on your personality quiz answers.",
        normalFont))

        doc.Add(New iTextSharp.text.Paragraph(" "))
        doc.Add(New iTextSharp.text.Paragraph(
        "Enjoy your Scoopify experience!",
        normalFont))

        doc.Close()

        Process.Start(New ProcessStartInfo(savePath) With {
    .UseShellExecute = True
})

    End Sub



    Private Function GetQuizTotal() As Decimal

        Dim total As Decimal = 0

        total += GetPrice(SelectedFlavor)
        total += GetPrice(SelectedContainer)
        total += GetPrice(SelectedTopping)
        total += GetPrice(SelectedSyrup)

        Return total

    End Function
    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button1.Enabled = False
        RichTextBox1.ReadOnly = True
        RichTextBox1.Font = New Drawing.Font("Georgia", 10, Drawing.FontStyle.Regular)
        LoadQuestion()
    End Sub
    Private Sub LoadQuestion()
        ComboBox1.Items.Clear()
        ComboBox1.Text = ""
        Select Case QuestionNo
            Case 0
                RichTextBox1.Text =
        "🍦 DESSERT PERSONALITY QUIZ

        Question 1

        Imagine you're planning your dream vacation. Which place would you choose?

        A. A tropical island with crystal-clear beaches
        B. A cozy mountain cabin with hot chocolate
        C. A lively amusement park full of fun
        D. A peaceful café while reading your favorite book"

                ComboBox1.Items.Add("A")
                ComboBox1.Items.Add("B")
                ComboBox1.Items.Add("C")
                ComboBox1.Items.Add("D")

            Case 1

                RichTextBox1.Text =
        "🍦 DESSERT PERSONALITY QUIZ

        Question 2

        Your friends describe you as...

        A. Energetic and adventurous
        B. Sweet and caring
        C. Funny and playful
        D. Calm and thoughtful"

                ComboBox1.Items.Add("A")
                ComboBox1.Items.Add("B")
                ComboBox1.Items.Add("C")
                ComboBox1.Items.Add("D")

            Case 2

                RichTextBox1.Text =
"🍦 DESSERT PERSONALITY QUIZ

Question 3

If you could receive one surprise gift today, what would you choose?

A. A travel adventure
B. A box of delicious chocolates
C. Tickets to an exciting concert
D. A relaxing spa day"

                ComboBox1.Items.Add("A")
                ComboBox1.Items.Add("B")
                ComboBox1.Items.Add("C")
                ComboBox1.Items.Add("D")

        End Select

    End Sub
    Private Function GetProductID(itemName As String) As Integer
        Dim id As Integer = 0
        Using conn As New MySqlConnection(connString)
            conn.Open()
            Dim sql As String = "SELECT product_id
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
    Private Sub SaveItem(conn As MySqlConnection,
                     transactionID As Integer,
                     itemName As String)

        Dim productID As Integer =
        GetProductID(itemName)

        If productID = 0 Then Exit Sub

        Dim price As Decimal = GetPrice(itemName)

        Dim sql As String = "INSERT INTO transaction_details
        (transaction_id,product_id,quantity,unit_price, subtotal)
         VALUES
         (@tid,@pid,1,@price,@price)"

        Dim cmd As New MySqlCommand(sql, conn)

        cmd.Parameters.AddWithValue("@tid", transactionID)
        cmd.Parameters.AddWithValue("@pid", productID)
        cmd.Parameters.AddWithValue("@price", price)

        cmd.ExecuteNonQuery()

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
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Select Case ComboBox1.Text
            Case "A"
                Tropical += 2
            Case "B"
                Caramel += 2
            Case "C"
                Cookies += 2
            Case "D"
                Caramel += 1
                Cookies += 1
        End Select

        QuestionNo += 1

        If QuestionNo < 3 Then
            LoadQuestion()
        Else
            ShowResult()
        End If
    End Sub
    Private Sub ShowResult()
        If Tropical >= Caramel And Tropical >= Cookies Then
            DisplayName = "🥭 Tropical Mango"
            SelectedFlavor = "Mango Sorbet"
            SelectedContainer = "Waffle Bowl"
            SelectedTopping = "Rainbow Sprinkles"
            SelectedSyrup = "Strawberry Puree"

        ElseIf Caramel >= Tropical And Caramel >= Cookies Then
            DisplayName = "🍯 Caramel Delight"
            SelectedFlavor = "Salted Caramel"
            SelectedContainer = "Paper Cup"
            SelectedTopping = "Whipped Cream"
            SelectedSyrup = "Warm Caramel"
        Else
            DisplayName = "🍪 Cookies & Cream"
            SelectedFlavor = "Cookies & Cream"
            SelectedContainer = "Waffle Cone"
            SelectedTopping = "Crushed Oreos"
            SelectedSyrup = "Hot Fudge"

        End If
        RichTextBox1.Text =
        "🎉 YOUR QUIZ IS COMPLETE!
           Congratulations!
        Based on your choices, your perfect Scoopify flavor is:
        " & DisplayName & "
        This flavor matches your personality the best!
        Click the 'Add to Order' button below to add your recommended ice cream to your order. 🍨"
        Button1.Enabled = True
        MessageBox.Show(
        "🍦══════════════════════════════🍦" & vbCrLf &
        "        DESSERT PERSONALITY RESULT" & vbCrLf &
        "🍦══════════════════════════════🍦" & vbCrLf & vbCrLf &
        "Congratulations!" & vbCrLf & vbCrLf &
        "Your recommended dessert is:" & vbCrLf &
        DisplayName & vbCrLf & vbCrLf &
        "We think this dessert perfectly matches your personality!" & vbCrLf &
        "Enjoy your Scoopify experience! 💜",
        "Scoopify Recommendation",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        SaveQuizRecommendation()

        Dim total As Decimal = GetQuizTotal()

        Dim orderText As String =
    DisplayName &
    " | Flavor: " & SelectedFlavor &
    " | Container: " & SelectedContainer &
    " | Topping: " & SelectedTopping &
    " | Syrup: " & SelectedSyrup &
    " | Total: ₱" & total.ToString("0.00")

        Form12.CheckedListBox1.Items.Add(orderText)

        Form12.UpdateOrderSummary()

        MessageBox.Show(
    "🛒 ADDED TO ORDER!" & vbCrLf &
    "Recommendation: " & DisplayName & vbCrLf & vbCrLf &
    "Flavor: " & SelectedFlavor & vbCrLf &
    "Container: " & SelectedContainer & vbCrLf &
    "Topping: " & SelectedTopping & vbCrLf &
    "Syrup: " & SelectedSyrup & vbCrLf & vbCrLf &
    "Thank you for choosing Scoopify!",
    "Order Successful",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information)

    End Sub

    Private Sub SaveQuizRecommendation()
        Using conn As New MySqlConnection(connString)
            conn.Open()
            Dim total As Decimal = 0
            total += GetPrice(SelectedFlavor)
            total += GetPrice(SelectedContainer)
            total += GetPrice(SelectedTopping)
            total += GetPrice(SelectedSyrup)

            Dim sql As String = "INSERT INTO transactions (customer_id, employee_id, total_amount, payment_method)
            VALUES (1,3,@total,'Cash')"

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@total", total)
            cmd.ExecuteNonQuery()
            Dim transactionID As Integer =
            cmd.LastInsertedId

            SaveItem(conn, transactionID, SelectedFlavor)
            SaveItem(conn, transactionID, SelectedContainer)
            SaveItem(conn, transactionID, SelectedTopping)
            SaveItem(conn, transactionID, SelectedSyrup)
        End Using

    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form4.Show()
        Me.Hide()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form9.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form5.Show()
        Me.Hide()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form12.Show()
        Me.Hide()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form11.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click

        If DisplayName = "" Then
            MessageBox.Show(
            "Please complete the quiz first before generating a report.",
            "Quiz Not Completed",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)

            Exit Sub
        End If

        GenerateQuizPDF()

        MessageBox.Show(
        "PDF report generated successfully!",
        "Report Generated",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information)

    End Sub
End Class


