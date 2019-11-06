function Set-ScXconnectHosts {
    [CmdletBinding(SupportsShouldProcess=$true)]
    param (
        [Parameter(Mandatory=$true)]
        [string]$ConfigPath
    )
    
    $inputJson  = "`"Endpoint.MarketingAutomation`": `"[concat('https://', parameter('SiteName'))]`""
    $outputJson = "`"Endpoint.MarketingAutomation`": `"[concat('https://', parameter('DnsName'))]`""
    
    $Out = Invoke-ScConfigureJson -ConfigPath $ConfigPath -inputJson $inputJson -outputJson $outputJson

    $inputJson  = "`"Endpoint.Collection`": `"[concat('https://', parameter('SiteName'))]`""
    $outputJson = "`"Endpoint.Collection`": `"[concat('https://', parameter('DnsName'))]`""
    
    $Out = Invoke-ScConfigureJson -ConfigPath $Out -inputJson $inputJson -outputJson $outputJson

    $inputJson  = "`"Endpoint.Processing.BlobStorage`": `"[concat('https://', parameter('SiteName'))]`""
    $outputJson = "`"Endpoint.Processing.BlobStorage`": `"[concat('https://', parameter('DnsName'))]`""
    
    $Out = Invoke-ScConfigureJson -ConfigPath $Out -inputJson $inputJson -outputJson $outputJson

    $inputJson  = "`"Endpoint.Processing.TableStorage`": `"[concat('https://', parameter('SiteName'))]`""
    $outputJson = "`"Endpoint.Processing.TableStorage`": `"[concat('https://', parameter('DnsName'))]`""
    
    $Out = Invoke-ScConfigureJson -ConfigPath $Out -inputJson $inputJson -outputJson $outputJson

    return $Out
} 