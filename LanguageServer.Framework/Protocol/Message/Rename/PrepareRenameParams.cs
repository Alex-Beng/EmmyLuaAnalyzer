﻿using System.Text.Json.Serialization;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Interface;
using EmmyLua.LanguageServer.Framework.Protocol.Model.TextDocument;

namespace EmmyLua.LanguageServer.Framework.Protocol.Message.Rename;

public class PrepareRenameParams : TextDocumentPositionParams, IWorkDoneProgressParams
{
    [JsonPropertyName("workDoneToken")]
    public string? WorkDoneToken { get; set; }
}
