
using System.Linq;


namespace ReportReader.TestCompiler
{


    public class RsRoslynCodeDom
    {



        public static string GetvbNetStandardLibPath()
        {
            string dir = System.IO.Path.GetDirectoryName(typeof(TestCompilerVB).Assembly.Location);
            dir = System.IO.Path.Combine(dir, "..", "..", "..", "..", "vbNetStandardLib");
            dir = System.IO.Path.GetFullPath(dir);

            return dir;
        }



        public static System.Reflection.Assembly GetNetStdAssembly()
        {
            System.Reflection.Assembly nsAssembly = null;

            System.Reflection.AssemblyName[] asms = typeof(Microsoft.VisualBasic.Constants).Assembly.GetReferencedAssemblies();


            foreach (System.Reflection.AssemblyName asm in asms)
            {
                if (asm.FullName.StartsWith("netstandard,", System.StringComparison.OrdinalIgnoreCase))
                {
                    nsAssembly = System.Reflection.Assembly.Load(asm.FullName);
                    break;
                }
            }

            return nsAssembly;


            //System.Reflection.Assembly[] asms = System.AppDomain.CurrentDomain.GetAssemblies();
            //
            //foreach (System.Reflection.Assembly asm in asms)
            //{
            //    if (asm.FullName.StartsWith("netstandard,", System.StringComparison.OrdinalIgnoreCase))
            //    {
            //        nsAssembly = asm;
            //        break;
            //    }
            //}

            // return nsAssembly;
        }



        // TestCompilerVB.Test2();
        private static System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> GetAssemblyReferences()
        {
            string rss = System.IO.Path.Combine(GetvbNetStandardLibPath(), "Libs", "AspNetCore.ReportingServices.dll");

            System.Reflection.Assembly rsAssembly = System.Reflection.Assembly.LoadFile(rss);
            System.Reflection.Assembly nsAssembly = GetNetStdAssembly();


            Microsoft.CodeAnalysis.MetadataReference[] references =
                new Microsoft.CodeAnalysis.MetadataReference[]
            {

                 Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(rsAssembly.Location)
                ,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(nsAssembly.Location)


                , // System.Private.CoreLib.dll
                Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(System.MarshalByRefObject).Assembly.Location
                )

                , // System.Runtime.dll
                Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(System.IO.FileAttributes).Assembly.Location
                )
                 
                //,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    //   typeof(System.Uri).Assembly.Location
                    //)

                //,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    //   typeof(System.Drawing.RectangleF).Assembly.Location
                    //)
                 
                 //,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                 //   typeof(System.Drawing.Graphics).Assembly.Location
                 //)
                 
                 //,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                 //   typeof(System.Data.Common.DbCommand).Assembly.Location
                 //)

                 //,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                 //   typeof(System.Data.SqlClient.SqlCommand).Assembly.Location
                 //)
                 
                 ,Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(
                    typeof(Microsoft.VisualBasic.Constants).Assembly.Location
                 )
                 

                // A bit hacky, if you need it
                //MetadataReference.CreateFromFile(Path.Combine(typeof(object).GetTypeInfo().Assembly.Location, "..", "mscorlib.dll")),
            };

            return references;
        }



        private static void CreateCompilationMultiFile(string fileName)
        {
            string[] filenames = new string[] { fileName };
            CreateCompilationMultiFile(filenames);
        }


        private static void CreateCompilationMultiFile(string[] filenames)
        {
            string assemblyName = System.Guid.NewGuid().ToString();
            System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> references =
                GetAssemblyReferences();

            Microsoft.CodeAnalysis.SyntaxTree[] syntaxTrees = new Microsoft.CodeAnalysis.SyntaxTree[filenames.Length];

            Microsoft.CodeAnalysis.VisualBasic.VisualBasicParseOptions op = null;

            for (int i = 0; i < filenames.Length; ++i)
            {
                string fileContent = System.IO.File.ReadAllText(filenames[i], System.Text.Encoding.UTF8);
                syntaxTrees[i] = Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxTree.ParseText(
                      fileContent
                    , op
                    , filenames[i]
                    , System.Text.Encoding.UTF8
                );
            }



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
               syntaxTrees,
               references,
               co
            );


            // byte[] compilationResult = compilation.EmitToArray();

            using (System.IO.MemoryStream dllStream = new System.IO.MemoryStream())
            {
                using (System.IO.MemoryStream pdbStream = new System.IO.MemoryStream())
                {
                    Microsoft.CodeAnalysis.Emit.EmitResult emitResult = compilation.Emit(dllStream, pdbStream);
                    if (!emitResult.Success)
                    {
                        CheckCompilationResult(emitResult);
                    } // End if (!emitResult.Success) 

                } // End Using pdbStream 

            } // End Using dllStream 



            /*
            // get the type Program from the assembly
            System.Type programType = assembly.GetType("Program");
            
            // Get the static Main() method info from the type
            System.Reflection.MethodInfo method = programType.GetMethod("Main");
            
            // invoke Program.Main() static method
            method.Invoke(null, null);
            */

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Microsoft.CodeAnalysis.Emit.EmitResult result = compilation.Emit(ms);
                ThrowExceptionIfCompilationFailure(result);
                ms.Seek(0, System.IO.SeekOrigin.Begin);


                System.Reflection.Assembly assembly2 = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
