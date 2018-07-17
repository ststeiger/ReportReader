using System;
using System.Collections.Generic;
using System.Text;

namespace ReportReader
{
    class CompilerTests
    {
        public static void Test()
        {

            TestCompilerCSharp.Test();
            TestCompilerVB.Test();
            RsCompiler.Test();
        }
    }
}
