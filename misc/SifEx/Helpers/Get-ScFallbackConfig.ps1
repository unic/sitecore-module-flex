Function Get-ScFallbackConfig{
    param (        
        [String]
        $config,
        [String]
        $fallbackValue
    )
    
    if($config){    
        return $config
    }
    
    return $fallbackValue
}
