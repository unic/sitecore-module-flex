Function Invoke-DisableIdentityServer {
    param(
        $BobConfig
    )
    $config = Get-ScInstallConfig -BobConfig $BobConfig

    if(-not ([System.Convert]::ToBoolean($BobConfig.InstallIdentity))){    
        Write-Host "Disabling Identity Server"
        $include = join-path $config.installationRoot "$($config.sitecore.SiteName)\App_Config\Include"
        Copy-Item -Path "$include\Examples\Sitecore.Owin.Authentication.Disabler.config.example" -Destination "$include\Sitecore.Owin.Authentication.Disabler.config"
        Copy-Item -Path "$include\Examples\Sitecore.Owin.Authentication.IdentityServer.Disabler.config.example" -Destination "$include\Sitecore.Owin.Authentication.IdentityServer.Disabler.config"
    }
}