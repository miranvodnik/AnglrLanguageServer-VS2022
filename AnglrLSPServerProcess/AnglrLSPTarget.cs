
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using StreamJsonRpc.Protocol;
using StreamJsonRpc.Reflection;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using Anglr.Compiler;
using AnglrJsonRpcMethods;

namespace AnglrLSPServerProcess
{
	using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;
    //
    //	deserialization of textDocument/references LSP request throws exception
    //	because ReferenceParams class, associated with this request, does not
    //	follow LSP specifications. Following classes (ReferencesContext, 
    //	ReferencesDocument and ReferencesParamsClass) solve this problem
    //

    //
    //	part of ReferencesParamsClass class
    //
    internal class ReferencesContext
	{
		public bool includeDeclaration;
	}

	//
	//	part of ReferencesParamsClass class
	//
	internal class ReferencesDocument
	{
		public Uri uri;
	}

	//
	//	this class is substutution for Microsoft.VisualStudio.LanguageServer.Protocol
	//	ReferenceParams class which throws exception when deserializing it with Newtonsoft.Json
	//
	internal class ReferencesParamsClass
	{
		public ReferenceContext context;
		public string partialResultToken;
		public ReferencesDocument textDocument;
		public Position position;
	}

	internal class AnglrLSPTarget
	{
		public ClientCapabilities clientCapabilities { get; private set; } = null;
		public AnglrLSP AnglrLspDriver { get; private set; } = null;
		public AnglrDocDictionary anglrDocDictionary { get; private set; } = new AnglrDocDictionary ();
		public event EventHandler IsInitialized;
		private bool debug = true;

        public AnglrLSPTarget (AnglrLSP lsp)
		{
			this.AnglrLspDriver = lsp;
			anglrCompiler.createParseTree = true;
			anglrCompiler.debug = false;
			anglrCompiler.loopDetection = true;
		}

        public void Log (MessageType messageType, string message, bool force = false) => AnglrLspDriver.Log (messageType, message);

        public void LogError (string message)
        {
            AnglrLspDriver.Log (MessageType.Error, message);
		}

		public void Notify (string methodName, object argument)
		{
			AnglrLspDriver.Notify (methodName, argument);
		}

		[JsonRpcMethod (Methods.InitializeName)]
		public object Initialize (JToken arg)
		{
			if (detailedInfo || detailedInitializeParams)
				Log (MessageType.Log, $"Initialize, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "Initialize");

			InitializeParams parameter = arg.ToObject<InitializeParams> ();
			this.clientCapabilities = parameter.Capabilities;

			// ANGLR Language Server Protocol Implementation Capabilities
			ServerCapabilities capabilities = new ServerCapabilities ()
			{
				ExecuteCommandProvider = null,
				//new ExecuteCommandOptions ()
				//{
				//	Commands = new string [] { "cmd-1", "cmd-2" }
				//},
				FoldingRangeProvider = new SumType<bool, FoldingRangeOptions>(false),
				RenameProvider = true,
				//DocumentOnTypeFormattingProvider = null,
				//new DocumentOnTypeFormattingOptions ()
				//{
				//	FirstTriggerCharacter = "%",
				//	MoreTriggerCharacter = new string [] { ":", "|" }
				//},
				DocumentRangeFormattingProvider = false,
				DocumentFormattingProvider = true,
				DocumentLinkProvider = null,
				//new DocumentLinkOptions ()
				//{
				//	ResolveProvider = true
				//},
				CodeLensProvider = null,
				//new CodeLensOptions ()
				//{
				//	ResolveProvider = true
				//},
				//CodeActionProvider = false,
				DocumentSymbolProvider = true,
				DocumentHighlightProvider = true,
				ReferencesProvider = true,
				ImplementationProvider = false,
				TypeDefinitionProvider = false,
				DefinitionProvider = true,
				SignatureHelpProvider = null,
				//new SignatureHelpOptions ()
				//{
				//	TriggerCharacters = new string [] { "a", "b" }
				//},
				HoverProvider = true,
				CompletionProvider =
				new CompletionOptions ()
				{
					ResolveProvider = true,
					TriggerCharacters = new string [] { ",", "." }
				},
				TextDocumentSync = new TextDocumentSyncOptions ()
				{
					Change = TextDocumentSyncKind.Incremental,
					OpenClose = true,
					Save = true,
					//new SaveOptions
					//{
					//	IncludeText = true
					//},
					WillSave = true,
					WillSaveWaitUntil = true
				},
				WorkspaceSymbolProvider = false,
				SemanticTokensOptions = new SemanticTokensOptions()
				{
					Full = true,
					Range = false,
					Legend = new SemanticTokensLegend()
					{
						TokenTypes = new string[] {
								"class",
								"variable",
								"enum",
								"comment",
								"string",
								"keyword",
							},
						TokenModifiers = new string[] {
								"declaration",
								"documentation",
							}
					}
				},
				Experimental = null
			};

			InitializeResult result = new InitializeResult ();
			result.Capabilities = capabilities;

			IsInitialized?.Invoke (this, new EventArgs ());

			return result;
		}

