
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
