Set-StrictMode -Version 2.0

#Requires -RunAsAdministrator


Get-ChildItem -Path $PSScriptRoot -Include *.ps1 -File -Recurse | ForEach-Object {
    try {
        . $_.FullName
    }
    catch {
        Write-Warning $_.Exception.Message
    }
}

Export-ModuleMember -Function * -Alias *