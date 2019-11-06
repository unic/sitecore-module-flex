Function New-SitecoreInstance{
    param(
        $BobConfig
    )
    $config = Get-ScInstallConfig -BobConfig $BobConfig
    $SitecoreConfig = $config.Sitecore


    write-host "Sitecore Config" -ForegroundColor Green 
        
    $scPath = Set-ScInstallationRoot -ConfigPath $SitecoreConfig.Path  -InstallationRoot $config.installationRoot
    
    # Change it to use the custom json
    $SitecoreConfig.Path =$scPath
    
    if([System.Convert]::ToBoolean($BobConfig.InstallDatabases)){
        New-ScSifSqlAdmin -server $SitecoreConfig.SqlServer -username $SitecoreConfig.SqlAdminUser -password $SitecoreConfig.SqlAdminPassword
        Write-Output $SitecoreConfig
        Install-SitecoreConfiguration @SitecoreConfig -Verbose *>&1 | Tee-Object Sitecore.log
    }else{
        # Configure Skip Tasks
        # Configure Webdeploy Databasses Tasks
        throw "Not Implemented Yet!"
    }
    # Add Extra Bindings
    Add-IISBindings -SiteName $SitecoreConfig.SiteName -bindings $config.SitecoreBindings

    
    if(([System.Convert]::ToBoolean($BobConfig.InstallDatabases))){    
        Write-Host "Disabling Identity Server"
        $include = join-path $config.installationRoot "$($config.sitecore.SiteName)\App_Config\Include"
        Copy-Item -Path "$include\Examples\Sitecore.Owin.Authentication.Disabler.config.example" -Destination "$include\Sitecore.Owin.Authentication.Disabler.config"
        Copy-Item -Path "$include\Examples\Sitecore.Owin.Authentication.IdentityServer.Disabler.config.example" -Destination "$include\Sitecore.Owin.Authentication.IdentityServer.Disabler.config"

    }
}