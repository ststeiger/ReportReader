﻿


namespace Fw4csReportExecution
{
    static class Program
    {
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

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }
    }
}

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
