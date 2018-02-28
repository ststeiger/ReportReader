
using System.Runtime.InteropServices.ComTypes;

namespace ReportReader
{


    public class RdlReader
    {

        protected System.Xml.XmlDocument m_document;
        protected string m_xml;
        protected System.Xml.XmlNamespaceManager m_nsmgr;
        protected Xml2CSharp.Report m_report;


        public string ReportName;


        public System.Collections.Generic.Dictionary<string, Xml2CSharp.ReportParameter> Parameters;
        public System.Collections.Generic.Dictionary<string, Xml2CSharp.DataSet> DataSets;



        public RdlReader()
        {
            this.Parameters = new System.Collections.Generic.Dictionary<string, Xml2CSharp.ReportParameter>(
                System.StringComparer.OrdinalIgnoreCase
            );

            this.DataSets = new System.Collections.Generic.Dictionary<string, Xml2CSharp.DataSet>(
                System.StringComparer.OrdinalIgnoreCase
            );

        }

        public RdlReader OpenXml(string xml)
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.XmlResolver = null;
            document.PreserveWhitespace = true;
            document.LoadXml(xml);

            string default_namespace = Xml2CSharp.Report.DEFAULT_NAMESPACE;
            string designer_namespace = Xml2CSharp.Report.DESIGNER_NAMESPACE;

            if (document.DocumentElement.HasAttribute("xmlns"))
                default_namespace = document.DocumentElement.Attributes["xmlns"].Value;

            if (document.DocumentElement.HasAttribute("xmlns:rd"))
                designer_namespace = document.DocumentElement.Attributes["xmlns:rd"].Value;

            this.m_xml = xml
                .Replace(default_namespace, Xml2CSharp.Report.DEFAULT_NAMESPACE)
                .Replace(designer_namespace, Xml2CSharp.Report.DESIGNER_NAMESPACE);

            document = null;


            document = new System.Xml.XmlDocument();
            document.XmlResolver = null;
            document.PreserveWhitespace = true;
            document.LoadXml(this.m_xml);

            this.m_report = Xml2CSharp.Report.DeserializeXmlFromString(this.m_xml);
            // this.m_report.ToJSON("newton.json");

            this.m_document = document;
            this.m_nsmgr = GetReportNamespaceManager(this.m_document);



            if (this.m_report.DataSets != null)
            {
                foreach (Xml2CSharp.DataSet ds in this.m_report.DataSets.DataSet)
                {
                    this.DataSets[ds.Name] = ds;
                } // Next ds 
            }


            if (this.m_report.ReportParameters != null)
            {
                foreach (Xml2CSharp.ReportParameter rp in this.m_report.ReportParameters.ReportParameter)
                {
                    this.Parameters[rp.Name] = rp;

                    // System.Console.WriteLine(rp.Name);

                    // rp.Name
                    // rp.Prompt    
                    // rp.DataType
                    // rp.Hidden // Ausgeblendet - intern: no parameter prompt, hidden true

                    // rp.ValidValues.DataSetReference.DataSetName
                    // rp.ValidValues.DataSetReference.LabelField
                    // rp.ValidValues.DataSetReference.ValueField 
                    // foreach(var pv in rp.ValidValues.ParameterValues.ParameterValue) pv.Value, pv.Label

                    // rp.DefaultValue.DataSetReference.DataSetName 
                    // rp.DefaultValue.DataSetReference.LabelField
                    // rp.DefaultValue.DataSetReference.ValueField 

                    // rp.DefaultValue.Values.Value

                    // rp.UsedInQuery (refresh data when parameter changes)
                    //   -- null: Automatic
                    //   -- true: always
                    //   -- false: never

                } // Next rp 
            }

            return this;
        }


        public string ToJSON(System.Xml.XmlNode node, string fileName)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(node
                , Newtonsoft.Json.Formatting.Indented);

            // b["Report"]["DataSources"].DataSource
            // b["Report"]["DataSources"].DataSource.DataSourceReference
            // b["Report"]["DataSources"].DataSource["rd:DataSourceID"]


            // b["Report"].Code
            // b["Report"].Language
            // b["Report"].ConsumeContainerWhitespace
            // b["Report"]["rd:ReportUnitType"]
            // b["Report"]["rd:ReportID"]


            // b["Report"]["DataSets"]
            // b["Report"]["DataSets"].DataSet[0]
            // b["Report"]["DataSets"].DataSet[b["Report"]["DataSets"].DataSet.length-1]

            // b["Report"]["DataSets"].DataSet[0]["@Name"]
            // b["Report"]["DataSets"].DataSet[0].Query
            // b["Report"]["DataSets"].DataSet[0].Query.CommandText
            // b["Report"]["DataSets"].DataSet[0].Query.CommandType
            // b["Report"]["DataSets"].DataSet[0].Query.DataSourceName
            // b["Report"]["DataSets"].DataSet[0].Query.Timeout
            // b["Report"]["DataSets"].DataSet[0].Query["rd:UseGenericDesigner"]


            // b["Report"]["DataSets"].DataSet[0].Query.QueryParameters
            // b["Report"]["DataSets"].DataSet[0].Query.QueryParameters.QueryParameter

            // b["Report"]["DataSets"].DataSet[0].Query.QueryParameters.QueryParameter[0]
            // b["Report"]["DataSets"].DataSet[0].Query.QueryParameters.QueryParameter[0]["@Name"]
            // b["Report"]["DataSets"].DataSet[0].Query.QueryParameters.QueryParameter[0].Value


