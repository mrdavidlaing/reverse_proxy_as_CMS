## Overview

[![Screencast of this in action](https://f.cloud.github.com/assets/227505/199198/7fe1e402-8076-11e2-8314-b8ebb2c84084.png)](https://cityindex.viewscreencasts.com/6da7bf63f4f524edb5a5388de3a5afbb)

([open screencast in separate window](https://cityindex.viewscreencasts.com/6da7bf63f4f524edb5a5388de3a5afbb))

## Pre-requisits:

1.  Windows 7 or Windows Server 2008 or later
1.  IIS 7.5 with:
  1. Url Rewrite 2.0 - http://www.iis.net/downloads/microsoft/url-rewrite
  1. Application Request Routing 2.5 - http://www.iis.net/downloads/microsoft/application-request-routing 
  1. IIS: Tracing  
  1. IIS: HTTP Logging
  1. IIS: Default document
  1. IIS: Static Content Compression
  
These are easiest to install using the Web Platform Installer - http://www.microsoft.com/web/downloads/platform.aspx

## Setup

1.  Checkout this repository to below the `c:\inetpub` folder.  (This location is highly recommended to avoid IIS file permission errors)
1.  Setup a fake domain name pointing to localhost by opening Notepad as Administrator and editing `c:\Windows\System32\drivers\etc\HOSTS` and adding the line

```
127.0.0.1		www.myblog.com
```
1.  Edit `C:\windows\system32\inetsrv\config\applicationHost.config` and add the element

```
<rewrite>
...
 <allowedServerVariables>
    <add name="HTTP_ACCEPT_ENCODING" />
  </allowedServerVariables>
...

```
1.  Restart IIS after these changes - from an Administrator command prompt run `iisreset`
1.  Create a new IIS 7.5 site pointing to the `c:\inetpub\reverse_proxy_as_CMS\IIS7\www.myblog.com` folder, with host header set to `www.myblog.com`.  Leave everything else at the defaults.
1.  Select the server node on the left, open the Application Request Routing feature, Proxy and check "Enable proxy"
1.  Open a (local) browser, and browse to `http://labs.cityindex.com/ciapi-client-libraries/`.  
1.  Open a (local) browser, and browse to `http://www.myblog.com`.  You should see content being proxied from http://labs.cityindex.com/ciapi-client-libraries/
