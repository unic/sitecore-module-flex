Function Remove-XConnectInstance{
    param(
        $BobConfig
    )
    $config = Get-ScInstallConfig -BobConfig $BobConfig
    $xConnectConfig = $config.xConnect


    write-host "Uninstall Xconnect Config" -ForegroundColor Green 
    Write-Output $xConnectConfig
        
    $xcPath = Set-ScXconnectHosts -ConfigPath $xConnectConfig.Path 
    $xcPath = Set-ScInstallationRoot -ConfigPath $xcPath -InstallationRoot $config.xConnectinstallationRoot
    
    # Change it to use the custom json
    $xConnectConfig.Path =$xcPath

    if([System.Convert]::ToBoolean($BobConfig.InstallDatabases)){
        New-ScSifSqlAdmin -server $xConnectConfig.SqlServer -username $xConnectConfig.SqlAdminUser -password $xConnectConfig.SqlAdminPassword
        UnInstall-SitecoreConfiguration @xConnectConfig -Verbose *>&1 | Tee-Object Xconnect-Uninstall.log
    }else{
        # Configure Skip Tasks
        # Configure Webdeploy Databasses Tasks
        throw "Not Implemented Yet!"
    }

}