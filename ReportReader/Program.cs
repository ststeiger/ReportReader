
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DB.Topology;

namespace ReportReader 
{

    public class ReportDetails
    {
        //public System.Collections.Generic.Dictionary<string, string> Dependencies;
        public System.Collections.Generic.List<string> Dependencies;
        public int Index;
        public string Name;
    }


    public class ReportData
    {
        public System.Collections.Generic.Dictionary<string, ReportDetails> Parameters;
    }


    class mygraph
    {

        public class wrap
        {
            protected  string m_x;
            protected  mygraph m_graph;
            
            public wrap(mygraph graph, string x)
            {
                this.m_graph = graph;
                this.m_x = x;
            }

            public wrap und(string b)
            {
                //m_graph.
                return this;
            }
            

            public wrap HängtAbVon(string b)
            {
                //m_graph.
                return this;
            }
        }

        public wrap Entity(string a)
        {
            return new wrap(this, a);
        }

        
    }
    
    
    // Read parameter names, types (textbox, dropdown), type
    // {
    //     parameters: [{ name: in_standort, displayType: dropdown, dataType:string 
    //                    multi: false/true , index: 1
    //                  }
    //     ]
    //     values: { proc: [], in_standort: [], in_gebaeude: [], in_stichtag: []}
    // }
    
    
    class Program 
    {

        public static void simpleex()
        {
            var sorter = new DB.Topology.TopologicalSorter(5);
            sorter.AddVertex(0);
            sorter.AddVertex(1);
            sorter.AddVertex(2);
            sorter.AddVertex(3);
            sorter.AddVertex(4);
            
            
            // tabellen, views, funktionen
            // graph.object("taskunion").hängtAbVon("gb,so");
            
            sorter.AddEdge(2, 3); // 3 hängt von 2 ab
            sorter.AddEdge(0, 1); // 1 hängt von 0 ab
            
            int[] res = sorter.Sort();
            // res = res.Reverse().ToArray();
            System.Console.WriteLine(res);
        }

