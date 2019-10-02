# Flex

Flex is the acronym for __Fl__exible Forms __Ex__perience, the module to empower 
Sitecore Authors and Marketers to create amazing web form experiences.

## Table of Content

* Changelog
* Installation Instructions
* Setup Dev Environment

## Changelog

### 3.23.3

* Fix from 3.23.2 reverted and approached differently to support TextOnlyField with dynamically overriden value. DefaultValue is not anymore saved to Value, instead TextValue has fallback to DefaultValue if Value is not set. Therefore the property to show TextOnlyField on views is now TextValue. Overrides are possible by changing Value.

### 3.23.2

* Fixes the issue in TextOnlyField, where after changing the language, the text was still shown in the original language

### 3.23.1

* Sitecore shared field mapping fix

### 3.23

* Title Level and Title Visually Hidden fields added to Form template
* Fix for Phone Validator issues when added to the field manually

### 3.22

* Fix for IsCascading field not available on all ListFields

### 3.21

* Fix for bug where data would be added to the wrong column

### 3.20

* BEKB-1787 Added possibility to set Text-Only Fields as describing the step's submit button, and therefore read in all screen reader modes

### 3.19

* Fixed the bug that caused backend validation error on nested dependent required fields that remain hidden

### 3.18

* Accessibility fix. Boolean attributes values lowercase only.

### 3.17

* BEKB-1812 Export form craches when it has a phone field - bug fixed,
phone validator regular expression is now stored in the sitecore config

### 3.16

* Added custom token {Salutation} to enable personal greeting based on the gender in Send Email Save Plug

### 3.15.1

* Phone validator fix, now works for all languages

### 3.15

* Added Dynamic Year And Today Range to Date Range Validator
* Phone Validator regex can be configured.

### 3.14

* Combined Readme.md from repository and Readme.txt from NuGet package
* Introduced an item update package in addition to the serialized item files
* Updated Unic.Bob.Scoop to latest version

### 3.13

* Fixed bug with nested dependent fields

### 3.12

* Fixed saving Lastname and Language to Contact

### 3.11

* Added detailed exception description when datasource is not a form

### 3.10

* Extended the checkbox list field and radio button list field with possibility 
to add a separate tooltip for each item

### 3.9

* Added possibility to redirect user after Double Optin confirmation
* Updated Unic.Bob.Scoop to V. 3.1.0

### 3.8.3

* Fixed Unicorn dependency to Modules.Configuration

### 3.8.2

* Fixed save and load actions "set field value to contact" and "load field value 
from contact" in non-english context

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

* Fixed broken `Label Link` functionality. `Label Links` could not be used due to a 
call to a removed overload of `GlassHtml.BeginRenderLink`, which resulted in exceptions 
during dynamic compilation. The newly added `alwaysRender` argument prevents the rendering 
of the element when the link is `null`.

### 3.5

* Update save-method in SaveToDatabaseService to Return ID of session entity after saving it to the database.

### 3.4

* Update `Glass.Mapper` to `4.5.0.4`
* Add call to `DependencyResolver.Finalise()` - this behaviour can be overriden in the 
`GlassConfig.FinaliseConfiguration` method. See http://glass.lu/Mapper/Sc/Releases for more information.

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

* Fix accidental ObjectConstruction pipeline abort by not completely following Glass.Mapper 4.3 
upgrade instruction

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

* Fixed standard values ID on Reusable Section, so that Show in Summary is 
ticked by default

### 2.5

* Added field "Show in Summary" for Reusable Sections

### 2.4

* Added label to a hidden field

## Installation Instructions

### IoC container

You must install an IoC container framework for Flex. Currently there are containers 
available for Ninject and SimpleInjector, which can be installed over NuGet:

  > Install-Package Unic.Flex.Ninject

or

  > Install-Package Unic.Flex.SimpleInjector

### Items

#### Unicorn/Rainbow Format

The serialized items for Flex (Unicorn/Rainbow based YAML files) are included in the 
Unic.Flex NuGet package and thus can be copied out of the 
`.\packages\Unic.Flex.[version]\serialization folder` to wherever your data folder for 
Unicorn is configured to deserialize the items back into the database.

The serialized items for the Unic.Configuration module (Unicorn/Rainbow based YAML files) 
are included in the Unic.Configuration NuGet package and thus can be copied out of the 
`.\packages\Unic.Configuration.[version]\serialization folder` to wherever your data folder for 
Unicorn is configured to deserialize the items back into the database.

#### Sitecore Default Update Package

An item udpate package with standard serialized items is also available in the Unic.Flex 
NuGet package under  `.\packages\Unic.Flex.[version]\serialization\Flex.update`.
You can install this package instead of deserializing the Unicorn/Rainbow Files.

Note: The Flex.update package also contains items to all dependent module items (e.g. Unic.Configuration)

### Install Database

Install the database in the "data" directory of Flex to your Sql server. Then put the 
following connection string into "ConnectionStrings.config":

    <add name="Flex" connectionString="Data Source=localhost;Initial Catalog=flex_data;Integrated Security=True" providerName="System.Data.SqlClient" />

### Assets

Flex depends on several assets. There are partial views available to include these. 
Please add the following lines of code in your layout view.

Before the closing `</head>`:

    @Html.Partial("~/Views/Modules/Flex/HeaderIncludes.cshtml")

Before the closing `</body>`:

    @Html.Partial("~/Views/Modules/Flex/BodyIncludes.cshtml")

### Client-side validation

To enable client side validation, add the following nodes to the `<appSettings>`-node 
of the `web.config`:

    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />

### Form Repository

You need to add a form repository in the content tree. The form repository must be created 
based on the following branch template:

    /sitecore/templates/Branches/Flex/Global/Repository Root

### Allow Rendering in Placeholder Settings

To add a form in the Page Editor, the the following rendering has to be enabled in your 
placeholder settings:

    /sitecore/layout/Renderings/Flex/Flex Form

## Setup for Development

For installing a Flex development instance, the following steps are required:

* Clone the repository
* Open the solution in Visual Studio
* Restore all NuGet packages
* Restart Visual Studio
* Execute `bump` in the Package Manager Console for the project *Unic.Flex.Integration.Website*

The development instance is now available under http://flex.local.