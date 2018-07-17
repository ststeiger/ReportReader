
Public Class IndustryMinutes


    Function getTimeFormat(time As Double) As Object
        Dim s As String
        Dim x As Double = time
        Dim h As Double = System.Math.Truncate(x)
        Dim m As Double = (x - h)
        Dim mcalc As Decimal = m * 0.6

        mcalc = Decimal.Round(mcalc, 2)
        Dim min As String = mcalc.ToString.Substring(mcalc.ToString.IndexOf(",") + 1)
        If min.Length = 1 Then min = min & "0"
        If (time > 0) And (min = "00") Then min = "01"
        s = h.ToString & ":" & min
        Return s
    End Function



    Public Function ToIndustryMinutes(obj As Object) As String
        If obj Is Nothing Then Return Nothing
        If obj Is System.DBNull.Value Then Return Nothing

        Dim d As Double = 0

        Try
            d = CDbl(obj)
        Catch ex As Exception
            Return ex.Message
        End Try

        Dim hours As Integer = CInt(System.Math.Floor(d))
        d = d Mod 1.0

        d *= 100.0
        d = System.Math.Round(d, System.MidpointRounding.AwayFromZero)

        ' FU 15.99597038
        If d >= 100.0 Then
            d -= 100.0
            hours += 1
            d = System.Math.Round(d, System.MidpointRounding.AwayFromZero)
        End If

        Dim str As String = d.ToString("N0", System.Globalization.CultureInfo.InvariantCulture).PadLeft(2, "0"c)
        str = str.Substring(str.Length - 2)
        str = hours.ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(2, "0"c) + ":" + str

        Return str
    End Function


    Sub TestMain()
        ' ParameterStore.ExecuteParameter()
        ' ParameterStore.Gebäude()
        ' ParameterStore.SelVB()
        ' ParameterStore.Nutzerdaten()


        System.Console.WriteLine(ToIndustryMinutes(8.75))
        System.Console.WriteLine(ToIndustryMinutes(15.99597038))

        System.Console.WriteLine(System.Environment.NewLine)
        System.Console.WriteLine("--- Press any key to continue --- ")
        System.Console.ReadKey()
    End Sub


End Class
