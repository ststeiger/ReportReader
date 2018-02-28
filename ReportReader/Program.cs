
namespace ReportReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string fn = "/var/opt/mssql/backup/SNB/SNB_Berichte/SNB_Berichte/AL_Anlageblatt.rdl";

            if (System.Environment.OSVersion.Platform != System.PlatformID.Unix)
                fn = @"D:\username\Documents\Visual Studio 2017\Projects\ReportReader\ReportReader\AL_Anlageblatt.rdl";

            RdlReader report = new RdlReader().OpenFile(fn);
            // report.GetDataSets();
            report.ToJSON();

            Json2CSharp.Report rep = Json2CSharp.Report.FromJsonFile("newton.json");
            System.Console.WriteLine(rep);

            System.Console.WriteLine(" --- Press any key to continue --- ");
            while (!System.Console.KeyAvailable)
                System.Threading.Thread.Sleep(500);
        }
    }
}