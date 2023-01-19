using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs.Preferences
{
  public partial class PrefColors : PrefBase
  {
    public PrefColors()
    {
      InitializeComponent();
    }



    public PrefColors( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "colors", "display", "theme" } );
      InitializeComponent();

      RefillColorList();

      comboElementBG.Items.Add( new Types.ColorSetting( "Auto", 0xffffffff ) );

      AddColor( "Custom", 0xffffffff );

      AddColor( "Black", 0xff000000 );
      AddColor( "White", 0xffffffff );
      AddColor( "Yellow", 0xffffff00 );
      AddColor( "Green", 0xff00ff00 );
      AddColor( "Red", 0xffff0000 );
      AddColor( "Blue", 0xff0000ff );
      AddColor( "Dark Green", 0xff008000 );
      AddColor( "Dark Red", 0xff800000 );
      AddColor( "Dark Blue", 0xff000080 );
    }



    private void btnImportSettings_Click( object sender, EventArgs e )
    {

    }



    private void btnExportSettings_Click( object sender, EventArgs e )
    {

    }



    private void AddColor( string Name, uint Value )
    {
      comboElementBG.Items.Add( new Types.ColorSetting( Name, Value ) );
      comboElementFG.Items.Add( new Types.ColorSetting( Name, Value ) );
    }



    private void RefillColorList()
    {
      listColoring.Items.Clear();
      foreach ( Types.ColorableElement element in System.Enum.GetValues( typeof( Types.ColorableElement ) ) )
      {
        if ( element == RetroDevStudio.Types.ColorableElement.LAST_ENTRY )
        {
          continue;
        }
        /*
        if ( element >= Types.ColorableElement.FIRST_GUI_ELEMENT )
        {
          // TODO - for now GUI elements not custom drawn (yet)
          break;
        }*/
        ListViewItem itemSCLocal = new ListViewItem( GR.EnumHelper.GetDescription( element ) );
        itemSCLocal.Tag = Core.Settings.SyntaxColoring[element];
        listColoring.Items.Add( itemSCLocal );
      }

    }



    private void listColoring_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        panelElementPreview.Invalidate();
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;

      bool colorFound = false;
      for ( int i = 1; i < comboElementFG.Items.Count; ++i )
      {
        Types.ColorSetting colorCombo = (Types.ColorSetting)comboElementFG.Items[i];

        if ( colorCombo.FGColor == ( color.FGColor | 0xff000000 ) )
        {
          comboElementFG.SelectedIndex = i;
          colorFound = true;
          break;
        }
      }
      if ( !colorFound )
      {
        // Custom
        Types.ColorSetting colorCombo = (Types.ColorSetting)comboElementFG.Items[0];
        colorCombo.FGColor = color.FGColor;
        comboElementFG.SelectedIndex = 0;
      }

      if ( color.Name == "Empty Space" )
      {
        foreach ( Types.ColorSetting color2 in comboElementBG.Items )
        {
          if ( color2.Name == "Auto" )
          {
            comboElementBG.Items.Remove( color2 );
            break;
          }
        }
      }
      else
      {
        if ( ( (Types.ColorSetting)comboElementBG.Items[0] ).Name != "Auto" )
        {
          comboElementBG.Items.Insert( 0, new Types.ColorSetting( "Auto" ) );
        }
      }

      if ( ( color.BGColorAuto )
      && ( color.Name != "Empty Space" ) )
      {
        comboElementBG.SelectedIndex = 0;
      }
      else
      {
        colorFound = false;
        int     startIndex  = 2;
        if ( color.Name == "Empty Space" )
        {
          startIndex = 1;
        }

        for ( int i = startIndex; i < comboElementBG.Items.Count; ++i )
        {
          Types.ColorSetting colorCombo = (Types.ColorSetting)comboElementBG.Items[i];

          if ( colorCombo.FGColor == color.BGColor )
          {
            comboElementBG.SelectedIndex = i;
            colorFound = true;
            break;
          }
        }
        if ( !colorFound )
        {
          // Custom
          Types.ColorSetting colorCombo = (Types.ColorSetting)comboElementBG.Items[1];
          colorCombo.FGColor = color.BGColor;
          if ( color.Name == "Empty Space" )
          {
            comboElementBG.SelectedIndex = 0;
          }
          else
          {
            comboElementBG.SelectedIndex = 1;
          }
        }
      }
      comboElementFG.Invalidate();
      comboElementBG.Invalidate();
      panelElementPreview.Invalidate();
    }



    private void ColorsChanged( Types.ColorableElement Color )
    {
      RefreshDisplayOnDocuments();
    }



    private void comboElementFG_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        panelElementPreview.Invalidate();
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;
      Types.ColorSetting comboColor = (Types.ColorSetting)comboElementFG.SelectedItem;

      if ( color.FGColor != comboColor.FGColor )
      {
        color.FGColor = comboColor.FGColor;
        panelElementPreview.Invalidate();

        ColorsChanged( (Types.ColorableElement)listColoring.SelectedIndices[0] );
      }
    }



    private void comboElementBG_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        panelElementPreview.Invalidate();
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;
      Types.ColorSetting comboColor = (Types.ColorSetting)comboElementBG.SelectedItem;

      bool    changed = false;

      if ( ( comboElementBG.SelectedIndex == 0 ) != color.BGColorAuto )
      {
        changed = true;
      }
      if ( ( comboElementBG.SelectedIndex > 0 )
      &&   ( ( color.BGColorAuto )
      ||     ( color.BGColor != comboColor.FGColor ) ) )
      {
        changed = true;
      }
      if ( changed )
      {
        if ( comboElementBG.SelectedIndex == 0 )
        {
          color.BGColorAuto = true;
        }
        else
        {
          color.BGColorAuto = false;
          color.BGColor = comboColor.FGColor;
        }
        panelElementPreview.Invalidate();
        RefreshDisplayOnDocuments();
      }
    }



    private void btnChooseFG_Click( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;

      System.Windows.Forms.ColorDialog colDlg = new ColorDialog();

      colDlg.Color = GR.Color.Helper.FromARGB( color.FGColor );
      if ( colDlg.ShowDialog() == DialogResult.OK )
      {
        Types.ColorSetting comboColor = (Types.ColorSetting)comboElementFG.Items[0];
        comboColor.FGColor = (uint)colDlg.Color.ToArgb();
        color.FGColor = (uint)colDlg.Color.ToArgb();
        comboElementFG.SelectedIndex = 0;
        comboElementFG.Invalidate();
        panelElementPreview.Invalidate();
        RefreshDisplayOnDocuments();
      }
    }



    private void btnChooseBG_Click( object sender, EventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;

      System.Windows.Forms.ColorDialog colDlg = new ColorDialog();

      colDlg.Color = GR.Color.Helper.FromARGB( color.BGColor );
      if ( colDlg.ShowDialog() == DialogResult.OK )
      {
        Types.ColorSetting comboColor = (Types.ColorSetting)comboElementBG.Items[1];
        comboColor.FGColor = (uint)colDlg.Color.ToArgb();
        color.BGColor = (uint)colDlg.Color.ToArgb();
        color.BGColorAuto = false;

        // select custom entry
        comboElementBG.SelectedIndex = ColorComboIndexOfCustomItem( color );
        comboElementBG.Invalidate();
        panelElementPreview.Invalidate();
        RefreshDisplayOnDocuments();
      }
    }



    private int ColorComboIndexOfCustomItem( ColorSetting Color )
    {
      // empty space has no "Auto" item
      if ( Color.Name == "Empty Space" )
      {
        return 0;
      }
      return 1;
    }



    private void btnSetDefaultsColors_Click( object sender, EventArgs e )
    {
      Core.Settings.SetDefaultColors();
      listColoring_SelectedIndexChanged( listColoring, e );
      RefreshDisplayOnDocuments();
    }



    private void comboElementFG_DrawItem( object sender, DrawItemEventArgs e )
    {
      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }
      Types.ColorSetting   color = (Types.ColorSetting)comboElementFG.Items[e.Index];

      System.Drawing.Rectangle colorBox = new Rectangle( e.Bounds.Left + 2, e.Bounds.Top + 2, 50, e.Bounds.Height - 4 );

      e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( color.FGColor ) ), colorBox );
      e.Graphics.DrawRectangle( System.Drawing.SystemPens.WindowFrame, colorBox );

      e.Graphics.DrawString( color.Name, comboElementFG.Font, new System.Drawing.SolidBrush( e.ForeColor ), e.Bounds.Left + 60, e.Bounds.Top + 2 );
    }



    private void comboElementBG_DrawItem( object sender, DrawItemEventArgs e )
    {
      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)comboElementBG.Items[e.Index];
      uint colorRGB = color.FGColor;
      if ( color.Name == "Auto" )
      {
        color = new RetroDevStudio.Types.ColorSetting( "Auto", Core.Settings.BGColor( RetroDevStudio.Types.ColorableElement.EMPTY_SPACE ), Core.Settings.BGColor( RetroDevStudio.Types.ColorableElement.EMPTY_SPACE ) );
        colorRGB = color.FGColor;
      }
      else if ( color.Name == "Custom" )
      {
        colorRGB = color.FGColor;
      }
      System.Drawing.Rectangle colorBox = new Rectangle( e.Bounds.Left + 2, e.Bounds.Top + 2, 50, e.Bounds.Height - 4 );

      e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( colorRGB ) ), colorBox );
      e.Graphics.DrawRectangle( System.Drawing.SystemPens.WindowFrame, colorBox );

      e.Graphics.DrawString( color.Name, comboElementBG.Font, new System.Drawing.SolidBrush( e.ForeColor ), e.Bounds.Left + 60, e.Bounds.Top + 2 );
    }



    private void panelElementPreview_Paint( object sender, PaintEventArgs e )
    {
      if ( listColoring.SelectedItems.Count == 0 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Window, e.ClipRectangle );
        return;
      }
      Types.ColorSetting color = (Types.ColorSetting)listColoring.SelectedItems[0].Tag;

      // Set format of string.
      StringFormat drawFormat = new StringFormat();
      drawFormat.Alignment = StringAlignment.Center;
      drawFormat.LineAlignment = StringAlignment.Center;

      if ( color.BGColorAuto )
      {
        var bgElementColor = Core.Settings.BGColor( RetroDevStudio.Types.ColorableElement.EMPTY_SPACE );
        e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( bgElementColor ) ), panelElementPreview.ClientRectangle );
      }
      else
      {
        e.Graphics.FillRectangle( new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( color.BGColor ) ), panelElementPreview.ClientRectangle );
      }

      e.Graphics.DrawString( "Sample Text",
                             panelElementPreview.Font,
                             new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( color.FGColor ) ),
                             new System.Drawing.RectangleF( 0.0f, 0.0f, (float)panelElementPreview.ClientRectangle.Width, (float)panelElementPreview.ClientRectangle.Height ),
                             drawFormat );
    }



  }
}
