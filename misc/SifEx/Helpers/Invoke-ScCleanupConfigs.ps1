Function Invoke-ScCleanupConfigs {
    [CmdletBinding()]
    param (        
        [Parameter(Mandatory=$true)]
        $BobConfig
    )

    $config = Get-ScInstallConfig $BobConfig
    
    if(Test-Path $config.SifConfigPaths){
        Get-ChildItem "$($config.SifConfigPaths)" -File -filter "*custom*.json" | Remove-Item
    }
}