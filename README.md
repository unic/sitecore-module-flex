# Sitecore Flex
Flex is the acronym for __Fl__exible Forms __Ex__perience, the module to create web forms within a Sitecore MVC solution. Flex is also known as a friend of [Bob the Builder](http://en.wikipedia.org/wiki/Bob_the_Builder "Bob the Builder"), a yellow-black cherry picker with a Northern Irish accent.

## Installation
### Assembly redirects
Assembly redirect are needed in the `web.config` of the Sitecore installation:

	<configuration>
	  <runtime>
	    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
		  <dependentAssembly>
	        <assemblyIdentity name="Ninject" publicKeyToken="c7192dc5380945e7" culture="neutral" />
	        <bindingRedirect oldVersion="0.0.0.0-3.2.0.0" newVersion="3.2.0.0" />
	      </dependentAssembly>
		  <dependentAssembly>
			<assemblyIdentity name="Castle.Core" publicKeyToken="407dd0808d44fbdc" culture="neutral" />
			<bindingRedirect oldVersion="0.0.0.0-3.3.0.0" newVersion="3.3.0.0" />
		  </dependentAssembly>
		  <dependentAssembly>
			<assemblyIdentity name="Castle.Windsor" publicKeyToken="407dd0808d44fbdc" culture="neutral" />
			<bindingRedirect oldVersion="0.0.0.0-3.3.0.0" newVersion="3.3.0.0" />
		  </dependentAssembly>
	    </assemblyBinding>
	  </runtime>
	</configuration>

### Client-side Validation
To enable client side validation, add the following nodes to the `<appSettings>` of the `web.config`:

    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
