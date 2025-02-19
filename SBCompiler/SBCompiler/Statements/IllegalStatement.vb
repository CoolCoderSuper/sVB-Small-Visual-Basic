﻿Namespace Microsoft.SmallVisualBasic.Statements
    Public Class IllegalStatement
        Inherits Statement

        Public Sub New(startToken As Token)
            Me.StartToken = startToken
        End Sub

        Public Overrides Function GetStatementAt(lineNumber As Integer) As Statement
            If lineNumber = StartToken.Line Then Return Me
            Return Nothing
        End Function


        Public Overrides Function ToString() As String
            Return "<<Illegal>>" & vbCrLf
        End Function
    End Class
End Namespace
