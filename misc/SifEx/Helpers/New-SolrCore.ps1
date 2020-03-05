function New-SolrCore {
    param (
        [Parameter(Mandatory=$true)]
        [object]$solrConfig
    )
    Write-Host "Installing Sitecore Cores" -ForegroundColor Green
    Write-Verbose ($solrConfig | Format-Table -AutoSize | Out-String)
    
    Set-ScSolrRoot -ConfigPath $solrConfig.configPath #-InstallationRoot $solrConfig.solrroot
    
    &"docker" stop "$($solrConfig.SolrContainerName)"
    Write-Output "Solr: $($solrConfig.SolrRoot)"
    $coreDirs = Get-ChildItem "$($solrConfig.SolrRoot)\*" -Directory  | Where-Object {$_.BaseName -like "$($solrConfig.CorePrefix)*"} 
    if(-not $solrConfig.IsXdb){
        $coreDirs = $coreDirs | Where-Object {-not($_.BaseName -like "*xdb*")}
    }else{
        $coreDirs = $coreDirs | Where-Object {$_.BaseName -like "*xdb*"}
    }
    $coreDirs | Get-ChildItem | Remove-Item -Recurse

    &"docker" start "$($solrConfig.SolrContainerName)"

    $solrConfig.remove("configPath")
    $solrConfig.remove("SolrContainerName")
    $solrConfig.remove("IsXdb")
    
    
    Install-SitecoreConfiguration @solrConfig
}