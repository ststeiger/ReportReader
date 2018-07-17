
using System.Linq;


namespace ReportReader
{
    
    
    public static class TestCompilerCSharp
    {
        
        
        public static void Test()
        {
            try
            {
                // code for class A
                string classAString =
                    @"public class A 
                    {
                        public static string Print() 
                        { 
                            return ""Hello "";
                        }
                    }";

                // code for class B (to spice it up, it is a 
                // subclass of A even though it is almost not needed
                // for the demonstration)
                string classBString =
                    @"public class B : A
                    {
                        public static string Print()
                        { 
                            return ""World!"";
                        }
                    }";

                // the main class Program contain static void Main() 
                // that calls A.Print() and B.Print() methods
                string mainProgramString =
                    @"public class Program
                    {
                        public static void Main()
                        {
                            System.Console.WriteLine(""Ni hao !"");
                            // System.Console.Write(A.Print()); 
                            // System.Console.WriteLine(B.Print());
                        }
                    }";

                #region class A compilation into A.netmodule

                // create Roslyn compilation for class A
                Microsoft.CodeAnalysis.CSharp.CSharpCompilation compilationA =
                    CreateCompilationWithMscorlib
                    (
                        "A",
                        classAString,
                        compilerOptions: new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(
                            Microsoft.CodeAnalysis.OutputKind.NetModule)
                    );

                // emit the compilation result to a byte array 
                // corresponding to A.netmodule byte code
                byte[] compilationAResult = compilationA.EmitToArray();

                // create a reference to A.netmodule
                Microsoft.CodeAnalysis.MetadataReference referenceA =
                    Microsoft.CodeAnalysis.ModuleMetadata
                        .CreateFromImage(compilationAResult)
                        .GetReference(display: "A.netmodule");

                #endregion class A compilation into A.netmodule


                #region class B compilation into B.netmodule

                // create Roslyn compilation for class A
                Microsoft.CodeAnalysis.CSharp.CSharpCompilation compilationB =
                    CreateCompilationWithMscorlib
                    (
                        "B",
                        classBString,
                        compilerOptions: new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(
                            Microsoft.CodeAnalysis.OutputKind.NetModule),

                        // since class B extends A, we need to 
                        // add a reference to A.netmodule
                        references: new[] {referenceA}
                    );

                // emit the compilation result to a byte array 
                // corresponding to B.netmodule byte code
                byte[] compilationBResult = compilationB.EmitToArray();

                // create a reference to B.netmodule
                Microsoft.CodeAnalysis.MetadataReference referenceB =
                    Microsoft.CodeAnalysis.ModuleMetadata
                        .CreateFromImage(compilationBResult)
                        .GetReference(display: "B.netmodule");

                #endregion class B compilation into B.netmodule

                #region main program compilation into the assembly

                

                // var met = Microsoft.CodeAnalysis.AssemblyMetadata.CreateFromFile(typeof(System.Console).Assembly.Location);
                Microsoft.CodeAnalysis.MetadataReference sysCorlib = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                Microsoft.CodeAnalysis.MetadataReference sysConsole = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Console).Assembly.Location);
                Microsoft.CodeAnalysis.MetadataReference sysRuntime = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location);
                // Microsoft.CodeAnalysis.MetadataReference sysRuntimeLoader = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Runtime.Loader.AssemblyLoadContext).Assembly.Location);
                // Microsoft.CodeAnalysis.MetadataReference sysAssembly = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Reflection.Assembly).Assembly.Location);

                // create the Roslyn compilation for the main program with
                // ConsoleApplication compilation options
                // adding references to A.netmodule and B.netmodule
                Microsoft.CodeAnalysis.CSharp.CSharpCompilation mainCompilation =
                    CreateCompilationWithMscorlib
                    (
                        "program",
                        mainProgramString,
                        // note that here we pass the OutputKind set to ConsoleApplication
                        compilerOptions: new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(
                            Microsoft.CodeAnalysis.OutputKind.ConsoleApplication
                            ),
                        references: new[] { sysRuntime, sysCorlib, sysConsole,  referenceA, referenceB }
                    );



                




                //.WithOptions(new CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary))

                // Emit the byte result of the compilation
                byte[] result = mainCompilation.EmitToArray();

                // Load the resulting assembly into the domain. 
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(result);
                // System.Type t = assembly.GetType("Program");
                // System.Reflection.MethodInfo mainMethod = t.GetMethod("Main");
                // mainMethod.Invoke(null, null);


                #endregion main program compilation into the assembly
                
                // .NET Core doesn't support modules... (by design)
                // load the A.netmodule and B.netmodule into the assembly.
                // assembly.LoadModule("A.netmodule", compilationAResult);
                // assembly.LoadModule("B.netmodule", compilationBResult);

                #region Test the program

                // here we get the Program type and 
                // call its static method Main()
                // to test the program. 
                // It should write "Hello world!"
                // to the console

                // get the type Program from the assembly
                System.Type programType = assembly.GetType("Program");

                // Get the static Main() method info from the type
                System.Reflection.MethodInfo method = programType.GetMethod("Main");

                // invoke Program.Main() static method
                method.Invoke(null, null);

                #endregion Test the program
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
        private static Microsoft.CodeAnalysis.CSharp.CSharpCompilation CreateCompilationWithMscorlib
        (
            string assemblyOrModuleName,
            string code,
            Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions compilerOptions = null,
            System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> references = null)
        {
            // create the syntax tree
            Microsoft.CodeAnalysis.SyntaxTree syntaxTree =
                Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseSyntaxTree(code, null, "");

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
            Microsoft.CodeAnalysis.CSharp.CSharpCompilation compilation =
                Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create
                (
                    assemblyOrModuleName,
                    new[] {syntaxTree},
                    options: compilerOptions,
                    references: allReferences
                );
            
            return compilation;
        } // End Function CreateCompilationWithMscorlib 
        
        
    } // End Class TestCompilerCSharp
    
    
} // End Namespace SchemaPorter