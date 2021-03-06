﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neutronium.Core.Infra;
using Neutronium.Core.WebBrowserEngine.JavascriptObject;
using Xilium.CefGlue;

namespace Neutronium.WebBrowserEngine.CefGlue.CefGlueImplementation
{
    internal class CefV8_Factory : IJavascriptObjectFactory
    {
        private static uint _Count = 0;

        private static readonly IDictionary<Type, Func<object, CefV8Value>> _Converters = new Dictionary<Type, Func<object, CefV8Value>>();
        private readonly IWebView _CefV8_WebView;

        static CefV8_Factory()
        {
            Register<string>(CefV8Value.CreateString);

            Register<Int64>((source) => CefV8Value.CreateDouble((double)source));
            Register<UInt64>((source) => CefV8Value.CreateDouble((double)source));
            Register<float>((source) => CefV8Value.CreateDouble((double)source));

            Register<Int32>(CefV8Value.CreateInt);
            Register<Int16>((source) => CefV8Value.CreateInt((int)source));

            Register<UInt32>(CefV8Value.CreateUInt);
            Register<UInt16>((source) => CefV8Value.CreateUInt((UInt32)source));

            //check two way and convertion back
            Register<char>((source) => CefV8Value.CreateString(new StringBuilder().Append(source).ToString()));
            Register<double>(CefV8Value.CreateDouble);
            Register<decimal>((source) => CefV8Value.CreateDouble((double)source));
            Register<bool>(CefV8Value.CreateBool);
            Register<DateTime>(CefV8Value.CreateDate);
        }

        
        private static void Register<T>(Func<T, CefV8Value> Factory)
        {
            _Converters.Add(typeof(T), (o) => Factory((T)o));
        }

        public CefV8_Factory(IWebView iCefV8_WebView)
        {
            _CefV8_WebView = iCefV8_WebView;
        }

        public bool CreateBasic(object ifrom, out IJavascriptObject res)
        {
            Func<object, CefV8Value> conv;
            if (!_Converters.TryGetValue(ifrom.GetType(), out conv))
            {
                res = null;
                return false;
            }

            res = new CefV8_JavascriptObject( _CefV8_WebView.Evaluate(() => conv(ifrom)) );
            return true;
        }

        public bool IsTypeBasic(Type itype)
        {
            if (itype == null)
                return false;

            return _Converters.ContainsKey(itype);
        }

        public IJavascriptObject CreateNull()
        {
            return new CefV8_JavascriptObject(CefV8Value.CreateNull());
        }

        public IJavascriptObject CreateUndefined()
        {
            return new CefV8_JavascriptObject(CefV8Value.CreateUndefined());
        }

        public IJavascriptObject CreateObject(bool iLocal)
        {
            return UpdateObject(CefV8Value.CreateObject(null));
        }

        public IJavascriptObject CreateInt(int value)
        {
            return new CefV8_JavascriptObject(CefV8Value.CreateInt(value));
        }

        public IJavascriptObject CreateString(string value)
        {
            return new CefV8_JavascriptObject(CefV8Value.CreateString(value));
        }

        public IJavascriptObject CreateBool(bool value)
        {
            return new CefV8_JavascriptObject(CefV8Value.CreateBool(value));
        }

        public IJavascriptObject CreateDouble(double value)
        {
            return new CefV8_JavascriptObject(CefV8Value.CreateDouble(value));
        }

        public IJavascriptObject CreateArray(IEnumerable<IJavascriptObject> iCollection)
        {
            var col = iCollection.ToList();
            var res = CefV8Value.CreateArray(col.Count);
            col.ForEach((el, i) => res.SetValue(i, el.Convert()));
            return UpdateObject(res);
        }

        private void BasicUpdateObject(CefV8Value ires)
        {
            if (ires != null)
            {
                ires.SetValue("_MappedId", CefV8Value.CreateUInt(_Count++),
                    CefV8PropertyAttribute.ReadOnly | CefV8PropertyAttribute.DontEnum | CefV8PropertyAttribute.DontDelete);
            }
        }

        private CefV8_JavascriptObject UpdateObject(CefV8_JavascriptObject ires)
        {
            BasicUpdateObject(ires.RawValue);
            return ires;
        }

        private CefV8_JavascriptObject UpdateObject(CefV8Value ires)
        {
            BasicUpdateObject(ires);
            return new CefV8_JavascriptObject(ires);
        }

        public IJavascriptObject CreateObject(string iCreationCode)
        {
            return _CefV8_WebView.Evaluate(() =>
            {
                IJavascriptObject res;

                _CefV8_WebView.Eval(iCreationCode, out res);

                return UpdateObject(res as CefV8_JavascriptObject);
            });
        }
    }
}
