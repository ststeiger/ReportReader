
using System.Xml.Serialization;
using System.Collections.Generic;


namespace ReportReader.Xml2CSharp
{


    [XmlRoot(ElementName = "DataSource",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSource
    {
        [XmlElement(ElementName = "DataSourceReference",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataSourceReference { get; set; }

        [XmlElement(ElementName = "SecurityType",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string SecurityType { get; set; }

        [XmlElement(ElementName = "DataSourceID",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string DataSourceID { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "DataSources",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSources
    {
        [XmlElement(ElementName = "DataSource",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSource DataSource { get; set; }
    }

    [XmlRoot(ElementName = "QueryParameter",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class QueryParameter
    {
        [XmlElement(ElementName = "Value",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "QueryParameters",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class QueryParameters
    {
        [XmlElement(ElementName = "QueryParameter",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public List<QueryParameter> QueryParameter { get; set; }
    }

    [XmlRoot(ElementName = "Query",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class Query
    {
        [XmlElement(ElementName = "DataSourceName",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataSourceName { get; set; }

        [XmlElement(ElementName = "QueryParameters",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public QueryParameters QueryParameters { get; set; }

        [XmlElement(ElementName = "CommandType",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string CommandType { get; set; }

        public bool IsStoredProcedure
        {
            get
            {
                return string.Equals(this.CommandType, "StoredProcedure", System.StringComparison.InvariantCultureIgnoreCase);
            }
        }


        public string DbDependencies
        {
            get
            {
                string cmd = Helpers.RemoveSqlComments(this.CommandText);
                cmd = cmd.Replace("[", "").Replace("]", "");
                cmd = cmd.Replace("dbo.", "");

                string[] tokens = cmd.Split(new char[] { '\r','\n', ' ', '\t', '(', ')' }, System.StringSplitOptions.RemoveEmptyEntries);

                bool hasAll = false;

                List<string> ls = new List<string>();

                if (tokens.Length == 1)
                    ls.Add(cmd);
                else
                {
                    for (int i = 1; i < tokens.Length; ++i)
                    {
                        if (
                               string.Equals(tokens[i - 1], "FROM", System.StringComparison.InvariantCultureIgnoreCase)
                            || string.Equals(tokens[i - 1], "JOIN", System.StringComparison.InvariantCultureIgnoreCase)
                            || string.Equals(tokens[i - 1], "APPLY", System.StringComparison.InvariantCultureIgnoreCase)
                            || string.Equals(tokens[i - 1], "EXECUTE", System.StringComparison.InvariantCultureIgnoreCase)
                            || string.Equals(tokens[i - 1], "EXEC", System.StringComparison.InvariantCultureIgnoreCase)
                            )
                        {
                            if (
                                   tokens[i].StartsWith("V_", System.StringComparison.InvariantCultureIgnoreCase)
                                || tokens[i].StartsWith("tfu_", System.StringComparison.InvariantCultureIgnoreCase)
                                || tokens[i].StartsWith("sp_", System.StringComparison.InvariantCultureIgnoreCase)
                                || tokens[i].StartsWith("T_", System.StringComparison.InvariantCultureIgnoreCase)

                                )
                            {
                                if (!string.Equals(tokens[i], "tfu_rpt_sel_alle", System.StringComparison.InvariantCultureIgnoreCase))
                                    ls.Add(tokens[i]);
                                else
                                    hasAll = true;
                            }
                        } // End if FROM, JOIN, APPLY
                    } // Next i 
                }

                if (ls.Count == 0 && hasAll)
                    ls.Add("Alle");

                return string.Join(", ", ls.ToArray());
            }
        }


        [XmlElement(ElementName = "CommandText",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string CommandText { get; set; }

        [XmlElement(ElementName = "Timeout",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public int Timeout { get; set; }

        [XmlElement(ElementName = "UseGenericDesigner",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string UseGenericDesigner { get; set; }
    }

    [XmlRoot(ElementName = "Field",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class Field
    {
        [XmlElement(ElementName = "DataField",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataField { get; set; }

        [XmlElement(ElementName = "TypeName",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string TypeName { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "Fields",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class Fields
    {
        [XmlElement(ElementName = "Field",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public List<Field> Field { get; set; }
    }

    [XmlRoot(ElementName = "DataSet",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSet
    {
        [XmlElement(ElementName = "Query",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public Query Query { get; set; }

        [XmlElement(ElementName = "Fields",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public Fields Fields { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "DataSets",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSets
    {
        [XmlElement(ElementName = "DataSet",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public List<DataSet> DataSet { get; set; }
    }

    [XmlRoot(ElementName = "Values",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class Values
    {
        [XmlElement(ElementName = "Value", 
            Namespace = Report.DEFAULT_NAMESPACE)]
        public List<string> Value { get; set; }
    }

    [XmlRoot(ElementName = "DefaultValue",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DefaultValue
    {
        [XmlElement(ElementName = "DataSetReference",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSetReference DataSetReference { get; set; }

        [XmlElement(ElementName = "Values",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public Values Values { get; set; }
    }

    [XmlRoot(ElementName = "ReportParameter",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class ReportParameter
    {
        [XmlElement(ElementName = "DataType",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataType { get; set; }

        [XmlElement(ElementName = "DefaultValue",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DefaultValue DefaultValue { get; set; }

        [XmlElement(ElementName = "Prompt",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Prompt { get; set; }


        public bool IsTranslated
        {
            get
            {
                return this.Prompt != null && this.Prompt.Contains("/");
            }
        }


        public string PromptGerman
        {
            get
            {
                if (!this.IsTranslated)
                    return this.Prompt;

                string[] prompts = this.Prompt.Split('/');
                if (prompts[0] != null)
                    prompts[0] = prompts[0].Trim();

                return prompts[0];
            }
        }



        [XmlElement(ElementName = "Hidden",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Hidden { get; set; }
        
        public bool IsHidden
        {
            get {

                return string.Equals(this.Hidden, "true", System.StringComparison.InvariantCultureIgnoreCase);
            }
        }
        


        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }


        [XmlElement(ElementName = "UsedInQuery", Namespace = Report.DEFAULT_NAMESPACE)]
        public string UsedInQuery { get; set; }

        [XmlElement(ElementName = "ValidValues",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public ValidValues ValidValues { get; set; }

        [XmlElement(ElementName = "MultiValue", Namespace = Report.DEFAULT_NAMESPACE)]
        public string MultiValue { get; set; }

        [XmlElement(ElementName = "Nullable", Namespace = Report.DEFAULT_NAMESPACE)]
        public string Nullable { get; set; }

        [XmlElement(ElementName = "AllowBlank", Namespace = Report.DEFAULT_NAMESPACE)]
        public string AllowBlank { get; set; }
    }

    [XmlRoot(ElementName = "DataSetReference",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSetReference
    {
        [XmlElement(ElementName = "DataSetName",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataSetName { get; set; }

        [XmlElement(ElementName = "ValueField",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string ValueField { get; set; }

        [XmlElement(ElementName = "LabelField",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string LabelField { get; set; }
    }


    [XmlRoot(ElementName = "ParameterValue", Namespace = Report.DEFAULT_NAMESPACE)]
    public class ParameterValue
    {
        [XmlElement(ElementName = "Value", Namespace = Report.DEFAULT_NAMESPACE)]
        public string Value { get; set; }
        [XmlElement(ElementName = "Label", Namespace = Report.DEFAULT_NAMESPACE)]
        public string Label { get; set; }
    }


    [XmlRoot(ElementName = "ParameterValues", Namespace = Report.DEFAULT_NAMESPACE)]
    public class ParameterValues
    {
        [XmlElement(ElementName = "ParameterValue", Namespace = Report.DEFAULT_NAMESPACE)]
        public List<ParameterValue> ParameterValue { get; set; }
    }


    [XmlRoot(ElementName = "ValidValues",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class ValidValues
    {
        [XmlElement(ElementName = "DataSetReference",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSetReference DataSetReference { get; set; }


        [XmlElement(ElementName = "ParameterValues", Namespace = Report.DEFAULT_NAMESPACE)]
        public ParameterValues ParameterValues { get; set; }
    }

    [XmlRoot(ElementName = "ReportParameters",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class ReportParameters
    {
        [XmlElement(ElementName = "ReportParameter",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public List<ReportParameter> ReportParameter { get; set; }
    }

    [XmlRoot(ElementName = "Report",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public partial class Report
    {
        public const string DEFAULT_NAMESPACE = "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition";
        public const string DESIGNER_NAMESPACE = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";


        [XmlElement(ElementName = "AutoRefresh",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string AutoRefresh { get; set; }

        [XmlElement(ElementName = "DataSources",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSources DataSources { get; set; }

        [XmlElement(ElementName = "DataSets",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSets DataSets { get; set; }

        [XmlElement(ElementName = "ReportParameters",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public ReportParameters ReportParameters { get; set; }

        [XmlElement(ElementName = "Code",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Code { get; set; }

        [XmlElement(ElementName = "Language",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Language { get; set; }

        [XmlElement(ElementName = "ConsumeContainerWhitespace",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string ConsumeContainerWhitespace { get; set; }

        [XmlElement(ElementName = "ReportUnitType",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string ReportUnitType { get; set; }

        [XmlElement(ElementName = "ReportID",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string ReportID { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "rd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Rd { get; set; }
    }


    public partial class Report
    {


        public string ToJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }


        public void ToJSON(string fileNameAndPath)
        {
            string json = this.ToJSON();
            System.IO.File.WriteAllText(fileNameAndPath, json, System.Text.Encoding.UTF8);
        }

        

        public void SerializeToXml(System.IO.TextWriter tw)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Report));

            using (System.IO.TextWriter twTextWriter = tw)
            {
                serializer.Serialize(twTextWriter, this);
                twTextWriter.Close();
            } // End Using twTextWriter

            serializer = null;
        } // End Sub SerializeToXml


        public void SerializeToXml(string fileNameAndPath)
        {

            using (System.IO.FileStream fs = System.IO.File.OpenWrite(fileNameAndPath))
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
                {
                    SerializeToXml(sw);
                } // End Using sw
            } // End Using fs

        } // End Sub SerializeToXml


        public string SerializeToXml()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string strReturnValue = null;

            SerializeToXml(new System.IO.StringWriter(sb));

            strReturnValue = sb.ToString();
            sb = null;

            return strReturnValue;
        } // End Function SerializeToXml
        


        ///////////////////////////////////////



        public static Report DeserializeXmlFromStream(System.IO.Stream strm)
        {
            System.Xml.Serialization.XmlSerializer deserializer = new System.Xml.Serialization.XmlSerializer(typeof(Report));
            Report ThisType = default(Report);

            using (System.IO.StreamReader srEncodingReader = new System.IO.StreamReader(strm, System.Text.Encoding.UTF8))
            {
                ThisType = (Report)deserializer.Deserialize(srEncodingReader);
                srEncodingReader.Close();
            } // End Using srEncodingReader

            deserializer = null;

            return ThisType;
        } // End Function DeserializeXmlFromStream
        

        public static Report DeserializeXmlFromEmbeddedRessource(string strRessourceName)
        {
            Report tReturnValue = default(Report);

            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();

            using (System.IO.Stream fstrm = ass.GetManifestResourceStream(strRessourceName))
            {
                tReturnValue = DeserializeXmlFromStream(fstrm);
                fstrm.Close();
            } // End Using fstrm

            return tReturnValue;
        } // End Function DeserializeXmlFromEmbeddedRessource


        public static Report DeserializeXmlFromFile(string fileName)
        {
            Report tReturnValue = default(Report);

            using (System.IO.FileStream fstrm = new System.IO.FileStream(fileName, System.IO.FileMode.Open
                , System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                tReturnValue = DeserializeXmlFromStream(fstrm);
                fstrm.Close();
            } // End Using fstrm

            return tReturnValue;
        } // End Function DeserializeXmlFromFile


        public static Report DeserializeXmlFromString(string s)
        {
            Report tReturnValue = default(Report);

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream))
                {
                    writer.Write(s);
                    writer.Flush();
                    stream.Position = 0;

                    tReturnValue = DeserializeXmlFromStream(stream);
                } // End Using writer

            } // End Using stream

            return tReturnValue;
        } // End Function DeserializeXmlFromString


    }


}
