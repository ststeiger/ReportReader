
using Microsoft.VisualBasic;


namespace Fw4csReportExecution
{


    public class TestCompiler
    {


        // http://stackoverflow.com/questions/2684278/why-does-microsoft-jscript-work-in-the-code-behind-but-not-within-a
        public static object Eval(string vbCode)
        {
            object retValue;

            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("Imports System" + Constants.vbCrLf);
            // sb.Append("Imports System.Xml" & vbCrLf)
            // sb.Append("Imports System.Data" & vbCrLf)
            // sb.Append("Imports System.Data.SqlClient" & vbCrLf)
            // sb.Append("Imports System.Math" & vbCrLf)
            sb.Append("Imports Microsoft.VisualBasic" + Constants.vbCrLf);

            sb.Append("Namespace MyEvalNamespace_2C04AF0B_8EC8_4D84_AD83_51FFBBBBC8C6BFAC7762_5502_40C9_AE35_A545434012A4528B58FB_641E_4D61_BA15_E64C58CD8CFC  " + Constants.vbCrLf);
            sb.Append("Class MyEvalClass " + Constants.vbCrLf);

            sb.Append("Public Function EvalCode() As Object " + Constants.vbCrLf);
            // sb.Append("YourNamespace.YourBaseClass thisObject = New YourNamespace.YourBaseClass()")
            sb.Append(vbCode + Constants.vbCrLf);
            sb.Append("End Function " + Constants.vbCrLf);
            sb.Append("End Class " + Constants.vbCrLf);
            sb.Append("End Namespace " + Constants.vbCrLf);
            // Debug.WriteLine(sb.ToString()) ' look at this to debug your eval string

            string codeToCompile = sb.ToString();
            sb.Clear();
            sb = null;


            // System.Console.WriteLine(codeToCompile)


            using (Microsoft.VisualBasic.VBCodeProvider codeProvider = new Microsoft.VisualBasic.VBCodeProvider())
            {
                // System.CodeDom.Compiler.ICodeCompiler icc = codeProvider.CreateCompiler();
                System.CodeDom.Compiler.CompilerParameters cp = new System.CodeDom.Compiler.CompilerParameters();

                cp.ReferencedAssemblies.Add("System.dll");
                // cp.ReferencedAssemblies.Add("System.Xml.dll");
                // cp.ReferencedAssemblies.Add("System.Data.dll");
                // cp.ReferencedAssemblies.Add("Microsoft.JScript.dll");

                // Sample code for adding your own referenced assemblies
                // cp.ReferencedAssemblies.Add(@"C:\YourProjectDir\bin\YourBaseClass.dll");
                // cp.ReferencedAssemblies.Add("YourBaseclass.dll");

                cp.CompilerOptions = "/t:library";
                cp.GenerateInMemory = true;


                // System.CodeDom.Compiler.CompilerResults cr = icc.CompileAssemblyFromSource(cp, codeToCompile);
                System.CodeDom.Compiler.CompilerResults cr = codeProvider.CompileAssemblyFromSource(cp, codeToCompile);

                System.Reflection.Assembly a = cr.CompiledAssembly;

                object o = a.CreateInstance("MyEvalNamespace_2C04AF0B_8EC8_4D84_AD83_51FFBBBBC8C6BFAC7762_5502_40C9_AE35_A545434012A4528B58FB_641E_4D61_BA15_E64C58CD8CFC.MyEvalClass");
                System.Type t = o.GetType();
                System.Reflection.MethodInfo mi = t.GetMethod("EvalCode");

                retValue = mi.Invoke(o, null);
            } // End Using codeProvider


            return retValue;
        } // End Function Eval  


    } // End Class TestCompiler 


} // End Using Namespace Fw4csReportExecution 
