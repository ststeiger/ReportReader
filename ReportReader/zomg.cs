
using System.Collections.Generic;


namespace Xml2CSharp
{
    
    
    public class DataSource
    {
        public string DataSourceReference { get; set; }
        public string SecurityType { get; set; }
        public string DataSourceID { get; set; }
        public string Name { get; set; }
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
        public List<QueryParameter> QueryParameter { get; set; }
    }

    public class Query
    {
        public string DataSourceName { get; set; }
        // public QueryParameters QueryParameters { get; set; }
        
        public List<QueryParameter> QueryParameters { get; set; }
        
        
        public string CommandType { get; set; }
        public string CommandText { get; set; }
        public string Timeout { get; set; }
        public string UseGenericDesigner { get; set; }
    }

    public class Field
    {
        public string DataField { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
    }

    public class Fields
    {
        public List<Field> Field { get; set; }
    }

    public class DataSet
    {
        public Query Query { get; set; }
        public Fields Fields { get; set; }
        public string Name { get; set; }
    }

    public class DataSets
    {
        public List<DataSet> DataSet { get; set; }
    }

    
//["Report"]["DataSets"].DataSet[b["Report"]["DataSets"].DataSet.length-1]
           
    
    public class Values
    {
        public string Value { get; set; }
    }

    public class DefaultValue
    {
        public Values Values { get; set; }
        public DataSetReference DataSetReference { get; set; }
    }

    public class ReportParameter
    {
        public string DataType { get; set; }
        public DefaultValue DefaultValue { get; set; }
        public string Prompt { get; set; }
        public string Hidden { get; set; }
        public string Name { get; set; }
        public ValidValues ValidValues { get; set; }
        public string MultiValue { get; set; }
        public string Nullable { get; set; }
        public string AllowBlank { get; set; }
    }

    public class DataSetReference
    {
        public string DataSetName { get; set; }
        public string ValueField { get; set; }
        public string LabelField { get; set; }
    }

    public class ValidValues
    {
        public DataSetReference DataSetReference { get; set; }
    }

    public class ReportParameters
    {
        public List<ReportParameter> ReportParameter { get; set; }
    }

    public class CellDefinition
    {
        public string ColumnIndex { get; set; }
        public string RowIndex { get; set; }
        public string ParameterName { get; set; }
    }

    public class CellDefinitions
    {
        public List<CellDefinition> CellDefinition { get; set; }
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


    public class Root
    {
        public Report @Report;

    }
    
    
    public class Report
    {
        public string AutoRefresh { get; set; }
        public DataSources DataSources { get; set; }
        
        //public List<DataSet> DataSet { get; set; }
        public DataSets DataSets { get; set; }
               
        
        
        // 
/*        
// public string ReportSections { get; set; }
        public ReportParameters ReportParameters { get; set; }
        public ReportParametersLayout ReportParametersLayout { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
        public string ConsumeContainerWhitespace { get; set; }
        public string ReportUnitType { get; set; }
        public string ReportID { get; set; }
        */
    }
    
    
}