#if NET46
                // Different in full .Net framework
                System.Reflection.Assembly assembly3 = Assembly.Load(ms.ToArray());
#endif
            } // End Using ms 

        }


        // https://gist.github.com/GeorgDangl/4a9982a3b520f056a9e890635b3695e0
        private static System.CodeDom.Compiler.CompilerResults CheckCompilationResult(Microsoft.CodeAnalysis.Emit.EmitResult emitResult)
        {

            System.CodeDom.Compiler.CompilerResults compilerResults = new System.CodeDom.Compiler.CompilerResults(null);
            compilerResults.NativeCompilerReturnValue = 0;


            if (!emitResult.Success)
            {

                foreach (Microsoft.CodeAnalysis.Diagnostic diagnostic in emitResult.Diagnostics)
                {
                    // options.TreatWarningsAsErrors
                    if (diagnostic.IsWarningAsError || diagnostic.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    {
                        compilerResults.NativeCompilerReturnValue = -1;

                        string errorNumber = diagnostic.Id;
                        string errorMessage = diagnostic.GetMessage();
                        string message = $"{errorNumber}: {errorMessage};";
                        Microsoft.CodeAnalysis.FileLinePositionSpan lineSpan = diagnostic.Location.GetLineSpan();

                        string fileName = lineSpan.Path;
                        int line = lineSpan.StartLinePosition.Line;
                        int col = lineSpan.StartLinePosition.Character;

                        compilerResults.Errors.Add(
                            new System.CodeDom.Compiler.CompilerError(fileName, line, col, errorNumber, errorMessage)
                        );

                        throw new System.Exception($"Compilation failed, first error is: {message}");
                    } // End if 

                } // Next diagnostic 

            } // End if (!emitResult.Success) 

            return compilerResults;
        } // End Sub CheckCompilationResult 




        private static void CreateCompilation2(string code)
        {
            Microsoft.CodeAnalysis.SyntaxTree syntaxTree =
                Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxTree.ParseText(code);

            string assemblyName = System.Guid.NewGuid().ToString();
            System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> references =
                GetAssemblyReferences();


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




            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Microsoft.CodeAnalysis.Emit.EmitResult result = compilation.Emit(ms);
                ThrowExceptionIfCompilationFailure(result);
                ms.Seek(0, System.IO.SeekOrigin.Begin);


                System.Reflection.Assembly assembly2 = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
#if NET46
                // Different in full .Net framework
                System.Reflection.Assembly assembly3 = Assembly.Load(ms.ToArray());
#endif
            }
        }



        private static void CreateCsCompilation(string code)
        {
            Microsoft.CodeAnalysis.SyntaxTree syntaxTree =
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(code);

            string assemblyName = System.Guid.NewGuid().ToString();
            System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> references =
                GetAssemblyReferences();

            Microsoft.CodeAnalysis.Compilation compilation = Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(
                    Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary)
            );

            // byte[] compilationResult = compilation.EmitToArray();
            // System.Reflection.Assembly.Load(compilationResult);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Microsoft.CodeAnalysis.Emit.EmitResult result = compilation.Emit(ms);
                ThrowExceptionIfCompilationFailure(result);
                ms.Seek(0, System.IO.SeekOrigin.Begin);


                System.Reflection.Assembly assembly2 = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
#if NET46
                // Different in full .Net framework
                System.Reflection.Assembly assembly3 = Assembly.Load(ms.ToArray());
#endif
            }


            // _compilation = compilation;
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
                System.Collections.Generic.List<Microsoft.CodeAnalysis.Diagnostic> compilationErrors =
                    result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error)
                    .ToList();

                if (compilationErrors.Any())
                {
                    Microsoft.CodeAnalysis.Diagnostic firstError = compilationErrors.First();
                    string errorNumber = firstError.Id;
                    string errorDescription = firstError.GetMessage();
                    string firstErrorMessage = $"{errorNumber}: {errorDescription};";
                    throw new System.Exception($"Compilation failed, first error is: {firstErrorMessage}");
                }
            }
        }


        public static void Test2()
        {
            string inputFile = System.IO.Path.Combine(GetvbNetStandardLibPath(), "Class1.vb");
            string inputFileText = System.IO.File.ReadAllText(inputFile);

            CreateCompilationMultiFile(inputFile);
            CreateCompilation2(inputFileText);
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

            Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions options =
                new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(
                    Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: enableOptimisations ? Microsoft.CodeAnalysis.OptimizationLevel.Release : Microsoft.CodeAnalysis.OptimizationLevel.Debug,
                    allowUnsafe: true
            );


            Microsoft.CodeAnalysis.Compilation compilation = Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create(
                assemblyName: "InMemoryAssembly", options: options)
                .AddReferences(_references)
                .AddSyntaxTrees(syntaxTree);

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            Microsoft.CodeAnalysis.Emit.EmitResult emitResult = compilation.Emit(stream);

            if (emitResult.Success)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                // System.Reflection.Metadata.AssemblyDefinition assembly = System.Reflection.Metadata.AssemblyDefinition.ReadAssembly(stream);
            }
        }


    } // End Class RsRoslynCodeDom 


} // End Namespace ReportReader.TestCompiler 
