## Pre-requisits:

1.  Windows 7 or Windows Server 2008 or later
1.  IIS 7.5 with:
  1. Url Rewrite 2.0 - http://www.iis.net/downloads/microsoft/url-rewrite
  1. Application Request Routing 2.5 - http://www.iis.net/downloads/microsoft/application-request-routing 
  1. IIS: Tracing - 
  
These are easiest to install using the Web Platform Installer - http://www.microsoft.com/web/downloads/platform.aspx

## Setup

1.  Setup a fake domain name pointing to localhost by opening Notepad as Administrator and editing `c:\Windows\System32\drivers\etc\HOSTS` and adding the line

```
127.0.0.1		www.mycompany.co.uk
```

1.  Create a new IIS 7.5 site pointing to the `www.mycompany.co.uk/` folder, with host header set to `www.mycompany.co.uk/`.  Leave everything else at the defaults.  Make sure you "Test settings" to ensure that IIS has the required permissions to read the website files.
1.  Select the server node on the left, open the Application Request Routing feature, Proxy and check "Enable proxy"


