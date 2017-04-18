using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class StudioCore
  {
    public MainForm           MainForm = null;
    public Types.StudioState  State = Types.StudioState.NORMAL;
    public StudioSettings     Settings = new StudioSettings();
    public Debugging          Debugging = new Debugging();
    public Compiling          Compiling;
    public Searching          Searching;
    public Navigating         Navigating;



    delegate void AddToOutputCallback( string Text );
    delegate void delSetStatus( string Text, bool ProgressVisible, int ProgressValue );


    public StudioCore()
    {
      Compiling = new Compiling( this );
      Searching = new Searching( this );
      Navigating = new Navigating( this );
    }



    public void AddToOutput( string Text )
    {
      if ( MainForm.InvokeRequired )
      {
        try
        {
          MainForm.Invoke( new AddToOutputCallback( AddToOutput ), new object[] { Text } );
        }
        catch ( System.ObjectDisposedException )
        {
        }
      }
      else
      {
        MainForm.m_Output.AppendText( Text );
      }
    }



    public void SetStatus( string Text, bool ProgressVisible, int ProgressValue )
    {
      if ( MainForm.InvokeRequired )
      {
        MainForm.Invoke( new delSetStatus( SetStatus ), new object[] { Text, ProgressVisible, ProgressValue } );
      }
      else
      {
        MainForm.statusProgress.Visible = ProgressVisible;
        MainForm.statusProgress.Value = ProgressValue;
        MainForm.statusLabelInfo.Text = Text;
      }
    }



    public void SetStatus( string Text )
    {
      if ( MainForm.InvokeRequired )
      {
        MainForm.Invoke( new delSetStatus( SetStatus ), new object[] { Text, false, 0 } );
      }
      else
      {
        MainForm.statusLabelInfo.Text = Text;
      }
    }





  }
}
