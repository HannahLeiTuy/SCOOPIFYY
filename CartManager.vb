Public Class CartItem
    Public Property ProductID As Integer
    Public Property ItemName As String
    Public Property Quantity As Integer
    Public Property Price As Decimal

    Public ReadOnly Property Subtotal As Decimal
        Get
            Return Price * Quantity
        End Get
    End Property
End Class

Public Module CartManager

    Public CartItems As New List(Of CartItem)

    Public Sub AddItem(productID As Integer, itemName As String, price As Decimal, Optional qty As Integer = 1)
        CartItems.Add(New CartItem With {
            .ProductID = productID,
            .ItemName = itemName,
            .Price = price,
            .Quantity = qty
        })
    End Sub

    Public Sub ClearCart()
        CartItems.Clear()
    End Sub

    Public Function GetTotal() As Decimal
        Dim total As Decimal = 0
        For Each item In CartItems
            total += item.Subtotal
        Next
        Return total
    End Function

End Module
