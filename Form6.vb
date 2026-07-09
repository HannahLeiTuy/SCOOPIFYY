Public Class Form6
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text.Trim = "" Then
            MessageBox.Show("🌸 Please enter your First Name.",
                            "Missing Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            TextBox1.Focus()
            Exit Sub
        End If

        If TextBox2.Text.Trim = "" Then
            MessageBox.Show("🌸 Please enter your Last Name.",
                            "Missing Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            TextBox2.Focus()
            Exit Sub
        End If

        CustomerFirstName = TextBox1.Text.Trim
        CustomerLastName = TextBox2.Text.Trim
        CustomerInfoCompleted = True

        MessageBox.Show(
            "🎉 INFORMATION SAVED SUCCESSFULLY!" &
            vbCrLf & vbCrLf &
            "Welcome to Scoopify, " &
            CustomerFirstName & "!" &
            vbCrLf &
            "We're excited to serve you delicious ice cream today! 🍦" &
            vbCrLf & vbCrLf &
            "Enjoy browsing our menu and have a sweet experience!",
            "Welcome to Scoopify!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)
        Form4.Show()
        Me.Hide()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If CustomerInfoCompleted = False Then

            MessageBox.Show("Please fill in your information first before accessing this feature.",
                            "Information Required",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Exit Sub
        End If
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If CustomerInfoCompleted = False Then

            MessageBox.Show("Please fill in your information first before accessing this feature.",
                            "Information Required",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)

            Exit Sub
        End If
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form11.Show()
        Me.Hide()
    End Sub
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button8.Click

    End Sub
    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub Button9_Click_1(sender As Object, e As EventArgs) Handles Button9.Click
        Form1.Show()
        Me.Hide()
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If CustomerInfoCompleted = False Then

            MessageBox.Show("Please fill in your information first before accessing this feature.",
                            "Information Required",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            Exit Sub
        End If
    End Sub
End Class


