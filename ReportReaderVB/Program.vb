
Module Program


    Function Xmlify(args As Object()) As String
        Dim str As String = Nothing

        Dim sb As New System.Text.StringBuilder
        For i As Integer = 0 To args.Length - 1 Step 1
            Dim s As String = System.Convert.ToString(args(i))
            If (String.IsNullOrEmpty(s)) Then
                Continue For
            End If

            sb.Append("<e>")
            sb.Append(s)
            sb.Append("</e>")
        Next i
        str = sb.ToString()

        Return str
    End Function


    Sub Main(args As String())
        Console.WriteLine(Xmlify(New String() {"abc", Nothing, "def", "ghi", "jkl", "mno"}))

        
        Dim opts As New System.CodeDom.Compiler.CompilerParameters()
        opts.CoreAssemblyFileName
        
        
        System.Console.WriteLine(System.Environment.NewLine)
        System.Console.WriteLine(" --- Press any key to continue --- ")
        System.Console.ReadKey()
    End Sub


End Module
