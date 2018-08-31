
namespace Fw4csReportExecution
{


    public class ConfigTypeMapper
    {


        public static System.Collections.Generic.Dictionary<string, string> MapConfigTypes()
        {
            System.Collections.Generic.Dictionary<string, string> dict = 
                new System.Collections.Generic.Dictionary<string, string>(
                System.StringComparer.InvariantCultureIgnoreCase
            );

            // .NET-Core to .NET 4
            dict.Add("System.Uri, System.Private.Uri", "System.Uri, System");
            dict.Add("System.String, System.Private.CoreLib", "System.String, mscorlib");
            dict.Add("System.Int32, System.Private.CoreLib", "System.Int32, System");
            dict.Add("System.Boolean, System.Private.CoreLib", "System.Boolean, mscorlib");
            dict.Add("System.Xml.XmlDocument, System.Private.Xml", "System.Xml.XmlDocument, System.Xml");

            // .NET 4 to .NET-Core 
            dict.Add("System.Uri, System", "System.Uri, System.Private.Uri");
            dict.Add("System.String, mscorlib", "System.String, System.Private.CoreLib");
            dict.Add("System.Int32, System", "System.Int32, System.Private.CoreLib");
            dict.Add("System.Boolean, mscorlib", "System.Boolean, System.Private.CoreLib");
            dict.Add("System.Xml.XmlDocument, System.Xml", "System.Xml.XmlDocument, System.Private.Xml");

            // .NET-Core: 
            // System.Uri, System.Private.Uri
            // System.String, System.Private.CoreLib
            // System.Int32, System.Private.CoreLib
            // System.Boolean, System.Private.CoreLib
            // System.Xml.XmlDocument, System.Private.Xml

            // .NET 4.0: 
            // System.Uri, System
            // System.String, mscorlib
            // System.Int32, System
            // System.Boolean, mscorlib
            // System.Xml.XmlDocument, System.Xml


            string sql = @"
SELECT * FROM T_FMS_Configuration 
WHERE FC_Type NOT IN 
(
     'System.Uri, System'
    ,'System.String, mscorlib'
    ,'System.Int32, System' 
    ,'System.Boolean, mscorlib'
    ,'System.Xml.XmlDocument, System.Xml' 
)

-- UPDATE T_FMS_Configuration SET FC_Type = 'System.String, mscorlib' WHERE FC_Type = 'System.String'; 
-- UPDATE T_FMS_Configuration SET FC_Type = 'System.String, mscorlib' WHERE FC_Type = 'System.String, System'; 

";

            return dict;
        } // End Function MapConfigTypes 


    } // End Class ConfigTypeMapper 


} // End Namespace Fw4csReportExecution 
