Imports MySql.Data.MySqlClient
Public Class Form6
    Dim connString As String =
    "server=localhost;database=Scoopify_Creamery;user=root;password=BugfixMaster#22;"

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

        Try
            Using conn As New MySqlConnection(connString)
                conn.Open()

                Dim query As String =
            "INSERT INTO customers(fname, lname) VALUES(@fname,@lname)"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@fname", TextBox1.Text.Trim())
                    cmd.Parameters.AddWithValue("@lname", TextBox2.Text.Trim())

                    cmd.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Database Error: " & ex.Message)
            Exit Sub
        End Try

        CustomerFirstName = TextBox1.Text.Trim()
        CustomerLastName = TextBox2.Text.Trim()

        CustomerInfoCompleted = True
        CustomerLoggedIn = True

        MessageBox.Show(
    "🎉 INFORMATION SAVED SUCCESSFULLY!" &
    vbCrLf & vbCrLf &
    "Welcome to Scoopify, " &
    CustomerFirstName & "!" &
    vbCrLf &
    "We're excited to serve you delicious ice cream today! 🍦",
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

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click

    End Sub
End Class
