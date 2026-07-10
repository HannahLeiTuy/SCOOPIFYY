<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form3))
        DataGridView1 = New DataGridView()
        Button1 = New Button()
        Button2 = New Button()
        report = New Button()
        TextBox1 = New TextBox()
        TextBox2 = New TextBox()
        TextBox3 = New TextBox()
        TextBox4 = New TextBox()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        ComboBox1 = New ComboBox()
        Button4 = New Button()
        Button3 = New Button()
        Button5 = New Button()
        Button6 = New Button()
        Button7 = New Button()
        Button8 = New Button()
        Button10 = New Button()
        ComboBox2 = New ComboBox()
        Button9 = New Button()
        Button12 = New Button()
        Button13 = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.BackgroundColor = SystemColors.ButtonFace
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(596, 528)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.Size = New Size(831, 289)
        DataGridView1.TabIndex = 0
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1384, 389)
        Button1.Name = "Button1"
        Button1.Size = New Size(94, 29)
        Button1.TabIndex = 1
        Button1.Text = "ADD"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(215, 555)
        Button2.Name = "Button2"
        Button2.Size = New Size(94, 29)
        Button2.TabIndex = 2
        Button2.Text = "UPDATE"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' report
        ' 
        report.Location = New Point(315, 723)
        report.Name = "report"
        report.Size = New Size(195, 29)
        report.TabIndex = 3
        report.Text = "Generate Inventory Report"
        report.UseVisualStyleBackColor = True
        ' 
        ' TextBox1
        ' 
        TextBox1.Font = New Font("Sitka Banner", 10.8F)
        TextBox1.Location = New Point(649, 388)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(125, 30)
        TextBox1.TabIndex = 4
        ' 
        ' TextBox2
        ' 
        TextBox2.Font = New Font("Sitka Banner", 10.8F)
        TextBox2.Location = New Point(780, 388)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(125, 30)
        TextBox2.TabIndex = 5
        ' 
        ' TextBox3
        ' 
        TextBox3.Font = New Font("Sitka Banner", 10.8F)
        TextBox3.Location = New Point(911, 388)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(125, 30)
        TextBox3.TabIndex = 6
        ' 
        ' TextBox4
        ' 
        TextBox4.Font = New Font("Sitka Banner", 10.8F)
        TextBox4.Location = New Point(1042, 388)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(125, 30)
        TextBox4.TabIndex = 7
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = SystemColors.ButtonHighlight
        Label2.Font = New Font("Rockwell", 18F)
        Label2.Location = New Point(632, 240)
        Label2.Name = "Label2"
        Label2.Size = New Size(108, 35)
        Label2.TabIndex = 9
        Label2.Text = "Label2"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.BackColor = SystemColors.ButtonHighlight
        Label3.Font = New Font("Rockwell", 18F)
        Label3.Location = New Point(878, 240)
        Label3.Name = "Label3"
        Label3.Size = New Size(108, 35)
        Label3.TabIndex = 10
        Label3.Text = "Label3"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.BackColor = SystemColors.ButtonHighlight
        Label4.Font = New Font("Rockwell", 18F)
        Label4.Location = New Point(1136, 240)
        Label4.Name = "Label4"
        Label4.Size = New Size(108, 35)
        Label4.TabIndex = 11
        Label4.Text = "Label4"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.BackColor = SystemColors.ButtonHighlight
        Label5.Font = New Font("Rockwell", 18F)
        Label5.Location = New Point(1384, 240)
        Label5.Name = "Label5"
        Label5.Size = New Size(108, 35)
        Label5.TabIndex = 12
        Label5.Text = "Label5"
        ' 
        ' ComboBox1
        ' 
        ComboBox1.FormattingEnabled = True
        ComboBox1.Items.AddRange(New Object() {"Ingredient", "Container", "Topping", "Syrup", "Supply"})
        ComboBox1.Location = New Point(226, 494)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(136, 28)
        ComboBox1.TabIndex = 13
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(315, 555)
        Button4.Name = "Button4"
        Button4.Size = New Size(94, 29)
        Button4.TabIndex = 14
        Button4.Text = "CLEAR"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(415, 555)
        Button3.Name = "Button3"
        Button3.Size = New Size(94, 29)
        Button3.TabIndex = 15
        Button3.Text = "DELETE"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(59, 555)
        Button5.Name = "Button5"
        Button5.Size = New Size(150, 28)
        Button5.TabIndex = 16
        Button5.Text = "REVENUE"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button6
        ' 
        Button6.Location = New Point(120, 655)
        Button6.Name = "Button6"
        Button6.Size = New Size(80, 24)
        Button6.TabIndex = 17
        Button6.Text = "Button6"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' Button7
        ' 
        Button7.Location = New Point(358, 653)
        Button7.Name = "Button7"
        Button7.Size = New Size(94, 29)
        Button7.TabIndex = 18
        Button7.Text = "Button7"
        Button7.UseVisualStyleBackColor = True
        ' 
        ' Button8
        ' 
        Button8.Location = New Point(96, 723)
        Button8.Name = "Button8"
        Button8.Size = New Size(94, 29)
        Button8.TabIndex = 19
        Button8.Text = "Button8"
        Button8.UseVisualStyleBackColor = True
        ' 
        ' Button10
        ' 
        Button10.Location = New Point(179, 613)
        Button10.Name = "Button10"
        Button10.Size = New Size(130, 26)
        Button10.TabIndex = 21
        Button10.Text = "flavor mood"
        Button10.UseVisualStyleBackColor = True
        ' 
        ' ComboBox2
        ' 
        ComboBox2.FormattingEnabled = True
        ComboBox2.Items.AddRange(New Object() {"Ingredient", "Container", "Topping", "Syrup", "Supply"})
        ComboBox2.Location = New Point(393, 494)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(151, 28)
        ComboBox2.TabIndex = 23
        ' 
        ' Button9
        ' 
        Button9.Location = New Point(59, 613)
        Button9.Name = "Button9"
        Button9.Size = New Size(98, 36)
        Button9.TabIndex = 24
        Button9.Text = "mystery"
        Button9.UseVisualStyleBackColor = True
        ' 
        ' Button12
        ' 
        Button12.Location = New Point(130, 778)
        Button12.Name = "Button12"
        Button12.Size = New Size(94, 29)
        Button12.TabIndex = 25
        Button12.Text = "Button12"
        Button12.UseVisualStyleBackColor = True
        ' 
        ' Button13
        ' 
        Button13.Location = New Point(343, 778)
        Button13.Name = "Button13"
        Button13.Size = New Size(94, 29)
        Button13.TabIndex = 26
        Button13.Text = "Button13"
        Button13.UseVisualStyleBackColor = True
        ' 
        ' Form3
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), Image)
        BackgroundImageLayout = ImageLayout.Stretch
        ClientSize = New Size(1599, 894)
        Controls.Add(Button13)
        Controls.Add(Button12)
        Controls.Add(Button9)
        Controls.Add(ComboBox2)
        Controls.Add(Button10)
        Controls.Add(Button8)
        Controls.Add(Button7)
        Controls.Add(Button6)
        Controls.Add(Button5)
        Controls.Add(Button3)
        Controls.Add(Button4)
        Controls.Add(ComboBox1)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(TextBox4)
        Controls.Add(TextBox3)
        Controls.Add(TextBox2)
        Controls.Add(TextBox1)
        Controls.Add(report)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(DataGridView1)
        Name = "Form3"
        Text = "Form3"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents report As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Button4 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents Button10 As Button
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Button9 As Button
    Friend WithEvents Button12 As Button
    Friend WithEvents Button13 As Button
End Class
