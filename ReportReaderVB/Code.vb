
Public Class RsCode


    Public Shared Function BuildIntegerListInserts(ByVal paramValues As Object()) As String
        Dim strRetValue As String = Nothing
        Dim insertStatements As New System.Text.StringBuilder()


        insertStatements.AppendLine("DECLARE @tblWorkaround TABLE ")
        insertStatements.AppendLine("( ")
        insertStatements.AppendLine("MultiSelectParameter integer ")
        insertStatements.AppendLine("); ")

        For Each paramValue As Object In paramValues
            insertStatements.Append("INSERT INTO @tblWorkaround(MultiSelectParameter) VALUES ( ")
            If paramValue Is Nothing Then
                insertStatements.Append("NULL")
            Else
                insertStatements.Append(paramValue)
            End If

            insertStatements.AppendLine("); ")
        Next paramValue

        insertStatements.AppendLine("SELECT * FROM @tblWorkaround")

        strRetValue = insertStatements.ToString()
        insertStatements.Length = 0
        insertStatements = Nothing

        Return strRetValue
    End Function ' BuildIntegerListInserts


    Public Shared Function BuildUidListInserts(ByVal paramValues As Object()) As String
        Dim strRetValue As String = Nothing
        Dim insertStatements As New System.Text.StringBuilder()

        insertStatements.AppendLine("DECLARE @tblWorkaround TABLE ")
        insertStatements.AppendLine("( ")
        insertStatements.AppendLine("MultiSelectParameter uniqueidentifier ")
        insertStatements.AppendLine("); ")

        For Each paramValue As Object In paramValues

            insertStatements.Append("INSERT INTO @tblWorkaround(MultiSelectParameter) VALUES ( ")
            If paramValue Is Nothing Then
                insertStatements.Append("NULL")
            Else
                insertStatements.Append("'")
                insertStatements.Append(paramValue)
                insertStatements.Append("'")
            End If

            insertStatements.AppendLine("); ")

        Next paramValue

        insertStatements.AppendLine("SELECT * FROM @tblWorkaround")

        strRetValue = insertStatements.ToString()
        insertStatements.Length = 0
        insertStatements = Nothing

        Return strRetValue
    End Function ' BuildUidListInserts



    Public Shared Function BuildGsIdentifierListInserts(ByVal paramValues As Object()) As String
        Dim strRetValue As String = Nothing
        Dim insertStatements As New System.Text.StringBuilder()

        insertStatements.AppendLine("DECLARE @tblWorkaround TABLE ")
        insertStatements.AppendLine("( ")
        insertStatements.AppendLine("MultiSelectParameter varchar(76) ")
        insertStatements.AppendLine("); ")

        For Each paramValue As Object In paramValues

            insertStatements.Append("INSERT INTO @tblWorkaround(MultiSelectParameter) VALUES ( ")
            If paramValue Is Nothing Then
                insertStatements.Append("NULL")
            Else
                insertStatements.Append("'")
                insertStatements.Append(paramValue)
                insertStatements.Append("'")
            End If

            insertStatements.AppendLine("); ")

        Next paramValue

        insertStatements.AppendLine("SELECT * FROM @tblWorkaround")

        strRetValue = insertStatements.ToString()
        insertStatements.Length = 0
        insertStatements = Nothing

        Return strRetValue
    End Function ' BuildGsIdentifierListInserts






    Public Function GetPageEnumeration(strFormatString As String, iPageNumber As Integer, iTotalNumberOfPages As Integer) As String
        ' Dim strFormatString As String = "Seite {0} von {1}"
        Return String.Format(strFormatString, iPageNumber, iTotalNumberOfPages)
    End Function


    Public Function GetDateForCulture(iDay As Integer, iMonth As Integer, iYear As Integer, strFormat As String, strReportLanguage As String) As String
        If String.IsNullOrEmpty(strReportLanguage) Then
            strReportLanguage = "DE"
        End If

        If String.IsNullOrEmpty(strFormat) Then
            strFormat = "d. MMMM yyy"
        End If

        Dim strCultureString As String = ""
        Select Case strReportLanguage.ToLower()
            Case "de"
                strCultureString = "de-ch"
                Exit Select
            Case "fr"
                strCultureString = "fr-ch"
                Exit Select
            Case "it"
                strCultureString = "it-ch"
                Exit Select
            Case "en"
                strCultureString = "en-us"
                Exit Select
            Case Else
                strCultureString = strReportLanguage
                Exit Select
        End Select
        ' End Switch strReportLanguage
        Dim ci As New System.Globalization.CultureInfo(strCultureString)
        Dim dt As New DateTime(iYear, iMonth, iDay)

        Dim strDateAsString As String = dt.ToString(strFormat, ci)
        strDateAsString = ci.TextInfo.ToTitleCase(strDateAsString)

        Return strDateAsString
    End Function ' GetDateForCulture





    Function Concat(x As String, y As String, z As String) As String
        If String.IsNullOrEmpty(x) Then
            If String.IsNullOrEmpty(y) Then
                Return ""
            Else
                Return y
            End If
        Else
            If String.IsNullOrEmpty(y) Then
                Return x
            Else
                Return x + z + y
            End If
        End If

        Return ""
    End Function








    ' ------------------------------------------------------------

    Public Function CalcRatio(ByVal Numerator As Object, ByVal Denominator As Object, ByVal DivZeroDefault As Object) As Object

        If Denominator <> 0 Then
            Return Numerator * 100 / Denominator
        Else
            If Numerator = 0 Then
                Return 0
            Else
                Return "∞" 'DivZeroDefault 
            End If
        End If
    End Function


    Public Function Ratio(ByVal Numerator As Object, ByVal Denominator As Object) As System.Nullable(Of Double)
        If Numerator Is Nothing Then
            Return Nothing
        End If

        If Denominator <> 0 Then
            Return CDbl(Numerator) / CDbl(Denominator)
        Else
            If Numerator = 0 Then
                Return 0.0
            Else
                Return Double.PositiveInfinity
            End If
        End If
    End Function


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



    Public Function SetDefaultZero(ByVal obj As Object) As Object
        If obj Is Nothing OrElse obj Is System.DBNull.Value Then
            Return 0.0
        End If

        Return obj
    End Function


    Public Function SetDefault(ByVal obj As Object) As Object
        Return SetDefaultZero(obj)
    End Function




    Public Function CalcRatio(ByVal Numerator As Object, ByVal Denominator As Object, ByVal DivZeroDefault As Object) As Object
        If Denominator <> 0 Then
            Return Numerator * 100 / Denominator
        Else
            If Numerator = 0 Then
                Return 0
            Else
                Return "∞" 'DivZeroDefault 
            End If
        End If
    End Function



    Public Function GetPageEnumeration(strFormatString As String, iPageNumber As Integer, iTotalNumberOfPages As Integer) As String
        ' Dim strFormatString As String = "Seite {0} von {1}"
        Return String.Format(strFormatString, iPageNumber, iTotalNumberOfPages)
    End Function


    Public Function GetDateForCulture(iDay As Integer, iMonth As Integer, iYear As Integer, strFormat As String, strReportLanguage As String) As String
        If String.IsNullOrEmpty(strReportLanguage) Then
            strReportLanguage = "DE"
        End If

        If String.IsNullOrEmpty(strFormat) Then
            strFormat = "d. MMMM yyy"
        End If

        Dim strCultureString As String = ""
        Select Case strReportLanguage.ToLower()
            Case "de"
                strCultureString = "de-ch"
                Exit Select
            Case "fr"
                strCultureString = "fr-ch"
                Exit Select
            Case "it"
                strCultureString = "it-ch"
                Exit Select
            Case "en"
                strCultureString = "en-us"
                Exit Select
            Case Else
                strCultureString = strReportLanguage
                Exit Select
        End Select
        ' End Switch strReportLanguage
        Dim ci As New System.Globalization.CultureInfo(strCultureString)
        Dim dt As New DateTime(iYear, iMonth, iDay)

        Dim strDateAsString As String = dt.ToString(strFormat, ci)
        strDateAsString = ci.TextInfo.ToTitleCase(strDateAsString)

        Return strDateAsString
    End Function ' GetDateForCulture



    Public Function GetDateForCulture(ByVal iDay As Integer, ByVal iMonthNum As Integer, ByVal iYear As Integer, ByVal strReportLanguage As String) As String
        Dim strReturnValue As String = ""
        Try
            strReportLanguage = strReportLanguage.ToLower()

            'Dim dtThisDateDate As DateTime = DateTime.Now
            'Dim dtThisDateDate As New DateTime(2010, 12, 31)
            Dim dtThisDateDate As New DateTime(iYear, iMonthNum, iDay)

            Dim strCultureString As String = ""
            Select Case strReportLanguage
                Case "de"
                    strCultureString = "de-ch"
                Case "fr"
                    strCultureString = "fr-ch"
                Case "it"
                    strCultureString = "it-ch"
                Case "en"
                    strCultureString = "en-us"
                Case Else
                    strCultureString = strReportLanguage
            End Select

            Dim ci As New System.Globalization.CultureInfo(strCultureString)

            Dim strFormat = ci.DateTimeFormat.LongDatePattern
            If ci.CompareInfo.IndexOf(strFormat, "dddd, ", System.Globalization.CompareOptions.IgnoreCase) > -1 Then
                strFormat = Mid(ci.DateTimeFormat.LongDatePattern, Len("dddd, ") + 1)
            End If

            strReturnValue = dtThisDateDate.ToString(strFormat, ci)
            'strReturnValue = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(strReturnValue)
            strReturnValue = ci.TextInfo.ToTitleCase(strReturnValue)
        Catch ex As Exception
            strReturnValue = iDay.ToString().PadLeft(2, "0") + "." + iMonthNum.ToString().PadLeft(2, "0") + "." + iYear.ToString()
        End Try

        Return strReturnValue
    End Function


    Function Concat(x As String, y As String, z As String) As String
        If String.IsNullOrEmpty(x) Then
            If String.IsNullOrEmpty(y) Then
                Return ""
            Else
                Return y
            End If
        Else
            If String.IsNullOrEmpty(y) Then
                Return x
            Else
                Return x + z + y
            End If
        End If

        Return ""
    End Function


End Class
