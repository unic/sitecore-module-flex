Function Get-ScPackagePath{
    param(
    [string]
    $package, 
    [string]
    $version,
    [string]
    $nugetOutput
    )
    $path = Join-Path $nugetOutput "$($package).$($version)"

    if(Test-Path $path){
        $zip = Get-ChildItem $path -Filter "*.zip" | Select-Object -First 1
        $zip = $zip.FullName
    }
    return $zip
}