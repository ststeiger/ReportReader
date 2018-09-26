
using System;
using System.Linq;


// using System.Reflection.Metadata;


// using Microsoft.CodeAnalysis.VisualBasic;
//using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.Emit;

// https://josephwoodward.co.uk/2016/12/in-memory-c-sharp-compilation-using-roslyn
// https://gist.github.com/GeorgDangl/4a9982a3b520f056a9e890635b3695e0
// https://github.com/dotnet/roslyn/issues/27899


namespace ReportReader
{
    
    
    public static class TestCompilerVB
    {
        
        // TestCompilerVB.Test2();
        private static System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> GetAssemblyReferences()
        {
            string rss = @"C:\Users\Administrator\Documents\Visual Studio 2017\Projects\ReportReader\vbNetStandardLib\Libs\AspNetCore.ReportingServices.dll";
            if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                rss = "/root/RiderProjects/ReportReader/vbNetStandardLib/Libs/AspNetCore.ReportingServices.dll";


            System.Reflection.Assembly rsAssembly = System.Reflection.Assembly.LoadFile(rss);


            System.Reflection.AssemblyName[] asms = typeof(Microsoft.VisualBasic.Constants).Assembly.GetReferencedAssemblies();
            System.Reflection.Assembly nsAssembly = null;

            foreach (System.Reflection.AssemblyName asm in asms)
            {
                if (asm.FullName.StartsWith("netstandard,", StringComparison.OrdinalIgnoreCase))
                {
                    nsAssembly = System.Reflection.Assembly.Load(asm.FullName);
                    break;
                }
            }




            //System.Reflection.Assembly[] asms = System.AppDomain.CurrentDomain.GetAssemblies();
            //string netStdAssembly = null;

            //foreach (System.Reflection.Assembly asm in asms)
            //{
            //    if (asm.FullName.StartsWith("netstandard,", StringComparison.OrdinalIgnoreCase))
            //    {
            //        netStdAssembly = asm.Location;
            //    }
            //}

            Microsoft.CodeAnalysis.MetadataReference[] references = 
                new Microsoft.CodeAnalysis.MetadataReference[]
            {
                
                Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    rsAssembly.Location
                )
                , // C:\Program Files\dotnet\shared\Microsoft.NETCore.App\2.0.9\System.Private.CoreLib.dll
                Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(nsAssembly.Location)

                
                , // C:\Program Files\dotnet\shared\Microsoft.NETCore.App\2.0.9\System.Private.CoreLib.dll
                Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(System.MarshalByRefObject).Assembly.Location 
                )
                , // C:\Program Files\dotnet\shared\Microsoft.NETCore.App\2.0.9\System.Runtime.dll
                Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(System.IO.FileAttributes).Assembly.Location
                 )
                 
