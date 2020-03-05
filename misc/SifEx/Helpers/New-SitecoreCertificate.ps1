function New-SitecoreCertificate {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true)]
        [object]$cert
    )
    
    Write-Host "Installing Certificates" -ForegroundColor Green
    Write-Verbose ($cert | Format-Table -AutoSize | Out-String)

    Remove-Item -Path $cert.CertPath -ErrorAction SilentlyContinue
    Install-SitecoreConfiguration @cert
}