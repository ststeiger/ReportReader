
namespace ReportReader.Xml2CSharp
{


    [System.Xml.Serialization.XmlRoot(ElementName = "DataSource",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSource
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataSourceReference",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataSourceReference { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "SecurityType",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string SecurityType { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "DataSourceID",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string DataSourceID { get; set; }

        [System.Xml.Serialization.XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "DataSources",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSources
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataSource",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSource DataSource { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "QueryParameter",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class QueryParameter
    {
        [System.Xml.Serialization.XmlElement(ElementName = "Value",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Value { get; set; }

        [System.Xml.Serialization.XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "QueryParameters",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class QueryParameters
    {
        [System.Xml.Serialization.XmlElement(ElementName = "QueryParameter",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public System.Collections.Generic.List<QueryParameter> QueryParameter { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "Query",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class Query
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataSourceName",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataSourceName { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "QueryParameters",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public QueryParameters QueryParameters { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "CommandType",
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

                System.Collections.Generic.List<string> ls = 
                    new System.Collections.Generic.List<string>();

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
                } // End else of if (tokens.Length == 1) 

                if (ls.Count == 0 && hasAll)
                    ls.Add("Alle");

                return string.Join(", ", ls.ToArray());
            } // End Get 

        } // End Property DbDependencies 


        [System.Xml.Serialization.XmlElement(ElementName = "CommandText",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string CommandText { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Timeout",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public int Timeout { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "UseGenericDesigner",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string UseGenericDesigner { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "Field",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class Field
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataField",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataField { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "TypeName",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string TypeName { get; set; }

        [System.Xml.Serialization.XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "Fields",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class Fields
    {
        [System.Xml.Serialization.XmlElement(ElementName = "Field",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public System.Collections.Generic.List<Field> Field { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "DataSet",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSet
    {
        [System.Xml.Serialization.XmlElement(ElementName = "Query",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public Query Query { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Fields",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public Fields Fields { get; set; }

        [System.Xml.Serialization.XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "DataSets",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSets
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataSet",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public System.Collections.Generic.List<DataSet> DataSet { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "Values",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class Values
    {
        [System.Xml.Serialization.XmlElement(ElementName = "Value", 
            Namespace = Report.DEFAULT_NAMESPACE)]
        public System.Collections.Generic.List<string> Value { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "DefaultValue",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DefaultValue
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataSetReference",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSetReference DataSetReference { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Values",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public Values Values { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "ReportParameter",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class ReportParameter
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataType",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataType { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "DefaultValue",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DefaultValue DefaultValue { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Prompt",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Prompt { get; set; }


        public bool IsTranslated
        {
            get
            {
                return this.Prompt != null && this.Prompt.Contains("/");
            }
        }


        protected string[] m_prompts;

        public string[] Prompts
        {
            get
            {
                if (this.m_prompts != null)
                    return this.m_prompts;

                if (!this.IsTranslated)
                    return this.m_prompts;

                this.m_prompts = this.Prompt.Split('/');

                for (int i = 0; i < this.m_prompts.Length; ++i)
                {
                    if (this.m_prompts[i] != null)
                        this.m_prompts[i] = this.m_prompts[i].Trim();
                } // Next i 

                return this.m_prompts;
            } // End Get 

        } // End Property Prompts 


        protected string m_prompt_DE;

        public string Prompt_DE
        {
            get
            {
                if(this.m_prompt_DE != null)
                    return this.m_prompt_DE;

                if (this.Prompts != null && this.Prompts.Length> 0)
                    this.m_prompt_DE = this.Prompts[0];
                else
                    this.m_prompt_DE = this.Prompt;

                return this.m_prompt_DE;
            }
        }


        protected string m_prompt_FR;

        public string Prompt_FR
        {
            get
            {
                if (this.m_prompt_FR != null)
                    return this.m_prompt_FR;

                if (this.Prompts != null && this.Prompts.Length > 1)
                    this.m_prompt_FR = this.Prompts[1];
                else this.m_prompt_FR = this.Prompt;

                return this.m_prompt_FR;
            }
        }


        protected string m_prompt_IT;

        public string Prompt_IT
        {
            get
            {
                if (this.m_prompt_IT != null)
                    return this.m_prompt_IT;

                if (this.Prompts != null && this.Prompts.Length > 2)
                    this.m_prompt_IT = this.Prompts[2];
                else
                    this.m_prompt_IT = this.Prompt;

                return this.m_prompt_IT;
            }
        }


        protected string m_prompt_EN;

        public string Prompt_EN
        {
            get
            {
                if (this.m_prompt_EN != null)
                    return this.m_prompt_EN;

                if (this.Prompts != null && this.Prompts.Length > 3)
                    this.m_prompt_EN = this.Prompts[3];
                else
                    this.m_prompt_EN = this.Prompt;

                return this.m_prompt_EN;
            }
        }


        
        protected System.Collections.Generic.Dictionary<string, string> m_promptDict;

        [System.Xml.Serialization.XmlIgnore()]
        public System.Collections.Generic.Dictionary<string, string> PromptDictionary
        {
            get
            {
                if (this.m_promptDict != null)
                    return this.m_promptDict;

                this.m_promptDict = new System.Collections.Generic.Dictionary<string, string>(
                    System.StringComparer.InvariantCultureIgnoreCase);

                this.m_promptDict["DE"] = this.Prompt_DE;
                this.m_promptDict["FR"] = this.Prompt_FR;
                this.m_promptDict["IT"] = this.Prompt_IT;
                this.m_promptDict["EN"] = this.Prompt_EN;

                return this.m_promptDict;
            }
        }
        

        [System.Xml.Serialization.XmlElement(ElementName = "Hidden",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Hidden { get; set; }
        
        public bool IsHidden
        {
            get {

                return string.Equals(this.Hidden, "true", System.StringComparison.InvariantCultureIgnoreCase);
            }
        }
        


        [System.Xml.Serialization.XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }


        [System.Xml.Serialization.XmlElement(ElementName = "UsedInQuery", Namespace = Report.DEFAULT_NAMESPACE)]
        public string UsedInQuery { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "ValidValues",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public ValidValues ValidValues { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "MultiValue", Namespace = Report.DEFAULT_NAMESPACE)]
        public string MultiValue { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Nullable", Namespace = Report.DEFAULT_NAMESPACE)]
        public string Nullable { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "AllowBlank", Namespace = Report.DEFAULT_NAMESPACE)]
        public string AllowBlank { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "DataSetReference",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class DataSetReference
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataSetName",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string DataSetName { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "ValueField",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string ValueField { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "LabelField",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string LabelField { get; set; }
    }


    [System.Xml.Serialization.XmlRoot(ElementName = "ParameterValue", Namespace = Report.DEFAULT_NAMESPACE)]
    public class ParameterValue
    {
        [System.Xml.Serialization.XmlElement(ElementName = "Value", Namespace = Report.DEFAULT_NAMESPACE)]
        public string Value { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "Label", Namespace = Report.DEFAULT_NAMESPACE)]
        public string Label { get; set; }
    }


    [System.Xml.Serialization.XmlRoot(ElementName = "ParameterValues", Namespace = Report.DEFAULT_NAMESPACE)]
    public class ParameterValues
    {
        [System.Xml.Serialization.XmlElement(ElementName = "ParameterValue", Namespace = Report.DEFAULT_NAMESPACE)]
        public System.Collections.Generic.List<ParameterValue> ParameterValue { get; set; }
    }


    [System.Xml.Serialization.XmlRoot(ElementName = "ValidValues",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class ValidValues
    {
        [System.Xml.Serialization.XmlElement(ElementName = "DataSetReference",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSetReference DataSetReference { get; set; }


        [System.Xml.Serialization.XmlElement(ElementName = "ParameterValues", Namespace = Report.DEFAULT_NAMESPACE)]
        public ParameterValues ParameterValues { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "ReportParameters",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public class ReportParameters
    {
        [System.Xml.Serialization.XmlElement(ElementName = "ReportParameter",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public System.Collections.Generic.List<ReportParameter> ReportParameter { get; set; }
    }

    [System.Xml.Serialization.XmlRoot(ElementName = "Report",
        Namespace = Report.DEFAULT_NAMESPACE)]
    public partial class Report
    {
        public const string DEFAULT_NAMESPACE = "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition";
        public const string DESIGNER_NAMESPACE = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";


        [System.Xml.Serialization.XmlElement(ElementName = "AutoRefresh",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string AutoRefresh { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "DataSources",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSources DataSources { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "DataSets",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public DataSets DataSets { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "ReportParameters",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public ReportParameters ReportParameters { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Code",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Code { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Language",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string Language { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "ConsumeContainerWhitespace",
            Namespace = Report.DEFAULT_NAMESPACE)]
        public string ConsumeContainerWhitespace { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "ReportUnitType",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string ReportUnitType { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "ReportID",
            Namespace = Report.DESIGNER_NAMESPACE)]
        public string ReportID { get; set; }

        [System.Xml.Serialization.XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [System.Xml.Serialization.XmlAttribute(AttributeName = "rd", Namespace = "http://www.w3.org/2000/xmlns/")]
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
