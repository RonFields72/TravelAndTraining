This template is provided for Southewestern Energy VS 2008 Web Application Projects.

config -- holds configuration files for web deployment

Roleproviders can be added by using the ICC web service. Please see http://swn-icc for details.

Change the Health Monitoring to suit your application needs. The production folder for all
health monitoring events is ITWEBAPP@swn.com

Release Notes: 
12/29/2008 -    Added Resources.resx and SiteMap.resx.  Globalized sitemap navigation and Application names.
09/22/2008 -	Added Health Monitoring.
				Added MailSettings.
				Fixed contentPlaceholder bug.
				Changed theme name to SWNTheme instead of SWN due to assembly naming conflicts.
				Added 3.5 AJAX configuration.
				Changed folder location to [VS2008 Standards].
				
Wish List:
* Replace configuration sections with configSource.
* Remove web deployment projects all together and use MSBuild to replace config sections by using the configSource
attribute.