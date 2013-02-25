---
layout: "page"
title: "IIS7 using ARR and UrlRewrite"
intro: "The Url Rewrite 2 and Application Resource Routing features of IIS7+ make it a credible reverse proxy."
---

The only area in which it falls short is automatic failover to cached content when the primary source fails.
However, the Url Rewrite 2 extension model make it straightforward to extend the functionality and add failover.

[This screencast](https://cityindex.viewscreencasts.com/2a77b29eeff04af19f2fa02c876758dc) and the guide below shows how this site can be served via IIS7.


## How it works

This site is made up of content from the following locations:

1. Static content (generated using [Jekyll](http://jekyllrb.com/)); published at [http://mrdavidlaing.github.com/reverse_proxy_as_CMS/](http://mrdavidlaing.github.com/reverse_proxy_as_CMS/)
1. A FAQ wiki (built on GitHub Wiki); published at [https://github.com/mrdavidlaing/reverse_proxy_as_CMS/wiki/FAQ](https://github.com/mrdavidlaing/reverse_proxy_as_CMS/wiki/FAQ)
1. Whitepaper PDFs; published on Google Drive at [https://googledrive.com/host/0B0iLBOBULteOZjNlYTVrVWljR2M/](https://googledrive.com/host/0B0iLBOBULteOZjNlYTVrVWljR2M/)

### The master site
This content is all pulled together under a single url [http://www.reverse-proxy-as-cms.info](http://www.reverse-proxy-as-cms.info) using [this IIS config](https://github.com/mrdavidlaing/reverse_proxy_as_CMS/blob/master/IIS7/www.reverse-proxy-as-cms.info/Web.config), which basically pulls content from each of the backend locations, and updates the HTML to contain the correct internal Urls and inject some branding.

### Static content
The static content is generated using [Jekyll](http://jekyllrb.com/); a simple HTML generation tool that converts markdown source to HTML and wraps it in a layout.  Importantly:

* Jekyll has very little functionality, and thus doesn't inject any random HTML fragments.  You are essentially writing pure HTML, CSS and Javascript, without any CMS injected items interfering.
* The tool runs once to generate a set of static HTML files for deployment. These have no server runtime dependancies; and are subsequently extremely fast for any web server to serve.

The content is pulled into the master site using the following Url Rewrite rule:

{% highlight xml%}
  <rule name="Route the http requests for /" stopProcessing="true">
    <match url="^(.*)" />
    <conditions logicalGrouping="MatchAll" trackAllCaptures="false" />
    <action type="Rewrite" url="http://mrdavidlaing.github.com/reverse_proxy_as_CMS/{R:1}" logRewrittenUrl="true" />
    <serverVariables>
      <set name="HTTP_ACCEPT_ENCODING" value="" />
    </serverVariables>
  </rule>
{% endhighlight %}

This rule must be at the end of the rules list, since it essentially traps all unmapped urls and fetches content for them from `http://mrdavidlaing.github.com/reverse_proxy_as_CMS/`

### FAQ wiki
The FAQ page is implemented as a GitHub Wiki; outsourcing the hassle of hosting this complext piece of software, and ensuring that people wanting to make edits don't have to register yet another account (they just use their existing GitHub account)

The content is pulled into the master site using the following Url Rewrite rule:

{% highlight xml%}
<rule name="Route the http requests for /wiki to GitHub wiki" stopProcessing="true">
  <match url="^/?wiki(.*)" />
  <conditions logicalGrouping="MatchAll" trackAllCaptures="false" />
  <action type="Rewrite" url="https://github.com/mrdavidlaing/reverse_proxy_as_CMS/wiki{R:1}" logRewrittenUrl="true" />
  <serverVariables>
  	<set name="HTTP_ACCEPT_ENCODING" value="" />
  </serverVariables>
</rule>
{% endhighlight %}

The HTML that is returned lacks any of our branding or navigation, so this is injected using the following set of Url Rewrite outbound rules:

{% highlight xml%}
<rule name="Replace Github Wiki nav with our nav" preCondition="IsHTML" enabled="true">
  <match pattern="&lt;div class=&quot;header(\s|.)*&lt;div class=&quot;tabnav&quot;&gt;" />
  <action type="Rewrite" value="{InjectHTMLUrlRewriteProvider:http://www.reverse-proxy-as-cms.info/navbar_only.html} &lt;div class=&quot;tabnav&quot;&gt;" /> 
</rule>
<rule name="Fix Github Wiki wrapper hack" preCondition="IsHTML" enabled="true">
  <match pattern="&lt;div id=&quot;wrapper&quot;" />
  <action type="Rewrite" value="&lt;div id=&quot;wrapper&quot; style=&quot;min-height:auto; margin-bottom:auto;&quot;" /> 
</rule>
<rule name="Replace Github Wiki footer" preCondition="IsHTML" enabled="true">
  <match pattern="&lt;!-- footer --&gt;(\s|.)*&lt;!-- /.#footer --&gt;" />
  <action type="Rewrite" value="&lt;!-- footer removed --&gt;" /> 
</rule>
{% endhighlight %}

Some items worth noting:

* These rules make use of a custom Url Rewrite Provider: `InjectHTMLUrlRewriteProvider` that enable injecting content pulled from an external HTML file - in this case `http://www.reverse-proxy-as-cms.info/navbar_only.html`
* The GitHub wiki markup contains some content that we don't want; thus we have 2 rules that remove HTML content from the proxied page.

### Whitepaper PDFs
Google Drive is a super simple way to publish files; and is used in this site to publish a set of PDF files.

The content is pulled into the master site using the following Url Rewrite rule:

{% highlight xml%}
<rule name="Route the http requests for /whitepapers" stopProcessing="true">
  <match url="^/?whitepapers/(.*)" />
  <conditions logicalGrouping="MatchAll" trackAllCaptures="false" />
  <action type="Rewrite" url="https://googledrive.com/host/0B0iLBOBULteOZjNlYTVrVWljR2M/{R:1}" logRewrittenUrl="true" />
  <serverVariables>
  	<set name="HTTP_ACCEPT_ENCODING" value="" />
  </serverVariables>
</rule>
{% endhighlight %}


## Running the example
The following instructions explain how to run the example:

### Pre-requisits:

1.  Windows 7 or Windows Server 2008 or later
1.  IIS 7.5 with:
  1. Url Rewrite 2.0 - http://www.iis.net/downloads/microsoft/url-rewrite
  1. Application Request Routing 2.5 - http://www.iis.net/downloads/microsoft/application-request-routing 
  1. IIS: Tracing
  1. IIS: HTTP Logging
  1. IIS: Default document
  1. IIS: Static Content Compression
  
Adding the IIS7 extensions mentioned above is easiest using the [Web Platform Installer](http://www.microsoft.com/web/downloads/platform.aspx)

### Setup

1.  Checkout the [master branch of this repository](https://github.com/mrdavidlaing/reverse_proxy_as_CMS) to below the `c:\inetpub` folder.  (This location is highly recommended to avoid IIS file permission errors)
1.  Setup a fake domain name pointing to localhost by opening Notepad as Administrator,  editing `c:\Windows\System32\drivers\etc\HOSTS` and adding the line

{% highlight text%}
127.0.0.1       www.reverse-proxy-as-cms.info
{% endhighlight %}

1.  Edit `C:\windows\system32\inetsrv\config\applicationHost.config` and add the element

{% highlight xml%}
<rewrite>
...
  <allowedServerVariables>
    <add name="HTTP_ACCEPT_ENCODING" />
  </allowedServerVariables>
...
{% endhighlight %}

1.  Create a new IIS site pointing to the `c:\inetpub\reverse_proxy_as_CMS\IIS7\www.reverse-proxy-as-cms.info` folder, processing requests with host header `www.reverse-proxy-as-cms.info` sent to port 80.  Leave everything else at the defaults.
1.  Select the server node on the left, open the Application Request Routing feature, Proxy and check "Enable proxy"
1.  Browse to `http://www.reverse-proxy-as-cms.info` using a browser on the same machine whoes HOST file you edited.  You will see content being proxied from the backend content sources.


