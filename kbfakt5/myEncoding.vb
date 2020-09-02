Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace kbfakt.Common.Encoding
    Public NotInheritable Class PetsciiEncoder
        Inherits Encoder
        Private _newLine As Byte() = Nothing
        Private _space As Byte() = Nothing
        Private _ascii As New Dictionary(Of Char, Byte)()

        Friend Sub New()
            _newLine = ASCIIEncoding.ASCII.GetBytes("" & Chr(10) & "")
            _space = ASCIIEncoding.ASCII.GetBytes(" ")
            For i As Byte = 0 To 254

                Dim c As Char = Chr(i) 'CChar(i)
                _ascii.Add(c, ASCIIEncoding.ASCII.GetBytes(New Char() {c})(0))
            Next
        End Sub

        Public Overloads Overrides Function GetByteCount(ByVal chars As Char(), ByVal index As Integer, ByVal count As Integer, ByVal flush As Boolean) As Integer
            Dim targetBytes As New List(Of Byte)()

            Dim sourceBytes As Byte() = ASCIIEncoding.ASCII.GetBytes(chars)
            For i As Integer = index To (index + count) - 1

                targetBytes.Add(TranslateCharacter(sourceBytes(i)))
            Next

            Return targetBytes.Count
        End Function

        Public Overloads Overrides Function GetBytes(ByVal chars As Char(), ByVal charIndex As Integer, ByVal charCount As Integer, ByVal bytes As Byte(), ByVal byteIndex As Integer, ByVal flush As Boolean) As Integer
            Dim targetBytes As New List(Of Byte)()

            Dim sourceBytes As Byte() = ASCIIEncoding.ASCII.GetBytes(chars)

            For Each b As Byte In sourceBytes
                targetBytes.Add(TranslateCharacter(b))
            Next
            For i As Integer = charIndex To (charIndex + charCount) - 1

                bytes(byteIndex + (i - charIndex)) = targetBytes(i)
            Next

            Return charCount
        End Function

        Private Function TranslateCharacter(ByVal Character As Byte) As Byte
            If Character >= 91 AndAlso Character <= 126 Then
                Return CByte((Character Xor 32))
            ElseIf Character >= _ascii("A"c) AndAlso Character <= _ascii("Z"c) Then
                Return CByte((Character Or 128))
            ElseIf Character = _newLine(0) Then
                Return 13
            End If

            Return Character
        End Function
    End Class

    Public NotInheritable Class PetsciiDecoder
        Inherits Decoder
        Private _newLine As Byte() = Nothing
        Private _space As Byte() = Nothing

        Friend Sub New()
            _newLine = ASCIIEncoding.ASCII.GetBytes("" & Chr(10) & "")
            _space = ASCIIEncoding.ASCII.GetBytes(" ")
        End Sub

        Public Overloads Overrides Function GetCharCount(ByVal bytes As Byte(), ByVal index As Integer, ByVal count As Integer) As Integer
            DecodeBytes(bytes, index, count, index)

            Return count

        End Function

        Public Overloads Overrides Function GetChars(ByVal bytes As Byte(), ByVal byteIndex As Integer, ByVal byteCount As Integer, ByVal chars As Char(), ByVal charIndex As Integer) As Integer
            Dim decodedChars As Char() = DecodeBytes(bytes, byteIndex, byteCount, charIndex)
            For i As Integer = byteIndex To (byteIndex + byteCount) - 1

                chars(charIndex + (i - byteIndex)) = decodedChars(i)
            Next

            Return byteCount
        End Function

        Private Function DecodeBytes(ByVal bytes As Byte(), ByVal byteIndex As Integer, ByVal byteCount As Integer, ByVal charIndex As Integer) As Char()
            Dim results As Char() = Nothing
            Dim output As New List(Of Byte)()
            Dim translated As Byte() = Nothing

            For Each b As Byte In bytes
                output.AddRange(TranslateByte(b))
            Next

            translated = output.ToArray()

            results = ASCIIEncoding.ASCII.GetChars(translated)

            Return results
        End Function

        Private Function TranslateByte(ByVal SourceByte As Byte) As Byte()
            Select Case SourceByte And 255
                Case 10, 13
                    Return _newLine
                Case 64, 96
                    Return New Byte() {SourceByte}
                Case 160, 224
                    Return _space
                Case Else
                    Select Case SourceByte And 224
                        Case 64, 96
                            Return New Byte() {CByte((SourceByte Xor CByte(32)))}
                        Case 192

                            Return New Byte() {CByte((SourceByte Xor CByte(128)))}

                    End Select

                    Return New Byte() {SourceByte}
            End Select
        End Function
    End Class

    Public Class Petscii
        Inherits System.Text.Encoding
        Public Shared Function GetPetsciiEncoder() As PetsciiEncoder
            Return New PetsciiEncoder()
        End Function

        Public Shared Function GetPetsciiDecoder() As PetsciiDecoder
            Return New PetsciiDecoder()
        End Function

        Public Overloads Overrides Function GetByteCount(ByVal chars As Char(), ByVal index As Integer, ByVal count As Integer) As Integer
            Return GetPetsciiEncoder().GetByteCount(chars, index, count, False)
        End Function

        Public Overloads Overrides Function GetBytes(ByVal chars As Char(), ByVal charIndex As Integer, ByVal charCount As Integer, ByVal bytes As Byte(), ByVal byteIndex As Integer) As Integer
            Return GetPetsciiEncoder().GetBytes(chars, charIndex, charCount, bytes, byteIndex, False)
        End Function

        Public Overloads Overrides Function GetCharCount(ByVal bytes As Byte(), ByVal index As Integer, ByVal count As Integer) As Integer
            Return GetPetsciiDecoder().GetCharCount(bytes, index, count)
        End Function

        Public Overloads Overrides Function GetChars(ByVal bytes As Byte(), ByVal byteIndex As Integer, ByVal byteCount As Integer, ByVal chars As Char(), ByVal charIndex As Integer) As Integer
            Return GetPetsciiDecoder().GetChars(bytes, byteIndex, byteCount, chars, charIndex)
        End Function

        Public Overloads Overrides Function GetMaxByteCount(ByVal charCount As Integer) As Integer
            Return charCount
        End Function

        Public Overloads Overrides Function GetMaxCharCount(ByVal byteCount As Integer) As Integer
            Return byteCount
        End Function
    End Class
End Namespace
