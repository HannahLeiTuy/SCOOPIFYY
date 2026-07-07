Imports System.Reflection.Emit
Imports MySql.Data.MySqlClient
Public Class Form7
    Private currentMood As String = ""
    Private orderTotal As Double = 0
    Dim connString As String = "server=localhost;database=Scoopify_Creamery;user=root;password=Hannah_lei07;"
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

                ListBox1.Items.Add("   " & SafeTag(False, False, hasGluten, hasEgg) &
                                   "  Strawberry - ₱85" &
                                   AllergenList(hasDairy, False, hasGluten, hasEgg))

                ListBox1.Items.Add("   " & SafeTag(False, False, False, False) & "  Passion Fruit Citrus Swirl - ₱70")

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

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, hasGluten, hasEgg) &
                                   "  Triple Fudge - ₱70" &
                                   AllergenList(hasDairy, False, hasGluten, hasEgg))

                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, hasGluten, hasEgg) &
                                   "  Rich Chocolate - ₱55" &
                                   AllergenList(hasDairy, False, hasGluten, hasEgg))

                ListBox1.Items.Add("   " & SafeTag(hasDairy, hasNuts, hasGluten, hasEgg) &
                                   "  Brownie Batter - ₱75" &
                                   AllergenList(hasDairy, hasNuts, hasGluten, hasEgg))

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
                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, False, False) &
                                   "  Vietnamese Coffee - ₱70" &
                                   AllergenList(hasDairy, False, False, False))
                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, hasGluten, hasEgg) &
                                   "  Tiramisu - ₱90" &
                                   AllergenList(hasDairy, False, hasGluten, hasEgg))
                ListBox1.Items.Add("   " & SafeTag(hasDairy, False, False, False) &
                                   "  Cold Brew - ₱70" &
                                   AllergenList(hasDairy, False, False, False))

                ListBox1.Items.Add("")
                ListBox1.Items.Add("💡 Best paired with: oat milk whip or biscotti crumble")
                ListBox1.Items.Add("")
                AppendAllergyNote()
        End Select
    End Sub
    Private Function GetProductID(itemName As String) As Integer
        Dim id As Integer = 0
        Using conn As New MySqlConnection(connString)
            conn.Open()

            Dim sql As String = "SELECT product_id
             FROM products
             WHERE item_name=@name"

            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@name", itemName)
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                id = Convert.ToInt32(result)
            End If
        End Using
        Return id
    End Function
    Private Sub SaveMoodOrder()
        Using conn As New MySqlConnection(connString)
            conn.Open()
            Dim cmd As New MySqlCommand("INSERT INTO transactions (customer_id, employee_id, total_amount, payment_method)
            VALUES (1,3,@total,'Cash')", conn)
            cmd.Parameters.AddWithValue("@total", orderTotal)
            cmd.ExecuteNonQuery()

            Dim transactionID As Integer = cmd.LastInsertedId
            For Each item As String In ListBox2.Items
                If Not item.Contains("₱") Then
                    Continue For
                End If
                Dim flavor As String = item
                If flavor.Contains("₱") Then
                    flavor = flavor.Substring(0, flavor.IndexOf("₱"))
                End If
                If flavor.Contains("-") Then
                    flavor = flavor.Split("-"c)(0)
                End If
                flavor = flavor.Replace("✅", "")
                flavor = flavor.Replace("⚠️", "")
                flavor = flavor.Trim()
                Dim productID As Integer = GetProductID(flavor)
                MessageBox.Show("Flavor found: " & flavor)
                If productID = 0 Then
                    MessageBox.Show(flavor & " was not found in Products table.")
                    Continue For
                End If
                Dim price As Decimal = Val(item.Substring(item.IndexOf("₱") + 1))
                Dim cmd2 As New MySqlCommand("INSERT INTO transaction_details
                (transaction_id, product_id, quantity, unit_price, subtotal)
                VALUES (@tid,@pid,1,@price,@price)", conn)

                cmd2.Parameters.AddWithValue("@tid", transactionID)
                cmd2.Parameters.AddWithValue("@pid", productID)
                cmd2.Parameters.AddWithValue("@price", price)
                cmd2.ExecuteNonQuery()

                Dim stockCmd As New MySqlCommand(
                "UPDATE inventory
                SET current_stock = current_stock - 1
                WHERE item_name = @name
                AND current_stock > 0", conn)
                stockCmd.Parameters.AddWithValue("@name", flavor)
                stockCmd.ExecuteNonQuery()

                Dim cupCmd As New MySqlCommand(
                "UPDATE inventory
                SET current_stock = current_stock - 1
                WHERE item_name = 'Paper Cup'
                AND current_stock > 0", conn)
                cupCmd.ExecuteNonQuery()

            Next
        End Using
    End Sub
    Private Function SafeTag(dairy As Boolean, nuts As Boolean, gluten As Boolean, egg As Boolean) As String
        If dairy OrElse nuts OrElse gluten OrElse egg Then
            Return "⚠️"
        Else
            Return "✅"
        End If
    End Function
    Private Function AllergenList(dairy As Boolean, nuts As Boolean,
                                  gluten As Boolean, egg As Boolean) As String
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

        If selectedItem.StartsWith("✅") Then
            ListBox2.Items.Add(selectedItem)
            Dim priceText As String = selectedItem.Substring(selectedItem.IndexOf("₱"c) + 1)
            Dim price As Double = Val(priceText)
            orderTotal += price

            MessageBox.Show(selectedItem & " added!" & Environment.NewLine &
                         "Current Total: ₱" & orderTotal)

        ElseIf selectedItem.StartsWith("⚠️") Then
            Dim answer As DialogResult = MessageBox.Show(
            "This flavor contains an allergen you filtered for. Add it anyway?",
            "Allergy Warning",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)

            If answer = DialogResult.Yes Then
                ListBox2.Items.Add(selectedItem)

                Dim priceText As String = selectedItem.Substring(selectedItem.IndexOf("₱"c) + 1)
                Dim price As Double = Val(priceText)
                orderTotal += price

                MessageBox.Show(selectedItem & " added!" & Environment.NewLine &
                             "Current Total: ₱" & orderTotal)
            End If
        Else
            MessageBox.Show("Please select an actual flavor from the list.")
        End If
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
        SaveMoodOrder()
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
        Form12.Show()
        Me.Hide()
    End Sub
End Class