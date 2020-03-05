Function New-ScSifSqlAdmin{
    param (     
        [String]   
        $server,
        [String]
        $username,
        [String]
        $password
    )
    Import-Module SqlServer -MinimumVersion 18.0.0
    $username = Get-ScFallbackConfig -config $username -fallbackValue "SIFAdmin"
    $password  = Get-ScFallbackConfig -config $password -fallbackValue "c4Ow0Rt8NwKW4uzo"
    $login = Get-SqlLogin -LoginName $username -LoginType SqlLogin -ServerInstance $server -ErrorAction SilentlyContinue
    if(-not $login){
        $Pass = ConvertTo-SecureString -String $password -AsPlainText -Force
        $credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $username, $Pass
        Add-SqlLogin -ServerInstance $server -LoginName $username -LoginType SqlLogin -LoginPSCredential $credential -Enable -GrantConnectSql            
        # not the best solution!
        $svr = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $server
        $svrole = $svr.Roles | where {$_.Name -eq 'sysadmin'}
        $svrole.AddMember($username)
    }
}