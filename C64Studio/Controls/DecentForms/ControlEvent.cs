using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace DecentForms
{
  public class ControlEvent
  {
    public enum EventType
    {
      NULL                = 0,
      MOUSE_UPDATE        = 0x00000001,
      MOUSE_DOWN          = 0x00000002,
      MOUSE_UP            = 0x00000003,
      MOUSE_WHEEL         = 0x00000004,
      MOUSE_ENTER         = 0x00000005,
      MOUSE_LEAVE         = 0x00000006,
      MOUSE_DOUBLE_CLICK  = 0x00000007,

      KEY_DOWN            = 0x00000010,
      KEY_PRESS           = 0x00000011,
      KEY_UP              = 0x00000012,

      FOCUSED             = 0x00000020,
      FOCUS_LOST          = 0x00000021
    }


    public EventType        Type = ControlEvent.EventType.NULL;

    public int              MouseX = 0;
    public int              MouseY = 0;
    public int              MouseWheelDelta = 0;
    public uint             MouseButtons = 0;

    public Keys             Key = Keys.None;

    public bool             Handled = false;
  }



}
