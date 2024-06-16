$file = "C64Models/Version/StudioVersion.cs";
$content = Get-Content $file;

$index = 0;
foreach ( $line in $content )
{
  if ( $line.Contains( "BuildNumber" ) )
  {
    $pos = $line.IndexOf( '"' );
    $pos2 = $line.IndexOf( '"', $pos + 1 );
    
    $buildNumber = $line.Substring( $pos + 1, $pos2 - $pos - 1 );
    
    $buildNumber = [int]$buildNumber;
    ++$buildNumber;
    
    $content[$index] = "    public const string       BuildNumber = `"" + $buildNumber + "`";";
  }
  ++$index;
}

Set-Content $file $content;
