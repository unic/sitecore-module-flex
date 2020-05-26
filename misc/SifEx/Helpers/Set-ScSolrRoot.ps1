function Set-ScSolrRoot {
    [CmdletBinding(SupportsShouldProcess=$true)]
    param (
        [Parameter(Mandatory=$true)]
        [string]$ConfigPath
    )
    Write-Host "Transforming config file: $ConfigPath"  -ForegroundColor Green

    $input = "`"Solr.Server`":      `"[joinpath(variable('Solr.FullRoot'), 'server', 'solr')]`""
    $output = "`"Solr.Server`": `"[resolvepath(parameter('SolrRoot'))]`""
    
    Invoke-ScConfigureJson -ConfigPath $ConfigPath -input $input -output $output

}    
