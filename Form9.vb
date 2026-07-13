Public Class Form9

    Private currentName As String = ""
    Private currentPrice As Decimal = 0
    Private currentProductID As Integer = 0

    Private rnd As New Random()

    Private mysteryFlavors(,) As String = {
        {"Galaxy Berry Blast", "Mixed berry ice cream with popping candy and galaxy sprinkles.", "99.00", "41"},
        {"Caramel Volcano Crunch", "Salted caramel ice cream with cookie crunch and caramel drizzle.", "95.00", "42"},
        {"Mango Paradise Swirl", "Mango ice cream with tropical fruit bits and mango syrup.", "92.00", "43"},
        {"Cookies Overload Supreme", "Cookies & cream ice cream loaded with Oreos and hot fudge.", "98.00", "44"}
    }

    Private dailyFlavor(3) As String
    Private weeklyFlavor(3) As String

    Private Sub ShuffleMysteryFlavors()

        Dim first As Integer = rnd.Next(0, 4)
        Dim second As Integer

        Do
            second = rnd.Next(0, 4)
        Loop While second = first

        For i As Integer = 0 To 3
            dailyFlavor(i) = mysteryFlavors(first, i)
            weeklyFlavor(i) = mysteryFlavors(second, i)
        Next

    End Sub

    Private Sub Form9_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen
        ShuffleMysteryFlavors()

        ListBox1.Font = New Font("Georgia", 11, FontStyle.Italic)
        ListBox1.HorizontalScrollbar = True

        Button3.Enabled = False

        ListBox1.Items.Clear()
        ListBox1.Items.Add("🍦 Welcome!")
        ListBox1.Items.Add("")
        ListBox1.Items.Add("Pick a mystery")
        ListBox1.Items.Add("flavor below ✨")

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ShowFlavor("🍓 Daily Flavor", dailyFlavor)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ShowFlavor("🎉 Weekly Flavor", weeklyFlavor)
    End Sub

    Private Sub ShowFlavor(title As String, flavor() As String)

        currentName = flavor(0)
        currentPrice = CDec(flavor(2))
        currentProductID = CInt(flavor(3))

        ListBox1.Items.Clear()

        ListBox1.Items.Add(title)
        ListBox1.Items.Add("────────────────")
        ListBox1.Items.Add("🍨 " & currentName)
        ListBox1.Items.Add("")
        ListBox1.Items.Add("💖 " & flavor(1))
        ListBox1.Items.Add("")
        ListBox1.Items.Add("💰 ₱" & currentPrice.ToString("0.00"))
        ListBox1.Items.Add("")
        ListBox1.Items.Add("✨ Limited Treat!")

        Button3.Enabled = True

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If currentName = "" Then
            MessageBox.Show(
                "Reveal a flavor first!",
                "Scoopify",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim answer As DialogResult

        answer = MessageBox.Show(
            "Add '" & currentName & "' to your order?" & vbCrLf &
            "Price: ₱" & currentPrice.ToString("0.00"),
            "Confirm Order",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If answer = DialogResult.Yes Then

            CartManager.AddItem(
                currentProductID,
                currentName,
                currentPrice)

            ListBox1.Items.Clear()

            ListBox1.Items.Add("🎉 Added!")
            ListBox1.Items.Add("")
            ListBox1.Items.Add("🍦 " & currentName)
            ListBox1.Items.Add("💰 ₱" & currentPrice.ToString("0.00"))
            ListBox1.Items.Add("")
            ListBox1.Items.Add("Added to Cart Successfully!")
            ListBox1.Items.Add("✨ Scoopify! 🍨")

            Button3.Enabled = False

            currentName = ""
            currentPrice = 0
            currentProductID = 0

        End If

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form4.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form5.Show()
        Me.Hide()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form11.Show()
        Me.Hide()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form12.RefreshCart()
        Form12.Show()
        Me.Hide()
    End Sub

End Class