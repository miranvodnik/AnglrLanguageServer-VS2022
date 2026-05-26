using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using StreamJsonRpc;
using Newtonsoft.Json.Linq;
using AnglrJsonRpcMethods;

namespace AnglrLSPExtension
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
    public class AnglrLspClientAccessPoint : ILanguageClient, ILanguageClientCustomMessage2, IVsTextViewCreationListener
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
        private IVsOutputWindowPane vsOutputWindowPane = null;

        /// <summary>
        /// Create Anglr Language Server Protocol process and connect
        /// to it through duplex named pipe named anglr-lsp-pipe
        /// </summary>
        /// <param name="token"></param>
        /// <returns>connection with Anglr Language Server Protocol process</returns>
        public async Task<Connection> ActivateAsync (CancellationToken token)
        {
            Log ("step 2");
            await Task.Yield ();

            Process currentProcess = Process.GetCurrentProcess ();
            if (false)
            {
                Log ($"ENVIRONMENT FOR CURRENT PROCESS");
                foreach (string env in currentProcess.StartInfo.Environment.Keys)
                    Log ($"$({env}) = {currentProcess.StartInfo.Environment [env]}");
                foreach (string env in currentProcess.StartInfo.EnvironmentVariables)
                    Log ($"ENV-VAR: {env}");
            }

            Log ("PID = " + currentProcess.Id + ", Name = " + currentProcess.ProcessName);

            try
            {
                Log ($"current working directory is {Directory.GetCurrentDirectory ()}");
                Log ($"extension {Assembly.GetExecutingAssembly ().GetName ().Name} location is {Assembly.GetExecutingAssembly ().Location}");
                Log ($"extension {Assembly.GetExecutingAssembly ().GetName ().Name} codebase is {Assembly.GetExecutingAssembly ().CodeBase}");

                string anglrPipeName = $"anglr-lsp-pipe-{currentProcess.Id}";
                NamedPipeServerStream anglrPipe = new NamedPipeServerStream (anglrPipeName, PipeDirection.InOut, -1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);

                using (Process process = new Process ())
                {
                    ProcessStartInfo info = new ProcessStartInfo ();
                    //info.FileName = $"{Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location)}" + @"\AnglrLSPServerProcess.exe";
                    info.FileName = @"D:\Users\Miran\source\repos\AnglrLanguageServer-VS2022\AnglrLSPServerProcess\bin\Release\AnglrLSPServerProcess.exe";
                    info.WorkingDirectory = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
                    //info.WorkingDirectory = Directory.GetCurrentDirectory ();
                    info.UseShellExecute = false;
                    info.CreateNoWindow = true;

                    Log ($"anglr LSP server path = {info.FileName}");
                    Log ($"anglr LSP working dir = {info.WorkingDirectory}");
                    Log ($"starting LSP process: '{info.FileName} {info.Arguments}'");

                    process.StartInfo = info;

                    if (false)
                    {
                        Log ($"ENVIRONMENT FOR LSP PROCESS");
                        foreach (string env in info.Environment.Keys)
                            Log ($"$({env}) = {info.Environment [env]}");
                        foreach (string env in info.EnvironmentVariables)
                            Log ($"ENV-VAR: {env}");
                    }

                    if (process.Start ())
                    {
                        Log ($"WaitForConnectionAsync ({anglrPipeName}) started");
                        await anglrPipe.WaitForConnectionAsync ();
                        Log ($"WaitForConnectionAsync ({anglrPipeName}) finished");
                        if (!anglrPipe.IsConnected)
                        {
                            Log ("out pipe not connected");
                        }

                        Log ("step 3: LSP process connected");
                        return connection = new Connection (anglrPipe, anglrPipe);
                    }
                }

                Log ("step 4: LSP process not started or not connected");
                return null;

            }
            catch (Exception e)
            {
                Log ($"LSP Client Error: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Handler for Loaded event
        /// </summary>
        /// <returns></returns>
        public async Task OnLoadedAsync ()
        {
            Log ($"step 5: {StartAsync?.GetInvocationList ().Length}");
            await StartAsync?.InvokeAsync (this, EventArgs.Empty);
            Log ("step 6");
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
            Log ("step 7");

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
            Log ($"step 8: {e.Message}");
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
            Log ("custom message attached");
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
                Log ($"LSP method {targetName} failed, error = {e.Message}");
            }
            return default;
        }

        #endregion

        #region IVsTextViewCreationListener implementation

        [Import]
        internal IVsEditorAdaptersFactoryService AdaptersFactory { get; private set; }

        [Import]
        internal IContentTypeRegistryService ContentTypeRegistryService { get; private set; }

        public void VsTextViewCreated (IVsTextView textViewAdapter)
        {
            try
            {
                if (!(AdaptersFactory.GetWpfTextView (textViewAdapter) is IWpfTextView view))
                    return;

                Microsoft.VisualStudio.Text.ITextBuffer buffer = view.TextBuffer;
                if (buffer == null)
                    return;

                string fileName = buffer.GetFileName ();
                if (fileName == null)
                    return;

                Log ($"text view created for: {fileName}");
                try
                {
                    AnglrLspClientAccessPoint.Instance.Log ($"get hierarchy completed");
                    //AnglrProductionsViewCommand anglrProductionsViewCommand = AnglrProductionsViewCommand.Instance;
                    //AnglrProductionsView anglrProductionsView = anglrProductionsViewCommand.window;
                    //AnglrProductionsViewControl anglrProductionsViewControl = anglrProductionsView.Control;
                    //anglrProductionsViewControl.AddTreeElement (fileName, 0);
                }
                catch (Exception e)
                {
                    AnglrLspClientAccessPoint.Instance.Log ($"get hierarchy failed: {e.Message}");
                    AnglrLspClientAccessPoint.Instance.Log ($"\t{e.StackTrace}");
                }
            }
            catch (Exception)
            {
            }

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
            vsOutputWindowPane = ServiceProvider.GlobalProvider.GetService (typeof (SVsGeneralOutputWindowPane)) as IVsOutputWindowPane;
            Log ("step 1");
            MiddleLayer = new AnglrLspMiddleLayer ();
            CustomMessageTarget = new AnglrCustomTarget ();
            Instance = this;
        }

        public void Log (string message)
        {
            if (!debug)
                return;
            //Trace.WriteLine ($"{DateTime.Now}.{DateTime.Now.Millisecond} {this.GetType ().Name} : {message}");
            if (vsOutputWindowPane != null)
                vsOutputWindowPane.OutputString ($"{DateTime.Now}.{DateTime.Now.Millisecond} {this.GetType ().Name} : {message}\n");
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

        #endregion
    }

    /// <summary>
    /// 'man in the middle' between Language Server Protocol client object
    /// and Language Server Protocol process
    /// </summary>
    internal class AnglrLspMiddleLayer : ILanguageClientMiddleLayer
    {
        public JsonRpc rpc { get; set; } = null;

        public bool CanHandle (string methodName)
        {
            //if (rpc != null)
            //	AnglrLspClientAccessPoint.Instance.Log ($"can   handle: {methodName}");
            //else
            //	AnglrLspClientAccessPoint.Instance.Log ($"can't handle: {methodName}");
            return (methodName == Methods.TextDocumentDidOpenName) || (methodName == Methods.TextDocumentDidChangeName) || (methodName == Methods.TextDocumentDidSaveName) || (methodName == Methods.TextDocumentDidCloseName);
        }

        public Task HandleNotificationAsync (string methodName, JToken methodParam, Func<JToken, Task> sendNotification)
        {
            //AnglrLspClientAccessPoint.Instance.Log ($"handle notification: {methodName}");//: '{methodParam.ToString (Newtonsoft.Json.Formatting.None, null)}'");
            switch (methodName)
            {
                case Methods.TextDocumentDidOpenName:
                {
                    DidOpenTextDocumentParams param = methodParam.ToObject<DidOpenTextDocumentParams> ();
                    AnglrLspClientAccessPoint.Instance.Log ($"anglr document opened: {param.TextDocument.Uri.LocalPath}");
                    //AnglrGetGetHierarchyItemParams anglrGetGetHierarchyItemParams = new AnglrGetGetHierarchyItemParams ()
                    //{
                    //    TextDocument = new TextDocumentIdentifier ()
                    //    {
                    //        Uri = param.TextDocument.Uri
                    //    },
                    //    ItemId = 0
                    //};
                    //Task<AnglrGetGetHierarchyItemResult> anglrGetGetHierarchyItemTask = rpc.InvokeAsync<AnglrGetGetHierarchyItemResult> (AnglrMethods.AnglrGetGetHierarchyItemName, anglrGetGetHierarchyItemParams);
                    //TaskAwaiter<AnglrGetGetHierarchyItemResult> taskAwaiter = anglrGetGetHierarchyItemTask.GetAwaiter ();
                    //taskAwaiter.OnCompleted
                    //    (() =>
                    //    {
                    //        anglrGetGetHierarchyItemTask.Wait ();
                    //        try
                    //        {
                    //            AnglrLspClientAccessPoint.Instance.Log ($"get hierarchy completed");
                    //            AnglrProductionsViewCommand anglrProductionsViewCommand = AnglrProductionsViewCommand.Instance;
                    //            AnglrProductionsView anglrProductionsView = anglrProductionsViewCommand.window;
                    //            AnglrProductionsViewControl anglrProductionsViewControl = anglrProductionsView.Control;
                    //            anglrProductionsViewControl.anglrHierarchy.Items.Add (param.TextDocument.Uri.LocalPath);
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            AnglrLspClientAccessPoint.Instance.Log ($"get hierarchy failed: {e.Message}");
                    //            AnglrLspClientAccessPoint.Instance.Log ($"\t{e.StackTrace}");
                    //        }
                    //    });
                }
                break;
                case Methods.TextDocumentDidChangeName:
                {
                    DidChangeTextDocumentParams param = methodParam.ToObject<DidChangeTextDocumentParams> ();
                    AnglrLspClientAccessPoint.Instance.Log ($"anglr document changed: {param.TextDocument.Uri.LocalPath}");
                }
                break;
                case Methods.TextDocumentDidSaveName:
                {
                    DidSaveTextDocumentParams param = methodParam.ToObject<DidSaveTextDocumentParams> ();
                    AnglrLspClientAccessPoint.Instance.Log ($"anglr document saved: {param.TextDocument.Uri.LocalPath}");
                }
                break;
                case Methods.TextDocumentDidCloseName:
                {
                    DidCloseTextDocumentParams param = methodParam.ToObject<DidCloseTextDocumentParams> ();
                    AnglrLspClientAccessPoint.Instance.Log ($"anglr document closed: {param.TextDocument.Uri.LocalPath}");
                }
                break;
            }
            return (rpc != null) ? rpc.InvokeAsync (methodName, methodParam) : sendNotification (methodParam);
        }

        public Task<JToken> HandleRequestAsync (string methodName, JToken methodParam, Func<JToken, Task<JToken>> sendRequest)
        {
            //AnglrLspClientAccessPoint.Instance.Log ($"handle request: {methodName}");//: '{methodParam.ToString (Newtonsoft.Json.Formatting.None, null)}'");
            return (rpc != null) ? rpc.InvokeAsync<JToken> (methodName, methodParam) : sendRequest (methodParam);
        }
    }

    internal class AnglrCustomTarget
    {

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
