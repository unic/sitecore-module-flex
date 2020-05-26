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

    # Disable Identity According to the Config
    Invoke-DisableIdentityServer -BobConfig $BobConfig
}