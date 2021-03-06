﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Neutronium.Core;
using Neutronium.Core.Infra;
using Neutronium.Core.JavascriptFramework;
using Neutronium.Core.WebBrowserEngine.JavascriptObject;

namespace Neutronium.JavascriptFramework.Vue
{
    public class VueSessionInjector : IJavascriptFrameworkManager
    {
        public string FrameworkName => "vue.js 1.0.25";
        public string Name => "VueInjector";
        private string _DebugScript;
        private const string _ToogleDebug = "window.vueDebug();";

        public IJavascriptViewModelManager CreateManager(IWebView webView, IJavascriptObject listener, IWebSessionLogger logger) 
        {
            return new VueVmManager(webView, listener, logger);
        }

        public string GetDebugScript()
        {
            if (_DebugScript != null)
                return _DebugScript;

            var loader = GetResourceReader();
            var almost = loader.Load("vuedebug.js");
            var updated = almost.Replace(@"build/devtools.js", GetFilePath("scripts/devtools.js"));        
            var builder = new StringBuilder(updated);
            builder.Append(GetPathInjectorscript());

            return _DebugScript = builder.ToString();
        }

        private string GetPathInjectorscript()
        {
            return $"(function(){{window.__vue__backend__path__='{GetFilePath("scripts/backend.js")}';window.__vue__logo__path__='{GetFilePath("resource/logo.png")}';}})();";
        }

        private static string GetFilePath(string scriptPath)
        {
            var path = Path.GetDirectoryName( typeof(VueSessionInjector).Assembly.Location);
            var fullPath = Path.Combine(path, scriptPath);
            return new Uri(fullPath).AbsoluteUri;
        }

        public string GetDebugToogleScript() 
        {
            return _ToogleDebug;
        }

        public string GetMainScript(bool debugContext)
        {
            var loader = GetResourceReader();
            return loader.LoadJavascript(GetJavascriptSource(debugContext), !debugContext, !debugContext);
        }

        private static IEnumerable<string> GetJavascriptSource(bool debugMode)
        {
            if (debugMode)
                yield return "hook";

            yield return "vue";
            yield return "subscribeArray";
            yield return "vueComandDirective";
            yield return "vueGlue";
        }

        public bool HasDebugScript()
        {
            return true;
        }

        private ResourceReader GetResourceReader()
        {
            return new ResourceReader("scripts", this);
        }
    }
}
