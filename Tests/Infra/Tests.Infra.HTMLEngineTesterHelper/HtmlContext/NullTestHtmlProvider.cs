﻿using Neutronium.Core.Infra;

namespace Tests.Infra.WebBrowserEngineTesterHelper.HtmlContext
{
    public class NullTestHtmlProvider : ITestHtmlProvider 
    {
        public string GetHtlmPath(TestContext context)
        {
            return $"{GetType().Assembly.GetPath()}\\Html\\Empty.html";
        }

        public string GetRelativeHtlmPath(TestContext context)
        {
            return "Html\\Empty.html";
        }
    }
}
