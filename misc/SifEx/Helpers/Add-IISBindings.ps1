
function Add-IISBindings {
    param (
        [Parameter(Mandatory=$true)]
        [string]$siteName,
        [Parameter(Mandatory=$true)]
        [string]$bindings
    )

    foreach($binding in ($bindings -Split ",")){
        Invoke-WebBindingTask -SiteName $siteName -Add @{ HostHeader= $binding }
        Invoke-HostHeaderTask -Hostname $binding 
    }    
}
