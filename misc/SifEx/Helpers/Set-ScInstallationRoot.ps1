function Set-ScInstallationRoot {
    [CmdletBinding(SupportsShouldProcess=$true)]
    param (
        [Parameter(Mandatory=$true)]
        [string]$ConfigPath,
        [Parameter(Mandatory=$true)]
        [string]$InstallationRoot
    )
    $escapedRoot = $InstallationRoot.ToString().Replace("\","\\")    
    $inputJson = "`"Site.PhysicalPath`": `"[joinpath(environment('SystemDrive'), 'inetpub', 'wwwroot', parameter('SiteName'))]`""
    $outputJson = "`"Site.PhysicalPath`": `"[joinpath('$escapedRoot', parameter('SiteName'))]`""
    
    return Invoke-ScConfigureJson -ConfigPath $ConfigPath -inputJson $inputJson -outputJson $outputJson
} 