using AnglrLogLibrary;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnglrLangExtension
{
	class AnglrSelectionEvents : IVsSelectionEvents
	{
		public AnglrLspClientAccessPoint LanguageClient { get; private set; }
        public IAnglrLogger AnglrLogger { get; private set; }
        
		public AnglrSelectionEvents (AnglrLspClientAccessPoint languageClient)
		{
			LanguageClient = languageClient;
			AnglrLogger = LanguageClient?.AnglrLogger ?? new VoidAnglrLogger ();
		}

		public int OnSelectionChanged (IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
		{
            AnglrLogger?.DebugLine ($"OnSelectionChanged: old = {((VSConstants.VSITEMID)itemidOld).ToString()}, new = {((VSConstants.VSITEMID)itemidNew).ToString()}");
			return VSConstants.S_OK;
		}

		public int OnElementValueChanged (uint elementid, object varValueOld, object varValueNew)
		{
            AnglrLogger?.DebugLine ($"OnElementValueChanged: element = {((VSConstants.VSSELELEMID)elementid).ToString()}");
			return VSConstants.S_OK;
		}

		public int OnCmdUIContextChanged (uint dwCmdUICookie, int fActive)
		{
            AnglrLogger?.DebugLine ($"OnCmdUIContextChanged: cookie = {dwCmdUICookie}, active = {fActive}");
			return VSConstants.S_OK;
		}
	}
}
