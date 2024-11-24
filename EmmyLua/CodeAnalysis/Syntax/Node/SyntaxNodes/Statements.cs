using EmmyLua.CodeAnalysis.Syntax.Kind;
using EmmyLua.CodeAnalysis.Syntax.Tree;

namespace EmmyLua.CodeAnalysis.Syntax.Node.SyntaxNodes;

public class LuaStatSyntax(int index, LuaSyntaxTree tree) : LuaSyntaxNode(index, tree)
{
    public IEnumerable<LuaCommentSyntax> Comments =>
        Tree.BinderData?.GetComments(this) ?? [];
}

// localStat -> local? nameList assign? exprList
public class LuaLocalStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken? Local => FirstChildToken(LuaTokenKind.TkLocal);

    public bool IsLocalDeclare => Assign != null;

    public IEnumerable<LuaLocalNameSyntax> NameList => ChildrenElement<LuaLocalNameSyntax>();

    public LuaSyntaxToken? Assign => FirstChildToken(LuaTokenKind.TkAssign);

    public IEnumerable<LuaExprSyntax> ExprList => ChildrenElement<LuaExprSyntax>();
}

// assignStat -> varList assign? exprList
public class LuaAssignStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public IEnumerable<LuaExprSyntax> VarList => ChildNodesBeforeToken<LuaExprSyntax>(LuaTokenKind.TkAssign);

    public IEnumerable<LuaExprSyntax> ExprList => ChildNodesAfterToken<LuaExprSyntax>(LuaTokenKind.TkAssign);

    public LuaSyntaxToken? Assign => FirstChildToken(LuaTokenKind.TkAssign);
}

// funcStat -> 
public class LuaFuncStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    /*
    normal func: function foo() end
    local func: local function foo() end
    method func: object.method = function() end
    colon func: function:method() end
    closure func: function() end
    */
    public bool IsLocal => FirstChildToken(LuaTokenKind.TkLocal) != null;

    public bool IsMethod => FirstChild<LuaIndexExprSyntax>() != null;

    public bool IsColonFunc => IndexExpr?.IsColonIndex == true;

    // local funcName
    public LuaLocalNameSyntax? LocalName => FirstChild<LuaLocalNameSyntax>();

    // funcName 
    public LuaNameExprSyntax? NameExpr => FirstChild<LuaNameExprSyntax>();

    // object:funcName
    public LuaIndexExprSyntax? IndexExpr => FirstChild<LuaIndexExprSyntax>();

    public LuaClosureExprSyntax? ClosureExpr => FirstChild<LuaClosureExprSyntax>();

    // 获得名字
    public LuaSyntaxElement? NameElement
    {
        get
        {
            foreach (var element in ChildrenElements)
            {
                if (element is LuaLocalNameSyntax { Name: { } name1 })
                {
                    return name1;
                }
                else if (element is LuaNameExprSyntax { Name: { } name2 })
                {
                    return name2;
                }
                else if (element is LuaIndexExprSyntax { KeyElement: { } keyElement })
                {
                    return keyElement;
                }
            }

            return null;
        }
    }
}

// ::<name>::
public class LuaLabelStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaNameToken? Name => FirstChild<LuaNameToken>();
}

// gotoStat -> goto name
public class LuaGotoStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken Goto => FirstChildToken(LuaTokenKind.TkGoto)!;

    public LuaNameToken? LabelName => FirstChild<LuaNameToken>();
}

// break
public class LuaBreakStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken Break => FirstChildToken(LuaTokenKind.TkBreak)!;
}

// returnStat -> return expr*
public class LuaReturnStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken Return => FirstChildToken(LuaTokenKind.TkReturn)!;

    public IEnumerable<LuaExprSyntax> ExprList => ChildrenElement<LuaExprSyntax>();
}

// ifStat -> if expr then block ifCaluseStat* end
public class LuaIfStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken If => FirstChildToken(LuaTokenKind.TkIf)!;

    public LuaExprSyntax? Condition => FirstChild<LuaExprSyntax>();

    public LuaSyntaxToken? Then => FirstChildToken(LuaTokenKind.TkThen);

    public LuaBlockSyntax? ThenBlock => FirstChild<LuaBlockSyntax>();

    public IEnumerable<LuaIfClauseStatSyntax> IfClauseStatementList => ChildrenElement<LuaIfClauseStatSyntax>();

    public LuaSyntaxToken End => FirstChildToken(LuaTokenKind.TkEnd)!;
}

// ifCaluseStat -> elseif? else? codtion? then block 
public class LuaIfClauseStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken? ElseIf => FirstChildToken(LuaTokenKind.TkElseIf);

    public LuaSyntaxToken? Else => FirstChildToken(LuaTokenKind.TkElse);

    public bool IsElseIf => ElseIf != null;

    public bool IsElse => Else != null;

    public LuaExprSyntax? Condition => FirstChild<LuaExprSyntax>();

    public LuaSyntaxToken? Then => FirstChildToken(LuaTokenKind.TkThen);

    public LuaBlockSyntax? Block => FirstChild<LuaBlockSyntax>();
}

// whileStat -> while expr do block end
public class LuaWhileStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken While => FirstChildToken(LuaTokenKind.TkWhile)!;

    public LuaExprSyntax? Condition => FirstChild<LuaExprSyntax>();

    public LuaSyntaxToken? Do => FirstChildToken(LuaTokenKind.TkDo);

    public LuaBlockSyntax? Block => FirstChild<LuaBlockSyntax>();

    public LuaSyntaxToken? End => FirstChildToken(LuaTokenKind.TkEnd);
}

// do block end 
public class LuaDoStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken Do => FirstChildToken(LuaTokenKind.TkDo)!;

    public LuaBlockSyntax? Block => FirstChild<LuaBlockSyntax>();

    public LuaSyntaxToken? End => FirstChildToken(LuaTokenKind.TkEnd);
}

// forStat -> for paramDef = expr, expr, expr do block end
public class LuaForStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaParamDefSyntax? IteratorName => FirstChild<LuaParamDefSyntax>();

    public LuaExprSyntax? InitExpr => FirstChild<LuaExprSyntax>();

    public LuaExprSyntax? LimitExpr => ChildrenElement<LuaExprSyntax>().Skip(1).FirstOrDefault();

    public LuaExprSyntax? Step => ChildrenElement<LuaExprSyntax>().Skip(2).FirstOrDefault();

    public LuaBlockSyntax? Block => FirstChild<LuaBlockSyntax>();
}

// forRangeStat -> for paramDef in expr do block end
public class LuaForRangeStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public IEnumerable<LuaParamDefSyntax> IteratorNames => ChildrenElement<LuaParamDefSyntax>();

    public IEnumerable<LuaExprSyntax> ExprList => ChildrenElement<LuaExprSyntax>();

    public LuaBlockSyntax? Block => FirstChild<LuaBlockSyntax>();
}

// repeatStat -> repeat block until expr
public class LuaRepeatStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaSyntaxToken Repeat => FirstChildToken(LuaTokenKind.TkRepeat)!;

    public LuaBlockSyntax? Block => FirstChild<LuaBlockSyntax>();

    public LuaSyntaxToken? Until => FirstChildToken(LuaTokenKind.TkUntil);

    public LuaExprSyntax? Condition => FirstChild<LuaExprSyntax>();
}

// callStat -> expr
public class LuaCallStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree)
{
    public LuaExprSyntax? Expr => FirstChild<LuaExprSyntax>();
}

public class LuaEmptyStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree);

public class LuaUnknownStatSyntax(int index, LuaSyntaxTree tree) : LuaStatSyntax(index, tree);
