# c sharp learning note

## 1. ? and ?? operator

```csharp
public IEnumerable<LuaCommentSyntax> Comments =>
        Tree.BinderData?.GetComments(this) ?? [];
```

## 模式匹配

```csharp
// 先定义一个类
class Address
{
    public string City { get; set; }
    public string Street { get; set; }
}

// 搭配 is 关键字使用
if (add is Address {City: "LA"}) {

}
// 赋值给ct
else if (add is Address {City: { } ct }) {

} 

```

# emmylua 语法规则

代码和doc是~~独立的~~（并不是）


直接看代码吧，有些语法都是直接到token了，不是很好理解为语法规则

emmylua -> block


block -> stat* comment*

## stat

stat -> comment* // 存疑？


stat -> localStat | assignStat | funcStat 
     | labelStat | gotoStat | breakStat 
     | returnStat | ifStat | ifclauseStat 
     | whileStat | doStat | forStat 
     | forRangeStat | repeatStat | callStat
     | emptyStat | unknwonStat


localStat -> localName* expr*


localName -> attribut nameToken


expr -> nameExpr | callExpr | binaryExpr
     | unaryExpr | tableExpr | closureExpr
     | literalExpr | parenExpr | indexExpr


nameExpr -> nameToken


callExpr -> expr callArgList # prefixExpr argList


callArgList -> leftParen(Tk) SingleArg(Tk) ArgList(expr) rightParen(Tk)


binaryExpr -> leftExpr(expr) binaryOp(Tk) rightExpr(expr)


unaryExpr -> unaryOp(Tk) expr(expr)


tableExpr -> tableField*






## doc

只有三个语法规则

comment -> (docTag*)? (Description*)? 


docTag -> Description?


Description -> SyntaxToken*   // TkDocDetail | TkDocContinue | TkEndOfLine
@开头 | --- | EOF





