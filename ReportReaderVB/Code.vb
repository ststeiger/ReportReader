
Public Class RsCode


    Function ToDateString(dateStringObject As Object) As String
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
