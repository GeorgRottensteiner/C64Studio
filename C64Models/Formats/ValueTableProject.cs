using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Formats
{
  public class ValueTable
  {
    public string               Formula = "";
    public GR.Memory.ByteBuffer Data = new GR.Memory.ByteBuffer();
    public string               StartValue = "";
    public string               EndValue = "";
    public string               StepValue = "";
    public List<string>         Values = new List<string>();


    public ValueTable()
    {
    }

    public ValueTable Clone()
    {
      ValueTable copy = new ValueTable();

      copy.Formula    = Formula;
      copy.Data       = new GR.Memory.ByteBuffer( Data );
      copy.StartValue = StartValue;
      copy.EndValue   = EndValue;
      copy.StepValue  = StepValue;
      copy.Values     = new List<string>( Values );

      return copy;
    }
  }



  public class ValueTableProject
  {
    public ValueTable     ValueTable = new ValueTable();

    public string         Name = "";



    public ValueTableProject()
    {
    }



    public GR.Memory.ByteBuffer GenerateTableData()
    {
      GR.Memory.ByteBuffer exportData = new GR.Memory.ByteBuffer();

      return exportData;
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      // version
      projectFile.AppendU32( 1 );
      // Name
      projectFile.AppendString( Name );
      // Value table
      // number of entries
      projectFile.AppendI32( ValueTable.Values.Count );
      foreach ( var entry in ValueTable.Values )
      {
        projectFile.AppendString( entry );
      }
      projectFile.AppendString( ValueTable.Formula );
      projectFile.AppendString( ValueTable.StartValue );
      projectFile.AppendString( ValueTable.EndValue );
      projectFile.AppendString( ValueTable.StepValue );

      return projectFile;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer DataIn )
    {
      if ( DataIn == null )
      {
        return false;
      }

      ValueTable.Values.Clear();

      GR.IO.MemoryReader memIn = DataIn.MemoryReader();

      uint version  = memIn.ReadUInt32();
      Name          = memIn.ReadString();

      int   numEntries = memIn.ReadInt32();
      for ( int i = 0; i < numEntries; ++i )
      {
        ValueTable.Values.Add( memIn.ReadString() );
      }
      ValueTable.Formula    = memIn.ReadString();
      ValueTable.StartValue = memIn.ReadString();
      ValueTable.EndValue   = memIn.ReadString();
      ValueTable.StepValue  = memIn.ReadString();
      return true;
    }



    public void Clear()
    {
      ValueTable.Data.Clear();
      ValueTable.Values.Clear();
      ValueTable.StartValue = "";
      ValueTable.EndValue = "";
      ValueTable.Formula = "";
      ValueTable.StepValue = "";
    }

  }
}