        static void Main(string[] args)
        {
            // tabellen, views, funktionen
            // graph.object("taskunion").hängtAbVon("gb,so");
            
            // mygraph graph = new mygraph();
            // graph.Entity("union").HängtAbVon("tabelle1").und("tabelle2").und("tabelle3");
            
            
            System.Collections.Generic.Dictionary<string, int> dict = 
                new System.Collections.Generic.Dictionary<string, int>(
                System.StringComparer.InvariantCultureIgnoreCase
            );
            
            var ls = new System.Collections.Generic.List<DB.Topology.TopologicObject>();
            
            ls.Add(new TopologicObject("bar"));
            ls.Add(new TopologicObject("foo"));
            
            ls.Add(new DB.Topology.TopologicObject("foobar")
                    .DependsOn("foo").DependsOn("bar")
            );
            
            ls.Add(new TopologicObject("omg").DependsOn("foo"));

            
            for (int i = 0; i < ls.Count; ++i)
            {
                ls[i].Index = i;
                dict.Add(ls[i].Name, i);
            }
            
            
            for (int i = 0; i < ls.Count; ++i)
            {
                for (int j = 0; j < ls[i].Dependencies.Count; ++j)
                {
                    int key = dict[ls[i].Dependencies[j].Name];
                    ls[i].Dependencies[j].Index = key;
                } // Next j 
            } // Next i 



            var hsAllObjects = new HashSet<int>();
            var hsDependence = new HashSet<Tuple<int, int>>();
            
            // new HashSet<Tuple<int, int>>(Tuple.Create(7, 11),
            
            var sorter = new DB.Topology.TopologicalSorter(ls.Count);
            
            
            for (int i = 0; i < ls.Count; ++i)
            {
                hsAllObjects.Add(i);
                sorter.AddVertex(i);
            }
            
            for (int i = 0; i < ls.Count; ++i)
            {
                for (int j = 0; j < ls[i].Dependencies.Count; ++j)
                {
                    sorter.AddEdge(ls[i].Dependencies[j].Index, i); // i depends on Dependency[j] 
                    // sorter.AddEdge(i, ls[i].Dependencies[j].Index); // i depends on Dependency[j] 
                    
                    hsDependence.Add(Tuple.Create(ls[i].Dependencies[j].Index, i));
                } // Next j 
                
                // if (ls[i].Dependencies.Count == 0) sorter.AddEdge(i, i);
            } // Next i 
            
            System.Diagnostics.Stopwatch sw= new Stopwatch();
            sw.Start();
            var ret = SortAlgorithms.Topological.Kahn.Sort(hsAllObjects,hsDependence);
            sw.Stop();
            long durKahn = sw.ElapsedMilliseconds;
            
            string[] sret = new string[ret.Count];
            
            sw.Reset();
            sw.Start();
            int[] res = sorter.Sort();
            sw.Stop();
            long durMat = sw.ElapsedMilliseconds;
            
            string[] sres = new string[res.Length];
            for (int i = 0; i < sres.Length; ++i)
            {
                sres[i] = ls[res[i]].Name;
                sret[i] = ls[ret[i]].Name;
            }
            
            // res = res.Reverse().ToArray();
            // And the winner is: NOT KAHN !
            System.Console.WriteLine("{0}, {1}", durKahn, durMat);
            System.Console.WriteLine(sres);
            System.Console.WriteLine(sret);
            
            
            
            //ReportData x = new ReportData();
            //foreach (string dep in x.Parameters[""].Dependencies)
            //{

            //}

            string aqn = typeof(System.Xml.XmlDocument).AssemblyQualifiedName;
            System.Console.WriteLine(aqn);


            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string fn = "/var/opt/mssql/backup/SNB/SNB_Berichte/SNB_Berichte/";
            if (System.Environment.OSVersion.Platform != System.PlatformID.Unix)
                // fn = @"D:\username\Documents\Visual Studio 2017\TFS\SQLReporting_VS2008\SNB_Berichte\SNB_Berichte\";
                fn = @"D:\username\Documents\Visual Studio 2017\TFS\SQLReporting_VS2008\Test\BeriProj1\BeriProj1\";

            string rep = System.IO.Path.Combine(fn, "UPS_Ist_Kosten_Standort.rdl");

            ProcessReport(sb, rep);


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


        static System.Collections.Generic.HashSet<string> GetDefaultReportObjects()
        {
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

            return hs;
        } // End Function GetDefaultReportObjects 


        static void ProcessReport(System.Text.StringBuilder sb, string fileName)
        {
            // Json2CSharp.Report rep = Json2CSharp.Report.FromJsonFile("newton.json");
            // System.Console.WriteLine(rep);

            RdlReader report = null;


            try
            {
                report = new RdlReader().OpenFile(fileName);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return;
            }

            
            // report.GetDataSets();
            

            foreach (System.Collections.Generic.KeyValuePair<string, Xml2CSharp.ReportParameter> kvp in report.Parameters)
            {
                string name = kvp.Key;
                Xml2CSharp.ReportParameter rp = kvp.Value;
                System.Console.WriteLine(rp);


                if (rp.ValidValues != null && rp.ValidValues.ParameterValues != null)
                {

                    foreach (Xml2CSharp.ParameterValue def in rp.ValidValues.ParameterValues.ParameterValue)
                    {
                        System.Console.WriteLine(def.Label); 
                        System.Console.WriteLine(def.Value); 
                    } // Next def 

                } // End if (rp.ValidValues != null && rp.ValidValues.ParameterValues != null) 


                if (rp.DefaultValue != null && rp.DefaultValue.Values != null)
                {

                    foreach (string def in rp.DefaultValue.Values.Value)
                    { 
                        System.Console.WriteLine(def); 
                    } // Next def 

                } // End if (rp.DefaultValue != null && rp.DefaultValue.Values != null) 

                if (rp.ValidValues != null && rp.ValidValues.DataSetReference != null)
                {
                    System.Console.WriteLine(rp.ValidValues.DataSetReference.DataSetName);
                    System.Console.Write("Label:     ");
                    System.Console.WriteLine(rp.ValidValues.DataSetReference.LabelField);
                    System.Console.Write("Value:     ");
                    System.Console.WriteLine(rp.ValidValues.DataSetReference.ValueField);
                }


                if (rp.DefaultValue != null && rp.DefaultValue.DataSetReference != null)
                { 
                    System.Console.WriteLine(rp.DefaultValue.DataSetReference.DataSetName);
                    // System.Console.WriteLine(rp.DefaultValue.DataSetReference.LabelField); // LabelField: Always NULL 
                    System.Console.Write("Value:     ");
                    System.Console.WriteLine(rp.DefaultValue.DataSetReference.ValueField);
                }



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

            System.Console.WriteLine("All Parameters");

            foreach (System.Collections.Generic.KeyValuePair<string, Xml2CSharp.DataSet> kvp in report.DataSets)
            {
                string name = kvp.Key;
                Xml2CSharp.DataSet ds = kvp.Value;

                
                System.Console.WriteLine(ds.Name);
                System.Console.WriteLine(ds.Name);

                // ds.Fields
                foreach (Xml2CSharp.Field thisField in ds.Fields.Field)
                {
                    System.Console.WriteLine(thisField.Name);
                    System.Console.WriteLine(thisField.DataField);
                    System.Console.WriteLine(thisField.TypeName);
                }


                System.Console.WriteLine(ds.Query.CommandType); // StoredProcedure OR null 
                System.Console.WriteLine(ds.Query.CommandText);
                System.Console.WriteLine(ds.Query.DataSourceName);
                // ds.Query.IsStoredProcedure
                // ds.Query.DbDependencies
                // ds.Query.Timeout
                // 
                System.Console.WriteLine(ds.Query.UseGenericDesigner);
                // System.Console.WriteLine(ds.Query.DbDependencies);

                if (ds.Query.QueryParameters != null)
                {

                    foreach (Xml2CSharp.QueryParameter para in ds.Query.QueryParameters.QueryParameter)
                    {
                        System.Console.WriteLine(para.Name); // <-- this is just the parametername of the stored procedure
                        System.Console.WriteLine(para.Value); // <-- this is where the value actually comes from 
                    } // Next para 

                } // End if (ds.Query.QueryParameters != null) 
                
            } // Next kvp 


            try
            {
                foreach (Xml2CSharp.QueryParameter p in report.DataSets["foo"].Query.QueryParameters.QueryParameter)
                {
                    System.Console.WriteLine(p.Name);
                    System.Console.WriteLine(p.Value);
                } // Next p 
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            


            System.Collections.Generic.HashSet<string> hs = GetDefaultReportObjects();

            sb.AppendLine(report.ReportName);


            sb.Append("  ");
            // https://www.quackit.com/html/html_special_characters.cfm
            // &#9672;   WHITE DIAMOND CONTAINING BLACK SMALL DIAMOND
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
            // ◈ 9672 WHITE DIAMOND CONTAINING BLACK SMALL DIAMOND
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
