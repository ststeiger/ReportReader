
' Imports Microsoft.VisualBasic


Public Class TestCompiler


    ' http://stackoverflow.com/questions/2684278/why-does-microsoft-jscript-work-in-the-code-behind-but-not-within-a
    Public Shared Function Eval(ByVal vbCode As String) As Object
        Dim retValue As Object

        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder("")

        sb.Append("Imports System" & vbCrLf)
        ' sb.Append("Imports System.Xml" & vbCrLf)
        ' sb.Append("Imports System.Data" & vbCrLf)
        ' sb.Append("Imports System.Data.SqlClient" & vbCrLf)
        ' sb.Append("Imports System.Math" & vbCrLf)
        sb.Append("Imports Microsoft.VisualBasic" & vbCrLf)

        sb.Append("Namespace MyEvalNamespace_2C04AF0B_8EC8_4D84_AD83_51FFBBBBC8C6BFAC7762_5502_40C9_AE35_A545434012A4528B58FB_641E_4D61_BA15_E64C58CD8CFC  " & vbCrLf)
        sb.Append("Class MyEvalClass " & vbCrLf)

        sb.Append("Public Function EvalCode() As Object " & vbCrLf)
        'sb.Append("YourNamespace.YourBaseClass thisObject = New YourNamespace.YourBaseClass()")
        sb.Append(vbCode & vbCrLf)
        sb.Append("End Function " & vbCrLf)
        sb.Append("End Class " & vbCrLf)
        sb.Append("End Namespace " & vbCrLf)
        'Debug.WriteLine(sb.ToString()) ' look at this to debug your eval string

        Dim codeToCompile As String = sb.ToString()
        sb.Clear()
        sb = Nothing


        ' System.Console.WriteLine(codeToCompile)


        Using codeProvider As Microsoft.VisualBasic.VBCodeProvider = New Microsoft.VisualBasic.VBCodeProvider

            'Dim icc As System.CodeDom.Compiler.ICodeCompiler = c.CreateCompiler()
            Dim cp As System.CodeDom.Compiler.CompilerParameters = New System.CodeDom.Compiler.CompilerParameters

            cp.ReferencedAssemblies.Add("System.dll")
            ' cp.ReferencedAssemblies.Add("System.Xml.dll")
            ' cp.ReferencedAssemblies.Add("System.Data.dll")
            ' cp.ReferencedAssemblies.Add("Microsoft.JScript.dll")

            ' Sample code for adding your own referenced assemblies
            'cp.ReferencedAssemblies.Add("c:\yourProjectDir\bin\YourBaseClass.dll")
            'cp.ReferencedAssemblies.Add("YourBaseclass.dll")

            cp.CompilerOptions = "/t:library"
            cp.GenerateInMemory = True

            'Dim cr As System.CodeDom.Compiler.CompilerResults = icc.CompileAssemblyFromSource(cp, codeToCompile)
            Dim cr As System.CodeDom.Compiler.CompilerResults = codeProvider.CompileAssemblyFromSource(cp, codeToCompile)

            Dim a As System.Reflection.Assembly = cr.CompiledAssembly

            Dim o As Object = a.CreateInstance("MyEvalNamespace_2C04AF0B_8EC8_4D84_AD83_51FFBBBBC8C6BFAC7762_5502_40C9_AE35_A545434012A4528B58FB_641E_4D61_BA15_E64C58CD8CFC.MyEvalClass")
            Dim t As Type = o.GetType()
            Dim mi As System.Reflection.MethodInfo = t.GetMethod("EvalCode")

            retValue = mi.Invoke(o, Nothing)
        End Using ' codeProvider


        Return retValue
    End Function


End Class
