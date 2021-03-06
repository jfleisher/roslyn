﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic.GenerateMember.GenerateMethod
    Friend Class VisualBasicCommonGenerationServiceMethods
        Public Shared Function AreSpecialOptionsActive(semanticModel As SemanticModel) As Boolean
            ' Check to see if option strict is on.  if that is the case we will want to still 
            ' generate methods even if they overshadow existing ones so the user has a partial fix.

            Dim root = semanticModel.SyntaxTree.GetRoot
            Dim optionStatement = CType(root.ChildNodes().FirstOrDefault(Function(n) n.IsKind(SyntaxKind.OptionStatement)), OptionStatementSyntax)
            If optionStatement?.ValueKeyword.IsKind(SyntaxKind.OnKeyword) Then
                Return True
            End If

            Dim options = TryCast(semanticModel.Compilation.Options, VisualBasicCompilationOptions)
            If options IsNot Nothing Then
                Return options.OptionStrict = OptionStrict.On
            End If

            Return False
        End Function

        Public Shared Function IsValidSymbol(symbol As ISymbol, semanticModel As SemanticModel) As Boolean
            ' We want to still generate a method even if this is a Namespace symbol because unknown method calls without
            ' parenthesis are bound as namespaces by the VB compiler.
            Return symbol.Kind = SymbolKind.Namespace Or AreSpecialOptionsActive(semanticModel)
        End Function
    End Class
End Namespace


