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


## doc

comment -> (docTag*)? (Description*)? 





