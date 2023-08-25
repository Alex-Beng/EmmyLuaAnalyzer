﻿namespace LuaLanguageServer.LuaCore.Kind;

public enum LuaTokenKind
{
    // KeyWord
    TkAnd,
    TkBreak,
    TkDo,
    TkElse,
    TkElseIf,
    TkEnd,
    TkFalse,
    TkFor,
    TkFunction,
    TkGoto,
    TkIf,
    TkIn,
    TkLocal,
    TkNil,
    TkNot,
    TkOr,
    TkRepeat,
    TkReturn,
    TkThen,
    TkTrue,
    TkUntil,
    TkWhile,


    TkWhitespace, // whitespace
    TkEndOfLine, // end of line
    TkPlus, // +
    TkMinus, // -
    TkMul, // *
    TkDiv, // /
    TkIDiv, // //
    TkDot, // .
    TkConcat, // ..
    TkDots, // ...
    TkComma, // ,
    TkAssign, // =
    TkEq, // ==
    TkGe, // >=
    TkLe, // <=
    TkNe, // ~=
    TkShl, // <<
    TkShr, // >>
    TkLt, // <
    TkGt, // >
    TkMod, // %
    TkPow, // ^
    TkLen, // #
    TkBitAnd, // &
    TkBitOr, // |
    TkBitXor, // ~
    TkColon, // :
    TkDbColon, // ::
    TkLeftBracket, // [
    TkRightBracket, // ]
    TkLeftParen, // (
    TkRightParen, // )
    TkLeftBrace, // {
    TkRightBrace, // }
    TkFlt, // float
    TkNumber, // number
    TkInt, // int
    TkName, // name
    TkString, // string
    TkLongString, // long string
    TkShortComment, // short comment
    TkLongComment, // long comment
    TkShebang, // shebang
    TkEof, // eof

    // error
    TkUnCompleteLongStringStart, // [==
    TkUnFinishedLongString, // ]]
    TkUnFinishedString, // string
}
