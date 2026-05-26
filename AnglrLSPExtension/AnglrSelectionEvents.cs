using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Threading;
using StreamJsonRpc;
using Newtonsoft.Json.Linq;

namespace AnglrLSPExtension
{
	class AnglrSelectionEvents : IVsSelectionEvents
	{
		public AnglrLspClientAccessPoint languageClient { get; private set; }
		public AnglrSelectionEvents (AnglrLspClientAccessPoint languageClient)
		{
			this.languageClient = languageClient;
		}

		public int OnSelectionChanged (IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
		{
			languageClient.Log ($"OnSelectionChanged: old = {((VSConstants.VSITEMID)itemidOld).ToString()}, new = {((VSConstants.VSITEMID)itemidNew).ToString()}");
			return VSConstants.S_OK;
		}

		public int OnElementValueChanged (uint elementid, object varValueOld, object varValueNew)
		{
			languageClient.Log ($"OnElementValueChanged: element = {((VSConstants.VSSELELEMID)elementid).ToString()}");
			return VSConstants.S_OK;
		}

		public int OnCmdUIContextChanged (uint dwCmdUICookie, int fActive)
		{
			languageClient.Log ($"OnCmdUIContextChanged: cookie = {dwCmdUICookie}, active = {fActive}");
			return VSConstants.S_OK;
		}
	}
}
