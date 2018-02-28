using System;
using System.Collections.Generic;
using System.Net;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace ReportReader.Json2CSharp
{


    public partial class Report
    {
        [JsonProperty("AutoRefresh")]
        public string AutoRefresh { get; set; }

        [JsonProperty("DataSources")]
        public DataSources DataSources { get; set; }

        [JsonProperty("DataSets")]
        public DataSets DataSets { get; set; }

        [JsonProperty("ReportParameters")]
        public ReportParameters ReportParameters { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Language")]
        public string Language { get; set; }

        [JsonProperty("ConsumeContainerWhitespace")]
        public string ConsumeContainerWhitespace { get; set; }

        [JsonProperty("ReportUnitType")]
        public string ReportUnitType { get; set; }

        [JsonProperty("ReportID")]
        public string ReportId { get; set; }

        [JsonProperty("Xmlns")]
        public object Xmlns { get; set; }

        [JsonProperty("Rd")]
        public string Rd { get; set; }
    }

    public partial class DataSets
    {
        [JsonProperty("DataSet")]
        public List<DataSet> DataSet { get; set; }
    }

    public partial class DataSet
    {
        [JsonProperty("Query")]
        public Query Query { get; set; }

        [JsonProperty("Fields")]
        public Fields Fields { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }

    public partial class Fields
    {
        [JsonProperty("Field")]
        public List<Field> Field { get; set; }
    }

    public partial class Field
    {
        [JsonProperty("DataField")]
        public string DataField { get; set; }

        [JsonProperty("TypeName")]
        public string TypeName { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }

    public partial class Query
    {
        [JsonProperty("DataSourceName")]
        public string DataSourceName { get; set; }

        [JsonProperty("QueryParameters")]
        public QueryParameters QueryParameters { get; set; }

        [JsonProperty("CommandType")]
        public string CommandType { get; set; }

        [JsonProperty("CommandText")]
        public string CommandText { get; set; }

        [JsonProperty("Timeout")]
        public string Timeout { get; set; }

        [JsonProperty("UseGenericDesigner")]
        public string UseGenericDesigner { get; set; }
    }

    public partial class QueryParameters
    {
        [JsonProperty("QueryParameter")]
        public List<QueryParameter> QueryParameter { get; set; }
    }

    public partial class QueryParameter
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }

    public partial class DataSources
    {
        [JsonProperty("DataSource")]
        public DataSource DataSource { get; set; }
    }

    public partial class DataSource
    {
        [JsonProperty("DataSourceReference")]
        public string DataSourceReference { get; set; }

        [JsonProperty("SecurityType")]
        public string SecurityType { get; set; }

        [JsonProperty("DataSourceID")]
        public string DataSourceId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }

    public partial class ReportParameters
    {
        [JsonProperty("ReportParameter")]
        public List<ReportParameter> ReportParameter { get; set; }
    }

    public partial class ReportParameter
    {
        [JsonProperty("DataType")]
        public string DataType { get; set; }

        [JsonProperty("DefaultValue")]
        public DefaultValue DefaultValue { get; set; }

        [JsonProperty("Prompt")]
        public string Prompt { get; set; }

        [JsonProperty("Hidden")]
        public string Hidden { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ValidValues")]
        public ValidValues ValidValues { get; set; }
    }

    public partial class DefaultValue
    {
        [JsonProperty("Values")]
        public Values Values { get; set; }

        [JsonProperty("DataSetReference")]
        public DataSetReference DataSetReference { get; set; }
    }

    public partial class DataSetReference
    {
        [JsonProperty("DataSetName")]
        public string DataSetName { get; set; }

        [JsonProperty("ValueField")]
        public string ValueField { get; set; }

        [JsonProperty("LabelField")]
        public string LabelField { get; set; }
    }

    public partial class Values
    {
        [JsonProperty("Value")]
        public string Value { get; set; }
    }

    public partial class ValidValues
    {
        [JsonProperty("DataSetReference")]
        public DataSetReference DataSetReference { get; set; }
    }

    public partial class Report
    {
        public static Report FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Report>(json, ReportReader.Json2CSharp.Converter.Settings);
        }

        public static Report FromJsonFile(string fileName)
        {
            string json = System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8);
            return FromJson(json);
        }

        public string ToJson(Report self)
        {
            return JsonConvert.SerializeObject(self, ReportReader.Json2CSharp.Converter.Settings);
        }
    }


    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter()
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal,
                },
            },
        };
    }


}
