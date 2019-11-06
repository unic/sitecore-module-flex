# Getting Sitecore Sources
Function Get-ScSitecorePackages {
    param (
    $config,
    [string]
    $nugetOutput
    )

    Invoke-ScDownloadPackage $config.SitecorePackage -version $config.SitecoreVersion -nugetOutput $nugetOutput
    Invoke-ScDownloadPackage $config.xConnectPackage -version $config.SitecoreVersion -nugetOutput $nugetOutput
}
