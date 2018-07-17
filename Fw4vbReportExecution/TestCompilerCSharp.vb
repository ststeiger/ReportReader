
Public Class TestCompilerCSharp


    Public Shared Function Eval(ByVal vbCode As String) As Object
        Dim retValue As Object
        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder("")
        sb.AppendLine("using System;")
        sb.AppendLine("using Microsoft.VisualBasic;")
        sb.AppendLine("namespace MyEvalNamespace_2C04AF0B_8EC8_4D84_AD83_51FFBBBBC8C6BFAC7762_5502_40C9_AE35_A545434012A4528B58FB_641E_4D61_BA15_E64C58CD8CFC  ")
        sb.AppendLine("{ ")
        sb.AppendLine("     public class MyEvalClass ")
        sb.AppendLine("     { ")
        sb.AppendLine("        public object EvalCode() ")
        sb.AppendLine("        { ")
        sb.Append("            ")
        sb.AppendLine(vbCode)
        sb.AppendLine("        } ")
        sb.AppendLine("    } ")
        sb.AppendLine("} ")
        Dim codeToCompile As String = sb.ToString()
        sb.Clear()
        sb = Nothing

        Using codeProvider As Microsoft.CSharp.CSharpCodeProvider = New Microsoft.CSharp.CSharpCodeProvider()
            Dim cp As System.CodeDom.Compiler.CompilerParameters = New System.CodeDom.Compiler.CompilerParameters()
            cp.ReferencedAssemblies.Add("System.dll")
            cp.CompilerOptions = "/t:library"
            cp.GenerateInMemory = True
            Dim cr As System.CodeDom.Compiler.CompilerResults = codeProvider.CompileAssemblyFromSource(cp, codeToCompile)
            Dim a As System.Reflection.Assembly = cr.CompiledAssembly
            Dim o As Object = a.CreateInstance("MyEvalNamespace_2C04AF0B_8EC8_4D84_AD83_51FFBBBBC8C6BFAC7762_5502_40C9_AE35_A545434012A4528B58FB_641E_4D61_BA15_E64C58CD8CFC.MyEvalClass")
            Dim t As System.Type = o.GetType()
            Dim mi As System.Reflection.MethodInfo = t.GetMethod("EvalCode")
            retValue = mi.Invoke(o, Nothing)
        End Using

        Return retValue
    End Function


End Class
