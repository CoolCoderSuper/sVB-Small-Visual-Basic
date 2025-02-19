﻿Imports Microsoft.SmallBasic
Imports Microsoft.SmallVisualBasic.Documents
Imports Microsoft.SmallVisualBasic.LanguageService
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Documents
Imports System.Windows.Markup
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation

Namespace Microsoft.SmallVisualBasic.Utility
    Partial Public Class HelpPanel
        Inherits Grid
        Implements INotifyPropertyChanged, IComponentConnector

        Private _itemWrapper As CompletionItemWrapper
        Private _paramsNameStyle As Style
        Private _paramsDescriptionStyle As Style
        Private _usageStyle As Style
        Private _typeStyle As Style
        Private _selectedStyle As Style
        Private _codeExampleStyle As Style
        Private _membersStyle As Style
        Private _linkStyle As Style

        Dim gotoRun As New TextRun()
        Dim WithEvents gotoDefinintion As New Hyperlink(gotoRun)

        Public Property CompletionItemWrapper As CompletionItemWrapper
            Get
                Return _itemWrapper
            End Get

            Set(value As CompletionItemWrapper)
                _itemWrapper = value
                Dim item = value.CompletionItem


                Dim doc = MainWindow.ActiveDocument
                Dim textView = CType(doc.EditorControl.TextView, Nautilus.Text.Editor.AvalonTextView)
                Dim popHelp = MainWindow.PopHelp

                If App.CntxMenuIsOpened OrElse CompletionItemWrapper.Documentation Is Nothing Then
                    popHelp.IsOpen = False
                    Return
                End If

                helpDocument.Blocks.Clear()
                methodType = _itemWrapper.Documentation?.Suffix

                Select Case value.SymbolType
                    Case SymbolType.Method, SymbolType.Subroutine
                        FillInfo(True)

                    Case SymbolType.Control, SymbolType.GlobalModule,
                             SymbolType.DynamicProperty,
                             SymbolType.Label, SymbolType.Literal,
                             SymbolType.GlobalVariable, SymbolType.LocalVariable
                        FillInfo(False)

                    Case SymbolType.Property, SymbolType.Event
                        Dim type = value.SymbolType.ToString() & ": " & GetTypeDisplayName()
                        Dim definition As New Span()
                        FillTitle(definition)
                        FillDescription(definition, If(type = "", "", type & "."))

                    Case SymbolType.Type
                        methodType = " Type"
                        Dim type = GetTypeDisplayName()
                        Dim definition As New Span()
                        FillTitle(definition)
                        FillDescription(definition, If(type = "", "", type & "."))

                    Case Else
                        popHelp.IsOpen = False
                        Return
                End Select

                FillExample()

                popHelp.PlacementTarget = textView
                Dim caret = textView.Caret
                popHelp.HorizontalOffset = 10
                popHelp.MaxWidth = textView.ActualWidth - 20
                popHelp.MaxHeight = Math.Min(250, textView.ActualHeight - caret.Top)
                popHelp.VerticalOffset = caret.Top + caret.Height + 5
                popHelp.IsOpen = True
                popHelp.Tag = textView
                AddHandler textView.ScrollChaged, AddressOf OnScrollChaged
            End Set
        End Property

        Private Function GetTypeDisplayName() As String
            Dim type = _itemWrapper.CompletionItem.MemberInfo?.DeclaringType
            Return WinForms.PreCompiler.GetTypeDisplayName(type)
        End Function

        Private Sub OnScrollChaged(senmder As Object, e As ScrollEventArgs)
            Dim popHelp = MainWindow.PopHelp
            popHelp.IsOpen = False
            Dim textView = CType(popHelp.Tag, Microsoft.Nautilus.Text.Editor.IAvalonTextView)
            If textView IsNot Nothing Then
                RemoveHandler textView.ScrollChaged, AddressOf OnScrollChaged
            End If
        End Sub

        Private Sub FillDescription(memberName As Span, belongsTo As String, Optional showSummery As Boolean = True)
            Dim documentation = CompletionItemWrapper.Documentation

            Dim p = New Paragraph()
            helpDocument.Blocks.Add(p)
            p.Inlines.Add(New TextRun With {
                    .Text = belongsTo,
                    .Style = _typeStyle
                }
            )

            p.Inlines.Add(memberName)

            If showSummery AndAlso documentation.Summary <> "" Then
                helpDocument.Blocks.Add(
                    New Paragraph(
                        New TextRun With {
                            .Text = documentation.Summary,
                            .Style = _paramsDescriptionStyle
                        }
                    ) With {.Margin = New Thickness(15, 0, 0, 5)}
                )
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private methodType As String

        Dim mainWindow As MainWindow = Helper.MainWindow

        Function IsCompletionListOpen() As Boolean
            Dim completionSurface As CompletionSurface = Nothing
            Dim properties = mainWindow.ActiveDocument.EditorControl.TextView.Properties
            Dim adornmentType = GetType(CompletionSurface)

            Return properties.TryGetProperty(adornmentType, completionSurface) AndAlso
                    completionSurface.IsAdornmentVisible

        End Function

        Public Sub New()
            Me.InitializeComponent()
            _paramsNameStyle = CType(FindResource("paramsNameStyle"), Style)
            _paramsDescriptionStyle = CType(FindResource("paramsDescriptionStyle"), Style)
            _usageStyle = CType(FindResource("usageStyle"), Style)
            _typeStyle = CType(FindResource("typeStyle"), Style)
            _selectedStyle = CType(FindResource("selectedStyle"), Style)
            _linkStyle = CType(FindResource("linkStyle"), Style)
            _codeExampleStyle = CType(FindResource("codeExampleStyle"), Style)
            _membersStyle = CType(FindResource("membersStyle"), Style)
            gotoDefinintion.NavigateUri = New Uri("___test", UriKind.RelativeOrAbsolute)
        End Sub

        Private Function GetIconForSymbolType(symbolType As SymbolType) As BitmapImage
            Select Case symbolType
                Case SymbolType.Event
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseEvent.png"))
                Case SymbolType.Keyword
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseKeyword.png"))
                Case SymbolType.Method
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseMethod.png"))
                Case SymbolType.Property
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseProperty.png"))
                Case SymbolType.Type
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseItem.png"))
                Case SymbolType.GlobalVariable
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseVariable.png"))
                Case SymbolType.Subroutine
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseSubroutine.png"))
                Case SymbolType.Label
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseLabel.png"))
                Case Else
                    Return New BitmapImage(New Uri("pack://application:,,/sVB;component/Resources/IntellisenseItem.png"))
            End Select
        End Function

        Private Sub FillInfo(hasParams As Boolean)
            Dim documentation = _itemWrapper.Documentation
            Dim item = _itemWrapper.CompletionItem
            Dim name = GetTypeDisplayName()
            Dim definition As New Span()
            Dim params = documentation.ParamsDoc.Keys
            Dim paramIndex = item.ParamIndex
            Dim addReturnOnly = paramIndex >= params.Count
            Dim addAllParams = paramIndex = -1 OrElse (addReturnOnly AndAlso documentation.Returns = "")

            FillTitle(definition, hasParams)

            FillDescription(definition, If(name = "", documentation.Prefix, name & "."), addAllParams)

            ' Add param Info
            If params.Count > 0 Then

                Dim paramsDocs As New Paragraph With {
                    .Margin = New Thickness(15, 0, 0, 5)
                }

                Dim inlines = paramsDocs.Inlines
                Dim n = params.Count - 1

                For i = 0 To n
                    If Not addAllParams AndAlso i <> paramIndex Then Continue For

                    Dim paramDoc = documentation.ParamsDoc(params(i))
                    If paramDoc <> "" Then
                        inlines.Add(New TextRun() With {
                                .Text = params(i) & ": ",
                                .Style = _paramsNameStyle
                            })

                        inlines.Add(New TextRun() With {
                                .Text = paramDoc & If(addAllParams AndAlso i < n, vbCrLf, ""),
                                .Style = _paramsDescriptionStyle
                            })
                    End If
                Next

                If inlines.Count > 0 Then
                    If addAllParams Then
                        helpDocument.Blocks.Add(
                            New Paragraph(New TextRun With {
                                .Text = $"Parameter{If(addAllParams AndAlso n > 1, "s", "")} Info: ",
                                .Style = _paramsNameStyle
                            })
                        )
                    End If

                    helpDocument.Blocks.Add(paramsDocs)
                End If
            End If

            ' Add Return info
            If (addAllParams OrElse addReturnOnly) AndAlso documentation.Returns <> "" Then
                Dim p = New Paragraph() With {.Margin = New Thickness(0, 0, 0, 5)}
                helpDocument.Blocks.Add(p)
                Dim inlines = p.Inlines
                inlines.Add(New TextRun With {
                        .Text = ResourceHelper.GetString("Returns") & ": ",
                        .Style = _paramsNameStyle
                    })

                inlines.Add(New TextRun With {
                        .Text = documentation.Returns,
                        .Style = _paramsDescriptionStyle
                    })
            End If
        End Sub

        Private Sub FillParams(definition As Span)
            Dim params = _itemWrapper.Documentation.ParamsDoc.Keys
            Dim paramIndex = _itemWrapper.CompletionItem.ParamIndex

            definition.Inlines.Add(New TextRun() With {
                    .Text = "(",
                    .Style = _typeStyle
             })

            Dim n = params.Count - 1
            For i = 0 To n
                definition.Inlines.Add(New TextRun() With {
                    .Text = params(i),
                    .Style = If(i = paramIndex, _selectedStyle, _typeStyle)
                 })

                If i < n Then
                    definition.Inlines.Add(New TextRun() With {
                         .Text = ", ",
                        .Style = _typeStyle
                    })
                End If
            Next

            definition.Inlines.Add(New TextRun() With {
                        .Text = ")",
                        .Style = _typeStyle
                 })
        End Sub

        Private Sub FillTitle(definition As Span, Optional hasParams As Boolean = False)
            Dim item = _itemWrapper.CompletionItem

            If item.DefinitionIdintifier.IsIllegal Then
                definition.Inlines.Add(New TextRun() With {
                    .Text = _itemWrapper.Display,
                    .Style = _typeStyle
                })
            Else
                gotoRun.Text = _itemWrapper.Display
                gotoRun.Style = _linkStyle
                definition.Inlines.Add(gotoDefinintion)
            End If

            If hasParams Then FillParams(definition)
            definition.Inlines.Add(New TextRun() With {
                .Text = methodType,
                .Style = _typeStyle
            })
        End Sub

        Private Sub FillTypeMembers()
            Dim value As TypeInfo = Nothing
            Dim detailsParagraph As New Paragraph
            If CompletionItemWrapper.Documentation IsNot Nothing Then
                Dim name = CompletionItemWrapper.CompletionItem.MemberInfo.Name

                If CompilerService.DummyCompiler.TypeInfoBag.Types.TryGetValue(name.ToLowerInvariant(), value) Then
                    For Each item In value.Properties.OrderBy(Function(a) a.Key)
                        detailsParagraph.Inlines.Add(New Image With {
                            .Source = GetIconForSymbolType(LanguageService.SymbolType.Property),
                            .Width = 16.0,
                            .Height = 16.0,
                            .Margin = New Thickness(2.0),
                            .VerticalAlignment = VerticalAlignment.Center
                        })
                        detailsParagraph.Inlines.Add(New TextRun With {
                            .Text = item.Value.Name & vbLf,
                            .Style = _membersStyle
                        })
                    Next

                    For Each item2 In value.Methods.OrderBy(Function(a) a.Key)
                        detailsParagraph.Inlines.Add(New Image With {
                            .Source = GetIconForSymbolType(LanguageService.SymbolType.Method),
                            .Width = 16.0,
                            .Height = 16.0,
                            .Margin = New Thickness(2.0),
                            .VerticalAlignment = VerticalAlignment.Center
                        })
                        detailsParagraph.Inlines.Add(New TextRun With {
                            .Text = item2.Value.Name & vbLf,
                            .Style = _membersStyle
                        })
                    Next

                    For Each item3 In value.Events.OrderBy(Function(a) a.Key)
                        detailsParagraph.Inlines.Add(New Image With {
                            .Source = GetIconForSymbolType(LanguageService.SymbolType.Event),
                            .Width = 16.0,
                            .Height = 16.0,
                            .Margin = New Thickness(2.0),
                            .VerticalAlignment = VerticalAlignment.Center
                        })
                        detailsParagraph.Inlines.Add(New TextRun With {
                            .Text = item3.Value.Name & vbLf,
                            .Style = _membersStyle
                        })
                    Next
                End If
            End If
        End Sub

        Private Sub FillExample()
            Dim detailsParagraph As New Paragraph
            If CompletionItemWrapper.Documentation IsNot Nothing AndAlso Not Equals(CompletionItemWrapper.Documentation.Example, Nothing) Then
                detailsParagraph.Inlines.Add(New TextRun With {
                    .Text = ResourceHelper.GetString("Example"),
                    .Style = _paramsNameStyle
                })

                detailsParagraph.Inlines.Add(New LineBreak())
                '__ = CompletionItemWrapper.Documentation.Example
                detailsParagraph.Inlines.Add(New TextRun With {
                    .Text = CompletionItemWrapper.Documentation.Example,
                    .Style = _codeExampleStyle
                })

                detailsParagraph.Inlines.Add(New LineBreak())
            End If
        End Sub

        Private Sub Notify([property] As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs([property]))
        End Sub

        Private Sub gotoDefinintion_RequestNavigate(sender As Object, e As RequestNavigateEventArgs) Handles gotoDefinintion.RequestNavigate
            MainWindow.PopHelp.IsOpen = False
            Dim item = _itemWrapper.CompletionItem
            Dim identifier = item.DefinitionIdintifier

            Select Case _itemWrapper.NavigateTo
                Case NavigateTo.None
                    SelectToken(identifier, MainWindow.ActiveDocument)

                Case NavigateTo.Designer
                    MainWindow.SelectControl(item.DisplayName)

                Case NavigateTo.GlobalModule
                    Dim progDir = MainWindow.ProjExplorer.ProjectDirectory
                    If progDir <> "" Then
                        Dim globalFile = IO.Path.Combine(progDir, "global.sb")
                        If IO.File.Exists(globalFile) Then
                            Dim doc = MainWindow.OpenDocIfNot(globalFile)
                            If identifier.Column > -1 Then SelectToken(identifier, doc)
                        End If
                    End If
            End Select
        End Sub

        Private Shared Sub SelectToken(identifier As Token, doc As TextDocument)
            Dim editor = doc.EditorControl
            Dim snapshot = editor.TextView.TextSnapshot
            Dim lineStart = snapshot.GetLineFromLineNumber(identifier.Line).Start
            Dim pos = lineStart + identifier.Column
            doc.EnsureAtTop(pos)
            editor.EditorOperations.SelectCurrentWord()
        End Sub
    End Class
End Namespace
