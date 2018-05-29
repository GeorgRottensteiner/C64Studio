using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio.Dialogs
{
  public partial class DlgDisassembler : Form
  {
    private Formats.DisassemblyProject  m_DisassemblyProject = new C64Studio.Formats.DisassemblyProject();

    private Parser.Disassembler         m_Disassembler = new C64Studio.Parser.Disassembler( Tiny64.Processor.Create6510() );

    private string                      m_OpenedFilename = "";

    private StudioCore                  m_Core = null;





    public DlgDisassembler( StudioCore Core )
    {
      m_Core = Core;
      InitializeComponent();
    }



    private void SetHexData( GR.Memory.ByteBuffer Data )
    {
      hexView.ByteProvider = new Be.Windows.Forms.DynamicByteProvider( Data.Data() );
      hexView.ByteProvider.Changed += new EventHandler( ByteProvider_Changed );
    }



    void ByteProvider_Changed( object sender, EventArgs e )
    {
      // changed
      GR.Memory.ByteBuffer    data = DataFromHex();

      m_Disassembler.SetData( data );
      m_DisassemblyProject.Data = data;
      UpdateDisassembly();
    }



    private GR.Memory.ByteBuffer DataFromHex()
    {
      Be.Windows.Forms.DynamicByteProvider dynProvider = (Be.Windows.Forms.DynamicByteProvider)hexView.ByteProvider;

      List<byte> dataBytes = dynProvider.Bytes;
      if ( dataBytes.Count == 0 )
      {
        return new GR.Memory.ByteBuffer();
      }

      long     dataStart = 0;
      long     dataLength = hexView.ByteProvider.Length;

      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( (uint)dataLength );
      for ( int i = 0; i < dataBytes.Count; ++i )
      {
        data.SetU8At( i, dataBytes[(int)dataStart + i] );
      }
      return data;
    }



    private void btnOpenBinary_Click( object sender, EventArgs e )
    {
      OpenFileDialog    openDialog = new OpenFileDialog();

      openDialog.Title = "Choose binary file";
      if ( openDialog.ShowDialog() == DialogResult.OK )
      {
        m_OpenedFilename = openDialog.FileName;

        GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( openDialog.FileName );
        if ( data != null )
        {
          ushort    loadAddress = data.UInt16At( 0 );
          m_Disassembler.SetData( data );//.SubBuffer( 2 ) );
          m_DisassemblyProject.Data = data;

          editStartAddress.Text = "$" + loadAddress.ToString( "X4" );

          SetHexData( data );

          UpdateDisassembly();
        }
      }
    }



    private void UpdateDisassembly()
    {
      int     topLine = editDisassembly.VisibleRange.Start.iLine;

      string  disassembly;

      if ( m_Disassembler.Disassemble( m_DisassemblyProject.DataStartAddress, m_DisassemblyProject.JumpedAtAddresses, m_DisassemblyProject.NamedLabels, true, out disassembly ) )
      {
        editDisassembly.Text = disassembly;

        int firstLine = editDisassembly.VisibleRange.Start.iLine;
        editDisassembly.VerticalScroll.Value += topLine - firstLine;
      }
      else
      {
        editDisassembly.Text = disassembly;
      }
    }



    private void editStartAddress_TextChanged( object sender, EventArgs e )
    {
      string    startAddressT = editStartAddress.Text;

      if ( startAddressT.Length > 0 )
      {
        if ( startAddressT[0] == '$' )
        {
          m_DisassemblyProject.DataStartAddress = GR.Convert.ToI32( startAddressT.Substring( 1 ), 16 );
        }
        else if ( startAddressT.StartsWith( "0x" ) )
        {
          m_DisassemblyProject.DataStartAddress = GR.Convert.ToI32( startAddressT.Substring( 2 ), 16 );
        }
        else
        {
          m_DisassemblyProject.DataStartAddress = GR.Convert.ToI32( startAddressT );
        }
        hexView.LineInfoOffset = m_DisassemblyProject.DataStartAddress;
        UpdateDisassembly();
      }
    }



    private void FillItemFromAddress( ListViewItem Item, int Address )
    {
      Item.Text = "$" + Address.ToString( "X4" ) + "   " + Address;
      Item.Tag = Address;
    }



    private void btnAddJumpAddress_Click( object sender, EventArgs e )
    {
      string    addressT = editJumpAddress.Text;
      int       address = 0;

      if ( addressT.Length > 0 )
      {
        if ( addressT[0] == '$' )
        {
          address = GR.Convert.ToI32( addressT.Substring( 1 ), 16 );
        }
        else if ( addressT.StartsWith( "0x" ) )
        {
          address = GR.Convert.ToI32( addressT.Substring( 2 ), 16 );
        }
        else
        {
          address = GR.Convert.ToI32( addressT );
        }
        if ( !m_DisassemblyProject.JumpedAtAddresses.ContainsValue( (ushort)address ) )
        {
          m_DisassemblyProject.JumpedAtAddresses.Add( (ushort)address );


          ListViewItem    item = new ListViewItem();
          FillItemFromAddress( item, address );
          listJumpedAtAddresses.Items.Add( item );

          UpdateDisassembly();
        }
      }
    }



    private void btnDeleteJumpedAtAddress_Click( object sender, EventArgs e )
    {
      if ( listJumpedAtAddresses.SelectedItems.Count == 0 )
      {
        return;
      }
      int     address = (int)listJumpedAtAddresses.SelectedItems[0].Tag;

      m_DisassemblyProject.JumpedAtAddresses.Remove( address );
      listJumpedAtAddresses.Items.Remove( listJumpedAtAddresses.SelectedItems[0] );

      UpdateDisassembly();
    }



    private void listJumpedAtAddresses_SelectedIndexChanged( object sender, EventArgs e )
    {
      btnDeleteJumpedAtAddress.Enabled = ( listJumpedAtAddresses.SelectedItems.Count != 0 );
    }



    private void btnOpenProject_Click( object sender, EventArgs e )
    {
      OpenFileDialog    openDialog = new OpenFileDialog();

      openDialog.Title = "Choose binary file";
      openDialog.Filter = "Disassembly Projects|*.disassembly|All Files|*.*";
      if ( openDialog.ShowDialog() == DialogResult.OK )
      {
        GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( openDialog.FileName );
        if ( ( data != null )
        &&   ( m_DisassemblyProject.ReadFromBuffer( data ) ) )
        {
          m_Disassembler.SetData( m_DisassemblyProject.Data );
          editStartAddress.Text = "$" + m_DisassemblyProject.DataStartAddress.ToString( "X4" );
          editDisassemblyProjectName.Text = m_DisassemblyProject.Description;
          listJumpedAtAddresses.Items.Clear();
          foreach ( int jumpAddress in m_DisassemblyProject.JumpedAtAddresses )
          {
            ListViewItem    item = new ListViewItem();
            FillItemFromAddress( item, jumpAddress );
            item.Tag = jumpAddress;
            listJumpedAtAddresses.Items.Add( item );
          }

          listNamedLabels.Items.Clear();
          foreach ( var namedLabel in m_DisassemblyProject.NamedLabels )
          {
            ListViewItem  item = new ListViewItem( namedLabel.Value );
            item.SubItems.Add( "$" + namedLabel.Key.ToString( "X4" ) );
            listNamedLabels.Items.Add( item );
          }


          SetHexData( m_DisassemblyProject.Data );
          UpdateDisassembly();
        }
      }
    }



    private void btnSaveProject_Click( object sender, EventArgs e )
    {
      SaveFileDialog    saveDialog = new SaveFileDialog();

      saveDialog.Title = "Choose binary file";
      saveDialog.Filter = "Disassembly Projects|*.disassembly|All Files|*.*";
      if ( saveDialog.ShowDialog() == DialogResult.OK )
      {
        GR.IO.File.WriteAllBytes( saveDialog.FileName, m_DisassemblyProject.SaveToBuffer() );
      }
    }



    private void editDisassemblyProjectName_TextChanged( object sender, EventArgs e )
    {
      m_DisassemblyProject.Description = editDisassemblyProjectName.Text;
    }



    private void btnExportAssembly_Click( object sender, EventArgs e )
    {
      string  disassembly;

      if ( !m_Disassembler.Disassemble( m_DisassemblyProject.DataStartAddress, m_DisassemblyProject.JumpedAtAddresses, m_DisassemblyProject.NamedLabels, false, out disassembly ) )
      {
        return;
      }

      string    newFilename = "disassembly.asm";

      try
      {
        newFilename = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_OpenedFilename ), System.IO.Path.GetFileNameWithoutExtension( m_OpenedFilename ) ) + ".asm";
      }
      catch ( Exception )
      {
        newFilename = "disassembly.asm";
      }

      if ( m_Core.MainForm.m_Solution != null )
      {
        while ( m_Core.MainForm.m_Solution.FilenameUsed( newFilename ) )
        {
          newFilename = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_OpenedFilename ), System.IO.Path.GetFileNameWithoutExtension( m_OpenedFilename ) ) + "1.asm";
        }
      }
      SourceASMEx document = new SourceASMEx( m_Core );

      document.ShowHint   = WeifenLuo.WinFormsUI.Docking.DockState.Document;
      document.Core       = m_Core;
      document.Text       = System.IO.Path.GetFileName( newFilename );
      document.FillContent( disassembly );
      document.Show( m_Core.MainForm.panelMain );
    }



    private void listNamedLabels_SelectedIndexChanged( object sender, EventArgs e )
    {
      btnDeleteNamedLabel.Enabled = ( listNamedLabels.SelectedItems.Count != 0 );
    }



    private void btnDeleteNamedLabel_Click( object sender, EventArgs e )
    {
      if ( listNamedLabels.SelectedItems.Count == 0 )
      {
        return;
      }
      int     address = GR.Convert.ToI32( listNamedLabels.SelectedItems[0].SubItems[1].Text.Substring( 1 ), 16 );

      m_DisassemblyProject.NamedLabels.Remove( address );
      listNamedLabels.Items.Remove( listNamedLabels.SelectedItems[0] );
      UpdateDisassembly();
    }



    private void btnAddNamedLabel_Click( object sender, EventArgs e )
    {
      string    addressT = editLabelAddress.Text;

      int   address = 0;

      if ( addressT[0] == '$' )
      {
        address = GR.Convert.ToI32( addressT.Substring( 1 ), 16 );
      }
      else if ( addressT.StartsWith( "0x" ) )
      {
        address = GR.Convert.ToI32( addressT.Substring( 2 ), 16 );
      }
      else
      {
        address = GR.Convert.ToI32( addressT );
      }

      if ( !m_DisassemblyProject.NamedLabels.ContainsKey( address ) )
      {
        m_DisassemblyProject.NamedLabels[address] = editLabelName.Text;

        ListViewItem  item = new ListViewItem( editLabelName.Text );
        item.SubItems.Add( "$" + address.ToString( "X4" ) );

        listNamedLabels.Items.Add( item );

        UpdateDisassembly();
      }
      else if ( m_DisassemblyProject.NamedLabels[address] != editLabelName.Text )
      {
        for ( int i = 0; i < listNamedLabels.Items.Count; ++i )
        {
          if ( address == GR.Convert.ToI32( listNamedLabels.Items[i].SubItems[1].Text.Substring( 1 ), 16 ) )
          {
            listNamedLabels.Items[i].Text = editLabelName.Text;
            break;
          }
        }
        m_DisassemblyProject.NamedLabels[address] = editLabelName.Text;
        UpdateDisassembly();
      }
    }



    private void editLabelAddress_TextChanged( object sender, EventArgs e )
    {
      btnAddNamedLabel.Enabled = IsValidNamedLabel();
    }



    private bool IsValidNamedLabel()
    {
      if ( ( editLabelAddress.Text.Length == 0 )
      ||   ( editLabelName.Text.Length == 0 ) )
      {
        return false;
      }
      return true;
    }



    private void btnReloadFile_Click( object sender, EventArgs e )
    {
      if ( string.IsNullOrEmpty( m_OpenedFilename ) )
      {
        return;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( m_OpenedFilename );
      if ( data != null )
      {
        m_Disassembler.SetData( data );
        m_DisassemblyProject.Data = data;

        SetHexData( data );

        UpdateDisassembly();
      }
    }

  }
}
