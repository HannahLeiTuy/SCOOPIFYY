Imports System.Windows
Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form6.Show()
        Me.Hide()
    End Sub


    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form6.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form11.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
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

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
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

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub
End Class