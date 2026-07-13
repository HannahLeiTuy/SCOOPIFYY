<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form6
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form6))
        TextBox1 = New TextBox()
        TextBox2 = New TextBox()
        Button1 = New Button()
        Button8 = New Button()
        Button7 = New Button()
        Button6 = New Button()
        Button5 = New Button()
        Button4 = New Button()
        Button3 = New Button()
        Button9 = New Button()
        Button10 = New Button()
        Button11 = New Button()
        SuspendLayout()
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(317, 469)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(238, 27)
        TextBox1.TabIndex = 0
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(326, 567)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(238, 27)
        TextBox2.TabIndex = 1
        ' 
        ' Button1
        ' 
        Button1.BackgroundImageLayout = ImageLayout.Stretch
        Button1.Image = CType(resources.GetObject("Button1.Image"), Image)
        Button1.Location = New Point(242, 671)
        Button1.Name = "Button1"
        Button1.Size = New Size(322, 76)
        Button1.TabIndex = 3
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button8
        ' 
        Button8.BackgroundImage = CType(resources.GetObject("Button8.BackgroundImage"), Image)
        Button8.BackgroundImageLayout = ImageLayout.Stretch
        Button8.Location = New Point(1489, 26)
        Button8.Name = "Button8"
        Button8.Size = New Size(66, 56)
        Button8.TabIndex = 21
        Button8.Text = "    "
        Button8.UseVisualStyleBackColor = True
        ' 
        ' Button7
        ' 
        Button7.Image = CType(resources.GetObject("Button7.Image"), Image)
        Button7.Location = New Point(1123, 36)
        Button7.Name = "Button7"
        Button7.Size = New Size(160, 34)
        Button7.TabIndex = 20
        Button7.Text = "    "
        Button7.UseVisualStyleBackColor = True
        ' 
        ' Button6
        ' 
        Button6.Image = CType(resources.GetObject("Button6.Image"), Image)
        Button6.Location = New Point(973, 36)
        Button6.Name = "Button6"
        Button6.Size = New Size(110, 34)
        Button6.TabIndex = 19
        Button6.Text = "    "
        Button6.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Image = CType(resources.GetObject("Button5.Image"), Image)
        Button5.Location = New Point(741, 36)
        Button5.Name = "Button5"
        Button5.Size = New Size(196, 34)
        Button5.TabIndex = 18
        Button5.Text = "    "
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Image = CType(resources.GetObject("Button4.Image"), Image)
        Button4.Location = New Point(613, 36)
        Button4.Name = "Button4"
        Button4.Size = New Size(94, 34)
        Button4.TabIndex = 17
        Button4.Text = "    "
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Image = CType(resources.GetObject("Button3.Image"), Image)
        Button3.Location = New Point(451, 36)
        Button3.Name = "Button3"
        Button3.Size = New Size(126, 34)
        Button3.TabIndex = 16
        Button3.Text = "    "
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button9
        ' 
        Button9.Image = CType(resources.GetObject("Button9.Image"), Image)
        Button9.Location = New Point(346, 36)
        Button9.Name = "Button9"
        Button9.Size = New Size(82, 34)
        Button9.TabIndex = 15
        Button9.Text = "    "
        Button9.UseVisualStyleBackColor = True
        ' 
        ' Button10
        ' 
        Button10.BackgroundImageLayout = ImageLayout.Stretch
        Button10.Image = CType(resources.GetObject("Button10.Image"), Image)
        Button10.Location = New Point(166, 167)
        Button10.Name = "Button10"
        Button10.Size = New Size(140, 42)
        Button10.TabIndex = 22
        Button10.UseVisualStyleBackColor = True
        ' 
        ' Button11
        ' 
        Button11.BackgroundImageLayout = ImageLayout.Stretch
        Button11.Image = CType(resources.GetObject("Button11.Image"), Image)
        Button11.Location = New Point(1323, 26)
        Button11.Name = "Button11"
        Button11.Size = New Size(135, 56)
        Button11.TabIndex = 23
        Button11.Text = "    "
        Button11.UseVisualStyleBackColor = True
        ' 
        ' Form6
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), Image)
        BackgroundImageLayout = ImageLayout.Stretch
        ClientSize = New Size(1594, 856)
        Controls.Add(Button11)
        Controls.Add(Button10)
        Controls.Add(Button8)
        Controls.Add(Button7)
        Controls.Add(Button6)
        Controls.Add(Button5)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button9)
        Controls.Add(Button1)
        Controls.Add(TextBox2)
        Controls.Add(TextBox1)
        Name = "Form6"
        Text = "Customer Sign Up"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button9 As Button
    Friend WithEvents Button10 As Button
    Friend WithEvents Button11 As Button
End Class
