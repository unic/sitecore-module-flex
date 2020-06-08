Function Install-SIF{
    param(
        $SitecoreFundamentalRequiredVersion,
        $SIFRequiredVersion,
        $config,
        $nugetOutput
    )
    # Check / Install Sitecore Fundamental
    $sfVer = [version]::Parse($SitecoreFundamentalRequiredVersion)
    $x=get-module -ListAvailable | Where-Object {$_.Name -like 'SitecoreFundamentals' -and $_.Version -eq $sfVer} | select -First 1
    if($x){
        Write-Output "Found SitecoreFundamentals!"        
    }else{
        $sifm = Join-Path (Join-Path $nugetOutput "$($config.SitecoreFundamentals).$($config.SitecoreFundamentalsVersion)") "SitecoreFundamentals"
        Copy-Item $sifm -Destination "$Env:Programfiles\WindowsPowerShell\Modules\SitecoreFundamentals\$SitecoreFundamentalRequiredVersion" -Recurse
    }
    Import-Module SitecoreFundamentals -RequiredVersion $SitecoreFundamentalRequiredVersion
    
    # Check / Install SIF
    $sifVer = [version]::Parse($SIFRequiredVersion)
    $x=get-module -ListAvailable | Where-Object {$_.Name -like 'SitecoreInstallFramework' -and $_.Version -eq $sifVer} | select -First 1
    if($x){
        Write-Output "Found SitecoreInstallFramework!"
    }else{
        $sifm = Join-Path (Join-Path $nugetOutput "$($config.SIF).$($config.SIFVersion)") "SitecoreInstallFramework"
        Copy-Item $sifm -Destination "$Env:Programfiles\WindowsPowerShell\Modules\SitecoreInstallFramework\$SIFRequiredVersion" -Recurse
    }

    Import-Module SitecoreInstallFramework -RequiredVersion $SIFRequiredVersion
}

