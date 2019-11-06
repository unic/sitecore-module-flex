Function Get-ScInstallConfig {
    [CmdletBinding()]
    param (        
        [Parameter(Mandatory=$true)]
        $BobConfig
    )
    Set-StrictMode -Off
    $packagePath = Join-Path $BobConfig.WebsitePath $BobConfig.PackagesRoot
    # General Vars
    $sitecorePackage         = Get-ScPackagePath $BobConfig.SitecorePackage -version $BobConfig.SitecoreVersion -nugetOutput $packagePath
    $xConnectPackage         = Get-ScPackagePath $BobConfig.xConnectPackage -version $BobConfig.SitecoreVersion -nugetOutput $packagePath
    $identityPackage         = Get-ScPackagePath $BobConfig.IdentityPacakge -version $BobConfig.SitecoreVersion -nugetOutput $packagePath
    $SifConfigPaths          = Join-Path $BobConfig.WebsitePath $BobConfig.SifConfigPaths
    $prefix                  = $BobConfig.WebsiteCodeName 
    $SitecoreAdminPassword   = Get-ScFallbackConfig $BobConfig.SitecoreAdminPassword "b"
    $LicenseFilePath         = Join-Path $BobConfig.WebsitePath $BobConfig.LicenseFile
    $SolrUrl                 = $BobConfig.SolrUrl 
    $SolrRoot                = Join-Path $BobConfig.WebsitePath $BobConfig.SolrRoot
    
    $installationRoot         = Join-Path $BobConfig.GlobalWebPath $BobConfig.WebsiteCodeName    
    $xConnectinstallationRoot = Join-Path $BobConfig.GlobalWebPath $BobConfig.WebsiteCodeName

    if(-not (Test-Path $LicenseFilePath)){
        throw "License not found at: $LicenseFilePath)"
    }
    
    # DB vars
    $sqlServer               = $BobConfig.DatabaseServer
    $sqlAdminUser            = Get-ScFallbackConfig $BobConfig.SqlAdminUser "SIFAdmin"    
    $sqlAdminPassword        = Get-ScFallbackConfig $BobConfig.SqlAdminUser "c4Ow0Rt8NwKW4uzo"

    $databaseUser            = Get-ScFallbackConfig $BobConfig.DatabaseUser $prefix
    $databasePassword        = Get-ScFallbackConfig $BobConfig.DatabasePassword "pDgv9h9OCQkiEd"
    $dbPrefix                = Get-ScFallbackConfig $BobConfig.DbPrefix $prefix

    $sitecoreConfiguration   = Get-ScFallbackConfig $BobConfig.xconnectConfiguration -fallbackValue "sitecore-xp0"
    # xConnect 
    $xconnectPostfix         = Get-ScFallbackConfig $BobConfig.xconnectPostfix -fallbackValue "xConnect"
    $xDbdatabaseUser         = Get-ScFallbackConfig $BobConfig.xDbdatabaseUser "${databaseUser}.${xconnectPostfix}"
    $xconnectInstance        = Get-ScFallbackConfig $BobConfig.xConnectWebsiteCodeName -fallbackValue "${prefix}.${xconnectPostfix}"
    $xconnectConfiguration   = Get-ScFallbackConfig $BobConfig.xconnectConfiguration -fallbackValue "xconnect-xp0"
    $XConnectEnvironment     = Get-ScFallbackConfig $BobConfig.XConnectEnvironment -fallbackValue "Development"

    # Identity
    $identityUser            = $databaseUser   # "$databaseUser.Identity"   for later!
    $identityPostfix         = Get-ScFallbackConfig $BobConfig.identityPostfix -fallbackValue "Identity"
    $identityInstance        = Get-ScFallbackConfig $BobConfig.IdentityWebsiteCodeName -fallbackValue "${prefix}.${identityPostfix}"
    $SitecoreIdentitySecret  = Get-ScFallbackConfig $BobConfig.SitecoreIdentitySecret -fallbackValue "UnicSitecoreIdentitySecret!"  

    # SQL Server Variables 
    $Sql = @{
        Server = $sqlServer
        AdminUser = $sqlAdminUser
        AdminPassword = $sqlAdminPassword 
        DbPrefix = $dbPrefix
    
        SqlCoreUser = "$($databaseUser)_coreuser"
        SqlMasterUser = $databaseUser
        SqlWebUser = $databaseUser
        SqlFormsUser = $databaseUser
        SqlExmMasterUser= $databaseUser

        SqlReportingUser = $xDbdatabaseUser
        SqlCollectionUser = $xDbdatabaseUser
        SqlProcessingPoolsUser = $xDbdatabaseUser
        SqlReferenceDataUser = $xDbdatabaseUser
        SqlMarketingAutomationUser = $xDbdatabaseUser
        SqlMessagingUser = $xDbdatabaseUser
        SqlProcessingTasksUser = $xDbdatabaseUser
        SqlProcessingEngineUser = $xDbdatabaseUser
        
        SqlSecurityUser = "$($identityUser)_securityuser"

        SqlCorePassword= $databasePassword
        SqlMasterPassword= $databasePassword
        SqlWebPassword= $databasePassword
        SqlReportingPassword= $databasePassword
        SqlFormsPassword= $databasePassword
        SqlMessagingPassword = $databasePassword
        SqlCollectionPassword = $databasePassword
        SqlProcessingPoolsPassword = $databasePassword
        SqlReferenceDataPassword = $databasePassword
        SqlMarketingAutomationPassword = $databasePassword
        SqlProcessingTasksPassword= $databasePassword
        SqlExmMasterPassword= $databasePassword        
        SqlProcessingEnginePassword = $databasePassword
        SqlSecurityPassword = $databasePassword
    }
    # Security Setting     
    $SecuritySetting =@{
        # 64 digits hexadecimal EXM Cryptographic Key.
        EXMCryptographicKey  = Get-ScFallbackConfig $BobConfig.EXMCryptographicKey -fallbackValue "0x2319030deedb02fa4dad2e8ab0cc7f15380807b1090b20a8c54fe955c687f358"
        # 64 digits hexadecimal EXM Authentication Key.    
        EXMAuthenticationKey = Get-ScFallbackConfig $BobConfig.EXMAuthenticationKey -fallbackValue "0x61f1d65b471adf3e3bbb3becc05ddd5695844e0383772c2bbbda7cb9be89b294"
        # PutYourCustomEncryptionKeyHereFrom32To256CharactersLong, The key to be used by Telerik controls to encrypt interaction with Content Editors. It's a random string between 32 and 256 symbols long.
        TelerikEncryptionKey = Get-ScFallbackConfig $BobConfig.TelerikEncryptionKey -fallbackValue "KRszIcqFaRVagkHEqKsz3eWEsSsBmk4mrucmOioTI77BRkect5LFnjiKZM1aWBCs"    
    }
    # --------------------------------------------------------------------------
    # Instance Configurations
    $instanceNames = @{
        Sitecore    = "${prefix}"
        XConnect    = "$xconnectInstance"
        Identity    = "$identityInstance"
    }
    $instanceNames.SitecoreHosts = ($BobConfig.IISBindings | Select-Object -First 1).'#text' 
    $instanceNames.SitecoreUrl = Get-ScFallbackConfig $BobConfig.XConnectUrl -fallbackValue "https://$($instanceNames.SitecoreHosts)"
    $instanceNames.SitecoreBindings =  [string]::Join(",",$BobConfig.IISBindings.'#text')

    $instanceNames.xConnectHosts = ($BobConfig.xConnectIISBindings | Select-Object -First 1).'#text' 
    $instanceNames.xConnectBindings =  [string]::Join(",",$BobConfig.xConnectIISBindings.'#text')
    $instanceNames.xconnectUrl = Get-ScFallbackConfig $BobConfig.XConnectUrl -fallbackValue "https://$($instanceNames.xConnectHosts)"

    if($BobConfig.IdentityIISBindings){
        $instanceNames.IdentityHosts = ($BobConfig.IdentityIISBindings | Select-Object -First 1).'#text' #[string]::Join(",",$BobConfig.IdentityIISBindings.'#text')        
    }
    $instanceNames.IdentityUrl = Get-ScFallbackConfig $BobConfig.IdentityUrl -fallbackValue "https://$($instanceNames.IdentityHosts)"

    # --------------------------------------------------------------------------
    # Certificate Settings
    $cert = @{
        Path                = Join-Path $SifConfigPaths "createcert.json"
        CertificateName     = "$($instanceNames.XConnect)_client"
        CertPath            =  Join-Path $packagePath "Certificates"
    }
    $identityCert = @{
        Path                = Join-Path $SifConfigPaths "createcert.json"
        CertificateName     = "$($instanceNames.Identity)"
        CertPath            =  Join-Path $packagePath "Certificates"
    }
    # --------------------------------------------------------------------------
    # Solr Configuration
    $sitecoreSolr = @{
        SolrContainerName   = $BobConfig.SolrContainerName
        configPath          = Join-Path $SifConfigPaths "sitecore-solr.json"
        Path                = Join-Path $SifConfigPaths "sitecore-solr-custom.json"        
        SolrRoot            = $SolrRoot
        SolrService         = "Not Needed!"
        CorePrefix          = $prefix
        SolrUrl             = $SolrUrl
        IsXdb               = $false
    }

    $xconnectsolr = @{
        SolrContainerName   = $BobConfig.SolrContainerName
        configPath          = Join-Path $SifConfigPaths "xconnect-solr.json"
        path                = Join-Path $SifConfigPaths "xconnect-solr-custom.json"
        solrroot            = $SolrRoot
        solrservice         = "Nott Needed!"
        coreprefix          = $prefix
        solrurl             = $SolrUrl
        IsXdb               = $true
    }

    # --------------------------------------------------------------------------
    $sitecore = @{    
        Path                                    = Join-Path $SifConfigPaths "${sitecoreConfiguration}.json"
        Package                                 = $sitecorePackage
        SiteName                                = $instanceNames.Sitecore
        XConnectCert                            = $cert.CertificateName
        LicenseFile                             = $licenseFilePath
        SitecoreAdminPassword                   = $SitecoreAdminPassword
        SolrCorePrefix                          = $Sql.DbPrefix
        SolrUrl                                 = $SolrUrl

        SqlServer                               = $Sql.Server
        SqlDbPrefix                             = $Sql.DbPrefix
        SqlAdminUser                            = $Sql.AdminUser
        SqlAdminPassword                        = $Sql.AdminPassword

        SqlCoreUser                             = $Sql.SqlCoreUser
        SqlMasterUser                           = $Sql.SqlMasterUser
        SqlWebUser                              = $Sql.SqlWebUser
        SqlFormsUser                            = $Sql.SqlFormsUser
        SqlReportingUser                        = $Sql.SqlReportingUser  
        SqlProcessingPoolsUser                  = $Sql.SqlProcessingPoolsUser
        SqlReferenceDataUser                    = $Sql.SqlReferenceDataUser
        SqlMarketingAutomationUser              = $Sql.SqlMarketingAutomationUser
        SqlProcessingTasksUser                  = $Sql.SqlProcessingTasksUser
        SqlExmMasterUser                        = $Sql.SqlExmMasterUser
        SqlMessagingUser                        = $Sql.SqlMessagingUser
        SqlSecurityUser                         = $Sql.SqlSecurityUser

        SqlCorePassword                         = $Sql.SqlCorePassword
        SqlMasterPassword                       = $Sql.SqlMasterPassword
        SqlWebPassword                          = $Sql.SqlWebPassword
        SqlReportingPassword                    = $Sql.SqlReportingPassword
        SqlFormsPassword                        = $Sql.SqlFormsPassword
        SqlMessagingPassword                    = $Sql.SqlMessagingPassword
        SqlProcessingPoolsPassword              = $Sql.SqlProcessingPoolsPassword
        SqlReferenceDataPassword                = $Sql.SqlReferenceDataPassword
        SqlMarketingAutomationPassword          = $Sql.SqlMarketingAutomationPassword
        SqlProcessingTasksPassword              = $Sql.SqlProcessingTasksPassword
        SqlExmMasterPassword                    = $Sql.SqlExmMasterPassword
        SqlSecurityPassword                     = $Sql.SqlSecurityPassword

        EXMCryptographicKey                     = $SecuritySetting.EXMCryptographicKey
        EXMAuthenticationKey                    = $SecuritySetting.EXMAuthenticationKey
        TelerikEncryptionKey                    = $SecuritySetting.TelerikEncryptionKey
        
        XConnectCollectionService               = $instanceNames.xconnectUrl
        XConnectReferenceDataService            = $instanceNames.xconnectUrl
        MarketingAutomationOperationsService    = $instanceNames.xconnectUrl
        MarketingAutomationReportingService     = $instanceNames.xconnectUrl
        
        CortexReportingService                  = $instanceNames.xconnectUrl
        SitecoreIdentityAuthority               = $instanceNames.IdentityUrl 

        SitecoreIdentitySecret                  = $SitecoreIdentitySecret
        HostMappingName                         = $instanceNames.SitecoreHosts
        DnsName                                 = $instanceNames.SitecoreHosts
    }
    

    # --------------------------------------------------------------------------
    $xconnect = @{
        Path                                                               = Join-Path $SifConfigPaths "${xconnectConfiguration}.json"
        Package                                                            = $xConnectPackage
        SiteName                                                           = $instanceNames.Xconnect
        LicenseFile                                                        = $LicenseFilePath
        SolrCorePrefix                                                     = $prefix
        SolrUrl                                                            = $SolrUrl
        SSLCert                                                            = ""
        XConnectCert                                                       = $cert.CertificateName
                                                                           
        SqlServer                                                          = $Sql.Server
        SqlDbPrefix                                                        = $Sql.DbPrefix
        SqlAdminUser                                                       = $Sql.AdminUser
        SqlAdminPassword                                                   = $Sql.AdminPassword
                                                                           
        SqlCollectionUser                                                  = $Sql.SqlCollectionUser
        SqlCollectionPassword                                              = $Sql.SqlCollectionPassword
        SqlProcessingPoolsUser                                             = $Sql.SqlProcessingPoolsUser
        SqlProcessingPoolsPassword                                         = $Sql.SqlProcessingPoolsPassword
        SqlReferenceDataUser                                               = $Sql.SqlReferenceDataUser
        SqlReferenceDataPassword                                           = $Sql.SqlReferenceDataPassword
        SqlMarketingAutomationUser                                         = $Sql.SqlMarketingAutomationUser
        SqlMarketingAutomationPassword                                     = $Sql.SqlMarketingAutomationPassword
        SqlMessagingUser                                                   = $Sql.SqlMessagingUser
        SqlMessagingPassword                                               = $Sql.SqlMessagingPassword
        SqlProcessingEngineUser                                            = $Sql.SqlProcessingEngineUser
        SqlProcessingEnginePassword                                        = $Sql.SqlProcessingEnginePassword
        SqlReportingUser                                                   = $Sql.SqlReportingUser
        SqlReportingPassword                                               = $Sql.SqlReportingPassword
                                                                           
        XConnectEnvironment                                                = $XConnectEnvironment
        XConnectLogLevel                                                   = "Information"
        HostMappingName                                                    = $instanceNames.xConnectHosts
        DnsName                                                            = $instanceNames.xConnectHosts

        MachineLearningServerUrl                                           = ""
        MachineLearningServerBlobEndpointCertificatePath                   = ""
        MachineLearningServerBlobEndpointCertificatePassword               = ""
        MachineLearningServerTableEndpointCertificatePath                  = ""
        MachineLearningServerTableEndpointCertificatePassword              = ""
        MachineLearningServerEndpointCertificationAuthorityCertificatePath = ""
    }
    
    $identity = @{
#        Path                       =   Join-Path $paths.Configs $identityConfigName
#        Package                    =   Join-Path $paths.Packages $identityPackageName
#        
#        SqlDbPrefix                =   $Sql.DbPrefix
#        SitecoreIdentityCert       =   $identityCert.CertificateName
#        LicenseFile                =   $paths.licenseFilePath
#        SiteName                   =   $instanceNames.Identity
#        SqlCoreUser                =   $Sql.SqlCoreUser
#        #SqlCoreDbName             =   
#        SqlCorePassword            =   $Sql.SqlCorePassword 
#        SqlServer                  =   $Sql.Server
#        PasswordRecoveryUrl        =   "http://$($sitecore.SiteName)" # The Identity Server password recovery URL, this should be the URL of the CM Instance
#        AllowedCorsOrigins         =   "http://$($sitecore.SiteName)" # Pipe-separated list of instances (URIs) that are allowed to login via Sitecore Identity.
#        ClientSecret              =   $SitecoreIdentitySecret
#        #CustomConfigurationFile   =            
#        HostMappingName            =   $instanceNames.Identity
#        DnsName                    =   $instanceNames.Identity
    }
    # ---------------------------------------------------------------------------
    # CD Server Settings
    $cd = $sitecore.clone()
    # Remove CM Specific Settings
    $cmSpecificVariables =@(
        "SitecoreAdminPassword",
        "SqlAdminUser",
        "SqlAdminPassword",
        "SqlCoreUser",
        "SqlCorePassword",
        "SqlMasterUser",
        "SqlMasterPassword",
        "SqlReportingUser",
        "SqlReportingPassword",
        "SqlProcessingPoolsUser",
        "SqlProcessingPoolsPassword",
        "SqlProcessingTasksUser",
        "SqlProcessingTasksPassword",
        "SqlReferenceDataUser",
        "SqlReferenceDataPassword",
        "SqlMarketingAutomationUser",
        "SqlMarketingAutomationPassword",
        "ExmEdsProvider",
        "TelerikEncryptionKey",
        "SitecoreIdentitySecret",
        "CortexReportingService")

    $sitecore.Keys  | ForEach{
        $z = $_
        if($cmSpecificVariables.Contains($z)){
            $cd.Remove($z)
        }
    }
    # ---------------------------------------------------------------------------
    return @{
        cert                        = $cert
        sitecore                    = $sitecore
        SitecoreBindings            = $instanceNames.SitecoreBindings
        sitecoreSolr                = $sitecoreSolr
        xConnect                    = $xconnect
        xConnectBindings            = $instanceNames.xConnectBindings
        xconnectsolr                = $xconnectsolr
        installationRoot            = $installationRoot
        xConnectinstallationRoot    = $xConnectinstallationRoot
        SifConfigPaths              = $SifConfigPaths
        
    }

}

