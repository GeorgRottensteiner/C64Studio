using RetroDevStudio.Types;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RetroDevStudio.Controls;



namespace RetroDevStudio.Dialogs
{
  public partial class DlgPaletteEditor : Form
  {
    private StudioCore      Core;

    private List<Palette>   OriginalOrder;


    public ColorSettings Colors
    {
      private set;
      get;
    }



    public List<int> PaletteMapping
    {
      private set;
      get;
    }



    public DlgPaletteEditor( StudioCore Core, ColorSettings Colors )
    {
      PaletteMapping = new List<int>();
      OriginalOrder = new List<Palette>();

      this.Colors = new ColorSettings( Colors );
      this.Core = Core;
      InitializeComponent();

      int palIndex = 0;

      foreach ( var pal in this.Colors.Palettes )
      {
        paletteList.Items.Add( new ArrangedItemEntry() { Text = pal.Name, Tag = pal } );

        OriginalOrder.Add( pal );
        PaletteMapping.Add( palIndex );
        ++palIndex;
      }
      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        listPalette.Items.Add( Colors.Palette.Colors[i] );
      }
      listPalette.SelectedIndex = 0;

      Core.Theming.ApplyTheme( this );
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private void listPalette_DrawItem( object sender, DrawItemEventArgs e )
    {
      e.DrawBackground();

      if ( e.Index == -1 )
      {
        return;
      }

      var textRect = new Rectangle( e.Bounds.Left, e.Bounds.Top, 30, e.Bounds.Height );
      if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
      {
        e.Graphics.DrawString( e.Index.ToString(), listPalette.Font, System.Drawing.SystemBrushes.HighlightText, textRect );
      }
      else
      {
        e.Graphics.DrawString( e.Index.ToString(), listPalette.Font, System.Drawing.SystemBrushes.ControlText, textRect );
      }
      var smallerRect = new Rectangle( e.Bounds.Left + 30, e.Bounds.Top, e.Bounds.Width - 30, e.Bounds.Height );
      e.Graphics.FillRectangle( Colors.Palette.ColorBrushes[e.Index], smallerRect );
    }



    private void listPalette_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      scrollR.Value   = Colors.Palette.Colors[listPalette.SelectedIndex].R;
      scrollG.Value   = Colors.Palette.Colors[listPalette.SelectedIndex].G;
      scrollB.Value   = Colors.Palette.Colors[listPalette.SelectedIndex].B;

      editR.Text      = Colors.Palette.Colors[listPalette.SelectedIndex].R.ToString();
      editRHex.Text   = Colors.Palette.Colors[listPalette.SelectedIndex].R.ToString( "X2" );
      editG.Text      = Colors.Palette.Colors[listPalette.SelectedIndex].G.ToString();
      editGHex.Text   = Colors.Palette.Colors[listPalette.SelectedIndex].G.ToString( "X2" );
      editB.Text      = Colors.Palette.Colors[listPalette.SelectedIndex].B.ToString();
      editBHex.Text   = Colors.Palette.Colors[listPalette.SelectedIndex].B.ToString( "X2" );

      panelColorPreview.Invalidate();
    }



