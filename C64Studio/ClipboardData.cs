using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace C64Studio
{
  public class ClipboardEntry
  {
    public GR.Image.MemoryImage       Image = null;
    public bool                       MultiColor = false;
    public int                        Color = 1;
    public int                        Index = 0;
    public GR.Memory.ByteBuffer       Data = null;
  }


  [Serializable()]
  public class ClipboardData : ISerializable 
  {
    public bool                       SelectionColumn = false;
    public List<ClipboardEntry>       Entries = new List<ClipboardEntry>();



    public ClipboardData()
    {
    }



    public ClipboardData( SerializationInfo info, StreamingContext ctxt )
    {
      //Get the values from info and assign them to the appropriate properties
      SelectionColumn = (bool)info.GetValue( "SelectionColumn", typeof( int ) );
      //Entries = (List<ClipboardEntry>)info.GetValue( "EmployeeName", typeof( string ) );
    }
        
    //Serialization function.
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
      //You can use any custom name for your name-value pair. But make sure you
      // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
      // then you should read the same with "EmployeeId"
      info.AddValue( "SelectionColumn", SelectionColumn );
      //info.AddValue("EmployeeName", EmpName);
    }

  }
}
