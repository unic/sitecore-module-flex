# Flex

Flex is the acronym for __Fl__exible Forms __Ex__perience, the module to empower 
Sitecore Authors and Marketers to create amazing web form experiences.

## Table of Content

* Changelog
* Installation Instructions
* Setup Dev Environment

## Changelog

### 5.0

* Migration to GitHub public repository, AppVeyor and NuGet.org
* Update Unic.Configuration.* to 4.0.1
* Update Unic.Profiling.* to 3.0.0

### 4.3.3
* Fix Double Optin Save Plug To field setting to handle values from Reusable Field

### 4.3.2
* Add setting for commandTimeout

### 4.3.1
* Update Unic.Configuration.* to 3.1.0
* Update Unic.Bob.Scrambler.* to 2.2.0

### 4.3
* Add handling of fields tokens inside salutation token

### 4.2
* Fix GetFieldValue for reusable fields

### 4.1.9

* Fix export to Excel with IdentityServer
* Move deploy-once serialization files from app\flex to modules\flex\appDefault

### 4.1.8

* Ensure that when database data export is triggered from ribbon button:
    * the same DB that is used by the command (master) is used by controller 
    * the translations visible in Sitecore client are in the client's language.

### 4.1.7

* Get rid of the error when trying to export data for a form that is not existing in database set as `Unic.Feature.Flex.InitialDatabase` (web by default) but existis in master.
* Improve error handling of exporting data with Content Editor ribbon button by displaying visible error message to the user.

### 4.1.6

* Remove setting to toggle exception handling on loading form datasource. This should be handled by overriding the ContextService in the consuming project

### 4.1.5

* Add setting to toggle exception handling on loading form datasource

### 4.1.4

* Expose additional method in Send Email Async.

### 4.1.3

* Configure Flex.ServerOrigin for ContentDelivery
* Add Send Email Async to Save Plug Folder insert options

### 4.1.2

* Remove lock on 9.2 version of Sitecore libraries

### 4.1.1

*  Bugfix to display all occurrences of "Send Email Async" plug in: "Control Panel\Flex Dashboard\Async Plugs".

### 4.1

* Update of database schema is required for solutions upgrading to this version. Use SQL script: \databases\Async Mail Plug Upgrade Script\async-mail-plug-upgrade-script.sql. Please adjust database name in script content.
* Added new SendEmailAsync saveplug. This saveplug send emails by newly installed mail client "MailKit". To use SendEmailAsync plug, async execution needs to be allowed in Global Config item of your project. Before using SendEmailAsync, default Agent for plug execution needs to be disabled (Flex.Unic.config). Instead of them please enable 
responsible sitecore tasks (command and schedule) available in "/sitecore/system/Tasks/Commands/". It is required to configure interval on schedule item: "/sitecore/system/Tasks/Schedules/Flex/Async Plug Execution Schedule".
Properties "Site Name", "Log Activity", "Log Tag" needs to be configured on command item:"/sitecore/system/Tasks/Commands/Flex/Async Plug Execution Command". 

* 'Flex.Jobs.HonorServerOrigin', 'Flex.ServerOrigin' are a new settings to ensure plug executor, that Jobs/Tasks are processed by the correct instance.
* 'Flex.Mailing.SmtpClient.MailKit.SecureSocketOptions' is a new setting to setup SecureSocketOptions for MailKit Smpt client.
* Because of refactoring in common mailer classes, a re-test of mailer plugs in your solution is recommended. This applies to plugs such as 'DoubleOptin', 'SendMail' and any custom plug implementations relying on Mailer classes.
* Limitations: sitecore task "Async Plug Execution Command" executes plugs always in context of site configured as "Site Name".

### 4.0

* Updated Glassmapper to Version 5.5.28 (THIS NEEDS TO BE DONE IN YOUR SC SOLUTION ALSO! For a Version compatible with GlassMapper 4.x please use the support branch support/3.28.x)
* Fixed support for query strings and anchors on form cancel links

### 3.28

* Added field with conditional rule to SavePlugBase. Rules are checked before executing Save Plug - if true the Save Plug is executed.

### 3.27

* DateIntervalValidator added for DatePicker Field - validator checks if selected date is within specified interval (days, months, years in the future or in the past)
* Added project Unic.Flex.Implementation.Tests and tests for DateIntervalValidator

### 3.26

* Allow Form and Field items to setup autocomplete attribute. Hint: please adjust custom themes on project level by line which setup autocomplete attribute of form (example: Views\Modules\Flex\Default\Form.cshtml).


### 3.25

* Fix an issue when mutliple validators with the same html attributes on the same field caused a .net exception. With the new logic, the first added validator takes priority.

### 3.24

This versions brings back changes which were implemented in a separate branch during one of our projects. Those changes have now been properly ported to be backwards compatible and are opt-in with feature toggles:

* Form validation has been introduced, allowing validators to properly reference and check other fields values. For opting in, add the `EnableFormValidationAttribute` on class level of your custom validator. An example has been introduced with the new `DateCompareValidator`.
* `Flex.Plugs.AllowLoadPlugsOnNonHttpGet` is a new setting that allows load plugs to be executed upon non-http GET requests, such as `POST`. In order to keep backwards compatibility, a new `IgnoreHttpMethodExecutionFilter` property has been introduced on `LoadPlugBase`, which need to be set to `true` (default is `false`).
* `Flex.Urls.HonorTrailingSlash` is a new setting that allows Flex to honor trailing slashes in URLs, such as `http://flex.local/Multistep-Form/Step-2/`.

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

You must install an IoC container framework for Flex. Currently for Sitecore 9
you should install SimpleInjector, which can be installed over NuGet:

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
	
## Glassmapper Dependencies

Flex will use the `Glass.Mapper.Sc.92.Core` for it's internal functionality. One of the
pipelines has dependency on `Glass.Mapper.Sc.92.MVC` so you need to install and deploy 
also this MVC package in your solution.
If your `Web` database has different name, other that standard, then you need to adjust
``
the `Unic.Feature.Flex.InitialDatabase` setting.

## Setup for Development

For installing a Flex development instance, the following steps are required:

* Clone the repository
* Run the `BumpSc9.ps1` from Solution root to install Sitecore 9 and xconnect
* Open the solution in Visual Studio
* Restore all NuGet packages
* Restart Visual Studio
* Execute `Set-ScSerializationReference` in the Package Manager Console
* Execute `Install-WebConfig` in the Package Manager Console
* Build the Project
* Call http://flex-sc9.local/unicorn.aspx and run a full sync
* In Content Editor, invoke `Revert Database` from the `Developer` ribbon strip

The development instance is now available under http://flex-sc9.local/.