using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "Tiny64Emu" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "easycash GmbH" )]
[assembly: AssemblyProduct( "Tiny64Emu" )]
[assembly: AssemblyCopyright( "Copyright © easycash GmbH 2017" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "65ebae59-a80a-4d92-94a4-5ea2016b5e94" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

#if NET5_0_OR_GREATER
[assembly: SupportedOSPlatform( "windows" )]
#endif
[assembly: AssemblyVersion( RetroDevStudio.Version.VersionBase + ".0" )]
[assembly: AssemblyFileVersion( RetroDevStudio.Version.VersionBase + ".0" )]
