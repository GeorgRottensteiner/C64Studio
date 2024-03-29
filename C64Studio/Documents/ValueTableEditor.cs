using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RetroDevStudio.Types;
using GR.Memory;
using RetroDevStudio.Controls;



namespace RetroDevStudio.Documents
{
  public partial class ValueTableEditor : BaseDocument
  {
    private ValueTableProject         m_Project = new ValueTableProject();

    private Parser.ASMFileParser      m_Parser = new Parser.ASMFileParser();
    private bool                      m_DoNotUpdate = false;



    public ValueTableEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.VALUE_TABLE;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      m_IsSaveable = true;
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      pictureGraphPreview.DisplayPage.Create( pictureGraphPreview.ClientSize.Width, pictureGraphPreview.ClientSize.Height, GR.Drawing.PixelFormat.Format32bppArgb );

      checkExportToDataIncludeRes.Checked = true;
      checkExportToDataWrap.Checked = true;

      pictureGraphPreview.DisplayPage.Box( 0, 0, pictureGraphPreview.DisplayPage.Width, pictureGraphPreview.DisplayPage.Height, (uint)System.Drawing.SystemColors.Window.ToArgb() );
      pictureGraphPreview.Invalidate();

      // default values
      m_Project.ValueTable.StartValue = "0";
      m_Project.ValueTable.StepValue = "1";
      m_Project.ValueTable.Formula = "x*2";
      m_Project.ValueTable.EndValue = "10";

      RefreshDisplayOptions();

      GenerateValues();
    }



    public override void RefreshDisplayOptions()
    {
      base.RefreshDisplayOptions();
      /*
      foreach ( TabPage tab in tabSpriteEditor.TabPages )
      {
        tab.BackColor = ( (LightToolStripRenderer)ToolStripManager.Renderer ).BackColor;
      }*/
    }



    private new bool Modified
    {
      get
      {
        return base.Modified;
      }
      set
      {
        if ( value )
        {
          SetModified();
        }
        else
        {
          SetUnmodified();
        }
        saveValueTableProjectToolStripMenuItem.Enabled = Modified;
      }
    }



