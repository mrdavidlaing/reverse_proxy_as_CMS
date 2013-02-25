---
layout: "page"
title: "IIS7 using ARR and UrlRewrite"
intro: "The Url Rewrite 2 and Application Resource Routing features of IIS7+ make it credible reverse proxy."
---

The only area in which it falls short is automatic failover to cached content when the primary source fails.
However, the Url Rewrite 2 extension model make it straightforward to extend the functionality and add failover.

The guide below shows how this site can be served via IIS7.

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

```
127.0.0.1       www.reverse-proxy-as-cms.info
```

1.  Edit `C:\windows\system32\inetsrv\config\applicationHost.config` and add the element

{% highlight %}
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

## How it works