    private void panelColorPreview_Paint( object sender, PaintEventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Control, e.ClipRectangle );
        return;
      }

      e.Graphics.FillRectangle( Colors.Palette.ColorBrushes[listPalette.SelectedIndex], panelColorPreview.ClientRectangle );
    }



    private void scrollR_Scroll( object sender, ScrollEventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];

      Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( scrollR.Value << 16 ) | ( curColor.G << 8 ) | ( curColor.B ) );

      curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

      Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
      Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

      editR.Text = Colors.Palette.Colors[listPalette.SelectedIndex].R.ToString();
      editRHex.Text = Colors.Palette.Colors[listPalette.SelectedIndex].R.ToString( "X2" );

      listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
      panelColorPreview.Invalidate();
    }



    private void scrollG_Scroll( object sender, ScrollEventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];

      Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( scrollG.Value << 8 ) | ( curColor.B ) );

      curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

      Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
      Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

      listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
      editG.Text = Colors.Palette.Colors[listPalette.SelectedIndex].G.ToString();
      editGHex.Text = Colors.Palette.Colors[listPalette.SelectedIndex].G.ToString( "X2" );
      panelColorPreview.Invalidate();
    }



    private void scrollB_Scroll( object sender, ScrollEventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];

      Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( curColor.G << 8 ) | ( scrollB.Value ) );

      curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

      Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
      Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

      listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
      editB.Text = Colors.Palette.Colors[listPalette.SelectedIndex].B.ToString();
      editBHex.Text = Colors.Palette.Colors[listPalette.SelectedIndex].B.ToString( "X2" );
      panelColorPreview.Invalidate();
    }



    private void editR_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];
      var newR = GR.Convert.ToI32( editR.Text );
      if ( newR != curColor.R )
      {
        Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( newR << 16 ) | ( curColor.G << 8 ) | ( curColor.B ) );

        curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

        Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
        Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editRHex_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];
      var newR = GR.Convert.ToI32( editRHex.Text, 16 );
      if ( newR != curColor.R )
      {
        Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( newR << 16 ) | ( curColor.G << 8 ) | ( curColor.B ) );

        curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

        Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
        Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editG_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];
      var newG = GR.Convert.ToI32( editG.Text );
      if ( newG != curColor.G )
      {
        Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( newG << 8 ) | ( curColor.B ) );

        curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

        Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
        Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editGHex_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];
      var newG = GR.Convert.ToI32( editGHex.Text, 16 );
      if ( newG != curColor.G )
      {
        Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( newG << 8 ) | ( curColor.B ) );

        curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

        Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
        Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editB_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];
      var newB = GR.Convert.ToI32( editB.Text );
      if ( newB != curColor.B )
      {
        Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( curColor.G << 8 ) | ( newB ) );

        curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

        Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
        Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private void editBHex_TextChanged( object sender, EventArgs e )
    {
      if ( listPalette.SelectedIndex == -1 )
      {
        return;
      }

      var curColor = Colors.Palette.Colors[listPalette.SelectedIndex];
      var newB = GR.Convert.ToI32( editBHex.Text, 16 );
      if ( newB != curColor.B )
      {
        Colors.Palette.ColorValues[listPalette.SelectedIndex] = (uint)( ( curColor.A << 24 ) | ( curColor.R << 16 ) | ( curColor.G << 8 ) | ( newB ) );

        curColor = GR.Color.Helper.FromARGB( Colors.Palette.ColorValues[listPalette.SelectedIndex] );

        Colors.Palette.Colors[listPalette.SelectedIndex] = curColor;
        Colors.Palette.ColorBrushes[listPalette.SelectedIndex] = new System.Drawing.SolidBrush( curColor );

        listPalette.Invalidate( listPalette.GetItemRectangle( listPalette.SelectedIndex ) );
        panelColorPreview.Invalidate();
      }
    }



    private ArrangedItemEntry paletteList_AddingItem( object sender )
    {
      var palette = new Palette( Colors.Palette );

      ArrangedItemEntry item = new ArrangedItemEntry();
      item.Text = "Palette";
      if ( !string.IsNullOrEmpty( editPaletteName.Text ) )
      {
        item.Text = editPaletteName.Text;
      }
      item.Tag = palette;

      Colors.Palettes.Add( palette );

      RebuildPaletteMapping();

      return item;
    }



    private void paletteList_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( Item == null )
      {
        editPaletteName.Enabled = false;
        return;
      }
      editPaletteName.Enabled = true;
      editPaletteName.Text    = Item.Text;

      Colors.ActivePalette = Item.Index;

      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        listPalette.Items[i] = Colors.Palette.Colors[i];
      }
      listPalette_SelectedIndexChanged( sender, new EventArgs() );
    }



    private void editPaletteName_TextChanged( object sender, EventArgs e )
    {
      if ( paletteList.SelectedItem != null )
      {
        paletteList.SelectedItem.Text = editPaletteName.Text;
        ( (Palette)paletteList.SelectedItem.Tag ).Name = editPaletteName.Text;
      }
    }



    private ArrangedItemEntry paletteList_CloningItem( object sender, ArrangedItemEntry Item )
    {
      var palette = new Palette( (Palette)Item.Tag );

      ArrangedItemEntry item = new ArrangedItemEntry();
      item.Text = "Palette";
      if ( !string.IsNullOrEmpty( editPaletteName.Text ) )
      {
        item.Text = editPaletteName.Text;
      }
      item.Tag = palette;

      Colors.Palettes.Add( palette );

      RebuildPaletteMapping();
      return item;
    }



    private void paletteList_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      Colors.Palettes.Remove( (Palette)Item.Tag );

      RebuildPaletteMapping();
    }



    private void paletteList_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      var newPalList = new List<Palette>();

      foreach ( ArrangedItemEntry item in paletteList.Items )
      {
        newPalList.Add( (Palette)item.Tag );
      }
      Colors.Palettes = newPalList;

      RebuildPaletteMapping();
    }



    private void RebuildPaletteMapping()
    {
      for ( int i = 0; i < OriginalOrder.Count; ++i )
      {
        var pal = OriginalOrder[i];

        PaletteMapping[i] = Colors.Palettes.IndexOf( pal );
      }
    }



    private void btnExportToData_Click( object sender, EventArgs e )
    {
      var palData = GatherPaletteData();

      string paletteASM = Util.ToASMData( palData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked );

      StringBuilder   sb = new StringBuilder();
      sb.Append( ";num entries " );
      sb.Append( palData.Length / 3 );
      sb.Append( Colors.Palettes[0].NumColors );
      sb.Append( ", color count " );
      sb.AppendLine();

      editDataExport.Text = sb + ";palette data" + Environment.NewLine + paletteASM + Environment.NewLine;
    }



    private ByteBuffer GatherPaletteData()
    {
      // get all palette datas, first all R, then all G, then all B
      var palData = new ByteBuffer();
      for ( int i = 0; i < Colors.Palettes.Count; ++i )
      {
        var curPal = Colors.Palettes[i];

        palData.Append( curPal.GetExportData( 0, curPal.NumColors, checkExportSwizzled.Checked ) );
      }

      // pal data has rgbrgbrgb, we need to copy all r,g,bs behind each other
      var orderedPalData = new ByteBuffer();
      for ( int i = 0; i < 3; ++i )
      {
        for ( int j = 0; j < Colors.Palettes.Count; ++j )
        {
          orderedPalData.Append( palData.SubBuffer( ( i + j * 3 ) * Colors.Palettes[0].NumColors, Colors.Palettes[0].NumColors ) );
        }
      }

      var finalPalData = new ByteBuffer();
      int totalNumColors = Colors.Palettes.Count * Colors.Palettes[0].NumColors;

      // extract R, G and B
      finalPalData.Append( orderedPalData.SubBuffer( 0, totalNumColors ) );
      finalPalData.Append( orderedPalData.SubBuffer( totalNumColors + 0, totalNumColors ) );
      finalPalData.Append( orderedPalData.SubBuffer( 2 * totalNumColors + 0, totalNumColors ) );

      return finalPalData;
    }



    private void btnExportToFile_Click( object sender, EventArgs e )
    {
      var palData = GatherPaletteData();

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save data as";
      saveDlg.Filter = "Binary Data|*.bin|All Files|*.*";
      /*
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }*/
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return;
      }
      GR.IO.File.WriteAllBytes( saveDlg.FileName, palData );
    }



    private void editDataExport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editDataExport.SelectAll();
        e.Handled = true;
      }
    }



    private void editDataExport_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyData == ( Keys.A | Keys.Control ) )
      {
        editDataExport.SelectAll();
      }
    }



    private void btnImportFromFile_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Open binary data", Constants.FILEFILTER_BINARY_FILES + Constants.FILEFILTER_ALL, out filename ) )
      {
        GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( filename );

        ImportFromData( data, checkImportSwizzle.Checked, checkImportColorsSorted.Checked );
      }
    }



    private bool OpenFile( string Caption, string FileFilter, out string Filename )
    {
      Filename = "";

      OpenFileDialog openDlg = new OpenFileDialog();

      openDlg.Title = Caption;
      openDlg.Filter = Core.MainForm.FilterString( FileFilter );
      if ( ( openDlg.ShowDialog() != DialogResult.OK )
      ||   ( string.IsNullOrEmpty( openDlg.FileName ) ) )
      {
        return false;
      }
      Filename = openDlg.FileName;
      return true;
    }



    private byte SwizzleByte( byte Value )
    {
      return (byte)( ( Value >> 4 ) | ( Value << 4 ) );
    }



    private void ImportFromData( ByteBuffer Data, bool Swizzle, bool SortedByRGB )
    {
      int numColorsInOnePalette = Colors.Palette.NumColors;
      int numColorsInImport = (int)( Data.Length / 3 );
      for ( int i = 0; i < numColorsInImport; ++i )
      {
        if ( i < Colors.Palettes.Count * numColorsInOnePalette )
        {
          uint    color = 0xff000000;

          if ( !SortedByRGB )
          {
            // RGBRGBRGB...
            if ( Swizzle )
            {
              color |= (uint)( SwizzleByte( Data.ByteAt( i * 3 ) ) << 16 );
              color |= (uint)( SwizzleByte( Data.ByteAt( i * 3 + 1 ) ) << 8 );
              color |= (uint)SwizzleByte( Data.ByteAt( i * 3 + 2 ) );
            }
            else
            {
              color |= (uint)( Data.ByteAt( i * 3 ) << 16 );
              color |= (uint)( Data.ByteAt( i * 3 + 1 ) << 8 );
              color |= (uint)Data.ByteAt( i * 3 + 2 );
            }
          }
          else
          {
            // RRRRRRRRRRRGGGGGGGGGGBBBBBBBBBBB
            if ( Swizzle )
            {
              color |= (uint)( SwizzleByte( Data.ByteAt( i ) ) << 16 );
              color |= (uint)( SwizzleByte( Data.ByteAt( (int)( i + numColorsInImport * 1 ) ) ) << 8 );
              color |= (uint)SwizzleByte( Data.ByteAt( (int)( i + numColorsInImport * 2 ) ) );
            }
            else
            {
              color |= (uint)( Data.ByteAt( i ) << 16 );
              color |= (uint)( Data.ByteAt( (int)( i + numColorsInImport * 1 ) ) << 8 );
              color |= (uint)Data.ByteAt( (int)( i + numColorsInImport * 2 ) );
            }
          }
          Colors.Palettes[i / numColorsInOnePalette].ColorValues[i % numColorsInOnePalette] = color;
        }
      }
      for ( int i = 0; i < Colors.Palettes.Count; ++i )
      {
        Colors.Palettes[i].CreateBrushes();
      }

      listPalette.Invalidate();
    }



    private void btnImportFromAssembly_Click( object sender, EventArgs e )
    {
      Parser.ASMFileParser asmParser = new RetroDevStudio.Parser.ASMFileParser();

      Parser.CompileConfig config = new Parser.CompileConfig();
      config.TargetType = Types.CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler = Types.AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + editDataImport.Text;
      if ( ( asmParser.Parse( temp, null, config, null ) )
      &&   ( asmParser.Assemble( config ) ) )
      {
        ImportFromData( asmParser.AssembledOutput.Assembly, checkImportSwizzle.Checked, checkImportColorsSorted.Checked );
      }
    }



  }
}
