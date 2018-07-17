
Public Class ParameterStore

    Private dict As System.Collections.Generic.Dictionary(Of String, ReportParameter)


    Public Sub New(ParamArray u As ReportParameter())
        Me.New()

        For i As Integer = 0 To u.Length - 1 Step 1
            Me.dict.Add(u(i).Key, u(i))
        Next

    End Sub


    Public Sub New(parameters As System.Collections.Generic.Dictionary(Of String, ReportParameter))
        Me.dict = parameters
    End Sub


    Public Sub New()
        dict = New System.Collections.Generic.Dictionary(Of String, ReportParameter)(System.StringComparer.OrdinalIgnoreCase)
    End Sub


    Public Sub Add(para As ReportParameter)
        Me.dict.Add(para.Key, para)
    End Sub


    Default Public Property Parameters(ByVal index As String) As ReportParameter
        Get
            Return dict(index)
        End Get

        Set(ByVal value As ReportParameter)
            dict(index) = value
        End Set

    End Property

    ' Z = iif(y=0, 0, x/y)  'Throws a divide by zero exception when y is 0
    Public Shared Function IIF(expression As Boolean, truePart As Object, falsePart As Object) As Object
        If expression Then
            Return truePart
        End If

        Return falsePart
    End Function


    Public Shared Function SelVB() As String
        Dim Parameters As ParameterStore = New ParameterStore()

        Parameters.Add(New ReportParameter("stadtkreis", "Alle", "@stadtkreis"))
        Parameters.Add(New ReportParameter("Gemeinde", "Alle", "@Gemeinde"))


        Return SelVB(Parameters)
    End Function


    Public Shared Function SelVB(Parameters As ParameterStore)
        Dim str As String = "SELECT     '00000000-0000-0000-0000-000000000000' AS ID, 'Alle' AS Name, - 1 AS Sort " &
"UNION " &
"SELECT     VB_ApertureID AS ID, VB_Name AS Name, VB_Sort AS Sort " &
"FROM         V_AP_BERICHT_SEL_VB " &
"WHERE ('" & Parameters!stadtkreis.Value & "' = '00000000-0000-0000-0000-000000000000' OR VB_KS_UID = '" & Parameters!Gemeinde.Value & "') " &
"ORDER BY Sort, Name "

        Return str
    End Function


    ' https://harouny.com/2012/10/15/render-asp-net-controls-user-controls-to-html-by-code/
    ' https://stackoverflow.com/questions/2351225/how-to-dynamically-render-asp-net-controls-from-string
    ' https://stackoverflow.com/questions/2351225/how-to-dynamically-render-asp-net-controls-from-string
    Public Shared Function Gebäude() As String
        Dim Parameters As ParameterStore = New ParameterStore()
        ' https://msdn.microsoft.com/en-us/library/microsoft.visualbasic.controlchars.cr(v=vs.110).aspx
        ' Microsoft.VisualBasic.ControlChars  My.InternalXmlHelper


        Parameters.Add(New ReportParameter("schulkreis", "Alle", "100000000-0000-0000-0000-000000000000"))
        Parameters.Add(New ReportParameter("stadtkreis", "Alle", "100000000-0000-0000-0000-000000000000"))
        Parameters.Add(New ReportParameter("schuleinheit", "Alle", "100000000-0000-0000-0000-000000000000"))


        Parameters.Add(New ReportParameter("portfolio1", "Alle", "@portfolio1"))
        Parameters.Add(New ReportParameter("subportfolio1", "Alle", "@subportfolio1"))
        Parameters.Add(New ReportParameter("denkmalschutz", "Alle", "100000000-0000-0000-0000-000000000000"))
        Parameters.Add(New ReportParameter("kontakt", "Alle", "100000000-0000-0000-0000-000000000000"))


        Dim str As String = Gebäude(Parameters)

        System.Console.WriteLine(str)
        Return str
    End Function



    Public Shared Function Gebäude(Parameters As ParameterStore) As String
        Dim str As String = "SELECT gbi.Gemeinde, gbi.Kreis, gbi.Vermessungsbezirk, gbi.Schulkreis, gbi.Standort, gbi.Gebaeude, gbi.SO_Name, gbi.GBI_Name, gbi.GBI_Adresse, gbi.GBI_Bemerkungen, gbi.GS_Lang, pf.PF1_ID, pf.PF1_Lang, pf.PF2_ID, pf.PF2_Lang, pf.PF3_ID, pf.PF3_Lang, pf.PF4_ID, pf.PF4_Lang, gbi.GS_UID,  gbi.SO_Sort, gbi.GBI_Sort, gbi.SK_Sort, gbi.GS_Sort, pf.PF1_Sort, pf.PF2_Sort, pf.PF3_Sort, pf.PF4_Sort, gbi.SK_UID AS SK_ID, gbi.GBI_IsDenkmalschutz, gbi.GBI_BemDenkmalschutz, gbi.SO_ID, gbi.GBI_ID, gbi.GM_ID, gbi.KS_ID, gbi.VB_ID, gbi.GM_Sort, gbi.KS_Sort, gbi.VB_Sort " &
