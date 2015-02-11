***********************
POST INSTALLATION TASKS
***********************

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
	
Items
-----
Please manually copy the serialized items in the serialization folder and sync the database. This has do be done after each update of Flex.


