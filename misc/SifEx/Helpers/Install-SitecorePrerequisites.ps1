Function Install-SitecorePrerequisites{
    param(
        $SifConfigPaths    
    )

    $configPath = Resolve-Path (Join-Path $SifConfigPaths "Prerequisites.json")
    $prereq = @{
        Path = $configPath
    }

    Install-SitecoreConfiguration @prereq -Verbose *>&1 | Tee-Object Prerequisites.log
}