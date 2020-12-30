using System;
using System.Collections.Generic;
using System.Text;

namespace C64Models.BASIC
{
  public class Opcode
  {
    public string         Command = "";
    public int            InsertionValue = -1;
    public string         ShortCut = null;

    public Opcode( string Command, int InsertionValue )
    {
      this.Command        = Command;
      this.InsertionValue = InsertionValue;
    }

    public Opcode( string Command, int InsertionValue, string ShortCut )
    {
      this.Command        = Command;
      this.InsertionValue = InsertionValue;
      this.ShortCut       = ShortCut;
    }
  };



  public class Dialect
  {
    public string                           Name = "";
    public Dictionary<string, Opcode>       Opcodes = new Dictionary<string, Opcode>();
    public SortedDictionary<ushort, Opcode> OpcodesFromByte = new SortedDictionary<ushort, Opcode>();
    public Dictionary<string, Opcode>       ExOpcodes = new Dictionary<string, Opcode>();



    public void Clear()
    {
      ExOpcodes.Clear();
      Opcodes.Clear();
      OpcodesFromByte.Clear();
    }



    public void AddOpcode( string Opcode, int ByteValue )
    {
      var opcode = new Opcode( Opcode, ByteValue );
      Opcodes[Opcode] = opcode;
      OpcodesFromByte[(ushort)ByteValue] = opcode;
    }



    public void AddOpcode( string Opcode, int ByteValue, string ShortCut )
    {
      var opcode = new Opcode( Opcode, ByteValue, ShortCut );
      Opcodes[Opcode] = opcode;
      OpcodesFromByte[(ushort)ByteValue] = opcode;
    }



    public void AddExOpcode( string Opcode, int ByteValue )
    {
      ExOpcodes[Opcode] = new Opcode( Opcode, ByteValue );
    }



    public static Dialect ReadBASICDialect( string File )
    {
      var dialect = new Dialect();
      using ( var reader = new GR.IO.BinaryReader( File ) )
      {
        string    line;
        bool      firstLine = true;
        int       lineIndex = 0;

        while ( reader.ReadLine( out line ) )
        {
          ++lineIndex;
          line = line.Trim();
          if ( ( string.IsNullOrEmpty( line ) )
          || ( line.StartsWith( "#" ) ) )
          {
            continue;
          }
          // skip header
          if ( firstLine )
          {
            firstLine = false;
            continue;
          }

          string[] parts = line.Split( ';' );
          if ( parts.Length != 3 )
          {
            return null;
          }
          dialect.AddOpcode( parts[0], GR.Convert.ToI32( parts[1], 16 ), parts[2] );
        }
      }
      dialect.Name = System.IO.Path.GetFileNameWithoutExtension( File );

      return dialect;
    }



  }
}