            // b["Report"]["DataSets"].DataSet[0].Fields
            // b["Report"]["DataSets"].DataSet[0].Fields.Field[0]
            // b["Report"]["DataSets"].DataSet[0].Fields.Field[0]["@Name"]
            // b["Report"]["DataSets"].DataSet[0].Fields.Field[0].DataField
            // b["Report"]["DataSets"].DataSet[0].Fields.Field[0]["rd:TypeName"]

            System.IO.File.WriteAllText(fileName, json, System.Text.Encoding.UTF8);
            // System.Console.WriteLine(json);

            json = "var a = \"" + System.Web.HttpUtility.JavaScriptStringEncode(json) + "\";";
            json += System.Environment.NewLine;
            json += "var b = JSON.parse(a)";


            string fnJS = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(fileName),
                System.IO.Path.GetFileNameWithoutExtension(fileName) + ".js"
            );

            System.IO.File.WriteAllText(fnJS, json, System.Text.Encoding.UTF8);

            return json;
        }


        public string ToJSON(System.Xml.XmlNode node)
        {
            return ToJSON(node, "newt.json");
        }


        public string ToJSON(string fileName)
        {
            return this.ToJSON(this.m_document, fileName);
        }


        public string ToJSON()
        {
            return ToJSON("newt.json");
        }


        public RdlReader OpenFile(string fileName)
        {
            string xml = System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8);
            this.ReportName = System.IO.Path.GetFileNameWithoutExtension(fileName);


            return this.OpenXml(xml);
        }


        private static System.Xml.XmlNamespaceManager GetReportNamespaceManager(System.Xml.XmlDocument doc)
        {
            if (doc == null)
                throw new System.ArgumentNullException(nameof(doc));

            System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager(doc.NameTable);

            // <Report xmlns="http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition" xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner">

            if (doc.DocumentElement != null)
            {
                // string strNamespace = doc.DocumentElement.NamespaceURI;
                // System.Console.WriteLine(strNamespace);
                // nsmgr.AddNamespace("dft", strNamespace);

                System.Xml.XPath.XPathNavigator xNav = doc.CreateNavigator();
                while (xNav.MoveToFollowing(System.Xml.XPath.XPathNodeType.Element))
                {
                    System.Collections.Generic.IDictionary<string, string> localNamespaces = xNav.GetNamespacesInScope(System.Xml.XmlNamespaceScope.Local);

                    foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in localNamespaces)
                    {
                        string prefix = kvp.Key;
                        if (string.IsNullOrEmpty(prefix))
                            prefix = "dft";

                        nsmgr.AddNamespace(prefix, kvp.Value);
                    } // Next kvp

                } // Whend



                //System.Xml.XmlNodeList _xmlNameSpaceList = doc.SelectNodes(@"//namespace::*[not(. = ../../namespace::*)]");

                //foreach (System.Xml.XmlNode currentNamespace in _xmlNameSpaceList)
                //{
                //    if (StringComparer.InvariantCultureIgnoreCase.Equals(currentNamespace.LocalName, "xmlns"))
                //    {
                //        nsmgr.AddNamespace("dft", currentNamespace.Value);
                //    }
                //    else
                //        nsmgr.AddNamespace(currentNamespace.LocalName, currentNamespace.Value);

                //}

                return nsmgr;
            } // End if (doc.DocumentElement != null)

            nsmgr.AddNamespace("dft", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
            // nsmgr.AddNamespace("dft", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");

            return nsmgr;
        } // End Function GetReportNamespaceManager


        public System.Collections.Generic.List<string> GetDataSets()
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();

            System.Xml.XmlNodeList xnProc = this.m_document
                .SelectNodes("/dft:Report/dft:DataSets/dft:DataSet", this.m_nsmgr);

            foreach (System.Xml.XmlNode ds in xnProc)
            {
                string aaa = ds.OuterXml;
                string bbb = this.ToJSON(ds);


                System.Console.WriteLine(bbb);
                System.Console.WriteLine(aaa);

                string name = ds.Attributes["Name"].Value;
                ls.Add(name);


            }

            return ls;
        } // End Function GetDataSets


        public System.Collections.Generic.List<string> GetParameters()
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();

            System.Xml.XmlNodeList xnProc = this.m_document
                .SelectNodes("/dft:Report/dft:ReportParameters/dft:ReportParameter"
                , this.m_nsmgr);

            foreach (System.Xml.XmlNode ds in xnProc)
            {
                string name = ds.Attributes["Name"].Value;
                ls.Add(name);
            }

            return ls;
        } // End Function GetParameters


        public bool HasDataSet(string dataSetName)
        {
            dataSetName = XmlTools.XmlEscape(dataSetName);

            System.Xml.XmlNode xnProc = this.m_document
                .SelectSingleNode("/dft:Report/dft:DataSets/dft:DataSet[@Name=\""
                                  + dataSetName + "\"]"
                    , this.m_nsmgr);

            return xnProc != null;
        } // End Function HasDataSet


        public string GetDataSetDefinition(string dataSetName)
        {
            dataSetName = XmlTools.XmlEscape(dataSetName);

            if (this.HasDataSet(dataSetName))
            {
                System.Xml.XmlNode xnSQL = this.m_document
                    .SelectSingleNode("/dft:Report/dft:DataSets/dft:DataSet[@Name=\""
                                         + dataSetName
                                         + "\"]/dft:Query/dft:CommandText"
                        , this.m_nsmgr);
                return xnSQL.InnerText;
            }

            return null;
        } // End Function GetDataSetDefinition


    }


}