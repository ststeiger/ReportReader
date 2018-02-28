
using System.Xml.Serialization;
using System.Collections.Generic;


namespace ReportReader.Xml2CSharp.ZOMGXML
{
    [XmlRoot(ElementName = "DataSource",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class DataSource
    {
        [XmlElement(ElementName = "DataSourceReference",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string DataSourceReference { get; set; }

        [XmlElement(ElementName = "SecurityType",
            Namespace = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")]
        public string SecurityType { get; set; }

        [XmlElement(ElementName = "DataSourceID",
            Namespace = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")]
        public string DataSourceID { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "DataSources",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class DataSources
    {
        [XmlElement(ElementName = "DataSource",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public DataSource DataSource { get; set; }
    }

    [XmlRoot(ElementName = "QueryParameter",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class QueryParameter
    {
        [XmlElement(ElementName = "Value",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "QueryParameters",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class QueryParameters
    {
        [XmlElement(ElementName = "QueryParameter",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public List<QueryParameter> QueryParameter { get; set; }
    }

    [XmlRoot(ElementName = "Query",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class Query
    {
        [XmlElement(ElementName = "DataSourceName",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string DataSourceName { get; set; }

        [XmlElement(ElementName = "QueryParameters",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public QueryParameters QueryParameters { get; set; }

        [XmlElement(ElementName = "CommandType",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string CommandType { get; set; }

        [XmlElement(ElementName = "CommandText",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string CommandText { get; set; }

        [XmlElement(ElementName = "Timeout",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string Timeout { get; set; }

        [XmlElement(ElementName = "UseGenericDesigner",
            Namespace = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")]
        public string UseGenericDesigner { get; set; }
    }

    [XmlRoot(ElementName = "Field",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class Field
    {
        [XmlElement(ElementName = "DataField",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string DataField { get; set; }

        [XmlElement(ElementName = "TypeName",
            Namespace = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")]
        public string TypeName { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "Fields",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class Fields
    {
        [XmlElement(ElementName = "Field",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public List<Field> Field { get; set; }
    }

    [XmlRoot(ElementName = "DataSet",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class DataSet
    {
        [XmlElement(ElementName = "Query",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public Query Query { get; set; }

        [XmlElement(ElementName = "Fields",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public Fields Fields { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "DataSets",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class DataSets
    {
        [XmlElement(ElementName = "DataSet",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public List<DataSet> DataSet { get; set; }
    }

    [XmlRoot(ElementName = "Values",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class Values
    {
        [XmlElement(ElementName = "Value",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "DefaultValue",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class DefaultValue
    {
        [XmlElement(ElementName = "Values",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public Values Values { get; set; }

        [XmlElement(ElementName = "DataSetReference",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public DataSetReference DataSetReference { get; set; }
    }

    [XmlRoot(ElementName = "ReportParameter",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class ReportParameter
    {
        [XmlElement(ElementName = "DataType",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string DataType { get; set; }

        [XmlElement(ElementName = "DefaultValue",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public DefaultValue DefaultValue { get; set; }

        [XmlElement(ElementName = "Prompt",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string Prompt { get; set; }

        [XmlElement(ElementName = "Hidden",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string Hidden { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "ValidValues",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public ValidValues ValidValues { get; set; }
    }

    [XmlRoot(ElementName = "DataSetReference",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class DataSetReference
    {
        [XmlElement(ElementName = "DataSetName",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string DataSetName { get; set; }

        [XmlElement(ElementName = "ValueField",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string ValueField { get; set; }

        [XmlElement(ElementName = "LabelField",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string LabelField { get; set; }
    }

    [XmlRoot(ElementName = "ValidValues",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class ValidValues
    {
        [XmlElement(ElementName = "DataSetReference",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public DataSetReference DataSetReference { get; set; }
    }

    [XmlRoot(ElementName = "ReportParameters",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class ReportParameters
    {
        [XmlElement(ElementName = "ReportParameter",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public List<ReportParameter> ReportParameter { get; set; }
    }

    [XmlRoot(ElementName = "Report",
        Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
    public class Report
    {
        [XmlElement(ElementName = "AutoRefresh",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string AutoRefresh { get; set; }

        [XmlElement(ElementName = "DataSources",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public DataSources DataSources { get; set; }

        [XmlElement(ElementName = "DataSets",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public DataSets DataSets { get; set; }

        [XmlElement(ElementName = "ReportParameters",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public ReportParameters ReportParameters { get; set; }

        [XmlElement(ElementName = "Code",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string Code { get; set; }

        [XmlElement(ElementName = "Language",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string Language { get; set; }

        [XmlElement(ElementName = "ConsumeContainerWhitespace",
            Namespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")]
        public string ConsumeContainerWhitespace { get; set; }

        [XmlElement(ElementName = "ReportUnitType",
            Namespace = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")]
        public string ReportUnitType { get; set; }

        [XmlElement(ElementName = "ReportID",
            Namespace = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")]
        public string ReportID { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "rd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Rd { get; set; }
    }
}