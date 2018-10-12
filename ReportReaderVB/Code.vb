
Public Class RsCode


    Function GetFirstLine(obj As Object) As String
        Dim str As String = System.Convert.ToString(obj)
        If (String.IsNullOrEmpty(str) OrElse str.Trim() = String.Empty) Then
            Return ""
        End If

        ' Dim lineArray As String() = str.Split(System.Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        Dim lineArray As String() = str.Split(New String() {System.Environment.NewLine}, System.StringSplitOptions.RemoveEmptyEntries)
        If lineArray IsNot Nothing AndAlso lineArray.Length > 0 Then
            Return lineArray(0)
        End If

        Return ""
    End Function


    Sub test(ag As String)
        'Microsoft.VisualBasic.Strings.Replace(ag, vbLf, System.Environment.NewLine)
        Microsoft.VisualBasic.Strings.AscW(ag)
    End Sub


    Function ToDateString(dateStringObject As Object) As System.Nullable(Of System.DateTime)
        Dim dateString As String = System.Convert.ToString(dateStringObject, System.Globalization.CultureInfo.InvariantCulture)

        If dateStringObject Is System.DBNull.Value Then
            Return Nothing
        End If


        If String.IsNullOrEmpty(dateString) Then
            Return Nothing
        End If

        Dim x As System.DateTime
        If System.DateTime.TryParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, x) Then
            Return x
        End If

        ' Return Nothing
        ' Return dateString
        Return CDate(dateString) ' #VALUE!
    End Function


    Function ToDateString1(dateStringObject As Object) As String
        Dim dateString As String = System.Convert.ToString(dateStringObject, System.Globalization.CultureInfo.InvariantCulture)

        If String.IsNullOrEmpty(dateString) Then
            Return ""
        End If

        Dim x As System.DateTime
        If System.DateTime.TryParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, x) Then
            Return x.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture)
        End If


        Return dateString
    End Function


    Function Xmlify(ParamArray args As Object()) As String
        Dim str As String = Nothing

        Dim sb As New System.Text.StringBuilder
        For i As Integer = 0 To args.Length - 1 Step 1
            Dim s As String = System.Convert.ToString(args(i))
            If (String.IsNullOrEmpty(s)) Then
                Continue For
            End If

            sb.Append("<e>")
            sb.Append(s)
            sb.Append("</e>")
        Next i
        str = sb.ToString()

        Return str
    End Function


End Class
