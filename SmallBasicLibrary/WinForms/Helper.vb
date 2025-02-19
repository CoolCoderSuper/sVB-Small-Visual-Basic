﻿Imports Microsoft.SmallVisualBasic.Library

Module Helper

    Private exeFile As String = Process.GetCurrentProcess().MainModule.FileName
    Private exeDir As String = IO.Path.GetDirectoryName(exeFile)
    Private errorFile As String = IO.Path.Combine(exeDir, "RuntimeErrors.txt")
    Private ClearErrors As Boolean = True

    Public Sub ReportError(msg As String, ex As Exception)
        Try
            If ClearErrors Then
                ClearErrors = False
                File.DeleteFile(errorFile)
            End If
            File.AppendContents(errorFile, Now & vbCrLf & msg & vbCrLf)

        Catch
        End Try

    End Sub
End Module
