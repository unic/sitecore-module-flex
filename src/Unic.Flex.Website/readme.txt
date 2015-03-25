  
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



Assembly redirects
------------------
Assembly redirect are needed in the web.config of the Sitecore installation. Please check the Flex web.config which one are needed and add them to the following node:

	<configuration>
	  <runtime>
	    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
		  ...
	    </assemblyBinding>
	  </runtime>
	</configuration>

Client-side Validation
----------------------
To enable client side validation, add the following nodes to the <appSettings> of the web.config:

    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />

Entity Framework
----------------
Add the following config to web.config:

	<configSections>
		<section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
	</configSections>
	<entityFramework>
		<providers>
			<provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
		</providers>
	</entityFramework>

Install the database in the data directory and add the connection string in the ConnectionStrings.config:

	<add name="Flex" connectionString="Data Source=localhost;Initial Catalog=flex_data;Integrated Security=True" providerName="System.Data.SqlClient" />
	



