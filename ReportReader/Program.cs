
namespace ReportReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string fn = "/var/opt/mssql/backup/SNB/SNB_Berichte/SNB_Berichte/AL_Anlageblatt.rdl";
            RdlReader report = new RdlReader().OpenFile(fn);
            // report.GetDataSets();
            report.ToJSON();

            string json = System.IO.File.ReadAllText("newt.json");
            OmgJSON.RootObject rep = Newtonsoft.Json.JsonConvert.DeserializeObject<OmgJSON.RootObject>(json);
            System.Console.WriteLine(rep);
            
            
            foreach (OmgJSON.DataSet ds in rep.Report.DataSets.DataSet)
            {
                object para = ds.Query.QueryParameters.QueryParameter;
                var t = para.GetType();
                if(!object.ReferenceEquals(t, typeof(Newtonsoft.Json.Linq.JArray)))
                    System.Console.WriteLine(t.FullName);
            }
            
            
            
            
            
            
            System.Console.WriteLine(" --- Press any key to continue --- ");
            while (!System.Console.KeyAvailable)
                System.Threading.Thread.Sleep(500);
        }
    }
}