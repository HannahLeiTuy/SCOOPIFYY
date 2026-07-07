Public Class Form9
    Private currentName As String = ""
    Private currentPrice As Decimal = 0

    Private dailyFlavor() As String = {
        "The Choco-Mint Extreme",
        "Rich chocolate with mint chips.",
        "95.00"
    }

    Private weeklyFlavor() As String = {
        "The Birthday Explosion",
        "Cake ice cream with sprinkles.",
        "90.00"
    }

    Private Sub Form9_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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
            MessageBox.Show("Reveal a flavor first!",
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

            ListBox1.Items.Clear()

            ListBox1.Items.Add("🎉 Added!")
            ListBox1.Items.Add("")
            ListBox1.Items.Add("🍦 " & currentName)
            ListBox1.Items.Add("💰 ₱" & currentPrice.ToString("0.00"))
            ListBox1.Items.Add("")
            ListBox1.Items.Add("Thanks for choosing")
            ListBox1.Items.Add("✨ Scoopify! 🍨")

            Button3.Enabled = False
            currentName = ""
            currentPrice = 0

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
        Form2.Show
        Hide
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
        Form12.Show()
        Me.Hide()
    End Sub
End Class