
using System.Linq;


namespace ReportReader
{
    
    
    public static class RsCompiler 
    {
        
        
        public static void Test()
        {
            try
            {

                // the main class Program contain static void Main() 
                // that calls A.Print() and B.Print() methods
                string mainProgramString =
                    @"
Option Strict Off
Option Explicit Off
Option Infer On

Public Class ReportParameter
    Public Key As String
    Public Label As String
    Public Value As String

    Public Sub New(key As String, lab As String, val As String)
        Me.Key = key
        Me.Label = lab
        Me.Value = val
    End Sub

    Public Sub New(key As String, val As String)
        Me.New(key, val, val)
    End Sub

End Class ' ReportParameter



Public Class ParameterStore

    Private dict As System.Collections.Generic.Dictionary(Of String, ReportParameter)


    Public Sub New(ParamArray u As ReportParameter())
        Me.New()

        For i As Integer = 0 To u.Length - 1 Step 1
            Me.dict.Add(u(i).Key, u(i))
        Next

    End Sub


    Public Sub New(parameters As System.Collections.Generic.Dictionary(Of String, ReportParameter))
        Me.dict = parameters
    End Sub


    Public Sub New()
        dict = New System.Collections.Generic.Dictionary(Of String, ReportParameter)(System.StringComparer.OrdinalIgnoreCase)
    End Sub


    Public Sub Add(para As ReportParameter)
        Me.dict.Add(para.Key, para)
    End Sub


    Default Public Property Parameters(ByVal index As String) As ReportParameter
        Get
            Return dict(index)
        End Get

        Set(ByVal value As ReportParameter)
            dict(index) = value
        End Set

    End Property


End Class ' ParameterStore



Public Class RsEval 


    ' Z = iif(y=0, 0, x/y)  'Throws a divide by zero exception when y is 0
    Public Shared Function IIF(expression As Boolean, truePart As Object, falsePart As Object) As Object
        If expression Then
            Return truePart
        End If

        Return falsePart
    End Function

     



    Public Shared Function SelVB() As String
        Dim Parameters As ParameterStore = New ParameterStore()

        Parameters.Add(New ReportParameter(""stadtkreis"", ""Alle"", ""@stadtkreis""))
        Parameters.Add(New ReportParameter(""Gemeinde"", ""Alle"", ""@Gemeinde""))


        SelVB(Parameters)
    End Function



    Public Shared Function SelVB(Parameters As ParameterStore)
        Dim str As String = ""SELECT     '00000000-0000-0000-0000-000000000000' AS ID, 'Alle' AS Name, - 1 AS Sort "" &
""UNION "" &
""SELECT     VB_ApertureID AS ID, VB_Name AS Name, VB_Sort AS Sort "" &
""FROM         V_AP_BERICHT_SEL_VB "" &
""WHERE ('"" & Parameters!stadtkreis.Value & ""' = '00000000-0000-0000-0000-000000000000' OR VB_KS_UID = '"" & Parameters!Gemeinde.Value & ""') "" &
""ORDER BY Sort, Name ""

        Return str
    End Function


    Public Shared Function Gebäude(Parameters As ParameterStore) As String
        Dim str As String = ""SELECT gbi.Gemeinde, gbi.Kreis, gbi.Vermessungsbezirk, gbi.Schulkreis, gbi.Standort, gbi.Gebaeude, gbi.SO_Name, gbi.GBI_Name, gbi.GBI_Adresse, gbi.GBI_Bemerkungen, gbi.GS_Lang, pf.PF1_ID, pf.PF1_Lang, pf.PF2_ID, pf.PF2_Lang, pf.PF3_ID, pf.PF3_Lang, pf.PF4_ID, pf.PF4_Lang, gbi.GS_UID,  gbi.SO_Sort, gbi.GBI_Sort, gbi.SK_Sort, gbi.GS_Sort, pf.PF1_Sort, pf.PF2_Sort, pf.PF3_Sort, pf.PF4_Sort, gbi.SK_UID AS SK_ID, gbi.GBI_IsDenkmalschutz, gbi.GBI_BemDenkmalschutz, gbi.SO_ID, gbi.GBI_ID, gbi.GM_ID, gbi.KS_ID, gbi.VB_ID, gbi.GM_Sort, gbi.KS_Sort, gbi.VB_Sort "" &
""FROM V_AP_BERICHT_GBI AS gbi LEFT OUTER JOIN V_AP_BERICHT_GBI_Portfolio AS pf ON gbi.GBI_ID = pf.GBI_ID "" &
""WHERE 1=1 "" &
IIF(Parameters!schulkreis.Value = ""00000000-0000-0000-0000-000000000000"", """", ""AND gbi.SK_UID='"" & Parameters!schulkreis.Value & ""' "") &
IIF(Parameters!stadtkreis.Value = ""00000000-0000-0000-0000-000000000000"", """", ""AND gbi.KS_ID='"" & Parameters!stadtkreis.Value & ""' "") &
IIF(Parameters!schuleinheit.Value = ""00000000-0000-0000-0000-000000000000"", ""AND 1=1 "", ""AND SE_UID='"" & Parameters!schuleinheit.Value & ""' "") &
IIF(Parameters!portfolio1.Value = ""00000000-0000-0000-0000-000000000000"", """", ""AND (pf.PF1_ID='"" & Parameters!portfolio1.Value & ""' "") &
IIF(Parameters!portfolio1.Value = ""00000000-0000-0000-0000-000000000000"", """", ""   OR pf.PF2_ID='"" & Parameters!portfolio1.Value & ""') "") &
IIF(Parameters!subportfolio1.Value = ""00000000-0000-0000-0000-000000000000"", """", ""AND (pf.PF3_ID='"" & Parameters!subportfolio1.Value & ""' "") &
IIF(Parameters!subportfolio1.Value = ""00000000-0000-0000-0000-000000000000"", """", ""OR pf.PF4_ID='"" & Parameters!subportfolio1.Value & ""') "") &
IIF(Parameters!denkmalschutz.Value = ""2"", """", ""AND gbi.GBI_IsDenkmalschutz="" & Parameters!denkmalschutz.Value & "" "") &
IIF(Parameters!kontakt.Value = ""00000000-0000-0000-0000-000000000000"", """", ""AND (gbi.GBI_ID IN (SELECT DPS_GBI_UID FROM dbo.T_Detail_Personen WHERE CONVERT(varchar(36), DPS_KF_UID) + CONVERT(varchar(36), DPS_KT_UID) ='"" & Parameters!kontakt.Value & ""' ))"")



        ' ERROR: IIF(Parameters!denkmalschutz.Value = ""2"", """", ""AND gbi.GBI_IsDenkmalschutz="" & Parameters!denkmalschutz.Value & "" "") &

        Return str
    End Function

    'Public Shared Function ExecuteDate() As Object
    '    Return DateAdd(""D""c, -1, DateAdd(""M""c, 1, new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1))).ToString(""dd.MM.yyyy"")
    'End Function


            ' https://stackoverflow.com/questions/6841275/what-does-this-mean-in-the-specific-line-of-code
    Public Shared Function ExecuteParameter(Parameters As ParameterStore) As String
        Dim str As String = ""SELECT 2 AS ID, 2 AS Sort, 'Alle' AS Name "" &
""UNION "" &
""SELECT DISTINCT PZ_IsAltlasten AS ID, PZ_IsAltlasten AS Sort, 'Ja' AS Name "" &
""FROM V_AP_BERICHT_SEL_PZ_Altlasten  "" &
""WHERE PZ_IsAltlasten = 1  "" &
IIF(Parameters!Vermessungsbezirk.Value = ""00000000-0000-0000-0000-000000000000"", IIF(Parameters!Kreis.Value = ""00000000-0000-0000-0000-000000000000"", IIF(Parameters!Gemeinde.Value = ""00000000-0000-0000-0000-000000000000"", """", ""AND GM_ApertureID = '"" & Parameters!Gemeinde.Value & ""' ""), ""AND KS_ApertureID = '"" & Parameters!Kreis.Value & ""' ""), ""AND VB_ApertureID = '"" & Parameters!Vermessungsbezirk.Value & ""' "") &
""UNION "" &
""SELECT DISTINCT PZ_IsAltlasten AS ID, PZ_IsAltlasten AS Sort, 'Nein' AS Name "" &
""FROM V_AP_BERICHT_SEL_PZ_Altlasten "" &
""WHERE PZ_IsAltlasten = 0 "" &
IIF(Parameters!Vermessungsbezirk.Value = ""00000000-0000-0000-0000-000000000000"", IIF(Parameters!Kreis.Value = ""00000000-0000-0000-0000-000000000000"", IIF(Parameters!Gemeinde.Value = ""00000000-0000-0000-0000-000000000000"", """", ""AND GM_ApertureID = '"" & Parameters!Gemeinde.Value & ""' ""), ""AND KS_ApertureID = '"" & Parameters!Kreis.Value & ""' ""), ""AND VB_ApertureID = '"" & Parameters!Vermessungsbezirk.Value & ""' "") &
""ORDER BY Sort DESC ""

        ' str.Trim(New Char() {"" ""c, vbTab, vbCr, vbLf})

        Return str
    End Function

End Class ' RsEval 


Public Class Program
    

    Public Shared Function Test() As String
        Return ""test"" & 5 
    End Function




    Public Shared Sub Main()
        Dim par As New ParameterStore()
        par.Add(New ReportParameter( ""schulkreis"", ""00000000-0000-0000-0000-000000000000"") )
        par.Add(New ReportParameter( ""stadtkreis"", ""00000000-0000-0000-0000-000000000000"") )
        par.Add(New ReportParameter( ""schuleinheit"", ""00000000-0000-0000-0000-000000000000"") )

        par.Add(New ReportParameter( ""portfolio1"", ""00000000-0000-0000-0000-000000000000"") )
        par.Add(New ReportParameter( ""subportfolio1"", ""00000000-0000-0000-0000-000000000000"") )
        par.Add(New ReportParameter( ""denkmalschutz"", ""00000000-0000-0000-0000-000000000000"") )
        par.Add(New ReportParameter( ""kontakt"", ""00000000-0000-0000-0000-000000000000"") )


        par.Add(New ReportParameter( ""Vermessungsbezirk"", ""00000000-0000-0000-0000-000000000000"") )
        par.Add(New ReportParameter( ""Kreis"", ""00000000-0000-0000-0000-000000000000"") )
        par.Add(New ReportParameter( ""Gemeinde"", ""00000000-0000-0000-0000-000000000000"") )
        Dim str As String = RsEval.ExecuteParameter(par) 
        str = RsEval.Gebäude(par) 
        str = RsEval.SelVB(par) 
        ' Dim obj As Object = RsEval.ExecuteDate();
        ' System.Console.WriteLine(obj) 
        str = Test()
        System.Console.WriteLine(str)
    End Sub
End Class

";

                Microsoft.CodeAnalysis.MetadataReference sysRuntime = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location);
                Microsoft.CodeAnalysis.MetadataReference vbRuntime = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(Microsoft.VisualBasic.Constants).Assembly.Location);
                Microsoft.CodeAnalysis.MetadataReference sysCorlib = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                Microsoft.CodeAnalysis.MetadataReference sysConsole = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Console).Assembly.Location);
                
                var co = new Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilationOptions
                (
                    Microsoft.CodeAnalysis.OutputKind.ConsoleApplication
                );
                
                
                co.WithOptionStrict(Microsoft.CodeAnalysis.VisualBasic.OptionStrict.Off);
                co.WithOptionExplicit(false);
                co.WithOptionInfer(true);



                // create the Roslyn compilation for the main program with
                // ConsoleApplication compilation options
                // adding references to A.netmodule and B.netmodule
                Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation mainCompilation =
                    CreateCompilationWithMscorlib
                    (
                        "program",
                        mainProgramString,
                        // note that here we pass the OutputKind set to ConsoleApplication
                        compilerOptions: co,
                        references: new[] { sysRuntime, vbRuntime, sysCorlib, sysConsole }
                    );
                
                // Emit the byte result of the compilation
                byte[] result = mainCompilation.EmitToArray();

                // Load the resulting assembly into the domain. 
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(result);
                
                
                
                // here we get the Program type and 
                // call its static method Main()
                // to test the program. 
                
                // get the type Program from the assembly
                System.Type programType = assembly.GetType("Program");

                // Get the static Main() method info from the type
                System.Reflection.MethodInfo method = programType.GetMethod("Main");

                // invoke Program.Main() static method
                method.Invoke(null, null);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        } // End Sub Test 
        
        
        // emit the compilation result into a byte array.
        // throw an exception with corresponding message
        // if there are errors
        private static byte[] EmitToArray
        (
            this Microsoft.CodeAnalysis.Compilation compilation
        )
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                // emit result into a stream
                Microsoft.CodeAnalysis.Emit.EmitResult emitResult = compilation.Emit(stream);

                if (!emitResult.Success)
                {
                    // if not successful, throw an exception
                    Microsoft.CodeAnalysis.Diagnostic firstError =
                        emitResult
                            .Diagnostics
                            .FirstOrDefault
                            (
                                diagnostic =>
                                    diagnostic.Severity ==
                                    Microsoft.CodeAnalysis.DiagnosticSeverity.Error
                            );

                    throw new System.Exception(firstError?.GetMessage());
                }
                
                // get the byte array from a stream
                return stream.ToArray();
            } // End Using stream 
            
        } // End Function EmitToArray 
        
        
        // a utility method that creates Roslyn compilation
        // for the passed code. 
        // The compilation references the collection of 
        // passed "references" arguments plus
        // the mscore library (which is required for the basic
        // functionality).
        private static Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation 
            CreateCompilationWithMscorlib
        (
            string assemblyOrModuleName,
            string code,
            Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilationOptions compilerOptions = null,
            System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> references = null)
        {
            // create the syntax tree
            Microsoft.CodeAnalysis.SyntaxTree syntaxTree =
                Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory.ParseSyntaxTree(code, null, "");

            // get the reference to mscore library
            Microsoft.CodeAnalysis.MetadataReference mscoreLibReference =
                Microsoft.CodeAnalysis.AssemblyMetadata
                    .CreateFromFile(typeof(string).Assembly.Location)
                    .GetReference();

            // create the allReferences collection consisting of 
            // mscore reference and all the references passed to the method
            System.Collections.Generic.IEnumerable<
                Microsoft.CodeAnalysis.MetadataReference> allReferences =
                new Microsoft.CodeAnalysis.MetadataReference[] {mscoreLibReference};
            if (references != null)
            {
                allReferences = allReferences.Concat(references);
            }
            
            // create and return the compilation
            Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation compilation =
                Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation.Create
                (
                    assemblyOrModuleName,
                    new[] {syntaxTree},
                    options: compilerOptions,
                    references: allReferences
                );
            
            return compilation;
        } // End Function CreateCompilationWithMscorlib 
        
        
    } // End Class TestCompilerVB
    
    
} // End Namespace SchemaPorter
