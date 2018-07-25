# Diplo Link Checker

**A Link Checker for Umbraco 7**

Diplo Link Checker is a dashboard add-on for Umbraco 7 that allows an editor to easily check their site for broken or problematic links.

## Features

* Able to check an entire site, or just a section or even a single page
* Completely asychnronous so can check multiple links simultaneously and provide real-time feedback
* Caches link status so only checks each unique link once (within a short period)
* Works for all types of links - external, internal, HTML, files, images and even CSS and JavaScript files
* Provides feedback on errors with help dialogue and an overview of HTTP status codes
* Quick edit facilty allows you to easily edit the page that contains the broken link directly within Umbraco
* Advanced options allow you to set the timeout period, toggle between viewing all checked links and only links that have problems
* You can whitelist HTTP codes and only report on those
* You can also configure it to ignore ports (if you are behind a reverse proxy, for example)

# Install

Package available at https://our.umbraco.org/projects/backoffice-extensions/diplo-link-checker

NuGet version at https://www.nuget.org/packages/Diplo.LinkChecker/

`PM> Install-Package Diplo.LinkChecker`
