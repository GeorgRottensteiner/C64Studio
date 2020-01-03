using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class ProjectSettings
  {
    public string       Name = "";
    public string       Filename = null;
    public string       BasePath = null;
    public ushort       DebugPort = 6510;
    public string       BuildTool = "";
    public string       RunTool = "";
    public string       MainDocument = "";
    private GR.Collections.Map<string,ProjectConfig>         Configs = new GR.Collections.Map<string, ProjectConfig>();
    public ProjectConfig    CurrentConfig = null;



    public ProjectConfig Configuration( string ConfigName )
    {
      if ( Configs.ContainsKey( ConfigName ) )
      {
        return Configs[ConfigName];
      }
      return null;
    }



    public void Configuration( string ConfigName, ProjectConfig Config )
    {
      if ( Configs.ContainsKey( ConfigName ) )
      {
        Configs[ConfigName] = Config;
        return;
      }
      Configs.Add( ConfigName, Config );
    }



    public IEnumerable<string> GetConfigurationNames()
    {
      return Configs.Keys;
    }



    internal IEnumerable<ProjectConfig> GetConfigurations()
    {
      return Configs.Values;
    }



    internal int GetConfigurationCount()
    {
      return Configs.Count;
    }



    internal void DeleteConfiguration( string ConfigName )
    {
      Configs.Remove( ConfigName );
    }



    internal ProjectConfig GetConfigurationByName( string ConfigName )
    {
      if ( !Configs.ContainsKey( ConfigName ) )
      {
        return null;
      }
      return Configs[ConfigName];
    }
  }
}
