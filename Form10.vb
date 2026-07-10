Public Class Form10

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form1.Show()
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
        Form9.Show()
        Me.Show()
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

        Form11.Show()
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

        Form12.RefreshCart()
        Form12.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CustomerLoggedIn = False Then
            MessageBox.Show(
                "🍨 Welcome to Scoopify!" & vbCrLf & vbCrLf &
                "Please log in first to enjoy our Build-Your-Own-Dessert experience!",
                "Login Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Exit Sub
        End If

        MessageBox.Show(
            "🍦 Welcome, " & CustomerFirstName & "!" & vbCrLf & vbCrLf &
            "Create your dream dessert with our Build-Your-Own-Dessert feature and make it uniquely yours!",
            "Build Your Own Dessert",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        Form4.Show()
        Me.Hide()
    End Sub

End Class