
namespace Fw4csReportExecution
{


    public class PdfIndexer
    {


        public static void Start(string dir)
        {
            string[] filez = System.IO.Directory.GetFiles(dir, "*.pdf", System.IO.SearchOption.AllDirectories);

            foreach (string file in filez)
            {
                string fileName = System.IO.Path.GetFileName(file);
                System.Console.WriteLine(fileName);

                try
                {

                    using (PdfSharp.Pdf.PdfDocument document = PdfSharp.Pdf.IO.PdfReader.Open(file))
                    {
                        System.Console.Write("   - ");
                        System.Console.WriteLine(document.Info.Title);
                    } // End Using document 

                } // End Try 
                catch (PdfSharp.Pdf.IO.PdfReaderException ex)
                {
                    System.Console.Write("   --- ");

                    if (string.Equals(ex.Message, "To modify the document the owner password is required", System.StringComparison.Ordinal))
                    {
                        System.Console.WriteLine("Protected");
                    }
                    else
                        System.Console.WriteLine(ex.Message);
                } // End Catch 
                catch (System.Exception ex)
                {
                    System.Console.Write("   ------ ");
                    System.Console.WriteLine(ex.Message);
                } // End Catch 

                System.Console.WriteLine(System.Environment.NewLine);
                System.Console.WriteLine(System.Environment.NewLine);
            } // Next file 

        } // End Sub Start(string dir) 


    } // End Class PdfIndexer 


} // End Namespace Fw4csReportExecution 
