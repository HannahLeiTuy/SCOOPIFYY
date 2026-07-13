Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient

Public Class Form8

    Dim connString As String =
    "server=localhost;database=Scoopify_Creamery;user=root;password=BugfixMaster#22;"

    Dim QuestionNo As Integer = 0
    Dim Tropical As Integer = 0
    Dim Caramel As Integer = 0
    Dim Cookies As Integer = 0

    Dim SelectedFlavor As String = ""
    Dim SelectedContainer As String = ""
    Dim SelectedTopping As String = ""
    Dim SelectedSyrup As String = ""

    Dim DisplayName As String = ""

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen

        Button1.Enabled = False

        RichTextBox1.ReadOnly = True
        RichTextBox1.Font =
            New Drawing.Font(
                "Georgia",
                10,
                Drawing.FontStyle.Regular)

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

            Case 1

                RichTextBox1.Text =
"🍦 DESSERT PERSONALITY QUIZ

Question 2

Your friends describe you as...

A. Energetic and adventurous
B. Sweet and caring
C. Funny and playful
D. Calm and thoughtful"

            Case 2

                RichTextBox1.Text =
"🍦 DESSERT PERSONALITY QUIZ

Question 3

If you could receive one surprise gift today, what would you choose?

A. A travel adventure
B. A box of delicious chocolates
C. Tickets to an exciting concert
D. A relaxing spa day"

        End Select

        ComboBox1.Items.Add("A")
        ComboBox1.Items.Add("B")
        ComboBox1.Items.Add("C")
        ComboBox1.Items.Add("D")

    End Sub

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
            "Your recommended dessert is:" &
            vbCrLf & vbCrLf &
            DisplayName,
            "Scoopify Recommendation",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        CartManager.AddItem(
            GetProductID(SelectedFlavor),
            SelectedFlavor,
            GetPrice(SelectedFlavor))

        CartManager.AddItem(
            GetProductID(SelectedContainer),
            SelectedContainer,
            GetPrice(SelectedContainer))

        CartManager.AddItem(
            GetProductID(SelectedTopping),
            SelectedTopping,
            GetPrice(SelectedTopping))

        CartManager.AddItem(
            GetProductID(SelectedSyrup),
            SelectedSyrup,
            GetPrice(SelectedSyrup))

        MessageBox.Show(
            "🛒 Added to cart! Go to your cart to check out.",
            "Order Successful",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

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
        Form12.RefreshCart()
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

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form7.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form12.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form5.Show()
        Me.Hide()
    End Sub
End Class