    private void openToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Open Value Table Project or File", Types.Constants.FILEFILTER_VALUE_TABLE_PROJECT + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        ImportValueTableProject( filename );
      }
    }



    public void Clear()
    {
      m_Project.Clear();
      listValues.Items.Clear();
    }



    public bool ImportValueTableProject( string Filename )
    {
      Clear();

      GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( Filename );
      if ( projectFile == null )
      {
        return false;
      }
      if ( !m_Project.ReadFromBuffer( projectFile ) )
      {
        return false;
      }

      m_DoNotUpdate = true;

      // TODO - controls
      foreach ( var value in m_Project.ValueTable.Values )
      {
        listValues.Items.Add( value );
      }
      editStartValue.Text     = m_Project.ValueTable.StartValue;
      editEndValue.Text       = m_Project.ValueTable.EndValue;
      editStepValue.Text      = m_Project.ValueTable.StepValue;
      editValueFunction.Text  = m_Project.ValueTable.Formula;
      checkGenerateDeltas.Checked = m_Project.ValueTable.GenerateDeltas;

      m_DoNotUpdate = false;

      Modified = false;

      if ( DocumentInfo.Element == null )
      {
        DocumentInfo.DocumentFilename = Filename;
      }

      saveValueTableProjectToolStripMenuItem.Enabled = true;
      closeValueTableProjectToolStripMenuItem.Enabled = true;
      EnableFileWatcher();
      return true;
    }



    protected override void OnCreateControl()
    {
      base.OnCreateControl();
      RedrawPreview();
    }



    public override bool LoadDocument()
    {
      if ( string.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      {
        return false;
      }
      try
      {
        ImportValueTableProject( DocumentInfo.FullPath );
      }
      catch ( System.IO.IOException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not load value table project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      pictureGraphPreview.Invalidate();
      SetUnmodified();
      return true;
    }



    protected override bool QueryFilename( out string Filename )
    {
      Filename = null;

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Value Table Project as";
      saveDlg.Filter = "Value Table Projects|*.valuetableproject|All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }
      Filename = saveDlg.FileName;
      return true;
    }



    protected override bool PerformSave( string Filename )
    {
      return SaveDocumentData( Filename, SaveToBuffer() );
    }



    private void closeCharsetProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( DocumentInfo.DocumentFilename == "" )
      {
        return;
      }
      if ( Modified )
      {
        var endButtons = MessageBoxButtons.YesNoCancel;
        if ( Core.ShuttingDown )
        {
          endButtons = MessageBoxButtons.YesNo;
        }
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your value table project. Save now?", "Save changes?", endButtons );
        if ( doSave == DialogResult.Cancel )
        {
          return;
        }
        if ( doSave == DialogResult.Yes )
        {
          Save( BaseDocument.SaveMethod.SAVE );
        }
      }
      Clear();
      DocumentInfo.DocumentFilename = "";
      Modified = false;

      closeValueTableProjectToolStripMenuItem.Enabled = false;
      saveValueTableProjectToolStripMenuItem.Enabled = false;
    }



    private void saveCharsetProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Save( SaveMethod.SAVE );
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      return m_Project.SaveToBuffer();
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private void checkExportToDataIncludeRes_CheckedChanged( object sender, EventArgs e )
    {
      editPrefix.Enabled = checkExportToDataIncludeRes.Checked;
    }



    private void editDataExport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( System.Windows.Forms.Control.ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editDataExport.SelectAll();
        e.Handled = true;
      }
    }



    private void editValueEntry_TextChanged( object sender, EventArgs e )
    {
      if ( listValues.SelectedIndices.Count == 0 )
      {
        return;
      }

      int     newIndex = listValues.SelectedIndices[0];

      if ( listValues.Items[newIndex].Text != editValueEntry.Text )
      {
        listValues.Items[newIndex].Text = editValueEntry.Text;
        SetModified();
      }
    }



    private ArrangedItemEntry listValues_AddingItem( object sender )
    {
      var item = new ArrangedItemEntry( editValueEntry.Text );

      return item;
    }



    private void listValues_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      ValuesChanged();
    }



    private void listValues_ItemMoved( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      ValuesChanged();
    }



    private void listValues_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      ValuesChanged();
    }



    private void ValuesChanged()
    {
      m_Project.ValueTable.Values.Clear();

      foreach ( ArrangedItemEntry item in listValues.Items )
      {
        m_Project.ValueTable.Values.Add( item.Text );
      }
      SetModified();
    }



    private void listValues_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( listValues.SelectedIndices.Count == 0 )
      {
        return;
      }
      int     newIndex = listValues.SelectedIndices[0];

      editValueEntry.Text = listValues.Items[newIndex].Text;
    }



    private void editValueFunction_TextChanged( object sender, EventArgs e )
    {
      if ( m_DoNotUpdate )
      {
        return;
      }
      if ( editValueFunction.Text != m_Project.ValueTable.Formula )
      {
        m_Project.ValueTable.Formula = editValueFunction.Text;
        SetModified();
      }
      GeneratorValuesChanged();
    }



    private void editStartValue_TextChanged( object sender, EventArgs e )
    {
      if ( m_DoNotUpdate )
      {
        return;
      }
      if ( editStartValue.Text != m_Project.ValueTable.StartValue )
      {
        m_Project.ValueTable.StartValue = editStartValue.Text;
        SetModified();
      }
      GeneratorValuesChanged();
    }



    private void editEndValue_TextChanged( object sender, EventArgs e )
    {
      if ( m_DoNotUpdate )
      {
        return;
      }
      if ( editEndValue.Text != m_Project.ValueTable.EndValue )
      {
        m_Project.ValueTable.EndValue = editEndValue.Text;
        SetModified();
      }
      GeneratorValuesChanged();
    }



    private void editStepValue_TextChanged( object sender, EventArgs e )
    {
      if ( m_DoNotUpdate )
      {
        return;
      }
      if ( editStepValue.Text != m_Project.ValueTable.StepValue )
      {
        m_Project.ValueTable.StepValue = editStepValue.Text;
        SetModified();
      }
      GeneratorValuesChanged();
    }



    private void GeneratorValuesChanged()
    {
      if ( !checkAutomatedGeneration.Checked )
      {
        return;
      }

      GenerateValues();
      RedrawPreview();
    }



    private void RedrawPreview()
    {
      pictureGraphPreview.DisplayPage.Box( 0, 0, pictureGraphPreview.DisplayPage.Width, pictureGraphPreview.DisplayPage.Height, (uint)System.Drawing.SystemColors.Window.ToArgb() );
      if ( string.IsNullOrEmpty( m_Project.ValueTable.Formula ) )
      {
        return;
      }

      // build preview
      double      min = double.MaxValue;
      double      max = double.MinValue;

      foreach ( var entry in m_Project.ValueTable.Values )
      {
        var calcedValue = Util.StringToDouble( entry );
        min = Math.Min( min, calcedValue );
        max = Math.Max( max, calcedValue );
      }
      if ( min == max )
      {
        pictureGraphPreview.DisplayPage.Line( 0, pictureGraphPreview.DisplayPage.Height / 2, pictureGraphPreview.DisplayPage.Width - 1, pictureGraphPreview.DisplayPage.Height / 2, 0 );
        pictureGraphPreview.Invalidate();
        return;
      }

      int   currentIndex = 0;
      int   prevY = 0;
      foreach ( var entry in m_Project.ValueTable.Values )
      {
        uint      color = 0xff000000;
        var calcedValue = Util.StringToDouble( entry );

        int     x1 = (int)( ( currentIndex - 1 ) * (double)pictureGraphPreview.DisplayPage.Width / ( m_Project.ValueTable.Values.Count - 1 ) );
        int     x2 = (int)( currentIndex * (double)pictureGraphPreview.DisplayPage.Width / ( m_Project.ValueTable.Values.Count - 1 ) );

        int     y = (int)( pictureGraphPreview.DisplayPage.Height - 1 - ( ( calcedValue - min ) * pictureGraphPreview.DisplayPage.Height / ( max - min ) ) );
        if ( currentIndex > 0 )
        {
          pictureGraphPreview.DisplayPage.Line( x1, prevY, x2, y, color );
        }

        prevY = y;
        ++currentIndex;
      }
      pictureGraphPreview.Invalidate();
    }



    private void GenerateValues()
    {
      m_Parser = new Parser.ASMFileParser();

      m_Parser.AddExtFunction( "sin", 1, 1, ExtSinus );
      m_Parser.AddExtFunction( "cos", 1, 1, ExtCosinus );
      m_Parser.AddExtFunction( "tan", 1, 1, ExtTan );
      m_Parser.AddExtFunction( "toradians", 1, 1, ExtToRadians );
      m_Parser.AddExtFunction( "todegrees", 1, 1, ExtToDegrees );

      double  startValue = Util.StringToDouble( m_Project.ValueTable.StartValue );
      double  endValue = Util.StringToDouble( m_Project.ValueTable.EndValue );
      double  stepValue = Util.StringToDouble( m_Project.ValueTable.StepValue );

      if ( ( startValue != endValue )
      &&   ( stepValue == 0 ) )
      {
        SetError( "Step value must not be equal zero" );
        return;
      }
      if ( ( startValue <= endValue )
      &&   ( stepValue < 0 ) )
      {
        SetError( "Step value must be positive" );
        return;
      }
      if ( ( startValue >= endValue )
      &&   ( stepValue > 0 ) )
      {
        SetError( "Step value must be negative" );
        return;
      }

      if ( checkClearPreviousValues.Checked )
      {
        m_Project.ValueTable.Values.Clear();
        listValues.Items.Clear();
      }

      double  curValue = startValue;
      bool    completed = false;
      bool    firstValue = true;
      double  lastValue = curValue;

      do
      {
        m_Parser.ClearASMInfo();
        m_Parser.AddConstantF( "x", m_Parser.CreateNumberSymbol( curValue ), 0, "", "", 0, 1 );
        var tokens = m_Parser.ParseTokenInfo( m_Project.ValueTable.Formula, 0, m_Project.ValueTable.Formula.Length, m_Parser.m_TextCodeMappingRaw );
        if ( tokens == null )
        {
          SetError( m_Parser.GetError().Code.ToString() );
          return;
        }
        if ( !m_Parser.EvaluateTokens( 0, tokens, m_Parser.m_TextCodeMappingRaw, out SymbolInfo result ) )
        {
          SetError( m_Parser.GetError().Code.ToString() );
          return;
        }

        double resultValue = result.ToNumber();
        if ( m_Project.ValueTable.GenerateDeltas )
        {
          if ( !firstValue )
          {
            m_Project.ValueTable.Values.Add( Util.DoubleToString( resultValue - lastValue ) );
            listValues.Items.Add( Util.DoubleToString( resultValue - lastValue ) );
          }
          firstValue = false;
        }
        else
        {
          m_Project.ValueTable.Values.Add( Util.DoubleToString( resultValue ) );
          listValues.Items.Add( Util.DoubleToString( resultValue ) );
        }

        lastValue = resultValue;
        curValue += stepValue;

        if ( startValue == endValue )
        {
          completed = true;
        }
        if ( ( stepValue > 0 )
        &&   ( curValue > endValue ) )
        {
          completed = true;
        }
        if ( ( stepValue < 0 )
        &&   ( curValue < endValue ) )
        {
          completed = true;
        }
      }
      while ( !completed );

      SetError( "OK" );

      m_Project.ValueTable.Data = new ByteBuffer( (uint)m_Project.ValueTable.Values.Count );
      int index = 0;
      foreach ( var entry in m_Project.ValueTable.Values )
      {
        m_Project.ValueTable.Data.SetU8At( index, GR.Convert.ToU8( entry ) );

        ++index;
      }
    }



    private List<TokenInfo> ExtSinus( List<TokenInfo> Arguments, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        SetError( "Invalid argument count" );
        return result;
      }
      if ( !m_Parser.EvaluateTokens( 0, Arguments, TextCodeMapping, out SymbolInfo functionResult ) )
      {
        SetError( "Invalid argument" );
        return result;
      }
      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_NUMBER,
        Content = Math.Sin( functionResult.ToNumber() * Math.PI / 180.0f ).ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtToRadians( List<TokenInfo> Arguments, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        SetError( "Invalid argument count" );
        return result;
      }
      if ( !m_Parser.EvaluateTokens( 0, Arguments, TextCodeMapping, out SymbolInfo functionResult ) )
      {
        SetError( "Invalid argument" );
        return result;
      }
      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_NUMBER,
        Content = ( functionResult.ToNumber() * Math.PI / 180.0f ).ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtToDegrees( List<TokenInfo> Arguments, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        SetError( "Invalid argument count" );
        return result;
      }
      if ( !m_Parser.EvaluateTokens( 0, Arguments, TextCodeMapping, out SymbolInfo functionResult ) )
      {
        SetError( "Invalid argument" );
        return result;
      }
      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_NUMBER,
        Content = ( functionResult.ToNumber() * 180.0f / Math.PI ).ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtCosinus( List<TokenInfo> Arguments, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        SetError( "Invalid argument count" );
        return result;
      }
      if ( !m_Parser.EvaluateTokens( 0, Arguments, TextCodeMapping, out SymbolInfo functionResult ) )
      {
        SetError( "Invalid argument" );
        return result;
      }
      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_NUMBER,
        Content = Math.Cos( functionResult.ToNumber() * Math.PI / 180.0f ).ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtTan( List<TokenInfo> Arguments, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        SetError( "Invalid argument count" );
        return result;
      }
      if ( !m_Parser.EvaluateTokens( 0, Arguments, TextCodeMapping, out SymbolInfo functionResult ) )
      {
        SetError( "Invalid argument" );
        return result;
      }
      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_NUMBER,
        Content = Math.Tan( functionResult.ToNumber() * Math.PI / 180.0f ).ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture )
      };
      result.Add( resultValue );
      return result;
    }



    private void SetError( string Message )
    {
      labelGenerationResult.Text = Message;
      if ( Message != "OK" )
      {
        labelGenerationResult.ForeColor = System.Drawing.Color.Red;
      }
      else
      {
        labelGenerationResult.ForeColor = System.Drawing.Color.Green;
      }
    }



    private void btnGenerateValues_Click( object sender, EventArgs e )
    {
      GenerateValues();
    }



    private void btnImportFromASM_Click( object sender, EventArgs e )
    {
      var output = Util.FromASMData( editDataExport.Text );
      if ( output == null )
      {
        return;
      }

      m_Project.ValueTable.Data.Clear();
      m_Project.ValueTable.Values.Clear();
      listValues.Items.Clear();

      for ( int i = 0; i < output.Length; ++i )
      {
        byte    nextValue = output.ByteAt( i );
        m_Project.ValueTable.Values.Add( nextValue.ToString() );
        m_Project.ValueTable.Data.AppendU8( nextValue );
        listValues.Items.Add( nextValue.ToString() );
      }
      SetModified();
    }



    private void btnExportToData_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer      exportData = GetValueData();

      int wrapByteCount = GR.Convert.ToI32( editWrapByteCount.Text );
      if ( wrapByteCount <= 0 )
      {
        wrapByteCount = 8;
      }
      string prefix = editPrefix.Text;
      bool wrapData = checkExportToDataWrap.Checked;
      bool prefixRes = checkExportToDataIncludeRes.Checked;
      if ( !prefixRes )
      {
        prefix = "";
      }

      string line = Util.ToASMData( exportData, wrapData, wrapByteCount, prefix );
      editDataExport.Text = line;
    }



    private ByteBuffer GetValueData()
    {
      var data = new ByteBuffer();

      if ( string.IsNullOrEmpty( m_Project.ValueTable.Formula ) )
      {
        foreach ( var entry in m_Project.ValueTable.Values )
        {
          data.AppendU8( GR.Convert.ToU8( entry ) );
        }
        return data;
      }

      GenerateValues();
      return m_Project.ValueTable.Data;
    }



    private void btnExportToBASICData_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer      exportData = GetValueData();

      int         lineDelta = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      int         curLineNumber = GR.Convert.ToI32( editExportBASICLineNo.Text );
      editDataExport.Text = Util.ToBASICData( exportData, curLineNumber, lineDelta, 80, 0, checkInsertSpaces.Checked );
    }



    private void btnExportToFile_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Data to";
      saveDlg.Filter = "Data File|*.dat|All Files|*.*";
      saveDlg.FileName = GR.Path.RenameExtension( DocumentInfo.FullPath, "dat" );
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      GR.IO.File.WriteAllBytes( saveDlg.FileName, GetValueData() );
    }



    private void btnImportFromFile_Click( object sender, EventArgs e )
    {
      string    filename;

      if ( !OpenFile( "Open data file", Types.Constants.FILEFILTER_VALUE_TABLE_DATA + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        return;
      }

      var buffer = GR.IO.File.ReadAllBytes( filename );
      if ( buffer == null )
      {
        return;
      }

      m_Project.ValueTable.Values.Clear();
      m_Project.ValueTable.Data = buffer;
      for ( int i = 0; i < buffer.Length; ++i )
      {
        m_Project.ValueTable.Values.Add( buffer.ByteAt( i ).ToString() );
      }
      SetModified();
    }



    private void btnImportFromHex_Click( object sender, EventArgs e )
    {
      string    binaryText = editDataExport.Text.Replace( " ", "" ).Replace( "\r", "" ).Replace( "\n", "" );

      GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer( binaryText );

      m_Project.ValueTable.Values.Clear();
      m_Project.ValueTable.Data = data;
      for ( int i = 0; i < data.Length; ++i )
      {
        m_Project.ValueTable.Values.Add( data.ByteAt( i ).ToString() );
      }
      SetModified();
    }



    protected override void OnLoad( EventArgs e )
    {
      base.OnLoad( e );
      pictureGraphPreview.DisplayPage.Box( 0, 0, pictureGraphPreview.DisplayPage.Width, pictureGraphPreview.DisplayPage.Height, (uint)System.Drawing.SystemColors.Window.ToArgb() );
      pictureGraphPreview.Invalidate();
    }



    private void pictureGraphPreview_SizeChanged( object sender, EventArgs e )
    {
      RedrawPreview();
    }



    private void checkGenerateDeltas_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_Project.ValueTable.GenerateDeltas != checkGenerateDeltas.Checked )
      {
        m_Project.ValueTable.GenerateDeltas = checkGenerateDeltas.Checked;
        SetModified();
        GeneratorValuesChanged();
      }
    }



    private void btnExportToBASICDataHex_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer      exportData = GetValueData();

      int         lineDelta = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      int         curLineNumber = GR.Convert.ToI32( editExportBASICLineNo.Text );
      editDataExport.Text = Util.ToBASICHexData( exportData, curLineNumber, lineDelta, checkInsertSpaces.Checked );
    }



    private ArrangedItemEntry listValues_CloningItem( object sender, ArrangedItemEntry Item )
    {
      var item = new ArrangedItemEntry( Item.Text );

      return item;
    }



  }
}
