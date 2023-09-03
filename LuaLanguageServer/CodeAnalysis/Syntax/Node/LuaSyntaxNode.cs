﻿using LuaLanguageServer.CodeAnalysis.Kind;
using LuaLanguageServer.CodeAnalysis.Syntax.Green;

namespace LuaLanguageServer.CodeAnalysis.Syntax.Node;

// 实现一个语法节点, 类似于roslyn的语法节点
public abstract class LuaSyntaxNode
{
    public LuaSyntaxKind Kind { get; }

    public GreenNode GreenNode { get; }

    public LuaSyntaxNode? Parent { get; }

    public LuaSyntaxNode(GreenNode greenNode)
    {
        Kind = greenNode.IsSyntaxNode ? greenNode.SyntaxKind : LuaSyntaxKind.None;
        GreenNode = greenNode;
    }
}
