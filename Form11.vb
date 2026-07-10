Public Class Form11

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If CustomerLoggedIn = False Then
            MessageBox.Show(
                "🛎️ Good day!" & vbCrLf & vbCrLf &
                "Please log in first before accessing our services.",
                "Login Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Exit Sub
        End If

        Me.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form4.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If CustomerLoggedIn = False Then
            MessageBox.Show(
                "🍦 Good day!" & vbCrLf & vbCrLf &
                "You need to log in first before placing an order.",
                "Login Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Exit Sub
        End If

        Form4.Show()
        Me.Hide()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If CustomerLoggedIn = False Then
            MessageBox.Show(
                "🎁 Good day!" & vbCrLf & vbCrLf &
                "Please log in first to access our exclusive one-time offer.",
                "Login Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Exit Sub
        End If

        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If CustomerLoggedIn = False Then
            MessageBox.Show(
                "🛒 Good day!" & vbCrLf & vbCrLf &
                "No orders yet!" & vbCrLf &
                "Please log in first to start adding delicious treats to your cart. 🍦💕",
                "Cart Empty",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Exit Sub
        End If

        Form12.Show()
        Me.Hide()
    End Sub

End Class