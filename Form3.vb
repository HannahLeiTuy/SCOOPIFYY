Imports MySql.Data.MySqlClient

Public Class Form3
    Dim connString As String = "server=localhost;database=Scoopify_Creamery;user=root;password=Hannah_lei07;"

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadInventory()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadInventory()
    End Sub

    Private Sub LoadInventory()
        Using conn As New MySqlConnection(connString)
            conn.Open()
            Dim query As String = "SELECT item_name, category, current_stock, reorder_level, status FROM inventory ORDER BY category, item_name"
            Dim adapter As New MySqlDataAdapter(query, conn)
            Dim table As New DataTable()
            adapter.Fill(table)
            DataGridView1.DataSource = table
        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim inventoryForm As New Form3()
        inventoryForm.Show()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class