Function Invoke-ScCheckSIFInstallation{
    param(
        $SitecoreFundamentalRequiredVersion,
        $SIFRequiredVersion
    )
    $sfVer = [version]::Parse($SitecoreFundamentalRequiredVersion)
    $sifVer = [version]::Parse($SIFRequiredVersion)
    $x=get-module -ListAvailable | Where-Object {
        ($_.Name -like 'SitecoreFundamentals' -and $_.Version -eq $sfVer) -or ($_.Name -like 'SitecoreInstallFramework' -and $_.Version -eq $sifVer)
     } | measure

    if(-not($x.count -eq 2)){
        return $false
    }
    Import-Module SitecoreFundamentals -RequiredVersion $SitecoreFundamentalRequiredVersion
    Import-Module SitecoreInstallFramework -RequiredVersion $SIFRequiredVersion
    return $true
}
