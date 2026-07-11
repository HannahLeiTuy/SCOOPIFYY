Imports MySql.Data.MySqlClient

Public Class Form13
    Dim id As Integer = 0

    Private Sub Form13_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Add("Daily")
        ComboBox1.Items.Add("Weekly")
        LoadGrid()
    End Sub

    ' Combines the old LoadData() and the search query in
    ' TextBox4_TextChanged into one method (they were near-identical
    ' copies of the same SQL).
    Private Sub LoadGrid(Optional search As String = "")
        Try
            conn.Open()
            Dim da As New MySqlDataAdapter("
                SELECT mystery_id, mystery_name, flavor_description, flavor_type,
                       price, current_stock, available_date, status
                FROM mystery
                WHERE mystery_name LIKE @search", conn)
            da.SelectCommand.Parameters.AddWithValue("@search", "%" & search & "%")

            Dim dt As New DataTable
            da.Fill(dt)
            DataGridView1.DataSource = dt

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub

        id = DataGridView1.Rows(e.RowIndex).Cells("mystery_id").Value
        TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells("mystery_name").Value.ToString()
        TextBox2.Text = DataGridView1.Rows(e.RowIndex).Cells("flavor_description").Value.ToString()
        ComboBox1.Text = DataGridView1.Rows(e.RowIndex).Cells("flavor_type").Value.ToString()
        TextBox3.Text = DataGridView1.Rows(e.RowIndex).Cells("price").Value.ToString()
        DateTimePicker1.Value = CDate(DataGridView1.Rows(e.RowIndex).Cells("available_date").Value)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If id = 0 Then
            MessageBox.Show("Select a record first.")
            Exit Sub
        End If

        Try
            conn.Open()
            Dim cmd As New MySqlCommand("
                UPDATE mystery
                SET mystery_name=@name, flavor_description=@desc, flavor_type=@type, price=@price, available_date=@date
                WHERE mystery_id=@id", conn)
            cmd.Parameters.AddWithValue("@name", TextBox1.Text)
            cmd.Parameters.AddWithValue("@desc", TextBox2.Text)
            cmd.Parameters.AddWithValue("@type", ComboBox1.Text)
            cmd.Parameters.AddWithValue("@price", TextBox3.Text)
            cmd.Parameters.AddWithValue("@date", DateTimePicker1.Value.Date)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Updated Successfully")
            LoadGrid()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("
                INSERT INTO mystery (product_id,mystery_name,flavor_description,flavor_type,price,current_stock,available_date)
                VALUES (41,@name,@desc,@type,@price,20,@date)", conn)
            cmd.Parameters.AddWithValue("@name", TextBox1.Text)
            cmd.Parameters.AddWithValue("@desc", TextBox2.Text)
            cmd.Parameters.AddWithValue("@type", ComboBox1.Text)
            cmd.Parameters.AddWithValue("@price", TextBox3.Text)
            cmd.Parameters.AddWithValue("@date", DateTimePicker1.Value.Date)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Added Successfully")
            LoadGrid()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If id = 0 Then
            MessageBox.Show("Select a record first.")
            Exit Sub
        End If

        If MessageBox.Show("Delete this item?", "Delete", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Try
                conn.Open()
                Dim cmd As New MySqlCommand("DELETE FROM mystery WHERE mystery_id=@id", conn)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Deleted Successfully")
                LoadGrid()
                Button4.PerformClick()

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                If conn.State = ConnectionState.Open Then conn.Close()
            End Try
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        id = 0
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        ComboBox1.SelectedIndex = -1
        DateTimePicker1.Value = Date.Today
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        LoadGrid(TextBox4.Text)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Form3.Show()
        Me.Hide()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Form10.Show()
        Me.Hide()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form4.Show()
        Me.Hide()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form9.Show()
        Me.Hide()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form5.Show()
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
End Class