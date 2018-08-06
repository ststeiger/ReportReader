
namespace ReportReader.Xml2Json2CSharp
{


    public class DataSource
    {
        [Newtonsoft.Json.JsonProperty("@Name")]
        public string Name {
            get;
            set;
        }
        
        public string DataSourceReference { get; set; }
        
        [Newtonsoft.Json.JsonProperty("rd:SecurityType")]
        public string SecurityType {
            get;
            set;
        }
        
        [Newtonsoft.Json.JsonProperty("rd:DataSourceID")]
        public string DataSourceID {
            get;
            set;
        }
    }

    public class DataSources
    {
        public DataSource DataSource { get; set; }
    }
    
    
    public class QueryParameter
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }


    public class QueryParameters
    {
        public object QueryParameter { get; set; }
        // public List<QueryParameter> QueryParameter { get; set; }
    }

    public class Query
    {
        public string DataSourceName { get; set; }
        public QueryParameters QueryParameters { get; set; }
        public string CommandType { get; set; }
        public string CommandText { get; set; }
        public string Timeout { get; set; }
        
        [Newtonsoft.Json.JsonProperty("rd:UseGenericDesigner")]
        public string UseGenericDesigner {
            get;
            set;
        }
    }

    public class Field
    {
        [Newtonsoft.Json.JsonProperty("@Name")]
        public string Name {
            get;
            set;
        }

        
        public string DataField { get; set; }
        
        [Newtonsoft.Json.JsonProperty("rd:TypeName")]
        public string TypeName {
            get;
            set;
        }
    }

    public class Fields
    {
        public System.Collections.Generic.List<Field> Field { get; set; }
    }

    public class DataSet
    {
        [Newtonsoft.Json.JsonProperty("@Name")]
        public string Name {
            get;
            set;
        }
        
        public Query Query { get; set; }
        public Fields Fields { get; set; }
    }

    public class DataSets
    {
        public System.Collections.Generic.List<DataSet> DataSet { get; set; }
    }

    public class Parameter
    {
        [Newtonsoft.Json.JsonProperty("@Name")]
        public string Name {
            get;
            set;
        }

        public string Value { get; set; }
    }

    public class Parameters
    {
        public System.Collections.Generic.List<Parameter> Parameter { get; set; }
    }

    public class Drillthrough
    {
        public string ReportName { get; set; }
        public Parameters Parameters { get; set; }
    }

    public class Action
    {
        public Drillthrough Drillthrough { get; set; }
    }

    public class Actions
    {
        public Action Action { get; set; }
    }

    public class ActionInfo
    {
        public Actions Actions { get; set; }
    }

    public class Visibility2
    {
        public string Hidden { get; set; }
    }

    public class ReportSections
    {
        //public ReportSection ReportSection { get; set; }
    }

    public class Values
    {
        public string Value { get; set; }
    }

    public class DataSetReference
    {
        public string DataSetName { get; set; }
        public string ValueField { get; set; }
    }

    public class DefaultValue
    {
        public Values Values { get; set; }
        public DataSetReference DataSetReference { get; set; }
    }

    public class DataSetReference2
    {
        public string DataSetName { get; set; }
        public string ValueField { get; set; }
        public string LabelField { get; set; }
    }

    public class ValidValues
    {
        public DataSetReference2 DataSetReference { get; set; }
    }

    public class ReportParameter
    {
        [Newtonsoft.Json.JsonProperty("@Name")]
        
        public string Name {
            get;
            set;
        }
        
        public string DataType { get; set; }
        public DefaultValue DefaultValue { get; set; }
        public string Prompt { get; set; }
        public string Hidden { get; set; }
        public ValidValues ValidValues { get; set; }
        public string MultiValue { get; set; }
        public string Nullable { get; set; }
        public string AllowBlank { get; set; }
    }

    public class ReportParameters
    {
        public System.Collections.Generic.List<ReportParameter> ReportParameter { get; set; }
    }

    public class CellDefinition
    {
        public string ColumnIndex { get; set; }
        public string RowIndex { get; set; }
        public string ParameterName { get; set; }
    }

    public class CellDefinitions
    {
        public System.Collections.Generic.List<CellDefinition> CellDefinition { get; set; }
    }

    public class GridLayoutDefinition
    {
        public string NumberOfColumns { get; set; }
        public string NumberOfRows { get; set; }
        public CellDefinitions CellDefinitions { get; set; }
    }

    public class ReportParametersLayout
    {
        public GridLayoutDefinition GridLayoutDefinition { get; set; }
    }

    public class Report
    {
        [Newtonsoft.Json.JsonProperty("@xmlns")]
        public string xmlns {
            get;
            set;
        }

        [Newtonsoft.Json.JsonProperty("@xmlns:rd")]
        public string rd {
            get;
            set;
        }
        
        public string AutoRefresh { get; set; }
        public DataSources DataSources { get; set; }
        public DataSets DataSets { get; set; }
        public ReportSections ReportSections { get; set; }
        public ReportParameters ReportParameters { get; set; }
        public ReportParametersLayout ReportParametersLayout { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
        public string ConsumeContainerWhitespace { get; set; }
        
        [Newtonsoft.Json.JsonProperty("rd:ReportUnitType")]
        public string ReportUnitType {
            get;
            set;
        }
        
        [Newtonsoft.Json.JsonProperty("rd:ReportID")]
        public string ReportID {
            get;
            set;
        }
    }

    public class Xml
    {
        [Newtonsoft.Json.JsonProperty("@version")]
        public string version { get; set; }
        
        [Newtonsoft.Json.JsonProperty("@encoding")]
        public string encoding { get; set; }
    }

    
    
    public class RootObject
    {
        
        [Newtonsoft.Json.JsonProperty("?xml")]
        public Xml xml {
            get;
            set;
        }

        public Report Report { get; set; }
    }

}