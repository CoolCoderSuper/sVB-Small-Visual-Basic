﻿Imports System.Collections.Generic
Imports System.Globalization
Imports System.Reflection.Emit
Imports System.Text
Imports Microsoft.SmallBasic.Completion
Imports Microsoft.SmallBasic.Expressions
Imports Microsoft.SmallBasic.Library

Namespace Microsoft.SmallBasic.Statements
    Public Class ForStatement
        Inherits LoopStatement


        Dim _InitialValue As Expression
        Public Property InitialValue As Expression
            Get
                Return _InitialValue
            End Get

            Set(value As Expression)
                _InitialValue = ToNumber(value)
            End Set
        End Property

        Private Function ToNumber(value As Expression) As Expression
            If value Is Nothing Then Return Nothing

            Dim valueToken = value.StartToken
            Dim typeName = New Token With {
                    .Line = valueToken.Line,
                    .Column = valueToken.Column,
                    .Type = TokenType.Identifier,
                    .Text = "Text"
            }

            Dim methodName = New Token With {
                    .Line = valueToken.Line,
                    .Column = valueToken.Column + 5,
                    .Type = TokenType.Identifier,
                    .Text = "ToNumber"
            }

            Dim args As New List(Of Expression)
            args.Add(value)

            Return New MethodCallExpression(
                valueToken,
                9,
                typeName,
                methodName,
                args
            ) With {
                .EndToken = value.EndToken,
                .Parent = Me,
                .OuterSubroutine = SubroutineStatement.Current
            }
        End Function

        Dim _FinalValue As Expression
        Public Property FinalValue As Expression
            Get
                Return _FinalValue
            End Get

            Set(value As Expression)
                _FinalValue = ToNumber(value)
            End Set
        End Property

        Dim _StepValue As Expression
        Public Property StepValue As Expression
            Get
                Return _StepValue
            End Get

            Set(value As Expression)
                _StepValue = ToNumber(value)
            End Set
        End Property

        Public Iterator As Token
        Public ForToken As Token
        Public ToToken As Token
        Public StepToken As Token
        Public Subroutine As SubroutineStatement
        Public EqualsToken As Token

        Public Overrides Function GetStatementAt(lineNumber As Integer) As Statement
            If lineNumber < StartToken.Line Then Return Nothing
            If lineNumber > EndLoopToken.Line Then Return Nothing
            If lineNumber <= Iterator.Line Then Return Me
            If lineNumber <= EqualsToken.Line Then Return Me
            If lineNumber <= _InitialValue?.EndToken.Line Then Return Me
            If lineNumber <= ToToken.Line Then Return Me
            If lineNumber <= _FinalValue?.EndToken.Line Then Return Me
            If lineNumber <= StepToken.Line Then Return Me
            If lineNumber <= _StepValue?.EndToken.Line Then Return Me

            For Each statment In Body
                Dim st = statment.GetStatementAt(lineNumber)
                If st IsNot Nothing Then Return st
            Next

            If lineNumber <= EndLoopToken.Line Then Return Me

            Return Nothing
        End Function

        Public Overrides Function GetKeywords() As LegalTokens
            Dim spans As New LegalTokens
            spans.Add(ForToken)
            spans.Add(ToToken)
            spans.Add(StepToken)
            For Each statement In JumpLoopStatements
                spans.Add(statement.StartToken)
            Next
            spans.Add(EndLoopToken)
            Return spans
        End Function

        Public Overrides Sub AddSymbols(symbolTable As SymbolTable)
            MyBase.AddSymbols(symbolTable)
            Iterator.Parent = Me
            ForToken.Parent = Me
            ToToken.Parent = Me
            StepToken.Parent = Me
            EndLoopToken.Parent = Me
            EqualsToken.Parent = Me

            If Iterator.Type <> TokenType.Illegal Then
                Dim id As New IdentifierExpression() With {
                    .Identifier = Iterator,
                    .Subroutine = Subroutine
                }
                Iterator.SymbolType = If(Subroutine Is Nothing, CompletionItemType.GlobalVariable, CompletionItemType.LocalVariable)
                symbolTable.AddIdentifier(Iterator)
                symbolTable.AddVariable(id, "The for loop counter (iterator)", Subroutine IsNot Nothing)
                If symbolTable.IsGlobalVar(id) Then
                    id.AddSymbolInitialization(symbolTable)
                End If
            End If

            If _InitialValue IsNot Nothing Then
                _InitialValue.Parent = Me
                _InitialValue.AddSymbols(symbolTable)
            End If

            If _FinalValue IsNot Nothing Then
                _FinalValue.Parent = Me
                _FinalValue.AddSymbols(symbolTable)
            End If

            If _StepValue IsNot Nothing Then
                _StepValue.Parent = Me
                _StepValue.AddSymbols(symbolTable)
            End If

            For Each item In Body
                item.Parent = Me
                item.AddSymbols(symbolTable)
            Next
        End Sub

        Public Overrides Sub PrepareForEmit(scope As CodeGenScope)
            For Each item In Body
                item.PrepareForEmit(scope)
            Next
        End Sub

        Private Shared loopNo As Integer

        Public Overrides Sub EmitIL(scope As CodeGenScope)

            Dim IsField = Subroutine Is Nothing
            Dim iteratorVar As LocalBuilder
            Dim iteratorField As System.Reflection.FieldInfo

            Dim conditionLabel = scope.ILGenerator.DefineLabel()
            Dim lblElseIf = scope.ILGenerator.DefineLabel()
            Dim loopBody = scope.ILGenerator.DefineLabel()
            ContinueLabel = scope.ILGenerator.DefineLabel()
            ExitLabel = scope.ILGenerator.DefineLabel()

            loopNo += 1
            _FinalValue.EmitIL(scope)

            Dim token As New Token With {
                .Text = $"__toValue__{loopNo}",
                .Type = TokenType.Identifier,
                .Parent = Me
            }

            Dim toValue = scope.CreateLocalBuilder(Subroutine, token)
            scope.ILGenerator.Emit(OpCodes.Stloc, toValue)

            If _StepValue IsNot Nothing Then
                _StepValue.EmitIL(scope)
            Else
                scope.ILGenerator.Emit(OpCodes.Ldc_R8, 1.0)
                scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.NumberToPrimitive, Nothing)
            End If

            token.Text = $"__stepValue__{loopNo}"
            Dim stepValue = scope.CreateLocalBuilder(Subroutine, token)
            scope.ILGenerator.Emit(OpCodes.Stloc, stepValue)

            _InitialValue.EmitIL(scope)

            If IsField Then
                iteratorField = scope.Fields(Iterator.NormalizedText)
                scope.ILGenerator.Emit(OpCodes.Stsfld, iteratorField)
                scope.ILGenerator.MarkLabel(ConditionLabel)
                scope.ILGenerator.Emit(OpCodes.Ldsfld, iteratorField)
            Else
                iteratorVar = scope.CreateLocalBuilder(Subroutine, Iterator)
                scope.ILGenerator.Emit(OpCodes.Stloc, iteratorVar)
                scope.ILGenerator.MarkLabel(ConditionLabel)
                scope.ILGenerator.Emit(OpCodes.Ldloc, iteratorVar)
            End If

            scope.ILGenerator.Emit(OpCodes.Ldloc, toValue)

            If _StepValue IsNot Nothing Then
                scope.ILGenerator.Emit(OpCodes.Ldloc, stepValue)
                scope.ILGenerator.Emit(OpCodes.Ldc_R8, 0.0)
                scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.NumberToPrimitive, Nothing)
                scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.LessThan, Nothing)
                scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.PrimitiveToBoolean, Nothing)
                scope.ILGenerator.Emit(OpCodes.Brfalse, lblElseIf)
                scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.GreaterThanOrEqualTo, Nothing)
                scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.PrimitiveToBoolean, Nothing)
                scope.ILGenerator.Emit(OpCodes.Brfalse, ExitLabel)
                scope.ILGenerator.Emit(OpCodes.Br, loopBody)
            End If

            scope.ILGenerator.MarkLabel(lblElseIf)
            scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.LessThanOrEqualTo, Nothing)
            scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.PrimitiveToBoolean, Nothing)
            scope.ILGenerator.Emit(OpCodes.Brfalse, ExitLabel)
            scope.ILGenerator.MarkLabel(loopBody)

            For Each item In Body
                item.EmitIL(scope)
            Next

            ' Increase Iterator
            scope.ILGenerator.MarkLabel(ContinueLabel)
            If IsField Then
                scope.ILGenerator.Emit(OpCodes.Ldsfld, iteratorField)
            Else
                scope.ILGenerator.Emit(OpCodes.Ldloc, iteratorVar)
            End If

            scope.ILGenerator.Emit(OpCodes.Ldloc, stepValue)
            scope.ILGenerator.EmitCall(OpCodes.Call, scope.TypeInfoBag.Add, Nothing)

            If IsField Then
                scope.ILGenerator.Emit(OpCodes.Stsfld, iteratorField)
            Else
                scope.ILGenerator.Emit(OpCodes.Stloc, iteratorVar)
            End If

            scope.ILGenerator.Emit(OpCodes.Br, ConditionLabel)
            scope.ILGenerator.MarkLabel(ExitLabel)
        End Sub

        Public Overrides Sub PopulateCompletionItems(
                         completionBag As CompletionBag,
                         line As Integer,
                         column As Integer,
                         globalScope As Boolean
                   )

            If ForToken.Line = line AndAlso column <= ForToken.EndColumn Then
                CompletionHelper.FillAllGlobalItems(completionBag, globalScope)

            ElseIf AfterIterator(line, column) Then
                CompletionHelper.FillLocals(completionBag, Subroutine?.Name.NormalizedText)
                If Subroutine Is Nothing Then
                    If completionBag.ForHelp AndAlso Not Iterator.IsIllegal Then
                        completionBag.CompletionItems.Add(New CompletionItem() With {
                            .Key = Iterator.NormalizedText,
                            .DisplayName = Iterator.Text,
                            .ItemType = CompletionItemType.GlobalVariable,
                            .DefinitionIdintifier = completionBag.SymbolTable.GlobalVariables(Iterator.NormalizedText)
                         })
                    Else
                        CompletionHelper.FillVariables(completionBag)
                    End If
                End If

            ElseIf InForHeader(line) Then
                CompletionHelper.FillKeywords(completionBag, {TokenType.To, TokenType.Step})
                CompletionHelper.FillExpressionItems(completionBag)
                CompletionHelper.FillSubroutines(completionBag, True)

            ElseIf Not EndLoopToken.IsIllegal AndAlso line = EndLoopToken.Line Then
                completionBag.CompletionItems.Clear()
                CompletionHelper.FillKeywords(completionBag, {EndLoopToken.Type})

            Else
                Dim statement = GetStatementContaining(Body, line)
                CompletionHelper.FillKeywords(completionBag, {TokenType.EndFor, TokenType.Next})
                statement?.PopulateCompletionItems(completionBag, line, column, globalScope:=False)
            End If
        End Sub

        Private Function AfterIterator(line As Integer, column As Integer) As Boolean
            If EqualsToken.IsIllegal Then
                If Iterator.IsIllegal Then Return line = ForToken.Line
                Return line = Iterator.Line
            End If

            If line < EqualsToken.Line Then Return True
            If line > EqualsToken.Line Then Return False
            Return column < EqualsToken.Column
        End Function

        Private Function InForHeader(line As Integer) As Boolean
            Dim line2 As Integer

            ' Note that for loop parts can be split over multi lines using the _ symbol,
            ' and some of these parts can be missing while the user is typing code

            If _StepValue IsNot Nothing Then
                line2 = _StepValue.EndToken.Line

            ElseIf Not StepToken.IsIllegal Then
                line2 = StepToken.Line

            ElseIf _FinalValue IsNot Nothing Then
                line2 = _FinalValue.EndToken.Line

            ElseIf Not ToToken.IsIllegal Then
                line2 = ToToken.Line

            ElseIf _InitialValue IsNot Nothing Then
                line2 = _InitialValue.EndToken.Line

            Else
                line2 = EqualsToken.Line
            End If

            Return line <= line2
        End Function

        Public Overrides Function ToString() As String
            Dim stringBuilder As StringBuilder = New StringBuilder()
            stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, "{0} {1} = {2} To {3}", ForToken.Text, Iterator.Text, _InitialValue, _FinalValue)

            If StepToken.ParseType <> 0 Then
                stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, "{0} {1}", New Object(1) {StepToken.Text, _StepValue})
            End If

            stringBuilder.AppendLine()

            For Each item In Body
                stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, "  {0}", New Object(0) {item})
            Next

            stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, "{0}" & VisualBasic.Constants.vbCrLf, New Object(0) {EndLoopToken.Text})
            Return stringBuilder.ToString()
        End Function
    End Class
End Namespace
