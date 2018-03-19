
="SELECT gbi.Gemeinde, gbi.Kreis, gbi.Vermessungsbezirk, gbi.Schulkreis, gbi.Standort, gbi.Gebaeude, gbi.SO_Name, gbi.GBI_Name, gbi.GBI_Adresse, gbi.GBI_Bemerkungen, gbi.GS_Lang, pf.PF1_ID, pf.PF1_Lang, pf.PF2_ID, pf.PF2_Lang, pf.PF3_ID, pf.PF3_Lang, pf.PF4_ID, pf.PF4_Lang, gbi.GS_UID,  gbi.SO_Sort, gbi.GBI_Sort, gbi.SK_Sort, gbi.GS_Sort, pf.PF1_Sort, pf.PF2_Sort, pf.PF3_Sort, pf.PF4_Sort, gbi.SK_UID AS SK_ID, gbi.GBI_IsDenkmalschutz, gbi.GBI_BemDenkmalschutz, gbi.SO_ID, gbi.GBI_ID, gbi.GM_ID, gbi.KS_ID, gbi.VB_ID, gbi.GM_Sort, gbi.KS_Sort, gbi.VB_Sort " &
"FROM V_AP_BERICHT_GBI AS gbi LEFT OUTER JOIN V_AP_BERICHT_GBI_Portfolio AS pf ON gbi.GBI_ID = pf.GBI_ID " &
"WHERE 1=1 " &
IIF(Parameters!schulkreis.Value="00000000-0000-0000-0000-000000000000", "","AND gbi.SK_UID='" & Parameters!schulkreis.Value & "' ") &
IIF(Parameters!stadtkreis.Value="00000000-0000-0000-0000-000000000000", "","AND gbi.KS_ID='" & Parameters!stadtkreis.Value & "' ") &
IIF(Parameters!schuleinheit.Value ="00000000-0000-0000-0000-000000000000","AND 1=1 ","AND SE_UID='" & Parameters!schuleinheit.Value & "' ") & 
IIF(Parameters!portfolio1.Value="00000000-0000-0000-0000-000000000000", "","AND (pf.PF1_ID='" & Parameters!portfolio1.Value & "' ") & 
IIF(Parameters!portfolio1.Value="00000000-0000-0000-0000-000000000000", "","   OR pf.PF2_ID='" & Parameters!portfolio1.Value & "') ") & 
IIF(Parameters!subportfolio1.Value="00000000-0000-0000-0000-000000000000", "","AND (pf.PF3_ID='" & Parameters!subportfolio1.Value & "' ") &
IIF(Parameters!subportfolio1.Value="00000000-0000-0000-0000-000000000000", "","OR pf.PF4_ID='" & Parameters!subportfolio1.Value & "') ") &
IIF(Parameters!denkmalschutz.Value=2, "","AND gbi.GBI_IsDenkmalschutz=" & Parameters!denkmalschutz.Value & " ") &
IIF(Parameters!kontakt.Value="00000000-0000-0000-0000-000000000000", "","AND (gbi.GBI_ID IN (SELECT DPS_GBI_UID FROM dbo.T_Detail_Personen WHERE CONVERT(varchar(36), DPS_KF_UID) + CONVERT(varchar(36), DPS_KT_UID) ='" & Parameters!kontakt.Value & "' ))" )
