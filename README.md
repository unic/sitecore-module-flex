# Sitecore Flex
Flex is the acronym for __Fl__exible Forms __Ex__perience, the module to create web forms within a Sitecore MVC solution. Flex is also known as a friend of [Bob the Builder](http://en.wikipedia.org/wiki/Bob_the_Builder "Bob the Builder"), a yellow-black cherry picker with a Northern Irish accent.

## Build
[![Build](https://teamcity.unic.com/httpAuth/app/rest/builds/buildType:Sitecore_Frameworks_SitecoreModules_SitecoreModuleFlex_Build/statusIcon)](https://teamcity.unic.com/viewType.html?buildTypeId=Sitecore_Frameworks_SitecoreModules_SitecoreModuleFlex_Build)

## Project Installation
For using Flex in a project, please install the module over NuGet:

	> Install-Package Unic.Flex

After installing, you should follow the instructions in the [readme file](https://git.unic.com/projects/BUECS/repos/sitecore-module-flex/browse/src/Unic.Flex.Website/readme.txt), which will pop up after the installation in your Visual Studio.

## Development Installation
You can simply install a Flex development instance with executing `bump` in the Package Manager Console. Be sure to execute the command with selected `Unic.Flex.Integration.Website` project.

	> bump

Your installation will then be available under http://flex.local. After `bump` is finished you need to serialize all the items needed. You can do this in three simple steps:

- Go to http://flex.local/unicorn.aspx and click on `Sync Integration Configuration Now`
- Go to http://flex.local/unicorn.aspx and click on `Sync Application Configuration Now`
- Login to the Content Editor, activate the `Developer` ribbon and click on `Update Database`
