## How to glue multiple content sources into one site using a reverse proxy

### Why?!

The days of the one-size-fits all CMS are over.  Like any mature market, content management has advanced to a stage where there is no single tool that can fulfil all the requirements of a modern content publisher well.

Specialisation like this is hardly unsurprising; it happens in all markets as they mature.  There was a time when a single insurance provider could satisfy all your insurance needs.  No longer; its likely that your insurance needs are covered by a collection of specialists, assembled to best fit your specific needs.  Housing insurance from the cheapest provider.  Motor insurance from a vintage car specialist.  Holiday insurance from a provider specializing in winter sports cover.

The same is true of content publishing; as it has become increasingly specialised:

1.  Brochureware / static pages - content and design that is highly individual and polished, and thus changes infrequently.
1.  Blog / news content - where new content is published frequently, and is thus  published using a standardised template to speed up delivery.
1.  Downloadable files - the sharing of offline content via web rather than email attachment.
1.  Forms and feedback - online forms and signup processes to gather information in a central place
1.  Forums and community - places for your the community reading your content to interact
1.  Push updates - Email newsletters, email discussion groups, realtime short messages.
1.  Video publishing
1.  WebApps - specialised single page applications & widgets

Along with technical specializations:

1.  Delivery accelleration
1.  Analytics
1.  A/B testing
1.  Fault tolerance
1.  Site search
1.  Access control

My experience with the one-size-fits all CMS that claim to address all of these needs is that they do perhaps one well, and the rest poorly.

Their implementation follows the disappointment cycle - from high hopes at the beginning of the project where the silver bullet marketing literature is believed, to growing disallusionment as the the system is found to be lacking in the specific area of content publishing that you care about most.  After enough frustration, the old system is thrown out for the shiny new system that promises to solve all your content problems.. 

The big lesson here, I believe, is that a one size fits all approach to content management is fundamentally flawed. 
 
Its time to select a set of specialist tools that best solve your publishing needs.  As those needs change, so should the collection of tools you use.

Its time to embracing the unix philospohy of "small single purpose tools" glued together to into a greater whole.

The glue? A caching reverse proxy web server. 

### HOWTO:

The proxy server manages your master URL - eg www.cityindex.co.uk - and 
is configured to pull content from a variety of backed sources.  If one of those back ends is down, it just serves the most recent content it has. 
 
This frees the content creators to use the best tool for their content. Static HTML files for static pages.  Dropbox for PDFs.  Wordpress for blogs.  ASP.NET apps for functionality. etc. etc. 
 