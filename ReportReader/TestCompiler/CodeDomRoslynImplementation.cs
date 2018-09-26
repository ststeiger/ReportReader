
namespace ReportReader.TestCompiler
{

    public class RoslynVisualBasicCompiler
        : System.CodeDom.Compiler.ICodeCompiler
    {


        public System.CodeDom.Compiler.CompilerResults CompileAssemblyFromDom(
              System.CodeDom.Compiler.CompilerParameters options
            , System.CodeDom.CodeCompileUnit compilationUnit)
        {
            throw new System.NotImplementedException();
        }


        public System.CodeDom.Compiler.CompilerResults CompileAssemblyFromDomBatch(
              System.CodeDom.Compiler.CompilerParameters options
            , System.CodeDom.CodeCompileUnit[] compilationUnits)
        {
            throw new System.NotImplementedException();
        }


        public System.CodeDom.Compiler.CompilerResults CompileAssemblyFromFile(
            System.CodeDom.Compiler.CompilerParameters options, string fileName)
        {
            throw new System.NotImplementedException();
        }


        public System.CodeDom.Compiler.CompilerResults CompileAssemblyFromFileBatch(
            System.CodeDom.Compiler.CompilerParameters options
            , string[] fileNames)
        {
            throw new System.NotImplementedException();
        }


        public System.CodeDom.Compiler.CompilerResults CompileAssemblyFromSource(
            System.CodeDom.Compiler.CompilerParameters options, string source)
        {
            throw new System.NotImplementedException();
        }


        public System.CodeDom.Compiler.CompilerResults CompileAssemblyFromSourceBatch(
              System.CodeDom.Compiler.CompilerParameters options
            , string[] sources)
        {
            throw new System.NotImplementedException();
        }


    }


    public class RoslynVbCodeGenerator 
        : System.CodeDom.Compiler.ICodeGenerator
    {
        public string CreateEscapedIdentifier(string value)
        {
            throw new System.NotImplementedException();
        }

        public string CreateValidIdentifier(string value)
        {
            throw new System.NotImplementedException();
        }

        public void GenerateCodeFromCompileUnit(
              System.CodeDom.CodeCompileUnit e
            , System.IO.TextWriter w
            , System.CodeDom.Compiler.CodeGeneratorOptions o)
        {
            throw new System.NotImplementedException();
        }

        public void GenerateCodeFromExpression(
              System.CodeDom.CodeExpression e
            , System.IO.TextWriter w
            , System.CodeDom.Compiler.CodeGeneratorOptions o)
        {
            throw new System.NotImplementedException();
        }

        public void GenerateCodeFromNamespace(
              System.CodeDom.CodeNamespace e
            , System.IO.TextWriter w
            , System.CodeDom.Compiler.CodeGeneratorOptions o)
        {
            throw new System.NotImplementedException();
        }

        public void GenerateCodeFromStatement(
              System.CodeDom.CodeStatement e
            , System.IO.TextWriter w
            , System.CodeDom.Compiler.CodeGeneratorOptions o)
        {
            throw new System.NotImplementedException();
        }

        public void GenerateCodeFromType(
              System.CodeDom.CodeTypeDeclaration e
            , System.IO.TextWriter w
            , System.CodeDom.Compiler.CodeGeneratorOptions o)
        {
            throw new System.NotImplementedException();
        }

        public string GetTypeOutput(System.CodeDom.CodeTypeReference type)
        {
            throw new System.NotImplementedException();
        }

        public bool IsValidIdentifier(string value)
        {
            throw new System.NotImplementedException();
        }

        public bool Supports(System.CodeDom.Compiler.GeneratorSupport supports)
        {
            throw new System.NotImplementedException();
        }

        public void ValidateIdentifier(string value)
        {
            throw new System.NotImplementedException();
        }
    }


    public class RoslynCodeDomProvider 
        : System.CodeDom.Compiler.CodeDomProvider
    {


        public override System.CodeDom.Compiler.ICodeCompiler CreateCompiler()
        {
            throw new System.NotImplementedException();
        }


        public override System.CodeDom.Compiler.ICodeGenerator CreateGenerator()
        {
            throw new System.NotImplementedException();
        }


        //public RoslynCompiler() { }
        //public RoslynCompiler(System.Collections.Generic.IDictionary<string, string> providerOptions) { }

        //public override string FileExtension { get; }
        //public override System.CodeDom.Compiler.LanguageOptions LanguageOptions { get; }
        //public override void GenerateCodeFromMember(System.CodeDom.CodeTypeMember member, System.IO.TextWriter writer, System.CodeDom.Compiler.CodeGeneratorOptions options) { }
        //public override System.ComponentModel.TypeConverter GetConverter(Type type) { return null; }

    }


}
