function Invoke-ScConfigureJson {
    [CmdletBinding(SupportsShouldProcess=$true)]
    param (
        [Parameter(Mandatory=$true)]
        [string]$ConfigPath,
        [Parameter(Mandatory=$true)]
        [string]$inputJson,
        [Parameter(Mandatory=$true)]
        [string]$outputJson
    )
    Write-Host "Transforming config file: $ConfigPath"  -ForegroundColor Green
    


    $inConfig = Resolve-Path $ConfigPath
    $inConfigDir = (Get-Item "$inConfig").Directory.FullName
    $inConfigFile = (get-item $inConfig).BaseName
    
    if($inConfigFile -like "*custom"){
        $outConfig =  Join-Path $inConfigDir "${inConfigFile}.json"
    }else{
        $outConfig =  Join-Path $inConfigDir "${inConfigFile}-custom.json"
    }
    

    (Get-Content $inConfig).Replace($inputJson,$outputJson) | Set-Content $outConfig
    
    return $outConfig
} 