"FROM V_AP_BERICHT_GBI AS gbi LEFT OUTER JOIN V_AP_BERICHT_GBI_Portfolio AS pf ON gbi.GBI_ID = pf.GBI_ID " &
"WHERE 1=1 " &
IIF(Parameters!schulkreis.Value = "00000000-0000-0000-0000-000000000000", "", "AND gbi.SK_UID='" & Parameters!schulkreis.Value & "' ") &
IIF(Parameters!stadtkreis.Value = "00000000-0000-0000-0000-000000000000", "", "AND gbi.KS_ID='" & Parameters!stadtkreis.Value & "' ") &
IIF(Parameters!schuleinheit.Value = "00000000-0000-0000-0000-000000000000", "AND 1=1 ", "AND SE_UID='" & Parameters!schuleinheit.Value & "' ") &
IIF(Parameters!portfolio1.Value = "00000000-0000-0000-0000-000000000000", "", "AND (pf.PF1_ID='" & Parameters!portfolio1.Value & "' ") &
IIF(Parameters!portfolio1.Value = "00000000-0000-0000-0000-000000000000", "", "   OR pf.PF2_ID='" & Parameters!portfolio1.Value & "') ") &
IIF(Parameters!subportfolio1.Value = "00000000-0000-0000-0000-000000000000", "", "AND (pf.PF3_ID='" & Parameters!subportfolio1.Value & "' ") &
IIF(Parameters!subportfolio1.Value = "00000000-0000-0000-0000-000000000000", "", "OR pf.PF4_ID='" & Parameters!subportfolio1.Value & "') ") &
IIF(Parameters!denkmalschutz.Value = "2", "", "AND gbi.GBI_IsDenkmalschutz=" & Parameters!denkmalschutz.Value & " ") &
IIF(Parameters!kontakt.Value = "00000000-0000-0000-0000-000000000000", "", "AND (gbi.GBI_ID IN (SELECT DPS_GBI_UID FROM dbo.T_Detail_Personen WHERE CONVERT(varchar(36), DPS_KF_UID) + CONVERT(varchar(36), DPS_KT_UID) ='" & Parameters!kontakt.Value & "' ))")



        ' ERROR: IIF(Parameters!denkmalschutz.Value = "2", "", "AND gbi.GBI_IsDenkmalschutz=" & Parameters!denkmalschutz.Value & " ") &

        Return str
    End Function



    ' https://harouny.com/2012/10/15/render-asp-net-controls-user-controls-to-html-by-code/
    ' https://stackoverflow.com/questions/2351225/how-to-dynamically-render-asp-net-controls-from-string
    ' https://stackoverflow.com/questions/2351225/how-to-dynamically-render-asp-net-controls-from-string
    Public Shared Function ExecuteParameter() As String
        Dim Parameters As ParameterStore = New ParameterStore()
        ' https://msdn.microsoft.com/en-us/library/microsoft.visualbasic.controlchars.cr(v=vs.110).aspx
        ' Microsoft.VisualBasic.ControlChars  My.InternalXmlHelper


        Parameters.Add(New ReportParameter("Vermessungsbezirk", "Alle", "00000000-0000-0000-0000-000000000000"))
        Parameters.Add(New ReportParameter("Kreis", "Alle", "00000000-0000-0000-0000-000000000000"))
        Parameters.Add(New ReportParameter("Gemeinde", "Alle", "00000000-0000-0000-0000-000000000000"))

        Dim str As String = ExecuteParameter(Parameters)

        System.Console.WriteLine(str)
        Return str
    End Function

    Public Shared Function Nutzerdaten() As String
        Dim Parameters As ParameterStore = New ParameterStore()
        ' https://msdn.microsoft.com/en-us/library/microsoft.visualbasic.controlchars.cr(v=vs.110).aspx
        ' Microsoft.VisualBasic.ControlChars  My.InternalXmlHelper

        Parameters.Add(New ReportParameter("Stadtkreis", "", "@Stadtkreis"))
        Parameters.Add(New ReportParameter("Schulkreis", "", "@Schulkreis"))
        Parameters.Add(New ReportParameter("Portfolio1", "", "@Portfolio1"))
        Parameters.Add(New ReportParameter("Subportfolio1", "", "@Subportfolio1"))
        Parameters.Add(New ReportParameter("Standort", "", "@Standort"))
        Parameters.Add(New ReportParameter("We", "", "@We"))




        Dim str As String = Nutzerdaten(Parameters)

        System.Console.WriteLine(str)
        Return str
    End Function



    Public Shared Function Nutzerdaten(Parameters As ParameterStore) As String
        Dim str As String = "SELECT '00000000-0000-0000-0000-000000000000' AS RPT_UID, 'Alle' AS RPT_Name, -1 AS RPT_Sort " &
