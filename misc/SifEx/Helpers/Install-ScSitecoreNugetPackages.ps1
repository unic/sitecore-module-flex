# Getting Sitecore Sources
Function Install-ScSitecoreNugetPackages {
    param (
    $config,
    [string]
    $nugetOutput,
    [string]
    $nuget
    )
    $sitecoreVersion = Get-ScFallbackConfig $config.SitecoreXp0WdpVersion $config.SitecoreVersion

    Invoke-ScDownloadPackage $config.SitecorePackage -version $SitecoreVersion -nugetOutput $nugetOutput -nuget $nuget
    Invoke-ScDownloadPackage $config.xConnectPackage -version $SitecoreVersion -nugetOutput $nugetOutput -nuget $nuget
}
