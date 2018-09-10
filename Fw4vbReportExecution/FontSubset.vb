
Public Class FontManager

    ' https://stackoverflow.com/questions/6504131/create-subset-of-a-truetype-font-file-using-net


    ' Requires PresentationCore (System.Windows.Media)
    Sub New()
        CreateSubSet("my baloney has a first name", New Uri("C:\Windows\Fonts\impact.ttf"))
    End Sub


    Public Sub CreateSubSet(sourceText As String, fontURI As Uri)
        Dim glyphTypeface As System.Windows.Media.GlyphTypeface = New System.Windows.Media.GlyphTypeface(fontURI)
        Dim Index As System.Collections.Generic.ICollection(Of UShort)
        Index = New System.Collections.Generic.List(Of UShort)

        Dim sourceTextBytes As Byte() = System.Text.Encoding.Unicode.GetBytes(sourceText)
        Dim sourceTextChars As Char() = System.Text.Encoding.Unicode.GetChars(sourceTextBytes)
        Dim sourceTextCharVal As Integer

        Dim glyphIndex As Integer
        For sourceTextCharPos = 0 To UBound(sourceTextChars)
            sourceTextCharVal = AscW(sourceTextChars(sourceTextCharPos))
            glyphIndex = glyphTypeface.CharacterToGlyphMap(sourceTextCharVal)
            Index.Add(glyphIndex)
        Next sourceTextCharPos

        Dim filebytes() As Byte = glyphTypeface.ComputeSubset(Index)

        ' https://referencesource.microsoft.com/#PresentationCore/Core/CSharp/system/windows/Media/GlyphTypeface.cs
        ' https://referencesource.microsoft.com/#PresentationCore/Core/CSharp/MS/Internal/FontFace/FontDriver.cs
        ' Return TrueTypeSubsetter.ComputeSubset(fontData, fileSize, SourceUri, _directoryOffset, glyphArray);

        Using fileStream As New System.IO.FileStream("C:\Users\Me\Documents\impact-subset.ttf", System.IO.FileMode.Create)
            fileStream.Write(filebytes, 0, filebytes.Length)
        End Using

    End Sub


End Class
