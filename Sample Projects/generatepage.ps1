# Enumerate all folders recursively
$folders = Get-ChildItem -Path . -Directory

foreach ( $folder in $folders ) 
{
  Write-Host ( "Parsing folder " + $folder.Name + "..." )
  if ( Test-Path ( "data/sys" + $folder.Name + ".png" ) )
  {
    Write-Host "-known system";
    
    $sysFolders = Get-ChildItem -Path $folder.FullName -Directory
    
    foreach ( $sampleFolder in $sysFolders ) 
    {
      Write-Host ( "found sample " + $sampleFolder.Name )
    }
  }
}
