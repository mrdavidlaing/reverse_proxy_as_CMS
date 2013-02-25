---
layout: "page"
title: "IIS7 using ARR and UrlRewrite"
intro: "The Url Rewrite 2 and Application Resource Routing features of IIS7+ make it a credible reverse proxy."
---

The only area in which it falls short is automatic failover to cached content when the primary source fails.
However, the Url Rewrite 2 extension model make it straightforward to extend the functionality and add failover.

The guide below shows how this site can be served via IIS7.

## How it works

This site is made up of content from the following locations:

1. Static content (generated using [Jekyll](http://jekyllrb.com/)); published at [http://mrdavidlaing.github.com/reverse_proxy_as_CMS/](http://mrdavidlaing.github.com/reverse_proxy_as_CMS/)
1. A FAQ wiki (built on GitHub Wiki); published at [https://github.com/mrdavidlaing/reverse_proxy_as_CMS/wiki/FAQ](https://github.com/mrdavidlaing/reverse_proxy_as_CMS/wiki/FAQ)
1. Whitepaper PDFs; published on Google Drive at [https://googledrive.com/host/0B0iLBOBULteOZjNlYTVrVWljR2M/](https://googledrive.com/host/0B0iLBOBULteOZjNlYTVrVWljR2M/)
1. A single page HTML5 WebApp (built using AngularJs), published at [http://angularjs_travis-ci_browserstack.cloudfoundry.com/](http://angularjs_travis-ci_browserstack.cloudfoundry.com/)

This content is all pulled together under a single url [http://www.reverse-proxy-as-cms.info](http://www.reverse-proxy-as-cms.info) using [this IIS config](https://github.com/mrdavidlaing/reverse_proxy_as_CMS/blob/master/IIS7/www.reverse-proxy-as-cms.info/Web.config), which basically pulls content from each of the backend locations, and updates the HTML to contain the correct internal Urls and inject some branding.


## Pre-requisits:

1.  Windows 7 or Windows Server 2008 or later
1.  IIS 7.5 with:
  1. Url Rewrite 2.0 - http://www.iis.net/downloads/microsoft/url-rewrite
  1. Application Request Routing 2.5 - http://www.iis.net/downloads/microsoft/application-request-routing 
  1. IIS: Tracing
  1. IIS: HTTP Logging
  1. IIS: Default document
  1. IIS: Static Content Compression
  
Adding the IIS7 extensions mentioned above is easiest using the [Web Platform Installer](http://www.microsoft.com/web/downloads/platform.aspx)

## Setup

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