"UNION  " &
"SELECT DISTINCT GBI_EV_UID AS RPT_UID, EV_Lang AS RPT_Name, EV_Sort AS RPT_Sort   " &
"FROM V_AP_BERICHT_DATA_Nutzerdaten " &
"LEFT JOIN dbo.T_Ref_EigentumsVerhaeltnisse " &
"	ON V_AP_BERICHT_DATA_Nutzerdaten.GBI_EV_UID = T_Ref_EigentumsVerhaeltnisse.EV_UID " &
"WHERE EV_Status = 1 " &
IIF(Parameters!Stadtkreis.Value = "00000000-0000-0000-0000-000000000000", " ", "AND VB_KS_UID = '" & Parameters!Stadtkreis.Value & "' ") &
IIF(Parameters!Schulkreis.Value = "00000000-0000-0000-0000-000000000000", " ", "AND GBI_SK_UID = '" & Parameters!Schulkreis.Value & "' ") &
IIF(Parameters!Portfolio1.Value = "00000000-0000-0000-0000-000000000000", " ", "AND (RM_S_PF_UID = '" & Parameters!Portfolio1.Value & "' ") &
IIF(Parameters!Portfolio1.Value = "00000000-0000-0000-0000-000000000000", " ", "OR RM_S_PF_UID = '" & Parameters!Portfolio1.Value & "') ") &
IIF(Parameters!Subportfolio1.Value = "00000000-0000-0000-0000-000000000000", " ", "AND (RM_SS_PF_UID = '" & Parameters!Subportfolio1.Value & "' ") &
IIF(Parameters!Subportfolio1.Value = "00000000-0000-0000-0000-000000000000", " ", "OR RM_SS_PF_UID = '" & Parameters!Subportfolio1.Value & "') ") &
IIF(Parameters!Standort.Value = "00000000-0000-0000-0000-000000000000", " ", "AND GBI_SO_UID = '" & Parameters!Standort.Value & "' ") &
IIF(Parameters!We.Value = "00000000-0000-0000-0000-000000000000", " ", "AND GBI_WENr_SAP = '" & Parameters!We.Value & "' ")

        System.Console.WriteLine(str)
        Return str
    End Function


    ' https://stackoverflow.com/questions/6841275/what-does-this-mean-in-the-specific-line-of-code
    Public Shared Function ExecuteParameter(Parameters As ParameterStore) As String
        Dim str As String = "SELECT 2 AS ID, 2 AS Sort, 'Alle' AS Name " &
"UNION " &
"SELECT DISTINCT PZ_IsAltlasten AS ID, PZ_IsAltlasten AS Sort, 'Ja' AS Name " &
"FROM V_AP_BERICHT_SEL_PZ_Altlasten  " &
"WHERE PZ_IsAltlasten = 1  " &
IIF(Parameters!Vermessungsbezirk.Value = "00000000-0000-0000-0000-000000000000", IIF(Parameters!Kreis.Value = "00000000-0000-0000-0000-000000000000", IIF(Parameters!Gemeinde.Value = "00000000-0000-0000-0000-000000000000", "", "AND GM_ApertureID = '" & Parameters!Gemeinde.Value & "' "), "AND KS_ApertureID = '" & Parameters!Kreis.Value & "' "), "AND VB_ApertureID = '" & Parameters!Vermessungsbezirk.Value & "' ") &
"UNION " &
"SELECT DISTINCT PZ_IsAltlasten AS ID, PZ_IsAltlasten AS Sort, 'Nein' AS Name " &
"FROM V_AP_BERICHT_SEL_PZ_Altlasten " &
"WHERE PZ_IsAltlasten = 0 " &
IIF(Parameters!Vermessungsbezirk.Value = "00000000-0000-0000-0000-000000000000", IIF(Parameters!Kreis.Value = "00000000-0000-0000-0000-000000000000", IIF(Parameters!Gemeinde.Value = "00000000-0000-0000-0000-000000000000", "", "AND GM_ApertureID = '" & Parameters!Gemeinde.Value & "' "), "AND KS_ApertureID = '" & Parameters!Kreis.Value & "' "), "AND VB_ApertureID = '" & Parameters!Vermessungsbezirk.Value & "' ") &
"ORDER BY Sort DESC "

        ' str.Trim(New Char() {" "c, vbTab, vbCr, vbLf})

        Return str
    End Function


End Class
