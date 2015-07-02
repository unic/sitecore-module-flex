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
        if(Test-Path $target) {
            # http://ss64.com/nt/fc.html
            FC.EXE $target $_.FullName | Out-Null
            if($LASTEXITCODE -eq 0) {
                Write-Host "Remove $target"
                rm $target
            }
            else {
                Write-Warning "$target and $( $_.FullName) are different. Skipping."
            }
        }
    }
}
else {
    Write-Warning "No SerializationPath found in the Bob.config."
}
