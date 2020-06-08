Function Invoke-ScDownloadPackage {
    param(
    [string]
    $name, 
    [string]
    $version,
    [string]
    $nugetOutput,
    [string]
    $nuget
    )
    . "$nuget" install "$name" -Version "$version" -OutputDirectory "$nugetOutput"
    Write-Output "Installing $name  $version  into $nugetOutput"
}
