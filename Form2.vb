Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient
Public Class Form2
    Dim attempts As Integer = 0

    Dim connString As String =
    "server=localhost;database=Scoopify_Creamery;user=root;password=Hannah_lei07;"
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.UseSystemPasswordChar = True

        Timer1.Enabled = False
        Timer1.Interval = 5000

        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Primary Server")
        ComboBox1.Items.Add("Production Assistant")
        ComboBox1.Items.Add("Cashier")
        ComboBox1.Items.Add("Store Manager")

        ComboBox1.SelectedIndex = -1
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text.Trim = "" And TextBox2.Text.Trim = "" Then
            MessageBox.Show("Please enter your username and password.",
                        "Missing Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
            Exit Sub
        End If

        If TextBox1.Text.Trim = "" Then
            MessageBox.Show("Please enter your username.",
                        "Username Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
            TextBox1.Focus()
            Exit Sub
        End If

        If TextBox2.Text.Trim = "" Then
            MessageBox.Show("Please enter your password.",
                        "Password Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
            TextBox2.Focus()
            Exit Sub
        End If

        If TextBox2.Text.Length < 8 Then
            MessageBox.Show("Password must be at least 8 characters long.",
                        "Invalid Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
            TextBox2.Focus()
            Exit Sub
        End If

        If ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("Please select your role.",
                        "Role Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
            ComboBox1.Focus()
            Exit Sub
        End If

        Try
            Using conn As New MySqlConnection(connString)
                conn.Open()

                Dim query As String =
            "SELECT * FROM employees " &
            "WHERE username=@username " &
            "AND password=@password " &
            "AND role=@role"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", TextBox1.Text.Trim())
                    cmd.Parameters.AddWithValue("@password", TextBox2.Text.Trim())
                    cmd.Parameters.AddWithValue("@role", ComboBox1.Text)

                    Dim reader As MySqlDataReader = cmd.ExecuteReader()

                    If reader.Read() Then
                        attempts = 0

                        Dim firstName As String = reader("fname").ToString()

                        MessageBox.Show(
                    "Welcome back, " & firstName & "!" & vbCrLf &
                    "Login Successful!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)

                        Form3.Show()
                        Me.Hide()

                    Else
                        attempts += 1

                        MessageBox.Show(
                    "Invalid username, password, or role." &
                    vbCrLf &
                    "Attempt " & attempts & " of 3.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show(
        "Database Connection Error:" &
        vbCrLf &
        ex.Message,
        "Database Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error)
        End Try

        If attempts >= 3 Then
            MessageBox.Show("Too many failed login attempts." & vbCrLf &
                        "Please wait 5 seconds before trying again.",
                        "Account Temporarily Locked",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)

            TextBox1.Enabled = False
            TextBox2.Enabled = False
            ComboBox1.Enabled = False
            Button1.Enabled = False

            Timer1.Start()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form10.Show()
        Me.Hide()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        attempts = 0
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        ComboBox1.Enabled = True
        Button1.Enabled = True

        TextBox1.Clear()
        TextBox2.Clear()
        ComboBox1.SelectedIndex = -1

        MessageBox.Show("You may now try logging in again.",
                        "Login Unlocked",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)

        TextBox1.Focus()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form1.Show()
        Me.Hide()

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form11.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form6.Show()
        Me.Hide()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        TextBox2.UseSystemPasswordChar = Not TextBox2.UseSystemPasswordChar

    End Sub
End Class