                 ,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(System.Uri).Assembly.Location
                 )

                 ,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(System.Drawing.RectangleF).Assembly.Location
                 )
                 
                 //,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                 //   typeof(System.Drawing.Graphics).Assembly.Location
                 //)
                 /*
                 ,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(System.Data.Common.DbCommand).Assembly.Location
                 )

                 ,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(System.Data.SqlClient.SqlCommand).Assembly.Location
                 )
                 */
                 ,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(Microsoft.VisualBasic.Constants).Assembly.Location
                 )
                 

                // A bit hacky, if you need it
                //MetadataReference.CreateFromFile(Path.Combine(typeof(object).GetTypeInfo().Assembly.Location, "..", "mscorlib.dll")),
            };
            
            return references;
        }


        private static void CreateCompilation2(string code)
        {
            Microsoft.CodeAnalysis.SyntaxTree syntaxTree = 
                Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxTree.ParseText(code);
            
            string assemblyName = System.Guid.NewGuid().ToString();
            var references = GetAssemblyReferences();
            
            
            Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilationOptions co = 
                new Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilationOptions
            (
                Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary
            );
            
            co.WithOptionStrict(Microsoft.CodeAnalysis.VisualBasic.OptionStrict.Off);
            co.WithOptionExplicit(false);
            co.WithOptionInfer(true);

            
            Microsoft.CodeAnalysis.Compilation compilation = Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation.Create(
               assemblyName,
               new[] { syntaxTree },
               references,
               co
            );
            
            
            byte[] compilationResult = compilation.EmitToArray();
            
            // Load the resulting assembly into the domain. 
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(compilationResult);
            
            /*
            // get the type Program from the assembly
            System.Type programType = assembly.GetType("Program");
            
            // Get the static Main() method info from the type
            System.Reflection.MethodInfo method = programType.GetMethod("Main");
            
            // invoke Program.Main() static method
            method.Invoke(null, null);
            */

            
            
            
            using (var ms = new System.IO.MemoryStream())
            {
                Microsoft.CodeAnalysis.Emit.EmitResult result = compilation.Emit(ms);
                ThrowExceptionIfCompilationFailure(result);
                ms.Seek(0, System.IO.SeekOrigin.Begin);


                System.Reflection.Assembly assembly2 = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
#if NET46
                // Different in full .Net framework
                var assembly3 = Assembly.Load(ms.ToArray());
#endif
            }
        }


        private static void CreateCsCompilation(string code)
        {
            Microsoft.CodeAnalysis.SyntaxTree syntaxTree = 
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(code);
            
            string assemblyName = System.Guid.NewGuid().ToString();
            var references = GetAssemblyReferences();

            Microsoft.CodeAnalysis.Compilation compilation = Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(
                    Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary)
            );

            // byte[] compilationResult = compilation.EmitToArray();
            // System.Reflection.Assembly.Load(compilationResult);
            
            using (var ms = new System.IO.MemoryStream())
            {
                Microsoft.CodeAnalysis.Emit.EmitResult result = compilation.Emit(ms);
                ThrowExceptionIfCompilationFailure(result);
                ms.Seek(0, System.IO.SeekOrigin.Begin);


                System.Reflection.Assembly assembly2 = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
#if NET46
                // Different in full .Net framework
                var assembly3 = Assembly.Load(ms.ToArray());
#endif
            }


            // _compilation = compilation;
        }


        public static void CreateAssemblyDefinition(string code)
        {
            Microsoft.CodeAnalysis.SyntaxTree syntaxTree = Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(code);
            
            System.Collections.Generic.IReadOnlyCollection<
                  Microsoft.CodeAnalysis.MetadataReference> _references = new[] {
                  Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Reflection.Binder).Assembly.Location),
                  Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.ValueTuple<>).Assembly.Location)
              };

            bool enableOptimisations = true;

            var options = new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(
            Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: enableOptimisations ? Microsoft.CodeAnalysis.OptimizationLevel.Release : Microsoft.CodeAnalysis.OptimizationLevel.Debug,
                allowUnsafe: true
            );


            Microsoft.CodeAnalysis.Compilation compilation = Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create(
                assemblyName: "InMemoryAssembly", options: options)
                .AddReferences(_references)
                .AddSyntaxTrees(syntaxTree);

            var stream = new System.IO.MemoryStream();
            var emitResult = compilation.Emit(stream);

            if (emitResult.Success)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                // System.Reflection.Metadata.AssemblyDefinition assembly = System.Reflection.Metadata.AssemblyDefinition.ReadAssembly(stream);
            }
        }
        
        
        // http://www.tugberkugurlu.com/archive/compiling-c-sharp-code-into-memory-and-executing-it-with-roslyn
        public static void Execute(System.Reflection.Assembly asm)
        {
            System.Type type = asm.GetType("RoslynCompileSample.Writer");
            object obj = System.Activator.CreateInstance(type);
            type.InvokeMember("Write",
                System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod,
                null,
                obj,
                new object[] { "Hello World" });
        }


        // https://gist.github.com/GeorgDangl/4a9982a3b520f056a9e890635b3695e0
        private static void ThrowExceptionIfCompilationFailure(Microsoft.CodeAnalysis.Emit.EmitResult result)
        {
            if (!result.Success)
            {
                var compilationErrors = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .ToList();

                if (compilationErrors.Any())
                {
                    var firstError = compilationErrors.First();
                    var errorNumber = firstError.Id;
                    var errorDescription = firstError.GetMessage();
                    var firstErrorMessage = $"{errorNumber}: {errorDescription};";
                    throw new System.Exception($"Compilation failed, first error is: {firstErrorMessage}");
                }
            }
        }


        public static void Test2()
        {
            string inputFile = @"D:\username\Documents\Visual Studio 2017\Projects\ReportReader\vbNetStandardLib\Class1.vb";
            inputFile = @"C:\Users\Administrator\Documents\Visual Studio 2017\Projects\ReportReader\vbNetStandardLib\Class1.vb";

            if(System.Environment.OSVersion.Platform == PlatformID.Unix)
                inputFile = @"/root/RiderProjects/ReportReader/vbNetStandardLib/Class1.vb";
            
            inputFile = System.IO.File.ReadAllText(inputFile);
            
            CreateCompilation2(inputFile);
        }




        public static void Test()
        {
            
            
            try
            {
                // code for class A
                string classAString =
                    @"

Public Class A
    Public Shared Function Print() As String
        Return ""Hello "" 
    End Function
End Class

";

                // code for class B (to spice it up, it is a 
                // subclass of A even though it is almost not needed
                // for the demonstration)
                string classBString =
                    @"

Public Class B
    Inherits A

    Public Shared Function Print() As String
        Return ""World!""
    End Function
End Class

";

                // the main class Program contain static void Main() 
                // that calls A.Print() and B.Print() methods
                string mainProgramString =
                    @"

Public Class Program

    Public Shared Sub Main()
        ' System.Console.Write(A.Print())
        ' System.Console.WriteLine(B.Print())
        System.Console.WriteLine(""Privet mir"")
    End Sub
End Class

";

                #region class A compilation into A.netmodule

                var co = new Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilationOptions
                (
                    Microsoft.CodeAnalysis.OutputKind.NetModule
                );
                
                co.WithOptionStrict(Microsoft.CodeAnalysis.VisualBasic.OptionStrict.Off);
                co.WithOptionExplicit(false);
                co.WithOptionInfer(true);
                
                
                // create Roslyn compilation for class A
                Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation compilationA =
                    CreateCompilationWithMscorlib
                    (
                        "A",
                        classAString,
                        compilerOptions: co
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
                Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation compilationB =
                    CreateCompilationWithMscorlib
                    (
                        "B",
                        classBString,
                        compilerOptions: co,
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


                Microsoft.CodeAnalysis.MetadataReference sysCorlib = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                Microsoft.CodeAnalysis.MetadataReference sysConsole = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Console).Assembly.Location);
                Microsoft.CodeAnalysis.MetadataReference sysRuntime = Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location);

                var co2 = new Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilationOptions(
                    Microsoft.CodeAnalysis.OutputKind.ConsoleApplication);
                

                // create the Roslyn compilation for the main program with
                // ConsoleApplication compilation options
                // adding references to A.netmodule and B.netmodule
                Microsoft.CodeAnalysis.VisualBasic.VisualBasicCompilation mainCompilation =
                    CreateCompilationWithMscorlib
                    (
                        "program",
                        mainProgramString,
                        // note that here we pass the OutputKind set to ConsoleApplication
                        compilerOptions: co2,
                        references: new[] { sysCorlib, sysConsole, sysRuntime, referenceA, referenceB }
                    );
                
                // Emit the byte result of the compilation
                byte[] result = mainCompilation.EmitToArray();

                // Load the resulting assembly into the domain. 
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(result);

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
