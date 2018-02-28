
namespace ReportReader
{


    class XmlTools
    {



        public static string XmlEscape(string unescaped)
        {
            string strReturnValue = null;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node = doc.CreateElement("root");
            node.InnerText = unescaped;
            strReturnValue = node.InnerXml;
            node = null;
            doc = null;

            return strReturnValue;
        } // End Function XmlEscape


        public static string XmlUnescape(string escaped)
        {
            string strReturnValue = null;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlNode node = doc.CreateElement("root");
            node.InnerXml = escaped;
            strReturnValue = node.InnerText;
            node = null;
            doc = null;

            return strReturnValue;
        } // End Function XmlUnescape


        public static System.Xml.XmlDocument File2XmlDocument(string strFileName)
        {
            // http://blogs.msdn.com/b/tolong/archive/2007/11/15/read-write-xml-in-memory-stream.aspx
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            // doc.Load(memorystream);
            // doc.Load(FILE_NAME);
            // doc.Load(strFileName);
            using (System.Xml.XmlTextReader xtrReader = new System.Xml.XmlTextReader(strFileName))
            {
                doc.Load(xtrReader);
                xtrReader.Close();
            } // End Using xtrReader

            return doc;
        } // End Function File2XmlDocument


        public static void SaveDocument(System.Xml.XmlDocument doc, string strFilename)
        {
            SaveDocument(doc, strFilename, false);
        } // End Sub SaveDocument


        public static void SaveDocument(System.Xml.XmlDocument origDoc, string filename, bool bDoReplace)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

            if (bDoReplace)
            {
                doc.LoadXml(origDoc.OuterXml.Replace("xmlns=\"\"", ""));
                // doc.LoadXml(doc.OuterXml.Replace(strTextToReplace, strReplacementText));
            }
            
            using (System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(filename, System.Text.Encoding.UTF8))
            {
                xtw.Formatting = System.Xml.Formatting.Indented; // if you want it indented
                xtw.Indentation = 4;
                xtw.IndentChar = ' ';

                doc.Save(xtw);
                xtw.Flush();
                xtw.Close();
            } // End Using xtw
            
            doc = null;
        } // End Sub SaveDocument
        
        
    }
    
    
}
