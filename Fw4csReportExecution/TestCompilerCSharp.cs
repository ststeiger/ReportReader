
using Microsoft.VisualBasic;


namespace Fw4csReportExecution
{


    public class TestCompilerCSharp
    {


        // http://stackoverflow.com/questions/2684278/why-does-microsoft-jscript-work-in-the-code-behind-but-not-within-a
        public static object Eval(string vbCode)
        {
            object retValue;

            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.AppendLine("using System;");
            // sb.Append("Imports System.Xml" & vbCrLf);
            // sb.Append("Imports System.Data" & vbCrLf);
            // sb.Append("Imports System.Data.SqlClient" & vbCrLf);
            // sb.Append("Imports System.Math" & vbCrLf);
            sb.AppendLine("using Microsoft.VisualBasic;");

            sb.Append("namespace MyEvalNamespace_2C04AF0B_8EC8_4D84_AD83_51FFBBBBC8C6BFAC7762_5502_40C9_AE35_A545434012A4528B58FB_641E_4D61_BA15_E64C58CD8CFC  " + Constants.vbCrLf);
            sb.AppendLine("{ ");
            sb.AppendLine("     public class MyEvalClass ");
            sb.AppendLine("     { ");

            sb.AppendLine("        public object EvalCode() ");
            sb.AppendLine("        { ");
            // sb.Append("YourNamespace.YourBaseClass thisObject = New YourNamespace.YourBaseClass()");
            sb.Append("            ");
            sb.AppendLine(vbCode);
            sb.AppendLine("        } "); // End Function 
            sb.AppendLine("    } "); // End Class
            sb.AppendLine("} "); // End Namespace

            // Debug.WriteLine(sb.ToString()) ' look at this to debug your eval string

            string codeToCompile = sb.ToString();
            sb.Clear();
            sb = null;


            // System.Console.WriteLine(codeToCompile)


            using (Microsoft.CSharp.CSharpCodeProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider())
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
