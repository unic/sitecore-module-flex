  
  ______   _        ______  __   __
 |  ____| | |      |  ____| \ \ / /
 | |__    | |      | |__     \ V / 
 |  __|   | |      |  __|     > <  
 | |      | |____  | |____   / . \ 
 |_|      |______| |______| /_/ \_\
                                   
                                   

Installation Prerequisites
--------------------------
Flex depends on the Unic configuration module, which must be installed on the Sitecore instance
before running Flex. It can be installed over NuGet:

    > Install-Package Unic.Configuration.Sitecore7


IoC container
-------------
You must install an IoC container framework for Flex. Currently there are containers available
for Ninject and SimpleInjector, which can be installed over NuGet:

    > Install-Package Unic.Flex.Ninect

or

    > Install-Package Unic.Flex.SimpleInjector


Items
-----
The serialized items were copied to the "serialization" folder in the root directory. Please copy
these items into your data directory (or adapt the serialization folder path) and update your
database.


Install Database
----------------
Install the database in the "data" directory of Flex to your Sql server. Then put the following
connection string into "ConnectionStrings.config":

    <add name="Flex" connectionString="Data Source=localhost;Initial Catalog=flex_data;Integrated Security=True" providerName="System.Data.SqlClient" />


Assets
------
Flex depends on several assets. There are partial views available to include these. Please add
the following lines of code in your layout view.

Before the closing </head>:

    @Html.Partial("~/Views/Modules/Flex/HeaderIncludes.cshtml")

Before the closing </body>:

    @Html.Partial("~/Views/Modules/Flex/BodyIncludes.cshtml")


Client-side validation
----------------------
To enable client side validation, add the following nodes to the <appSettings>-node of the "web.config":

    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />


Form Repository
---------------
You need to add a form repository in the content tree. The form repository must be created based
on the following branch template:

    /sitecore/templates/Branches/Flex/Global/Repository Root


Allow Rendering in Placeholder Settings
---------------------------------------
To add a form in the Page Editor, the the following rendering has to be enabled in your placeholder
settings:

	/sitecore/layout/Renderings/Flex/Flex Form