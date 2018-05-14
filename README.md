# Sitecore Flex
Flex is the acronym for __Fl__exible Forms __Ex__perience, the module to create web forms within a Sitecore MVC solution. Flex is also known as a friend of [Bob the Builder](http://en.wikipedia.org/wiki/Bob_the_Builder "Bob the Builder"), a yellow-black cherry picker with a Northern Irish accent.

## Build
[![Build](https://teamcity.unic.com/httpAuth/app/rest/builds/buildType:Sitecore_Frameworks_SitecoreModules_SitecoreModuleFlex_Build/statusIcon)](https://teamcity.unic.com/viewType.html?buildTypeId=Sitecore_Frameworks_SitecoreModules_SitecoreModuleFlex_Build)

## Changelog

### 3.8.1
* Added handling for custom field types when setting their value

### 3.8
* Added new SavePlug for Double Optin

### 3.7
* Added new action to the saverules for saving the current language to a contact 
* Added new action to the saverules for identifying the contact

### 3.6
* Added UrlReferrer Protection to QueryStringLoader Load Plug.

### 3.5.1
* Fixed broken `Label Link` functionality. `Label Links` could not be used due to a call to a removed overload of `GlassHtml.BeginRenderLink`, which resulted in exceptions during dynamic compilation. The newly added `alwaysRender` argument prevents the rendering of the element when the link is `null`.

### 3.5
* Update save-method in SaveToDatabaseService to Return ID of session entity after saving it to the database.

### 3.4

- Update `Glass.Mapper` to `4.5.0.4`
- Add call to `DependencyResolver.Finalise()` - this behaviour can be overriden in the `GlassConfig.FinaliseConfiguration` method. See http://glass.lu/Mapper/Sc/Releases for more information

### 3.3

* Added new Load and Save Plugs `Execute Rule`
* Added multiple actions to prefill a form with values from the xDB profile
* Added multiple actions to save values from the form to the xDB profile
* SCMFLEX-46 fixed bug with asynchrous exeuction of async save plugs

### 3.2
* Update Castle.Core to 4.2.1
* Update Glass.Mapper to 4.4.0.199
* Rename model parameter in FlexController HttpPost Action to prevent name clashes with Nitro.net
* Replace ForEach extension call to use Glass.Mapper's implementation, as it has been dropped from Castle

### 3.1
* Improve performance of the form database export
* Improve performance of the form database export button in content editor (active or disabled check)
* Improve performance of the insertion of form data into the database
* Switch to Roslyn compiler to support C# 6 features

### 3.0.1
* Fix accidental ObjectConstruction pipeline abort by not completely following Glass.Mapper 4.3 upgrade instruction

### 3.0
* Update Glass.Mapper to 4.3.4.197

### 2.9 
* Added aria-invalid param when validation failed

### 2.8 
* Fix encoding special characters in plain text email

### 2.7 
* Fix Ajax Validators with Virutal Folders (SCMFLEX-51)

### 2.6.1
* fix dependent field display for multilist checkboxes (SCMFLEX-52)

### 2.6
* Added Show in Summary functionality for Reusable Fields

### 2.5.1
* Fixed standard values ID on Reusable Section, so that Show in Summary is ticked by default

### 2.5 
* Added field "Show in Summary" for Reusable Sections

### 2.4
* Added label to a hidden field

## Project Installation
For using Flex in a project, please install the module over NuGet:

	> Install-Package Unic.Flex

After installing, you should follow the instructions in the [readme file](https://git.unic.com/projects/BUECS/repos/sitecore-module-flex/browse/src/Unic.Flex.Website/readme.txt), which will pop up after the installation in your Visual Studio.

## Development Installation
For installing a Flex development instance, the following steps are required:

- Clone the repository
- Open the solution in Visual Studio
- Restore all NuGet packages
- Restart Visual Studio
- Execute `bump` in the Package Manager Console for the project *Unic.Flex.Integration.Website*

The development instance is now available under http://flex.local.
