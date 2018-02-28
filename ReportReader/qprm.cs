
using System.Collections.Generic;


namespace Xml2C1111Sharp
{
    
    
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
        public QueryParameters QueryParameters { get; set; }
        public string CommandType { get; set; }
        public string CommandText { get; set; }
        public string Timeout { get; set; }
    }
    
    
    public class TypeName
    {
        public string Rd { get; set; }
        public string Text { get; set; }
    }
    
    
    public class Field
    {
        public string DataField { get; set; }
        public TypeName TypeName { get; set; }
        public string Name { get; set; }
    }
    
    
    public class Fields
    {
        public List<Field> Field { get; set; }
    }
    
    
    public class Document
    {
        public Query Query { get; set; }
        public Fields Fields { get; set; }
    }
}