using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Web.Iis.Rewrite;

namespace InjectHTMLUrlRewriteProvider
{
    public class InjectHTMLUrlRewriteProvider: IRewriteProvider, IProviderDescriptor
    {
        private string _replacePattern;
        public void Initialize(IDictionary<string, string> settings, IRewriteContext rewriteContext)
        {
            if (!settings.TryGetValue("replacePattern", out _replacePattern) || string.IsNullOrEmpty(_replacePattern))
                throw new ArgumentException("replacePattern provider setting is required and cannot be empty");
        }

        
        public string Rewrite(string url)
        {
            var webClient = new WebClient();
            return webClient.DownloadString(url);
        }

        public IEnumerable<SettingDescriptor> GetSettings()
        {
            yield return new SettingDescriptor("replacePattern", "pattern to replace");
        }
    }
}
