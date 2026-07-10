Imports MySql.Data.MySqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Diagnostics

Public Class Form3
    Dim id As Integer
    Sub RefreshData()
        LoadInventory()
        Dashboard()
    End Sub
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshData()
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        If DataGridView1.Columns.Contains("inventory_id") Then
            id = CInt(DataGridView1.CurrentRow.Cells("inventory_id").Value)
            TextBox1.Text = DataGridView1.CurrentRow.Cells("item_name").Value.ToString()
            ComboBox1.Text = DataGridView1.CurrentRow.Cells("category").Value.ToString()
            TextBox2.Text = DataGridView1.CurrentRow.Cells("current_stock").Value.ToString()
            TextBox3.Text = DataGridView1.CurrentRow.Cells("reorder_level").Value.ToString()

        End If
    End Sub

    Sub LoadInventory()
        Try
            conn.Open()
            Dim da As New MySqlDataAdapter("
        SELECT inventory_id, item_name, category, current_stock, reorder_level, status
        FROM inventory
        ORDER BY inventory_id", conn)

            Dim dt As New DataTable
            da.Fill(dt)
            DataGridView1.DataSource = dt
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            DataGridView1.Columns(0).Visible = False
            DataGridView1.Columns(1).HeaderText = "Item Name"
            DataGridView1.Columns(2).HeaderText = "Category"
            DataGridView1.Columns(3).HeaderText = "Current Stock"
            DataGridView1.Columns(4).HeaderText = "Reorder Level"
            DataGridView1.Columns(5).HeaderText = "Status"
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.MultiSelect = False
            DataGridView1.ReadOnly = True
            DataGridView1.AllowUserToAddRows = False

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Sub Dashboard()
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("SELECT COUNT(*) FROM inventory", conn)
            Label2.Text = cmd.ExecuteScalar().ToString()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

        Try
            conn.Open()
            Dim cmd As New MySqlCommand("SELECT COUNT(*) FROM inventory WHERE status='In Stock'", conn)
            Label3.Text = cmd.ExecuteScalar().ToString()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

        Try
            conn.Open()
            Dim cmd As New MySqlCommand("SELECT COUNT(*) FROM inventory WHERE status='Low Stock'", conn)
            Label4.Text = cmd.ExecuteScalar().ToString()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

        Try
            conn.Open()
            Dim cmd As New MySqlCommand("SELECT IFNULL(SUM(total_amount),0) FROM transactions", conn)
            Label5.Text = "₱" & cmd.ExecuteScalar().ToString()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If id = 0 Then
            MessageBox.Show("Please select an item first.")
            Exit Sub
        End If

        Try
            conn.Open()
            Dim cmd As New MySqlCommand("UPDATE inventory SET item_name=@a, category=@b, current_stock=@c, reorder_level=@d WHERE inventory_id=@id", conn)
            cmd.Parameters.AddWithValue("@a", TextBox1.Text)
            cmd.Parameters.AddWithValue("@b", ComboBox1.Text)
            cmd.Parameters.AddWithValue("@c", TextBox2.Text)
            cmd.Parameters.AddWithValue("@d", TextBox3.Text)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            MessageBox.Show("Item Updated Successfully!")
            RefreshData()
            Button4.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Private Sub report_Click(sender As Object, e As EventArgs) Handles report.Click

        Try
            Dim folderPath As String =
            Path.Combine(Application.StartupPath, "Reports")

            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim filePath As String =
            Path.Combine(folderPath,
            "Inventory_Report_" &
            DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".pdf")

            Dim doc As New Document(PageSize.A4, 30, 30, 30, 30)

            PdfWriter.GetInstance(doc,
                              New FileStream(filePath,
                                             FileMode.Create))

            doc.Open()

            Dim titleFont As Font =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22)

            Dim headerFont As Font =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)

            Dim normalFont As Font =
            FontFactory.GetFont(FontFactory.HELVETICA, 11)

            Dim title As New Paragraph(
            "🍦 SCOOPIFY CREAMERY 🍦",
            titleFont)

            title.Alignment = Element.ALIGN_CENTER

            doc.Add(title)

            Dim subtitle As New Paragraph(
            "Inventory Report",
            headerFont)

            subtitle.Alignment = Element.ALIGN_CENTER

            doc.Add(subtitle)

            doc.Add(New Paragraph(
            "Generated on: " &
            DateTime.Now.ToString(
            "MMMM dd, yyyy hh:mm tt"),
            normalFont))

            doc.Add(New Paragraph(" "))


            Dim pdfTable As New PdfPTable(5)

            pdfTable.WidthPercentage = 100

            pdfTable.SetWidths(
            {3, 2, 2, 2, 2})

            pdfTable.AddCell("Item Name")
            pdfTable.AddCell("Category")
            pdfTable.AddCell("Stock")
            pdfTable.AddCell("Reorder Level")
            pdfTable.AddCell("Status")

            For Each row As DataGridViewRow In DataGridView1.Rows

                If Not row.IsNewRow Then

                    pdfTable.AddCell(row.Cells(1).Value.ToString())
                    pdfTable.AddCell(row.Cells(2).Value.ToString())
                    pdfTable.AddCell(row.Cells(3).Value.ToString())
                    pdfTable.AddCell(row.Cells(4).Value.ToString())
                    pdfTable.AddCell(row.Cells(5).Value.ToString())

                End If

            Next

            doc.Add(pdfTable)

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(
            "Total Inventory Items: " &
            (DataGridView1.Rows.Count - 1),
            normalFont))

            doc.Add(New Paragraph(" "))
            doc.Add(New Paragraph(
            "Generated by Scoopify Inventory Management System",
            normalFont))

            doc.Close()

            MessageBox.Show(
            "Inventory report generated successfully!",
            "Success",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

            Process.Start("explorer.exe", folderPath)

        Catch ex As Exception

            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        id = 0
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        ComboBox1.SelectedIndex = -1
        TextBox1.Focus()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If TextBox1.Text = "" Or ComboBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
            MessageBox.Show("Please complete all fields.")
            Exit Sub
        End If

        Try
            conn.Open()
            Dim check As New MySqlCommand("SELECT COUNT(*) FROM inventory WHERE item_name=@name", conn)
            check.Parameters.AddWithValue("@name", TextBox1.Text)
            If check.ExecuteScalar() > 0 Then
                MessageBox.Show("Item already exists.")
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
                Exit Sub
            End If

            Dim cmd As New MySqlCommand("INSERT INTO inventory(item_name,category,current_stock,reorder_level) VALUES(@a,@b,@c,@d)", conn)
            cmd.Parameters.AddWithValue("@a", TextBox1.Text)
            cmd.Parameters.AddWithValue("@b", ComboBox1.Text)
            cmd.Parameters.AddWithValue("@c", TextBox2.Text)
            cmd.Parameters.AddWithValue("@d", TextBox3.Text)
            cmd.ExecuteNonQuery()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            MessageBox.Show("Item Added Successfully!")
            RefreshData()
            Button4.PerformClick()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not Char.IsControl(e.KeyChar) And Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If id = 0 Then
            MessageBox.Show("Please select an item first.")
            Exit Sub
        End If

        If MessageBox.Show("Delete this item?", "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Try
                conn.Open()
                Dim cmd As New MySqlCommand("DELETE FROM inventory WHERE inventory_id=@id", conn)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.ExecuteNonQuery()
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If

                MessageBox.Show("Item Deleted!")
                RefreshData()
                Button4.PerformClick()

            Catch ex As Exception
                MessageBox.Show(ex.Message)
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        Try
            conn.Open()
            Dim da As New MySqlDataAdapter("
            SELECT inventory_id,item_name,category,current_stock,reorder_level,status
            FROM inventory
            WHERE item_name LIKE @search", conn)
            da.SelectCommand.Parameters.AddWithValue("@search", "%" & TextBox4.Text & "%")
            Dim dt As New DataTable
            da.Fill(dt)
            DataGridView1.DataSource = dt
            DataGridView1.Columns(0).Visible = False
            DataGridView1.Columns(1).HeaderText = "Item Name"
            DataGridView1.Columns(2).HeaderText = "Category"
            DataGridView1.Columns(3).HeaderText = "Current Stock"
            DataGridView1.Columns(4).HeaderText = "Reorder Level"
            DataGridView1.Columns(5).HeaderText = "Status"
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub
    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

        Try
            conn.Open()
            Dim da As New MySqlDataAdapter("
            SELECT inventory_id,item_name,category,current_stock,reorder_level,status
            FROM inventory
            WHERE item_name LIKE @search", conn)
            da.SelectCommand.Parameters.AddWithValue("@search", "%" & TextBox4.Text & "%")
            Dim dt As New DataTable
            da.Fill(dt)
            DataGridView1.DataSource = dt
            DataGridView1.Columns(0).Visible = False
            DataGridView1.Columns(1).HeaderText = "Item Name"
            DataGridView1.Columns(2).HeaderText = "Category"
            DataGridView1.Columns(3).HeaderText = "Current Stock"
            DataGridView1.Columns(4).HeaderText = "Reorder Level"
            DataGridView1.Columns(5).HeaderText = "Status"
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Try

            conn.Open()

            Dim da As New MySqlDataAdapter("
        SELECT
        item_name,
        category,
        description,
        price,
        status
        FROM products
        ORDER BY item_name", conn)

            Dim dt As New DataTable

            da.Fill(dt)

            DataGridView1.DataSource = dt

            conn.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        End Try

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        Try

            conn.Open()

            Dim da As New MySqlDataAdapter("
        SELECT *
        FROM inventory
        WHERE category=@cat", conn)

            da.SelectCommand.Parameters.AddWithValue("@cat", ComboBox2.Text)

            Dim dt As New DataTable

            da.Fill(dt)

            DataGridView1.DataSource = dt

            conn.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        End Try

    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click

        Try

            conn.Open()

            Dim da As New MySqlDataAdapter("
        SELECT *
        FROM inventory
        WHERE status='Low Stock'
        OR status='Out of Stock'", conn)

            Dim dt As New DataTable

            da.Fill(dt)

            DataGridView1.DataSource = dt

            conn.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        End Try

    End Sub
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click

        Try

            conn.Open()

            Dim da As New MySqlDataAdapter("
        SELECT
        mystery.mystery_name,
        products.item_name,
        mystery.flavor_description,
        mystery.flavor_type,
        mystery.price,
        mystery.available_date,
        mystery.status
        FROM mystery
        INNER JOIN products
        ON mystery.product_id = products.product_id", conn)

            Dim dt As New DataTable

            da.Fill(dt)

            DataGridView1.DataSource = dt

            conn.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        End Try

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        Try

            conn.Open()

            Dim da As New MySqlDataAdapter("
        SELECT
        flavor_mood.mood_name,
        products.item_name,
        flavor_mood.flavor_description,
        flavor_mood.price
        FROM flavor_mood
        INNER JOIN products
        ON flavor_mood.product_id = products.product_id", conn)

            Dim dt As New DataTable

            da.Fill(dt)

            DataGridView1.DataSource = dt

            conn.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        End Try

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Try

            conn.Open()

            Dim da As New MySqlDataAdapter("
        SELECT
        transaction_id,
        transaction_date,
        total_amount,
        payment_method
        FROM transactions
        ORDER BY transaction_date DESC", conn)

            Dim dt As New DataTable

            da.Fill(dt)

            DataGridView1.DataSource = dt

            conn.Close()

        Catch ex As Exception

            MessageBox.Show(ex.Message)

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        End Try

    End Sub
End Class