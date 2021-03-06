﻿using System;
using Neutronium.Core;
using Neutronium.Core.JavascriptFramework;

namespace Neutronium.WPF
{
    /// <summary>
    /// Singleton object that allows you to register IWPFWebWindowFactory
    /// to the current session.
    /// </summary>
    public interface IHTMLEngineFactory : IDisposable
    {
        /// <summary>
        /// Find a IWPFWebWindowFactory by name.
        /// This method is called internally by HTLM_WPF.Component controls
        /// </summary>
        /// <param name="engineName">
        /// the name of the factory to be found
        /// </param>
        /// <returns>
        /// IWPFWebWindowFactory registered with a similar name.
        ///</returns>
        IWPFWebWindowFactory ResolveJavaScriptEngine(string engineName);

        /// <summary>
        /// register a IWPFWebWindowFactory using its Name property
        /// </summary>
        /// <param name="wpfWebWindowFactory">
        /// IWPFWebWindowFactory to be registered
        /// </param>
        void RegisterHTMLEngine(IWPFWebWindowFactory wpfWebWindowFactory);

        /// <summary>
        /// Find a IJavascriptFrameworkManager by name.
        /// This method is called internally by HTLM_WPF.Component controls
        /// </summary>
        /// <param name="frameworkName">
        /// the name of the factory to be found
        /// </param>
        /// <returns>
        /// IJavascriptFrameworkManager registered with a similar name.
        ///</returns>
        IJavascriptFrameworkManager ResolveJavaScriptFramework(string frameworkName);

        /// <summary>
        /// register a IJavascriptFrameworkManager using its Name property
        /// </summary>
        /// <param name="javascriptFrameworkManager">
        /// IJavascriptFrameworkManager to be registered
        /// </param>
        void RegisterJavaScriptFramework(IJavascriptFrameworkManager javascriptFrameworkManager);

        /// <summary>
        /// get or set WebSessionLogger
        /// </summary>
        IWebSessionLogger WebSessionLogger { get; set; }
    }
}
