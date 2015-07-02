# Sitecore Flex
Flex is the acronym for __Fl__exible Forms __Ex__perience, the module to create web forms within a Sitecore MVC solution. Flex is also known as a friend of [Bob the Builder](http://en.wikipedia.org/wiki/Bob_the_Builder "Bob the Builder"), a yellow-black cherry picker with a Northern Irish accent.

## Build
[![Build](https://teamcity.unic.com/httpAuth/app/rest/builds/buildType:Sitecore_Frameworks_SitecoreModules_SitecoreModuleFlex_Build/statusIcon)](https://teamcity.unic.com/viewType.html?buildTypeId=Sitecore_Frameworks_SitecoreModules_SitecoreModuleFlex_Build)

## Project Installation
For using Flex in a project, please install the module over NuGet:

	> Install-Package Unic.Flex

After installing, you should follow the instructions in the [readme file](https://git.unic.com/projects/BUECS/repos/sitecore-module-flex/browse/src/Unic.Flex.Website/readme.txt), which will pop up after the installation in your Visual Studio.

## Development Installation (DRAFT)

### Steps
- Clone repo
- Add the path to the Sitecore playground installation in `Bob.config.user`
- Compile
- Manually copy a a IoC framework assembly (maybe this can be done with a sample project like in `Commex`)
- Enable client-side validation in `web.config`
- Install database
- Serialize items with Unicorn (config module and Flex, maybe the config module items can be added to a saple serialization folder like in `Commex`)

### Assembly redirects

Assembly redirect are needed in the `web.config` of the Sitecore installation. Please check the Flex `web.config` which one are needed and add them to the following node:

	<configuration>
	  <runtime>
	    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
		  ...
	    </assemblyBinding>
	  </runtime>
	</configuration>

### Client-side Validation
To enable client side validation, add the following nodes to the `<appSettings>` of the `web.config`:

    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />

### Entity Framework
Add the following config to `web.config`:

	<configSections>
		<section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
	</configSections>
	<entityFramework>
		<providers>
			<provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
		</providers>
	</entityFramework>

Install the database in the `data` directory and add the connection string in the `ConnectionStrings.config`:

	<add name="Flex" connectionString="Data Source=localhost;Initial Catalog=flex_data;Integrated Security=True" providerName="System.Data.SqlClient" />

### Items
Please manually copy the serialized items in the `serialization` folder and sync the database. This has do be done after each update of Flex.