		[JsonRpcMethod (Methods.InitializedName)]
		public void Initialized (JToken arg)
		{
			if (detailedInfo || detailedInitializedParams)
			{
				if (arg != null)
					Log (MessageType.Log, $"Initialized, jtoken = {arg.ToString ()}");
				Log (MessageType.Log, "Initialized");
			}
			else
				Log (MessageType.Log, "Initialized");
			InitializedParams parameter = arg.ToObject<InitializedParams> ();
		}

		[JsonRpcMethod (Methods.TextDocumentReferencesName)]
		public Location[] TextDocumentReferences (JToken arg)
		{
			if (detailedInfo || detailedReferenceParams)
				Log (MessageType.Log, $"TextDocumentReferences, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentReferences");
			try
			{
				ReferenceParams param = null;
				param = arg.ToObject<ReferenceParams> ();
				AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
				if (docCtx == null)
				{
					Log (MessageType.Warning, $"TextDocumentReferences doc '{param.TextDocument.Uri.AbsolutePath}' not found");
					return null;
				}
				return docCtx.TextDocumentReferences (this, param);
			}
			catch (Exception e)
			{
				try
				{
					ReferencesParamsClass referencesParamsClass = arg.ToObject<ReferencesParamsClass> ();
					AnglrDocContext docCtx = anglrDocDictionary.Find (referencesParamsClass.textDocument.uri.AbsolutePath);
					if (docCtx == null)
					{
						Log (MessageType.Warning, $"TextDocumentReferences doc '{referencesParamsClass.textDocument.uri.AbsolutePath}' not found");
						return null;
					}
					return docCtx.TextDocumentReferences (this, referencesParamsClass);
				}
				catch (Exception ee)
				{
					Log (MessageType.Error, $"TextDocumentReferences, json '{arg.ToString ()}' cannot be deserialized");
					return null;
				}
			}
		}

		[JsonRpcMethod (Methods.ShutdownName)]
		public object Shutdown (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"Shutdown, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "Shutdown");
			object obj = arg.ToObject<object> ();
			return null;
		}

