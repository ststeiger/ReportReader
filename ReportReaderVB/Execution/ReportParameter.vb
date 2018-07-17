
Public Class ReportParameter
    Public Key As String
    Public Label As String
    Public Value As String

    Public Sub New(key As String, lab As String, val As String)
        Me.Key = key
        Me.Label = lab
        Me.Value = val
    End Sub

    Public Sub New(key As String, val As String)
        Me.New(key, val, val)
    End Sub

End Class
