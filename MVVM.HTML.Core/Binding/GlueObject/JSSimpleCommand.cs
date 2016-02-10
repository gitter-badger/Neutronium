﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

using MVVM.Component;

using MVVM.HTML.Core.V8JavascriptObject;
using MVVM.HTML.Core.Binding.Mapping;

namespace MVVM.HTML.Core.HTMLBinding
{
    public class JSSimpleCommand : GlueBase, IJSObservableBridge
    {
        private ISimpleCommand _JSSimpleCommand;
        private IWebView _IWebView;
        public JSSimpleCommand(IWebView iCefV8Context, ISimpleCommand icValue)
        {
            _IWebView = iCefV8Context;
            _JSSimpleCommand = icValue;
            JSValue = _IWebView.Factory.CreateObject(true);
        }

        public IJavascriptObject JSValue { get; private set; }

        private IJavascriptObject _MappedJSValue;

        public IJavascriptObject MappedJSValue { get { return _MappedJSValue; } }

        public void SetMappedJSValue(IJavascriptObject ijsobject, IJavascriptToCSharpConverter mapper)
        {
            _MappedJSValue = ijsobject;
            _MappedJSValue.Bind("Execute", _IWebView, (c, o, e) => Execute(e, mapper));
        }

        private object Convert(IJavascriptToCSharpConverter mapper, IJavascriptObject value)
        {
            var found = mapper.GetCachedOrCreateBasic(value, null);
            return (found != null) ? found.CValue : null;
        }

        private object GetArguments(IJavascriptToCSharpConverter mapper, IJavascriptObject[] e)
        {
            return (e.Length == 0) ? null : Convert(mapper, e[0]);
        }

        private void Execute(IJavascriptObject[] e, IJavascriptToCSharpConverter mapper)
        {
            _JSSimpleCommand.Execute(GetArguments(mapper, e));
        }

        public object CValue
        {
            get { return _JSSimpleCommand; }
        }

        public JSCSGlueType Type
        {
            get { return JSCSGlueType.SimpleCommand; }
        }

        public IEnumerable<IJSCSGlue> GetChildren()
        {
            return Enumerable.Empty<IJSCSGlue>();
        }

        protected override void ComputeString(StringBuilder sb, HashSet<IJSCSGlue> alreadyComputed)
        {
            sb.Append("{}");
        }
    }
}
