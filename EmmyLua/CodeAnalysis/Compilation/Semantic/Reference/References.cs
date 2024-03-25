﻿using EmmyLua.CodeAnalysis.Compilation.Declaration;
using EmmyLua.CodeAnalysis.Compilation.Infer;
using EmmyLua.CodeAnalysis.Document;
using EmmyLua.CodeAnalysis.Syntax.Node;
using EmmyLua.CodeAnalysis.Syntax.Node.SyntaxNodes;

namespace EmmyLua.CodeAnalysis.Compilation.Semantic.Reference;

public class References(SearchContext context)
{
    public IEnumerable<LuaReference> FindReferences(LuaSyntaxElement element)
    {
        var declarationTree = context.Compilation.GetDeclarationTree(element.Tree.Document.Id);
        if (declarationTree is null)
        {
            return Enumerable.Empty<LuaReference>();
        }

        var declaration = declarationTree.FindDeclaration(element, context);
        return declaration switch
        {
            LocalLuaDeclaration localDeclaration => LocalReferences(localDeclaration, declarationTree),
            GlobalLuaDeclaration globalDeclaration => GlobalReferences(globalDeclaration),
            MethodLuaDeclaration methodDeclaration => MethodReferences(methodDeclaration),
            DocFieldLuaDeclaration fieldDeclaration => DocFieldReferences(fieldDeclaration),
            TableFieldLuaDeclaration tableFieldDeclaration => TableFieldReferences(tableFieldDeclaration),
            NamedTypeLuaDeclaration namedTypeDeclaration => NamedTypeReferences(namedTypeDeclaration),
            _ => Enumerable.Empty<LuaReference>()
        };
    }

    private IEnumerable<LuaReference> LocalReferences(LuaDeclaration declaration, LuaDeclarationTree declarationTree)
    {
        var references = new List<LuaReference>();
        var parentBlock = declaration.SyntaxElement?.Ancestors.OfType<LuaBlockSyntax>().FirstOrDefault();
        if (parentBlock is not null)
        {
            references.Add(new LuaReference(declaration.SyntaxElement!.Location, declaration.SyntaxElement));
            foreach (var node in parentBlock.Descendants
                         .Where(it => it.Position > declaration.Position))
            {
                if (node is LuaNameExprSyntax nameExpr && nameExpr.Name?.RepresentText == declaration.Name)
                {
                    if (declarationTree.FindDeclaration(nameExpr, context) == declaration)
                    {
                        references.Add(new LuaReference(nameExpr.Location, nameExpr));
                    }
                }
            }
        }

        return references;
    }

    private IEnumerable<LuaReference> GlobalReferences(LuaDeclaration declaration)
    {
        var references = new List<LuaReference>();
        var globalName = declaration.Name;
        var nameExprs = context.Compilation.ProjectIndex.GetNameExprs(globalName);

        foreach (var nameExpr in nameExprs)
        {
            var declarationTree = context.Compilation.GetDeclarationTree(nameExpr.Tree.Document.Id);
            if (declarationTree?.FindDeclaration(nameExpr, context) == declaration)
            {
                references.Add(new LuaReference(nameExpr.Location, nameExpr));
            }
        }

        return references;
    }

    private IEnumerable<LuaReference> FieldReferences(LuaDeclaration declaration, string fieldName)
    {
        var references = new List<LuaReference>();
        var indexExprs = context.Compilation.ProjectIndex.GetIndexExprs(fieldName);
        foreach (var indexExpr in indexExprs)
        {
            var declarationTree = context.Compilation.GetDeclarationTree(indexExpr.Tree.Document.Id);
            if (declarationTree?.FindDeclaration(indexExpr, context) == declaration)
            {
                references.Add(new LuaReference(indexExpr.KeyElement.Location, indexExpr.KeyElement));
            }
        }

        return references;
    }

    private IEnumerable<LuaReference> MethodReferences(MethodLuaDeclaration declaration)
    {
        switch (declaration.Feature)
        {
            case DeclarationFeature.Local:
            {
                if (declaration.SyntaxElement is { Tree.Document.Id: { } id })
                {
                    var declarationTree = context.Compilation.GetDeclarationTree(id);
                    if (declarationTree is not null)
                    {
                        return LocalReferences(declaration, declarationTree);
                    }
                }

                break;
            }
            case DeclarationFeature.Global:
            {
                return GlobalReferences(declaration);
            }
            default:
            {
                if (declaration.IndexExpr is { Name: { } name })
                {
                    return FieldReferences(declaration, name);
                }

                break;
            }
        }

        return Enumerable.Empty<LuaReference>();
    }

    private IEnumerable<LuaReference> DocFieldReferences(DocFieldLuaDeclaration fieldDeclaration)
    {
        var references = new List<LuaReference>();
        if (fieldDeclaration is { Name: { } name, FieldDef: { } fieldDef })
        {
            if (fieldDef.FieldElement is { } fieldElement)
            {
                references.Add(new LuaReference(fieldElement.Location, fieldElement));
            }

            var parentType = context.Compilation.ProjectIndex.GetParentType(fieldDef);
            if (parentType is not null)
            {
                var members = context.FindMember(parentType, name);
                foreach (var member in members)
                {
                    if (member is TableFieldLuaDeclaration { Name: { } name2, TableField.KeyElement: { } keyElement })
                    {
                        references.Add(new LuaReference(keyElement.Location, keyElement));
                        break;
                    }
                }
            }

            references.AddRange(FieldReferences(fieldDeclaration, name));
        }

        return references;
    }

    private IEnumerable<LuaReference> TableFieldReferences(TableFieldLuaDeclaration declaration)
    {
        var references = new List<LuaReference>();
        if (declaration is { Name: { } name, TableField: { } fieldDef })
        {
            if (fieldDef.KeyElement is { } keyElement)
            {
                references.Add(new LuaReference(keyElement.Location, keyElement));
            }

            var parentType = context.Compilation.ProjectIndex.GetParentType(fieldDef);
            if (parentType is not null)
            {
                var members = context.FindMember(parentType, name);
                foreach (var member in members)
                {
                    if (member is DocFieldLuaDeclaration { Name: { } name2, FieldDef.FieldElement: { } fieldElement })
                    {
                        references.Add(new LuaReference(fieldElement.Location, fieldElement));
                    }
                }
            }

            references.AddRange(FieldReferences(declaration, name));
        }

        return references;
    }

    private IEnumerable<LuaReference> NamedTypeReferences(NamedTypeLuaDeclaration declaration)
    {
        var references = new List<LuaReference>();
        if (declaration is { Name: { } name, NameToken: { } nameToken })
        {
            references.Add(new LuaReference(nameToken.Location, nameToken));
            var nameTypes = context.Compilation.ProjectIndex.GetNameTypes(name);
            foreach (var nameType in nameTypes)
            {
                var declarationTree = context.Compilation.GetDeclarationTree(nameType.Tree.Document.Id);
                if (declarationTree?.FindDeclaration(nameType, context) == declaration && nameType.Name is { } name2)
                {
                    references.Add(new LuaReference(name2.Location, name2));
                }
            }
        }

        return references;
    }
}
