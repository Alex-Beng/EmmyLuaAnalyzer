# c sharp learning note

## 1. ? and ?? operator

```csharp
public IEnumerable<LuaCommentSyntax> Comments =>
        Tree.BinderData?.GetComments(this) ?? [];
```

# emmylua 语法规则

代码和doc是独立的

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


callArgList -> 


## doc

只有三个语法规则

comment -> (docTag*)? (Description*)? 


docTag -> Description?


Description -> SyntaxToken*   // TkDocDetail | TkDocContinue | TkEndOfLine
@开头 | --- | EOF





