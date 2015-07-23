param($installPath, $toolsPath, $package, $project)

if((Get-Module Scoop) -and ((Get-Module scoop | select -first 1).Path)) {
    $scoopPath = (Get-Module scoop | select -first 1).Path
    $configPath =  Join-path (Split-path $scoopPath) "..\packages\Unic.Bob.Config\tools\BobConfig"
    if($configPath) {
        Import-Module $configPath

        $config = Get-ScProjectConfig
    }
    else {
        Write-Warning "BobConfig not found."
    }
}
else {
    Write-Warning "Scoop not found."
}

if($config -and $config.SerializationPath) {
    $serializationPath = Join-Path $config.WebsitePath $config.SerializationPath
    $appSerializationPath = Join-Path $serializationPath "app"
    $sourceSerialization = (Join-Path $installPath "serialization") + "\"

    ls $sourceSerialization -Recurse |  ? { ! $_.PSIsContainer } | % {
        $relativePath = $_.FullName.Substring($sourceSerialization.Length)
        $target = Join-Path $appSerializationPath $relativePath
        if(-not (Test-Path $target))  {
            cp $_.FullName $target
        }
        else {
            Write-Warning "Skip $relativePath because it already exists in project: $target"
        }
    }
}
else {
    if ((Test-Path (Join-Path $toolsPath uninstall.items.log)) -ne $true) {
        $module = (Join-Path $env:temp Sitecore.NuGet.1.0.dll);
        if ((Test-Path $module) -ne $true) {
            Copy-Item (Join-Path $toolsPath Sitecore.NuGet.1.0.dll) $module
        }
        Import-Module $module

        Install-Items -toolspath $toolsPath -project $project -dte $dte
        Install-Serverfiles -toolspath $toolsPath -project $project -dte $dte
    }
}
