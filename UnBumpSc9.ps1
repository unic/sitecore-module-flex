Function Invoke-UnBumpSc9{
    param(
        [Parameter(Mandatory=$true)]
        [string]
        $PackagesRoot
    )
    If(![Environment]::Is64BitProcess) 
    {
        throw "Please run 64-bit PowerShell"        
    }

    $scoopPath = Get-ChildItem $packageRoot\* -Directory | Where-Object {$_.BaseName -like "Unic.Bob.Scoop*"} | Select-Object -Last 1
    if(-not(Test-Path $scoopPath)){
        throw "Scoop is not installed!"
    }
    Import-Module (Join-Path $scoopPath "tools\packages\Unic.Bob.Scratch\tools\packages\Unic.Bob.Wendy\tools\Wendy\Wendy.psm1") -Force
    $nuget  = Join-Path $scoopPath "tools\packages\Unic.Bob.Skip\tools\packages\NuGet.CommandLine\tools\NuGet.exe"
    # Get BOB Configuration
    $config = Get-ScProjectConfig

    # Check SIF Installation
    $SIFInstallation = Invoke-ScCheckSIFInstallation -SIFRequiredVersion $config.SIFRequiredVersion -SitecoreFundamentalRequiredVersion $config.SitecoreFundamentalsRequiredVersion

    if(-not $SIFInstallation){
        Invoke-ScDownloadPackage $config.SitecoreFundamentals -version $config.SitecoreFundamentalsVersion -nugetOutput $PackagesRoot -nuget $nuget
        Invoke-ScDownloadPackage $config.SIF -version $config.SIFVersion -nugetOutput $PackagesRoot -nuget $nuget
        
        Write-Output "Installing SIF"
        Install-Sif -SIFRequiredVersion $config.SIFRequiredVersion -SitecoreFundamentalRequiredVersion $config.SitecoreFundamentalsRequiredVersion -config $config -nugetOutput $PackagesRoot
    }

    ## Uninstall xConnect Tasks
    Remove-XConnectInstance $config
    ## Uninstall Sitecore
    Remove-SitecoreInstance $config

    # Remove Custom Configs
    Invoke-ScCleanupConfigs $config
}

$confirmation = Read-Host "Are you Sure You Want To Uninstall Sitecore and xConnect:(y/n)"
if($confirmation -ne 'y'){
    return 0
}

# Project Specific Configs
Import-Module (Resolve-Path(Join-Path $PSScriptRoot "misc\SifEx\SifEx.psm1")) -Force
$packageRoot = Resolve-Path(Join-Path $PSScriptRoot "packages")

# Start Solr 
$solrPath = Get-Item (Resolve-Path(Join-Path $PSScriptRoot "misc\solr\Stop-Solr.cmd"))
#Start-Process -FilePath $solrPath.FullName -WorkingDirectory $solrPath.Directory

# Start Installation
Invoke-UnBumpSc9 -PackagesRoot $packageRoot