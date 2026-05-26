using Anglr.Parser;
using AnglrDebuggerJsonRpcMessages;
using AnglrJsonRpcMethods;
using AnglrLogLibrary;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnglrLangExtension
{
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// Lnguage Server Protol client
    /// </summary>
    [ContentType ("anglr")]
    [Export (typeof (ILanguageClient))]
    [Export (typeof (ILanguageClientCustomMessage2))]
    [Export (typeof (IVsTextViewCreationListener))]
    [TextViewRole (PredefinedTextViewRoles.Editable)]
    public class AnglrLspClientAccessPoint : ILanguageClient, ILanguageClientCustomMessage2
    {
        #region ILanguageClient Interface implementation

        public string Name { get; private set; } = "Anglr Language Extension";
        public IEnumerable<string> ConfigurationSections { get; private set; } = null;
        public object InitializationOptions { get; private set; } = null;
        public IEnumerable<string> FilesToWatch { get; private set; } = null;
        public event AsyncEventHandler<EventArgs> StartAsync;
        public event AsyncEventHandler<EventArgs> StopAsync;

        internal const string UiContextGuidString = "DACA25B4-E9C8-43EE-8B93-4B249FA99B27";
        private Guid uiContextGuid = new Guid (UiContextGuidString);
        public IAnglrLogService AnglrLogService {  get; private set; }
        public IAnglrLogger AnglrLogger {  get; private set; }

        /// <summary>
        /// Create Anglr Language Server Protocol process and connect
        /// to it through duplex named pipe named anglr-lsp-pipe
        /// </summary>
        /// <param name="token"></param>
        /// <returns>connection with Anglr Language Server Protocol process</returns>
        public async Task<Connection> ActivateAsync (CancellationToken token)
        {
            AnglrLogger?.DebugLine ("step 2");
            await Task.Yield ();

            Process currentProcess = Process.GetCurrentProcess ();
            if (false)
            {
                AnglrLogger?.DebugLine ($"ENVIRONMENT FOR CURRENT PROCESS");
                foreach (string env in currentProcess.StartInfo.Environment.Keys)
                    AnglrLogger?.DebugLine ($"$({env}) = {currentProcess.StartInfo.Environment [env]}");
                foreach (string env in currentProcess.StartInfo.EnvironmentVariables)
                    AnglrLogger?.DebugLine ($"ENV-VAR: {env}");
            }

            AnglrLogger?.DebugLine ($"PID = {currentProcess.Id}, Name = {currentProcess.ProcessName}");

            try
            {
                AnglrLogger?.DebugLine ($"current working directory is {Directory.GetCurrentDirectory ()}");
                AnglrLogger?.DebugLine ($"extension {Assembly.GetExecutingAssembly ().GetName ().Name} location is {Assembly.GetExecutingAssembly ().Location}");
                AnglrLogger?.DebugLine ($"extension {Assembly.GetExecutingAssembly ().GetName ().Name} codebase is {Assembly.GetExecutingAssembly ().CodeBase}");

                using (Process process = new Process ())
                {
                    ProcessStartInfo info = new ProcessStartInfo ();
                    info.WorkingDirectory = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
                    info.FileName = $@"{info.WorkingDirectory}\AnglrLSPServerProcess.exe";
                    info.UseShellExecute = false;
                    info.CreateNoWindow = true;

                    AnglrLogger?.DebugLine ($"anglr LSP server path = {info.FileName}");
                    AnglrLogger?.DebugLine ($"anglr LSP working dir = {info.WorkingDirectory}");
                    AnglrLogger?.DebugLine ($"starting LSP process: '{info.FileName} {info.Arguments}'");

                    process.StartInfo = info;

                    if (false)
                    {
                        AnglrLogger?.DebugLine ($"ENVIRONMENT FOR LSP PROCESS");
                        foreach (string env in info.Environment.Keys)
                            AnglrLogger?.DebugLine ($"$({env}) = {info.Environment [env]}");
                        foreach (string env in info.EnvironmentVariables)
                            AnglrLogger?.DebugLine ($"ENV-VAR: {env}");
                    }

                    if (process.Start ())
                    {
                        string anglrPipeName = $"anglr-lsp-pipe-{process.Id}";
                        NamedPipeServerStream anglrPipe = new NamedPipeServerStream (anglrPipeName, PipeDirection.InOut, -1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);

                        AnglrLogger?.DebugLine ($"WaitForConnectionAsync ({anglrPipeName}) started");
                        await anglrPipe.WaitForConnectionAsync ();
                        AnglrLogger?.DebugLine ($"WaitForConnectionAsync ({anglrPipeName}) finished");
                        if (!anglrPipe.IsConnected)
                        {
                            AnglrLogger?.DebugLine ("out pipe not connected");
                        }

                        AnglrLogger?.DebugLine ("step 3: LSP process connected");
                        return connection = new Connection (anglrPipe, anglrPipe);
                    }
                }

                AnglrLogger?.DebugLine ("step 4: LSP process not started or not connected");
                return null;

            }
            catch (Exception e)
            {
                AnglrLogger?.ErrorLine ($"LSP Client Error: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Handler for Loaded event
        /// </summary>
        /// <returns></returns>
        public async Task OnLoadedAsync ()
        {
            AnglrLogger?.DebugLine ($"step 5: {StartAsync?.GetInvocationList ().Length}");
            await StartAsync?.InvokeAsync (this, EventArgs.Empty);
            AnglrLogger?.DebugLine ("step 6");
        }

        /// <summary>
        /// Handler for SrverInitialized event
        /// Now that language client has been successfully initialized,
        /// we can get some global services:
        /// - ANGLR browser provider
        /// - more to come
        /// </summary>
        /// <returns>successfully completed task</returns>
        public async Task OnServerInitializedAsync ()
        {
            AnglrLogger?.DebugLine ("step 7");

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync ();
        }

        /// <summary>
        /// Handler for ServerInitializedFailed event. It does nothing
        /// </summary>
        /// <param name="e"></param>
        /// <returns>successfully completed task</returns>
        public async Task OnServerInitializeFailedAsync (Exception e)
        {
            await Task.Yield ();
            AnglrLogger?.DebugLine ($"step 8: {e.Message}");
        }

        public Task<InitializationFailureContext> OnServerInitializeFailedAsync (ILanguageClientInitializationInfo initializationState)
        {
            string message = "ANGLR Language Client failed to activate";
            string exception = initializationState.InitializationException?.ToString () ?? string.Empty;
            message = $"{message}\n {exception}";

            var failureContext = new InitializationFailureContext ()
            {
                FailureMessage = message,
            };

            return Task.FromResult (failureContext);
        }

        /// <summary>
        /// Start event manipulation routines
        /// </summary>
        event AsyncEventHandler<EventArgs> ILanguageClient.StartAsync
        {
            add => StartAsync += value;
            remove => StartAsync -= value;
        }

        /// <summary>
        /// Stop event manipulation routines
        /// </summary>
        event AsyncEventHandler<EventArgs> ILanguageClient.StopAsync
        {
            add => StopAsync += value;
            remove => StopAsync -= value;
        }

        #endregion

        #region ILanguageClientCustomMessage2 implementation
        public object MiddleLayer { get; private set; }
        public object CustomMessageTarget { get; private set; }

        public async Task AttachForCustomMessageAsync (JsonRpc rpc)
        {
            await Task.Yield ();
            ((AnglrLspMiddleLayer) MiddleLayer).rpc = anglrRpc = rpc;
            AnglrLogger?.DebugLine ("custom message attached");
        }

        public async Task<TResult> InvokeAsync<TResult> (string targetName, object argument)
        {
            try
            {
                if (anglrRpc == null)
                    return default;
                return await anglrRpc.InvokeAsync<TResult> (targetName, argument);
            }
            catch (Exception e)
            {
                AnglrLogger?.ErrorLine ($"LSP method {targetName} failed, error = {e.Message}");
            }
            return default;
        }

        #endregion

        #region AnglrLspClientAccessPoint implementation
        public Connection connection { get; private set; } = null;
        public static AnglrLspClientAccessPoint Instance { get; private set; } = null;

        public bool ShowNotificationOnInitializeFailed => true;

        public static bool debug = true;

        private JsonRpc anglrRpc = null;

        public AnglrLspClientAccessPoint ()
        {
            AnglrLogService = ServiceProvider.GlobalProvider.GetService (typeof (SAnglrLogService)) as IAnglrLogService;
            AnglrLogger = AnglrLogService?.Logger ?? new VoidAnglrLogger ();
            AnglrLogger?.DebugLine ("step 1");
            MiddleLayer = new AnglrLspMiddleLayer (this);
            CustomMessageTarget = new AnglrCustomTarget (AnglrLogger);
            Instance = this;
        }

        public void Log (AnglrLogLevel logLevel, string message, string indent = null) => AnglrLogger?.Log (logLevel, message);

        #endregion
    }

    /// <summary>
    /// 'man in the middle' between Language Server Protocol client object
    /// and Language Server Protocol process
    /// </summary>
    internal class AnglrLspMiddleLayer : ILanguageClientMiddleLayer
    {
        public JsonRpc rpc { get; set; } = null;

        private AnglrLspClientAccessPoint anglrLspClientAccessPoint = null;
        private IAnglrLangService anglrLangService = null;
        private IAnglrLogger anglrLogger = null;

        public AnglrLspMiddleLayer (AnglrLspClientAccessPoint anglrLspClientAccessPoint)
        {
            this.anglrLspClientAccessPoint = anglrLspClientAccessPoint;
            //anglrLangService = Package.GetGlobalService (typeof (SAnglrLangService)) as IAnglrLangService;
            anglrLangService = ThreadHelper.JoinableTaskFactory.Run (() => AnglrLangExtensionPackage.Instance.GetServiceAsync (typeof (SAnglrLangService))) as IAnglrLangService;
            anglrLogger = anglrLangService?.AnglrLogger ?? new VoidAnglrLogger ();
        }

        public void Log (AnglrLogLevel logLevel, string message, string indent = null) => anglrLogger?.Log (logLevel, message);

        public bool CanHandle (string methodName)
        {
            return
                (methodName == Methods.TextDocumentDidOpenName) ||
                (methodName == Methods.TextDocumentDidChangeName) ||
                (methodName == Methods.TextDocumentDidSaveName) ||
                (methodName == Methods.TextDocumentDidCloseName);
        }

        public async Task HandleNotificationAsync (string methodName, JToken methodParam, Func<JToken, Task> sendNotification)
        {
            switch (methodName)
            {
                case Methods.TextDocumentDidOpenName:
                {
                    DidOpenTextDocumentParams param = methodParam.ToObject<DidOpenTextDocumentParams> ();
                    anglrLogger?.InfoLine ($"LSP: anglr document opened: {param.TextDocument.Uri.LocalPath}");
                }
                break;
                case Methods.TextDocumentDidChangeName:
                {
                    DidChangeTextDocumentParams param = methodParam.ToObject<DidChangeTextDocumentParams> ();
                    anglrLogger?.InfoLine ($"LSP: anglr document changed: {param.TextDocument.Uri.LocalPath}");
                }
                break;
                case Methods.TextDocumentDidSaveName:
                {
                    DidSaveTextDocumentParams param = methodParam.ToObject<DidSaveTextDocumentParams> ();
                    anglrLogger?.InfoLine ($"LSP: anglr document saved: {param.TextDocument.Uri.LocalPath}");
                }
                break;
                case Methods.TextDocumentDidCloseName:
                {
                    try
                    {
                        DidCloseTextDocumentParams param = methodParam.ToObject<DidCloseTextDocumentParams> ();
                        string fileName = param.TextDocument.Uri.LocalPath;
                        anglrLogger?.InfoLine ($"LSP: anglr document closed: {fileName}");
                        await anglrLangService.CloseAnglrFileAsync (fileName);
                    }
                    catch (Exception ex)
                    {
                        anglrLogger?.DebugLine ($"LSP: TextDocumentDidClose throws exception: {ex.Message}");
                    }
                }
                break;
                case AnglrMethods.AnglrGetParserMagicNumberName:
                {
                    AnglrGetParserMagicNumberResult anglrGetParserMagicNumberResult = methodParam.ToObject<AnglrGetParserMagicNumberResult> ();
                    anglrLogger?.DebugLine ($"LSP: document magic number: {anglrGetParserMagicNumberResult?.MagicNumber}");
                }
                break;
            }
            if (rpc != null)
            {
                await rpc.InvokeAsync (methodName, methodParam);
                if (methodName == Methods.TextDocumentDidOpenName)
                {
                    try
                    {
                        DidOpenTextDocumentParams param = methodParam.ToObject<DidOpenTextDocumentParams> ();
                        string fileName = param.TextDocument.Uri.LocalPath;
                        anglrLogger?.DebugLine ($"LSP: anglr document opened: {fileName}");
                        await anglrLangService.OpenAnglrFileAsync (fileName);
                    }
                    catch (Exception ex)
                    {
                        anglrLogger?.ErrorLine ($"LSP: TextDocumentDidOpen throws exception: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    }
                }
            }
            else
                await sendNotification (methodParam);
        }

        public Task<JToken> HandleRequestAsync (string methodName, JToken methodParam, Func<JToken, Task<JToken>> sendRequest)
        {
            return (rpc != null) ? rpc.InvokeAsync<JToken> (methodName, methodParam) : sendRequest (methodParam);
        }
    }

    internal class AnglrCustomTarget
    {
        public IAnglrLogger AnglrLogger { get; private set; }

        public AnglrCustomTarget (IAnglrLogger anglrLogger)
        {
            AnglrLogger = anglrLogger ?? new VoidAnglrLogger ();
        } 

        [JsonRpcMethod (AnglrMethods.AnglrLspLogMessageName)]
        public void AnglrLspLogMessage (JToken arg)
        {
            AnglrLspLogMessageNotification  notification = arg.ToObject<AnglrLspLogMessageNotification> ();
            AnglrLogger?.InfoLine ($"{AnglrMethods.AnglrLspLogMessageName}: {notification.LogLevel} - {notification.Message}");
        }
    }

    public static class AnglrExtensions
    {

        public static string GetFileName (this ITextBuffer buffer)
        {
            ThreadHelper.ThrowIfNotOnUIThread ();
            if (buffer == null)
                return null;

            IVsTextBuffer bufferAdapter;
            buffer.Properties.TryGetProperty (typeof (IVsTextBuffer), out bufferAdapter);
            if (bufferAdapter == null)
                return null;

            var persistFileFormat = bufferAdapter as IPersistFileFormat;
            string ppzsFilename = null;
            if (persistFileFormat != null)
                persistFileFormat.GetCurFile (out ppzsFilename, out _);

            return ppzsFilename;
        }
    }

    class AnglrContentDefinition
    {
        [Export]
        [Name ("anglr")]
        [BaseDefinition (CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
        internal static ContentTypeDefinition AnglrContentTypeDefinition;

        [Export]
        [FileExtension (".anglr")]
        [ContentType ("anglr")]
        internal static FileExtensionToContentTypeDefinition AnglrFileExtensionDefinition;
    }
}
