
namespace ReportReader 
{ 


    class Program 
    {


        static void Main(string[] args)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string fn = "/var/opt/mssql/backup/SNB/SNB_Berichte/SNB_Berichte/";
            if (System.Environment.OSVersion.Platform != System.PlatformID.Unix)
                fn = @"D:\username\Documents\Visual Studio 2017\TFS\SQLReporting_VS2008\SNB_Berichte\SNB_Berichte\";
            
            string[] reports = System.IO.Directory.GetFiles(fn, "*.rdl", System.IO.SearchOption.TopDirectoryOnly);

            foreach (string report in reports)
            {
                string fileName = System.IO.Path.GetFileName(report);
                if (fileName.StartsWith("xxx", System.StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (fileName.StartsWith("yyy", System.StringComparison.InvariantCultureIgnoreCase))
                    continue;
                
                if (fileName.StartsWith("zzz", System.StringComparison.InvariantCultureIgnoreCase))
                    continue;

                ProcessReport(sb, report);
            } // Next report 

            string content = sb.ToString();
            System.IO.File.WriteAllText("docu.txt", content, System.Text.Encoding.UTF8);

            System.Console.WriteLine(" --- Press any key to continue --- ");
            while (!System.Console.KeyAvailable)
                System.Threading.Thread.Sleep(500);
        } // End Sub Main 


        static void ProcessReport(System.Text.StringBuilder sb, string fileName)
        {
            // Json2CSharp.Report rep = Json2CSharp.Report.FromJsonFile("newton.json");
            // System.Console.WriteLine(rep);
            
            RdlReader report = new RdlReader().OpenFile(fileName);
            // report.GetDataSets();
            // report.ToJSON();
            
            
            foreach (System.Collections.Generic.KeyValuePair<string, Xml2CSharp.ReportParameter> kvp in report.Parameters)
            {
                string name = kvp.Key;
                Xml2CSharp.ReportParameter rp = kvp.Value;
                
                // rp.Name
                // rp.Prompt
                
                
                // rp.DataType // DatenTyp
                // rp.AllowBlank // Leeren Wert ("") zulassen 
                // rp.Nullable // NULL-Wert zulassen
                // rp.MultiValue // Mehrere Werte zulassen
                
                // rp.Hidden // Ausgeblendet 
                
                // rp.ValidValues // Verfügbare Werte
                // Keiner / Werte Angeben / Werte aus Abfrage abrufen 
                // rp.DefaultValue // Standardwerte
                // Kein Standrartwert // Werte angeben // Werte aus Abfrage abrufen 
                
                // Erweitert
                // Aktualisierungszeitpunkt bestimmen
                // - Aktualisierungszeitpunkt automatisch bestimmen 
                // - Immer aktualisieren
                // - Nie aktualisieren
                
                // Berichtsteilbenachrichtigungen 
                // Benachrichtigen, wenn dieser Berichtsteil auf dem Server aktualisiert wird

                // rp.IsHidden
                // rp.IsTranslated
                
                
                // rp.PromptGerman
                
                // rp.UsedInQuery
                // rp.ValidValues.DataSetReference
                // rp.ValidValues.DataSetReference.DataSetName
            } // Next kvp 
            
            
            foreach (System.Collections.Generic.KeyValuePair<string, Xml2CSharp.DataSet> kvp in report.DataSets)
            {
                string name = kvp.Key;
                Xml2CSharp.DataSet ds = kvp.Value;
                
                foreach (Xml2CSharp.QueryParameter p in ds.Query.QueryParameters.QueryParameter)
                {
                    System.Console.WriteLine(p.Name);
                    System.Console.WriteLine(p.Value);
                } // Next p 
                
            } // Next kvp 
            
            
            foreach (Xml2CSharp.QueryParameter p in report.DataSets["foo"].Query.QueryParameters.QueryParameter)
            {
                
            } // Next p 
            
            
            
            System.Collections.Generic.HashSet<string> hs = 
                new System.Collections.Generic.HashSet<string>(
                    System.StringComparer.InvariantCultureIgnoreCase
            );
            
            hs.Add("DATA_Report_Translation");
            hs.Add("DATA_Report_Title");
            hs.Add("SEL_Report_Title");
            hs.Add("LANG_Report_Title");
            
            hs.Add("SEL_User");
            hs.Add("SEL_Benutzer");
            
            hs.Add("SEL_Image");
            hs.Add("LANG_DateFormatString");
            hs.Add("LANG_PageFormatString");
            
            
            sb.AppendLine(report.ReportName);
            
            
            sb.Append("  ");
            sb.Append((char)9672); // https://www.w3schools.com/charsets/ref_utf_geometric.asp
            sb.AppendLine(" Parameter:");
            
            foreach (string key in report.Parameters.Keys)
            {
                Xml2CSharp.ReportParameter para = report.Parameters[key];

                if (para.IsHidden)
                    continue;

                sb.Append("    -- ");
                sb.Append(para.PromptGerman);

                if (para.IsTranslated)
                    sb.Append(" (übersetzt)");
                else
                    sb.Append(" (NICHT übersetzt)");

                if (para.ValidValues != null)
                {
                    sb.Append(" (Dataset ");
                    if(para.ValidValues.DataSetReference == null)
                        sb.Append("Eingebettete Parameter");
                    else
                        sb.Append(para.ValidValues.DataSetReference.DataSetName);
                    sb.Append(") ");
                }
                else
                {
                    sb.Append(" (Default: ");
                    if(para.DefaultValue == null)
                        sb.Append("Nichts");
                    else
                        sb.Append(para.DefaultValue.Values.Value);

                    sb.Append(") ");
                }

                sb.AppendLine();
            } // Next key 


            sb.Append("  ");
            sb.Append((char)9672); // https://www.w3schools.com/charsets/ref_utf_geometric.asp
            sb.AppendLine(" DataSets:");
            foreach (string key in report.DataSets.Keys)
            {
                Xml2CSharp.DataSet ds = report.DataSets[key];
                
                if(hs.Contains(ds.Name)) // Default-report objects (LANG_, Translation, title)
                    continue;
                
                sb.Append("    -- ");
                sb.Append(ds.Name);
                
                //if (ds.Query.IsStoredProcedure)
                if (ds.Query.DbDependencies.Length > 0)
                {
                    sb.Append("  ");
                    sb.Append("  (");
                    sb.Append(ds.Query.DbDependencies);
                    sb.Append(")  ");
                } // End if (ds.Query.DbDependencies.Length > 0) 
                
                sb.AppendLine();
            } // Next key 
            
            sb.AppendLine(System.Environment.NewLine);
            sb.AppendLine(System.Environment.NewLine);
        } // End Sub Main 


    } // End Class Program 


} // End Namespace ReportReader 
