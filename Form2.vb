Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Public Class Form2
    Dim attempts As Integer = 0
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
        If ComboBox1.SelectedIndex = -1 Then
            MessageBox.Show("Please select your role.",
                            "Role Required",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            ComboBox1.Focus()
            Exit Sub
        End If

        If TextBox1.Text = "danhuertas" Then
            If ComboBox1.Text <> "Primary Server" Then
                MessageBox.Show("Incorrect role selected.",
                                "Access Denied",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Exit Sub
            End If
            If TextBox2.Text = "dan123" Then
                attempts = 0
                MessageBox.Show("Welcome back, Dan!" & vbCrLf &
                                "Login Successful!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                Form3.Show()
                Me.Hide()
            Else
                attempts += 1
                MessageBox.Show("Incorrect password." & vbCrLf &
                                "Attempt " & attempts & " of 3.",
                                "Login Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
            End If

        ElseIf TextBox1.Text = "andreasocito" Then
            If ComboBox1.Text <> "Production Assistant" Then
                MessageBox.Show("Incorrect role selected.",
                                "Access Denied",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Exit Sub
            End If
            If TextBox2.Text = "andrea123" Then
                attempts = 0
                MessageBox.Show("Welcome back, Andrea!" & vbCrLf &
                                "Login Successful!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                Form3.Show()
                Me.Hide()
            Else
                attempts += 1
                MessageBox.Show("Incorrect password." & vbCrLf &
                                "Attempt " & attempts & " of 3.",
                                "Login Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
            End If

        ElseIf TextBox1.Text = "clowiegermino" Then
            If ComboBox1.Text <> "Cashier" Then
                MessageBox.Show("Incorrect role selected.",
                                "Access Denied",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Exit Sub
            End If
            If TextBox2.Text = "clowie123" Then
                attempts = 0
                MessageBox.Show("Welcome back, Clowie!" & vbCrLf &
                                "Login Successful!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                Form3.Show()
                Me.Hide()
            Else
                attempts += 1
                MessageBox.Show("Incorrect password." & vbCrLf &
                                "Attempt " & attempts & " of 3.",
                                "Login Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
            End If

        ElseIf TextBox1.Text = "pearlgirao" Then
            If ComboBox1.Text <> "Store Manager0" Then
                MessageBox.Show("Incorrect role selected.",
                                "Access Denied",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Exit Sub
            End If
            If TextBox2.Text = "pearl123" Then
                attempts = 0
                MessageBox.Show("Welcome back, Pearl!" & vbCrLf &
                                "Login Successful!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                Form3.Show()
                Me.Hide()
            Else
                attempts += 1
                MessageBox.Show("Incorrect password." & vbCrLf &
                                "Attempt " & attempts & " of 3.",
                                "Login Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
            End If
        Else
            attempts += 1
            MessageBox.Show("The username you entered does not exist." & vbCrLf &
                            "Attempt " & attempts & " of 3.",
                            "Login Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End If

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
        TextBox2.UseSystemPasswordChar = Not TextBox2.UseSystemPasswordChar
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
End Class


