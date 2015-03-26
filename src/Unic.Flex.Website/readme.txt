  
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


Form Repository
---------------
You need to add a form repository in the content tree. The form repository must be created based
on the following branch template:

/sitecore/templates/Branches/Flex/Global/Repository Root


Install Database
----------------
Install the database in the "data" directory of Flex to your Sql server. Then put the following
connection string into "ConnectionStrings.config":

<add	name="Flex"
		connectionString="Data Source=localhost;Initial Catalog=flex_data;Integrated Security=True"
		providerName="System.Data.SqlClient" />




Client-side Validation
----------------------
To enable client side validation, add the following nodes to the <appSettings> of the web.config:

    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />



