Function Invoke-BumpSc9{
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

    # Check and Install SitecorePrerequisites
    if([System.Convert]::ToBoolean($config.InstallIdentity)){
        $SifConfigPaths = Join-Path $config.WebsitePath $config.SifConfigPaths
        Install-SitecorePrerequisites -SifConfigPaths $SifConfigPaths
        # Required Restart
        exit 0  
    }

    # Get Configuration
    $scconfig = Get-ScInstallConfig -BobConfig $config    

    ## Begin xConnect Tasks
    # Install xConnect Certificates    
    New-SitecoreCertificate -cert $scconfig.cert
    
    # Install xConnect Solr Cores in Docker
    New-SolrCore $scconfig.xconnectsolr

    # Install xConnect
    New-XConnectInstance $config
    ## End xConnect Tasks

    ## Begin Sitecore Tasks
    # Install Sitecore Solr Cores in Docker
    New-SolrCore $scconfig.sitecoreSolr

    # Install Sitecore
    New-SitecoreInstance $config
    ## End xConnect Tasks

    # Remove Custom Configs
    Invoke-ScCleanupConfigs $config
}


# Project Specific Configs
Import-Module (Resolve-Path(Join-Path $PSScriptRoot "misc\SifEx\SifEx.psm1")) -Force
$packageRoot = Resolve-Path(Join-Path $PSScriptRoot "packages")

# Start Solr 
$solrPath = Get-Item (Resolve-Path(Join-Path $PSScriptRoot "misc\solr\Start-Solr.cmd"))
Start-Process -FilePath $solrPath.FullName -WorkingDirectory $solrPath.Directory

# Start Installation
Invoke-BumpSc9 -PackagesRoot $packageRoot