		[JsonRpcMethod (Methods.ExitName)]
		public void Exit (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"Exit, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "Exit");
			object obj = arg.ToObject<object> ();
		}
		//
		// Summary:
		//     Strongly typed message object for "textDocument/rename".
		[JsonRpcMethod (Methods.TextDocumentRenameName)]
		public WorkspaceEdit TextDocumentRename (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, "TextDocumentRename, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentRename");
			RenameParams param = arg.ToObject<RenameParams> ();
			AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
				return null;
			return docCtx.TextDocumentRename (this, param);
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/signatureHelp'.
		[JsonRpcMethod (Methods.TextDocumentSignatureHelpName)]
		public SignatureHelp TextDocumentSignatureHelp (JToken arg)
		{
			if (detailedInfo || detailedSignatureHelp)
				Log (MessageType.Log, $"TextDocumentSignatureHelp, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentSignatureHelp");
			TextDocumentPositionParams param = arg.ToObject<TextDocumentPositionParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/willSave'.
		[JsonRpcMethod (Methods.TextDocumentWillSaveName)]
		public void TextDocumentWillSave (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentWillSave, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentWillSave");
			WillSaveTextDocumentParams param = arg.ToObject<WillSaveTextDocumentParams> ();
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/willSaveWaitUntil'.
		[JsonRpcMethod (Methods.TextDocumentWillSaveWaitUntilName)]
		public TextEdit[] TextDocumentWillSaveWaitUntil (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentWillSaveWaitUntil, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentWillSaveWaitUntil");
			WillSaveTextDocumentParams param = arg.ToObject<WillSaveTextDocumentParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'window/logMessage'.
		[JsonRpcMethod (Methods.WindowLogMessageName)]
		public void WindowLogMessage (JToken arg)
		{
			//if (detailedInfo)
			//	Log ("WindowLogMessage, jtoken = " + arg.ToString ());
			//else
			//	Log ("WindowLogMessage");
			LogMessageParams param = arg.ToObject<LogMessageParams> ();
		}
		//
		// Summary:
		//     Strongly typed message object for 'window/showMessage'.
		[JsonRpcMethod (Methods.WindowShowMessageName)]
		public void WindowShowMessage (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"WindowShowMessage, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "WindowShowMessage");
			ShowMessageParams param = arg.ToObject<ShowMessageParams> ();
		}
		//
		// Summary:
		//     Strongly typed message object for 'telemetry/event'.
		[JsonRpcMethod (Methods.TelemetryEventName)]
		public void TelemetryEvent (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TelemetryEvent, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TelemetryEvent");
			object param = arg.ToObject<object> ();
		}
		//
		// Summary:
		//     Strongly typed message object for 'workspace/applyEdit'.
		[JsonRpcMethod (Methods.WorkspaceApplyEditName)]
		public ApplyWorkspaceEditResponse WorkspaceApplyEdit (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"WorkspaceApplyEdit, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "WorkspaceApplyEdit");
			ApplyWorkspaceEditParams param = arg.ToObject<ApplyWorkspaceEditParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'workspace/didChangeConfiguration'.
		[JsonRpcMethod (Methods.WorkspaceDidChangeConfigurationName)]
		public void WorkspaceDidChangeConfiguration (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"WorkspaceDidChangeConfiguration, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "WorkspaceDidChangeConfiguration");
			DidChangeConfigurationParams param = arg.ToObject<DidChangeConfigurationParams> ();
		}
		//
		// Summary:
		//     Strongly typed message object for 'workspace/executeCommand'
		[JsonRpcMethod (Methods.WorkspaceExecuteCommandName)]
		public object WorkspaceExecuteCommand (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"WorkspaceExecuteCommand, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "WorkspaceExecuteCommand");
			ExecuteCommandParams param = arg.ToObject<ExecuteCommandParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'workspace/symbol'.
		[JsonRpcMethod (Methods.WorkspaceSymbolName)]
		public SymbolInformation[] WorkspaceSymbol (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"WorkspaceSymbol, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "WorkspaceSymbol");
			WorkspaceSymbolParams param = arg.ToObject<WorkspaceSymbolParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'workspace/didChangeWatchedFiles'.
		[JsonRpcMethod (Methods.WorkspaceDidChangeWatchedFilesName)]
		public void WorkspaceDidChangeWatchedFiles (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"WorkspaceDidChangeWatchedFiles, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "WorkspaceDidChangeWatchedFiles");
			DidChangeWatchedFilesParams param = arg.ToObject<DidChangeWatchedFilesParams> ();
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/publishDiagnostics'.
		[JsonRpcMethod (Methods.TextDocumentPublishDiagnosticsName)]
		public void TextDocumentPublishDiagnostics (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentPublishDiagnostics, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentPublishDiagnostics");
			PublishDiagnosticParams param = arg.ToObject<PublishDiagnosticParams> ();
		}

		//
		// Summary:
		//     Strongly typed message object for 'textDocument/implementation'.
		[JsonRpcMethod (Methods.TextDocumentImplementationName)]
		public SumType<Location, Location[]>? TextDocumentImplementation (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentImplementation, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentImplementation");
			TextDocumentPositionParams param = arg.ToObject<TextDocumentPositionParams> ();
			return new Location ()
			{
				Uri = param.TextDocument.Uri,
				Range = new Range ()
				{
					Start = param.Position,
					End = param.Position
				}
			};
		}

		//
		// Summary:
		//     Strongly typed message object for 'textDocument/typeDefinition'.
		[JsonRpcMethod (Methods.TextDocumentTypeDefinitionName)]
		public SumType<Location, Location[]>? TextDocumentTypeDefinition (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentTypeDefinition, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentTypeDefinition");
			TextDocumentPositionParams param = arg.ToObject<TextDocumentPositionParams> ();
			return new Location ()
			{
				Uri = param.TextDocument.Uri,
				Range = new Range ()
				{
					Start = param.Position,
					End = param.Position
				}
			};
		}

		//
		// Summary:
		//     Strongly typed message object for 'textDocument/foldingRange'.
		[JsonRpcMethod (Methods.TextDocumentFoldingRangeName)]
		public FoldingRange[] TextDocumentFoldingRange (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentTypeDefinition, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentTypeDefinition");
			FoldingRangeParams param = arg.ToObject<FoldingRangeParams> ();
			return null;
		}

		//
		// Summary:
		//     Strongly typed message object for 'window/showMessageRequest'.
		[JsonRpcMethod (Methods.WindowShowMessageRequestName)]
		public MessageActionItem WindowShowMessageRequest (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"WindowShowMessageRequest, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "WindowShowMessageRequest");
			ShowMessageRequestParams param = arg.ToObject<ShowMessageRequestParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/rangeFormatting'.
		[JsonRpcMethod (Methods.TextDocumentRangeFormattingName)]
		public TextEdit[] TextDocumentRangeFormatting (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentRangeFormatting, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentRangeFormatting");
			DocumentRangeFormattingParams param = arg.ToObject<DocumentRangeFormattingParams> ();
			return null;
		}

		//
		// Summary:
		//     Strongly typed message object for 'textDocument/didClose'.
		[JsonRpcMethod (Methods.TextDocumentDidCloseName)]
		public void TextDocumentDidClose (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentDidClose, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentDidClose");
			DidCloseTextDocumentParams param = arg.ToObject<DidCloseTextDocumentParams> ();
			AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
			{
				Log (MessageType.Warning, $"TextDocumentDidClose, document not found: '{param.TextDocument.Uri.AbsolutePath}'");
				return;
			}
			docCtx.TextDocumentDidClose (this, param);
			int count = anglrDocDictionary.Remove (param.TextDocument.Uri.AbsolutePath);
			//Log ("number of open documents = " + count);
		}

		//
		// Summary:
		//     Strongly typed message object for 'textDocument/hover'
		[JsonRpcMethod (Methods.TextDocumentHoverName)]
		public async Task<Hover> TextDocumentHoverAsync (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentHover, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentHover");
			TextDocumentPositionParams param = arg.ToObject<TextDocumentPositionParams> ();
			AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
			{
				Log (MessageType.Warning, "TextDocumentHover, cannot find document info");
				return null;
			}
			return await docCtx.TextDocumentHoverAsync (this, param);
		}
		//
		// Summary:
		//     Strongly typed message object for 'client/registerCapability'.
		[JsonRpcMethod (Methods.ClientRegisterCapabilityName)]
		public object ClientRegisterCapability (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"ClientRegisterCapability, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "ClientRegisterCapability");
			RegistrationParams param = arg.ToObject<RegistrationParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/codeAction'
		[JsonRpcMethod (Methods.TextDocumentCodeActionName)]
		public Command[] TextDocumentCodeAction (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentCodeAction, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentCodeAction");
			CodeActionParams param = arg.ToObject<CodeActionParams> ();
			return null;
			Command [] cmdList = new Command [10];
			int index = 1;
			foreach (Command cmd in cmdList)
			{
				cmd.CommandIdentifier = "cmd-" + index;
				cmd.Title = "Command " + index++;
				cmd.Arguments = new object[] { "arg-1", "arg-2" };
			}
			return cmdList;
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/codeLens'
		[JsonRpcMethod (Methods.TextDocumentCodeLensName)]
		public CodeLens[] TextDocumentCodeLens (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentCodeLens, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentCodeLens");
			CodeLensParams param = arg.ToObject<CodeLensParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'codeLens/resolve'
		[JsonRpcMethod (Methods.CodeLensResolveName)]
		public CodeLens CodeLensResolve (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"CodeLensResolve, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "CodeLensResolve");
			CodeLens param = arg.ToObject<CodeLens> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/completion'.
		[JsonRpcMethod (Methods.TextDocumentCompletionName)]
		public object TextDocumentCompletion (JToken arg)
		{
			if (detailedInfo || detailedCompletion)
				Log (MessageType.Log, $"TextDocumentCompletion, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentCompletion");
			try
			{
				CompletionParams param = arg.ToObject<CompletionParams> ();
				return null;
			}
			catch (Exception e)
			{
				Log (MessageType.Error, "TextDocumentCompletion exception: " + e.Message);
				return null;
			}
		}
		//
		// Summary:
		//     Strongly typed message object for 'completionItem/resolve'.
		[JsonRpcMethod (Methods.TextDocumentCompletionResolveName)]
		public CompletionItem TextDocumentCompletionResolve (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentCompletionResolve, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentCompletionResolve");
			CompletionItem param = arg.ToObject<CompletionItem> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/definition'.
		[JsonRpcMethod (Methods.TextDocumentDefinitionName)]
		public object TextDocumentDefinition (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentDefinition, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentDefinition");
			TextDocumentPositionParams param = arg.ToObject<TextDocumentPositionParams> ();
			AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
				return null;
			return docCtx.TextDocumentDefinition (this, param);
		}

		//
		// Summary:
		//     Strongly typed message object for 'textDocument/didOpen'.
		[JsonRpcMethod (Methods.TextDocumentDidOpenName)]
		public void TextDocumentDidOpen (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentDidOpen, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentDidOpen");
			DidOpenTextDocumentParams param = arg.ToObject<DidOpenTextDocumentParams> ();
			AnglrDocContext docCtx = new AnglrDocContext (param, this);
			anglrDocDictionary.Add (docCtx);
			docCtx.TextDocumentDidOpen (this, param);
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/didChange'.
		[JsonRpcMethod (Methods.TextDocumentDidChangeName)]
		public void TextDocumentDidChange (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentDidChange, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentDidChange");
			DidChangeTextDocumentParams param = arg.ToObject<DidChangeTextDocumentParams> ();
			AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
				return;
			docCtx.TextDocumentDidChange (this, param);
		}

		//
		// Summary:
		//     Strongly typed message object for 'textDocument/didSave'.
		[JsonRpcMethod (Methods.TextDocumentDidSaveName)]
		public void TextDocumentDidSave (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentDidSave, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Error, "TextDocumentDidSave");
			DidSaveTextDocumentParams param = arg.ToObject<DidSaveTextDocumentParams> ();
			AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
				return;
			docCtx.TextDocumentDidSave (this, param);
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/documentHighlight'.
		[JsonRpcMethod (Methods.TextDocumentDocumentHighlightName)]
		public DocumentHighlight[] TextDocumentDocumentHighlight (JToken arg)
		{
			TextDocumentPositionParams param = arg.ToObject<TextDocumentPositionParams> ();
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentDocumentHighlight, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentDocumentHighlight");
			AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
				return null;
			return docCtx.TextDocumentDocumentHighlight (this, param);
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/documentLink'.
		[JsonRpcMethod (Methods.TextDocumentDocumentLinkName)]
		public DocumentLink TextDocumentDocumentLink (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentDocumentLink, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentDocumentLink");
			DocumentLinkParams param = arg.ToObject<DocumentLinkParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'documentLink/resolve'.
		[JsonRpcMethod (Methods.DocumentLinkResolveName)]
		public DocumentLink DocumentLinkResolve (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"DocumentLinkResolve, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "DocumentLinkResolve");
			DocumentLink param = arg.ToObject<DocumentLink> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/documentSymbol'.
		[JsonRpcMethod (Methods.TextDocumentDocumentSymbolName)]
		public SymbolInformation[] TextDocumentDocumentSymbol (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentDocumentSymbol, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Error, "TextDocumentDocumentSymbol");
			DocumentSymbolParams param = arg.ToObject<DocumentSymbolParams> ();
			AnglrDocContext docCtx = anglrDocDictionary.Find (param.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
				return null;
			return docCtx.TextDocumentDocumentSymbol (this, param);
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/formatting'.
		[JsonRpcMethod (Methods.TextDocumentFormattingName)]
		public TextEdit[] TextDocumentFormatting (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentFormatting, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Error, "TextDocumentFormatting");
			DocumentFormattingParams param = arg.ToObject<DocumentFormattingParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'textDocument/onTypeFormatting'.
		[JsonRpcMethod (Methods.TextDocumentOnTypeFormattingName)]
		public TextEdit[] TextDocumentOnTypeFormatting (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"TextDocumentOnTypeFormatting, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "TextDocumentOnTypeFormatting");
			DocumentOnTypeFormattingParams param = arg.ToObject<DocumentOnTypeFormattingParams> ();
			return null;
		}
		//
		// Summary:
		//     Strongly typed message object for 'client/unregisterCapability'.
		[JsonRpcMethod (Methods.ClientUnregisterCapabilityName)]
		public object ClientUnregisterCapability (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"ClientUnregisterCapability, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, "ClientUnregisterCapability");
			UnregistrationParams param = arg.ToObject<UnregistrationParams> ();
			return null;
		}

		[JsonRpcMethod (AnglrMethods.AnglrGetClassificationSpansName)]
		public async Task <AnglrGetClassificationSpansResult> AnglrGetClassificationSpansAsync (JToken arg)
		{
			if (detailedInfo)
				Log (MessageType.Log, $"{AnglrMethods.AnglrGetClassificationSpansName}, jtoken = {arg.ToString ()}");
			else
				Log (MessageType.Log, $"{AnglrMethods.AnglrGetClassificationSpansName}");
			AnglrGetClassificationSpansParams anglrGetClassificationSpansParams = arg.ToObject<AnglrGetClassificationSpansParams> ();
			AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetClassificationSpansParams.TextDocument.Uri.AbsolutePath);
			if (docCtx == null)
				return null;
			return await docCtx.AnglrGetClassificationSpansAsync (this, anglrGetClassificationSpansParams);
		}

        [JsonRpcMethod (AnglrMethods.AnglrGetGetHierarchyItemName)]
        public AnglrGetGetHierarchyItemResult AnglrGetGetHierarchyItem (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetGetHierarchyItemName}, jtoken = {arg.ToString ()}");
            AnglrGetGetHierarchyItemParams anglrGetGetHierarchyItemParams = arg.ToObject<AnglrGetGetHierarchyItemParams> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetGetHierarchyItemParams.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetGetHierarchyItem (this, anglrGetGetHierarchyItemParams);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetDictionaryItemName)]
        public AnglrGetDictionaryItemResult AnglrGetDictionaryItem (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetDictionaryItemName}, jtoken = {arg.ToString ()}");
            AnglrGetDictionaryItemParams anglrGetDictionaryItemParams = arg.ToObject<AnglrGetDictionaryItemParams> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetDictionaryItemParams.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetDictionaryItem (this, anglrGetDictionaryItemParams);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetParserStateItemName)]
        public AnglrGetParserStateItemResult AnglrGetParserStateItem (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetParserStateItemName}, jtoken = {arg.ToString ()}");
            AnglrGetParserStateItemParams anglrGetParserStateItemParams = arg.ToObject<AnglrGetParserStateItemParams> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetParserStateItemParams.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetParserStateItem (this, anglrGetParserStateItemParams);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetParserStatesInfoName)]
        public AnglrGetParserStatesInfoResult AnglrGetParserStatesInfo (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetParserStatesInfoName}, jtoken = {arg.ToString ()}");
            AnglrGetParserStatesInfoParams anglrGetParserStatesInfoParams = arg.ToObject<AnglrGetParserStatesInfoParams> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetParserStatesInfoParams.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetParserStatesInfo (this, anglrGetParserStatesInfoParams);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetParserStateLinkName)]
        public AnglrGetParserStateLinkResult AnglrGetParserStateLink (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetParserStateLinkName}, jtoken = {arg.ToString ()}");
            AnglrGetParserStateLinkParams anglrGetParserStateLinkParams = arg.ToObject<AnglrGetParserStateLinkParams> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetParserStateLinkParams.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetParserStateLink (this, anglrGetParserStateLinkParams);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetParserMagicNumberName)]
        public AnglrGetParserMagicNumberResult AnglrGetParserMagicNumber (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetParserMagicNumberName}, jtoken = {arg.ToString ()}");
            AnglrGetParserMagicNumberParams anglrGetParserMagicNumberParams = arg.ToObject<AnglrGetParserMagicNumberParams> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetParserMagicNumberParams.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetParserMagicNumber (this, anglrGetParserMagicNumberParams);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetParserSyntaxRuleName)]
        public AnglrGetParserSyntaxRuleResult AnglrGetParserSyntaxRule (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetParserSyntaxRuleName}, jtoken = {arg.ToString ()}");
            AnglrGetParserSyntaxRuleParams anglrGetParserSyntaxRuleParams = arg.ToObject<AnglrGetParserSyntaxRuleParams> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetParserSyntaxRuleParams.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetParserSyntaxRule (this, anglrGetParserSyntaxRuleParams);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetParserSyntaxRulesName)]
        public AnglrGetParserSyntaxRulesResult AnglrGetParserSyntaxRules (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetParserSyntaxRulesName}, jtoken = {arg.ToString ()}");
            AnglrGetParserSyntaxRulesParams anglrGetParserSyntaxRulesParams = arg.ToObject<AnglrGetParserSyntaxRulesParams> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetParserSyntaxRulesParams.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetParserSyntaxRules (this, anglrGetParserSyntaxRulesParams);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetItemNavigationInfoName)]
        public AnglrGetItemNavigationInfoResponse AnglrGetItemNavigationInfo (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetItemNavigationInfoName}, jtoken = {arg.ToString ()}");
            AnglrGetItemNavigationInfoRequest anglrGetItemNavigationInfoRequest = arg.ToObject<AnglrGetItemNavigationInfoRequest> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetItemNavigationInfoRequest.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetItemNavigationInfo (this, anglrGetItemNavigationInfoRequest);
        }

        [JsonRpcMethod (AnglrMethods.AnglrGetCompileFragmentName)]
        public AnglrGetCompileFragmentResponse AnglrGetCompileFragment (JToken arg)
        {
            Log (MessageType.Log, $"{AnglrMethods.AnglrGetCompileFragmentName}, jtoken = {arg.ToString ()}");
            AnglrGetCompileFragmentRequest anglrGetCompileFragmentRequest = arg.ToObject<AnglrGetCompileFragmentRequest> ();
            AnglrDocContext docCtx = anglrDocDictionary.Find (anglrGetCompileFragmentRequest.TextDocument.Uri.AbsolutePath);
            if (docCtx == null)
                return null;
            return docCtx.AnglrGetCompileFragment (this, anglrGetCompileFragmentRequest);
        }

        private bool detailedInfo = true;
		private bool detailedInitializeParams = false;
		private bool detailedInitializedParams = true;
		private bool detailedReferenceParams = false;
		private bool detailedSignatureHelp = true;
		private bool detailedCompletion = true;
	}
}
