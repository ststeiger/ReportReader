
Module Module1


    Sub Main()
        Dim result As Object = TestCompiler.Eval("Return (1+1).ToString() + ""px"" ")
        System.Console.WriteLine(result)

        System.Console.WriteLine(System.Environment.NewLine)
        System.Console.WriteLine(" --- Press any key to continue --- ")
        System.Console.ReadKey()
    End Sub


End Module
