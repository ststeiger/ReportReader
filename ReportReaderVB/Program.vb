
Module Program


    Sub Main(args As String())
        Dim Code As New RsCode()


        System.Console.WriteLine(Code.GetFirstLine("hello
there"))

        ' Dim strXml As String = Code.Xmlify("a", "b", "c")
        ' System.Console.WriteLine(Code.Xmlify(New String() {"abc", Nothing, "def", "ghi", "jkl", "mno"}))

        'MOST_RECENT_HIRE_DATE
        Dim str As String = Code.ToDateString("20121201")
        System.Console.WriteLine(str)



        ' Dim opts As New System.CodeDom.Compiler.CompilerParameters()
        ' System.Console.WriteLine(opts.CoreAssemblyFileName)


        System.Console.WriteLine(System.Environment.NewLine)
        System.Console.WriteLine(" --- Press any key to continue --- ")
        System.Console.ReadKey()
    End Sub


End Module
