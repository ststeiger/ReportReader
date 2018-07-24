
namespace Fw4csReportExecution
{


    static class Program
    {


        public static System.Collections.Generic.Dictionary<string, string> MapConfigTypes()
        {
            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>(
                System.StringComparer.InvariantCultureIgnoreCase
                );

            // .NET-Core to .NET 4
            dict.Add("System.Uri, System.Private.Uri", "System.Uri, System");
            dict.Add("System.String, System.Private.CoreLib", "System.String, mscorlib");
            dict.Add("System.Int32, System.Private.CoreLib", "System.Int32, System");
            dict.Add("System.Boolean, System.Private.CoreLib", "System.Boolean, mscorlib");
            dict.Add("System.Xml.XmlDocument, System.Private.Xml", "System.Xml.XmlDocument, System.Xml");

            // .NET 4 to .NET-Core 
            dict.Add("System.Uri, System", "System.Uri, System.Private.Uri");
            dict.Add("System.String, mscorlib", "System.String, System.Private.CoreLib");
            dict.Add("System.Int32, System", "System.Int32, System.Private.CoreLib");
            dict.Add("System.Boolean, mscorlib", "System.Boolean, System.Private.CoreLib");
            dict.Add("System.Xml.XmlDocument, System.Xml", "System.Xml.XmlDocument, System.Private.Xml");

            // .NET-Core: 
            // System.Uri, System.Private.Uri
            // System.String, System.Private.CoreLib
            // System.Int32, System.Private.CoreLib
            // System.Boolean, System.Private.CoreLib
            // System.Xml.XmlDocument, System.Private.Xml

            // .NET 4.0: 
            // System.Uri, System
            // System.String, mscorlib
            // System.Int32, System
            // System.Boolean, mscorlib
            // System.Xml.XmlDocument, System.Xml


            string sql = @"

SELECT * FROM T_FMS_Configuration 
WHERE FC_Type NOT IN 
(
     'System.Uri, System'
    ,'System.String, mscorlib'
    ,'System.Int32, System' 
    ,'System.Boolean, mscorlib'
    ,'System.Xml.XmlDocument, System.Xml' 
)

-- UPDATE T_FMS_Configuration SET FC_Type = 'System.String, mscorlib' WHERE FC_Type = 'System.String'; 
-- UPDATE T_FMS_Configuration SET FC_Type = 'System.String, mscorlib' WHERE FC_Type = 'System.String, System'; 

";

            return dict;
        }


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
#if false
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Form1());
#endif
            // PdfIndexer.Start(@"D:\username\Desktop\Books\sammel");

            object result = TestCompiler.Eval("Return (1+1).ToString() + \"px\" ");
            System.Console.WriteLine(result);

            result = null;
            result = TestCompilerCSharp.Eval("return (1+1).ToString() + \"px\"; ");
            System.Console.WriteLine(result);

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


    } // End Class Program 


} // End Namespace Fw4csReportExecution 

/*
 ="SELECT 2 AS ID, 2 AS Sort, 'Alle' AS Name " &
"UNION " &
"SELECT  DISTINCT  GBI_IsDenkmalschutz AS ID, GBI_IsDenkmalschutz AS Sort, 'Ja' AS Name " &
"FROM         V_AP_BERICHT_SEL_GBI_Status_Denkmal  " &
"WHERE GBI_IsDenkmalschutz = 1  " &
"UNION " &
"SELECT  DISTINCT  GBI_IsDenkmalschutz AS ID, GBI_IsDenkmalschutz AS Sort, 'Nein' AS Name " &
"FROM         V_AP_BERICHT_SEL_GBI_Status_Denkmal " &
"WHERE GBI_IsDenkmalschutz = 0 " &
"ORDER BY Sort DESC "
*/
