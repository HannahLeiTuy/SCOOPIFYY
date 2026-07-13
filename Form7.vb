Imports System.Reflection.Emit
Imports MySql.Data.MySqlClient
Public Class Form7
    Private currentMood As String = ""
    Private orderTotal As Double = 0
    Dim connString As String =
    "server=localhost;database=Scoopify_Creamery;user=root;password=BugfixMaster#22;"

    Private currentFlavors As New List(Of MoodFlavor)
    Public Class MoodFlavor
        Public ProductID As Integer
        Public ItemName As String
        Public Description As String
        Public Price As Decimal
    End Class
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListBox1.Visible = True
        ListBox2.Visible = True
        currentMood = "Happy"
        ShowFlavors()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListBox1.Visible = True
        ListBox2.Visible = True
        currentMood = "Sad"
        ShowFlavors()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListBox1.Visible = True
        ListBox2.Visible = True
        currentMood = "Tired"
        ShowFlavors()
    End Sub
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If currentMood <> "" Then ShowFlavors()
    End Sub
    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If currentMood <> "" Then ShowFlavors()
    End Sub
    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If currentMood <> "" Then ShowFlavors()
    End Sub
    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If currentMood <> "" Then ShowFlavors()
    End Sub
    Private Sub ShowFlavors()
        ListBox1.Items.Clear()
        currentFlavors.Clear()

        Dim hasDairy As Boolean = CheckBox1.Checked
        Dim hasNuts As Boolean = CheckBox2.Checked
        Dim hasGluten As Boolean = CheckBox3.Checked
        Dim hasEgg As Boolean = CheckBox4.Checked

        Select Case currentMood
            Case "Happy"
                ListBox1.Items.Add("🌟 FRUITY SUNSHINE BLEND")
                ListBox1.Items.Add("─────────────────────────────────────────────")
                ListBox1.Items.Add("Bright, vibrant, full of life — just like you right now!")
                ListBox1.Items.Add("Your scoop is a burst of tropical fruit.")
                ListBox1.Items.Add("")
                ListBox1.Items.Add("🍦 Available Flavors:")

                ListBox1.Items.Add("   " & SafeTag(False, False, False, False) & "  Mango Sorbet - ₱65")
                currentFlavors.Add(New MoodFlavor With {.ProductID = 1, .ItemName = "Mango Sorbet", .Price = 65})
                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, hasGluten, hasEgg) & "  Strawberry - ₱85" & AllergenList(hasDairy, False, hasGluten, hasEgg))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 2, .ItemName = "Strawberry", .Price = 85})
                ListBox1.Items.Add("   " & SafeTag(False, False, False, False) & "  Passion Fruit Citrus Swirl - ₱70")
                currentFlavors.Add(New MoodFlavor With {.ProductID = 3, .ItemName = "Passion Fruit Citrus Swirl", .Price = 70})
                ListBox1.Items.Add("")
                ListBox1.Items.Add("💡 Best paired with: waffle cone or fresh fruit cup")
                ListBox1.Items.Add("")
                AppendAllergyNote()

            Case "Sad"
                ListBox1.Items.Add("🍫 DEEP DARK CHOCOLATE")
                ListBox1.Items.Add("─────────────────────────────────────────────")
                ListBox1.Items.Add("Rich, velvety, deeply satisfying.")
                ListBox1.Items.Add("Chocolate understands you on a soul level.")
                ListBox1.Items.Add("")
                ListBox1.Items.Add("🍦 Available Flavors:")

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, False, hasEgg) &
                   "  Belgian Dark Chocolate - ₱60" &
                   AllergenList(hasDairy, False, False, hasEgg))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 4, .ItemName = "Belgian Dark Chocolate", .Price = 60})

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, hasGluten, hasEgg) &
                   "  Triple Fudge - ₱70" &
                   AllergenList(hasDairy, False, hasGluten, hasEgg))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 5, .ItemName = "Triple Fudge", .Price = 70})

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, hasGluten, hasEgg) &
                   "  Rich Chocolate - ₱55" &
                   AllergenList(hasDairy, False, hasGluten, hasEgg))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 6, .ItemName = "Rich Chocolate", .Price = 55})

                ListBox1.Items.Add("   " & SafeTag(hasDairy, hasNuts, hasGluten, hasEgg) &
                   "  Brownie Batter - ₱75" &
                   AllergenList(hasDairy, hasNuts, hasGluten, hasEgg))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 7, .ItemName = "Brownie Batter", .Price = 75})

                ListBox1.Items.Add("")
                ListBox1.Items.Add("💡 Best paired with: hot fudge drizzle or extra sprinkles")
                ListBox1.Items.Add("")
                AppendAllergyNote()

            Case "Tired"
                ListBox1.Items.Add("☕ COFFEE REVIVAL")
                ListBox1.Items.Add("─────────────────────────────────────────────")
                ListBox1.Items.Add("A gentle kick of caffeine wrapped in creamy goodness.")
                ListBox1.Items.Add("Your afternoon pick-me-up, dessert edition.")
                ListBox1.Items.Add("")
                ListBox1.Items.Add("🍦 Available Flavors:")

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, False, False) &
                   "  Espresso Chip - ₱80" &
                   AllergenList(hasDairy, False, False, False))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 8, .ItemName = "Espresso Chip", .Price = 80})

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, False, False) &
                   "  Vietnamese Coffee - ₱70" &
                   AllergenList(hasDairy, False, False, False))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 9, .ItemName = "Vietnamese Coffee", .Price = 70})

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, hasGluten, hasEgg) &
                   "  Tiramisu - ₱90" &
                   AllergenList(hasDairy, False, hasGluten, hasEgg))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 10, .ItemName = "Tiramisu", .Price = 90})

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, False, False) &
                   "  Cold Brew - ₱70" &
                   AllergenList(hasDairy, False, False, False))
                currentFlavors.Add(New MoodFlavor With {.ProductID = 11, .ItemName = "Cold Brew", .Price = 70})

                ListBox1.Items.Add("")
                ListBox1.Items.Add("💡 Best paired with: oat milk whip or biscotti crumble")
                ListBox1.Items.Add("")
                AppendAllergyNote()
        End Select
    End Sub

    Private Sub LoadFlavorsFromDB(mood As String, hasDairy As Boolean, hasNuts As Boolean, hasGluten As Boolean, hasEgg As Boolean)
        Using conn As New MySqlConnection(connString)
            conn.Open()
            Dim sql As String =
            "SELECT fm.product_id, p.item_name, fm.price
             FROM flavor_mood fm
             JOIN products p ON fm.product_id = p.product_id
             WHERE fm.mood_name = @mood"

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@mood", mood)

            Dim reader = cmd.ExecuteReader()
            While reader.Read()
                Dim f As New MoodFlavor With {
                .ProductID = Convert.ToInt32(reader("product_id")),
                .ItemName = reader("item_name").ToString().Trim(),
                .Price = Convert.ToDecimal(reader("price"))
            }
                currentFlavors.Add(f)

                Dim tag As String = SafeTag(hasDairy, hasNuts, hasGluten, hasEgg)
                ListBox1.Items.Add("   " & tag & "  " & f.ItemName & " - ₱" & f.Price.ToString("0.00") &
                AllergenList(hasDairy, hasNuts, hasGluten, hasEgg))
            End While
            reader.Close()
        End Using
    End Sub

    Private Function SafeTag(dairy As Boolean, nuts As Boolean, gluten As Boolean, egg As Boolean) As String
        If dairy OrElse nuts OrElse gluten OrElse egg Then
            Return "⚠️"
        Else
            Return "✅"
        End If
    End Function
    Private Function AllergenList(dairy As Boolean, nuts As Boolean, gluten As Boolean, egg As Boolean) As String
        Dim found As New List(Of String)
        If dairy Then found.Add("Dairy")
        If nuts Then found.Add("Nuts")
        If gluten Then found.Add("Gluten")
        If egg Then found.Add("Egg")
        If found.Count > 0 Then
            Return "  (Contains: " & String.Join(", ", found) & ")"
        End If
        Return ""
    End Function
    Private Sub AppendAllergyNote()
        Dim active As New List(Of String)
        If CheckBox1.Checked Then active.Add("Dairy")
        If CheckBox2.Checked Then active.Add("Nuts")
        If CheckBox3.Checked Then active.Add("Gluten")
        If CheckBox4.Checked Then active.Add("Egg")

        If active.Count > 0 Then
            ListBox1.Items.Add("⚠️ Active Allergy Filters: " & String.Join(", ", active))
        Else
            ListBox1.Items.Add("✅ No allergy filters active — all flavors shown.")
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        If ListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Please select a flavor first.")
            Exit Sub
        End If

        Dim selectedItem As String = ListBox1.SelectedItem.ToString().Trim()

        If Not selectedItem.Contains("₱") Then
            MessageBox.Show("Please select an actual flavor.")
            Exit Sub
        End If

        If selectedItem.StartsWith("⚠️") Then
            Dim answer As DialogResult = MessageBox.Show(
            "This flavor contains an allergen you filtered for. Add it anyway?",
            "Allergy Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If answer = DialogResult.No Then Exit Sub
        End If

        Dim cleanText As String = selectedItem.Replace("✅", "").Replace("⚠️", "").Trim()
        Dim matched As MoodFlavor = Nothing

        For Each f In currentFlavors
            If cleanText.StartsWith(f.ItemName) Then
                matched = f
                Exit For
            End If
        Next

        If matched Is Nothing Then
            MessageBox.Show("Could not match this flavor to a product. Please try again.")
            Exit Sub
        End If

        orderTotal += matched.Price
        CartManager.AddItem(matched.ProductID, matched.ItemName, matched.Price)
        ListBox2.Items.Add(matched.ItemName & " - ₱" & matched.Price.ToString("0.00"))

        MessageBox.Show(matched.ItemName & " added to cart!" & Environment.NewLine &
        "Current Total: ₱" & orderTotal.ToString("0.00"))

    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form5.Show()
        Me.Hide()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If ListBox2.Items.Count = 0 Then
            MessageBox.Show("Your order is empty. Add a flavor first.")
            Exit Sub
        End If

        Dim receipt As String = "🧾 ORDER RECEIPT" & Environment.NewLine
        receipt &= "─────────────────────────" & Environment.NewLine
        For Each orderItem As Object In ListBox2.Items
            receipt &= orderItem.ToString() & Environment.NewLine
        Next
        receipt &= "─────────────────────────" & Environment.NewLine
        receipt &= "TOTAL: ₱" & orderTotal
        MessageBox.Show(receipt, "Thank you for your order!")
        ListBox2.Items.Clear()
        orderTotal = 0
        Form5.Show()
        Me.Hide()

    End Sub
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Form1.Show()
        Me.Hide()
    End Sub
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Form4.Show()
        Me.Hide()
    End Sub
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form9.Show()
        Me.Hide()
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form5.Show()
        Me.Hide()
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Form2.Show()
        Hide()
    End Sub
    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Form11.Show()
        Me.Hide()
    End Sub
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Form12.RefreshCart()
        Form12.Show()
        Me.Hide()
    End Sub
End Class