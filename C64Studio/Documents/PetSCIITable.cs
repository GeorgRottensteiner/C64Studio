using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public class PetSCIITable : BaseDocument
  {
    private System.Windows.Forms.ListView   listChars;
    private System.Drawing.Font             _DefaultFont;


  
    public PetSCIITable( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      InitializeComponent();

      _DefaultFont = listChars.Font;


      listChars.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize );

      foreach ( Types.C64Character character in Types.ConstantData.PetSCIIToChar.Values )
      {
        if ( character.HasChar )
        {
          ListViewItem item = new ListViewItem();
          //item.Text = character.Desc;
          item.Text = "" + character.CharValue;
          /*
          if ( character.ShortDesc.Length > 0 )
          {
            item.Text = character.ShortDesc;
          }
          else
          {
            item.Text = character.Desc;
          }*/
          item.Tag = character;

          listChars.Items.Add( item );
        }
      }
    }

    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( PetSCIITable ) );
      this.listChars = new System.Windows.Forms.ListView();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      this.SuspendLayout();
      // 
      // listChars
      // 
      this.listChars.Activation = System.Windows.Forms.ItemActivation.OneClick;
      this.listChars.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listChars.GridLines = true;
      this.listChars.Location = new System.Drawing.Point( 0, 0 );
      this.listChars.MultiSelect = false;
      this.listChars.Name = "listChars";
      this.listChars.OwnerDraw = true;
      this.listChars.Size = new System.Drawing.Size( 534, 390 );
      this.listChars.TabIndex = 0;
      this.listChars.TileSize = new System.Drawing.Size( 80, 40 );
      this.listChars.UseCompatibleStateImageBehavior = false;
      this.listChars.View = System.Windows.Forms.View.Tile;
      this.listChars.ItemActivate += new System.EventHandler( this.listChars_ItemActivate );
      this.listChars.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler( this.listChars_DrawItem );
      // 
      // PetSCIITable
      // 
      this.ClientSize = new System.Drawing.Size( 534, 390 );
      this.Controls.Add( this.listChars );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      this.Name = "PetSCIITable";
      this.Text = "PETSCII";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      this.ResumeLayout( false );

    }



    private void listChars_DrawItem( object sender, System.Windows.Forms.DrawListViewItemEventArgs e )
    {
      if ( ( e.State & ( ListViewItemStates.Selected | ListViewItemStates.Hot ) ) != 0 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Highlight, e.Bounds );
      }
      else
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Info, e.Bounds );
      }
      e.Graphics.DrawRectangle( System.Drawing.Pens.Black, e.Bounds );

      Types.C64Character character = (Types.C64Character)e.Item.Tag;
      if ( character == null )
      {
        return;  
      }
      System.Drawing.Brush  brush = System.Drawing.SystemBrushes.WindowText;

      if ( ( e.State & ( ListViewItemStates.Selected | ListViewItemStates.Hot ) ) != 0 )
      {
        brush = System.Drawing.SystemBrushes.HighlightText;
      }
      e.Graphics.DrawString( e.Item.Text, listChars.Font, brush, new System.Drawing.PointF( e.Bounds.Left + 2, e.Bounds.Top + 2 ) );
      e.Graphics.DrawString( character.PetSCIIValue.ToString( "X02" ), _DefaultFont, brush, new System.Drawing.PointF( e.Bounds.Left + 40, e.Bounds.Top + 2 ) );
      e.Graphics.DrawString( character.Desc, _DefaultFont, brush, new System.Drawing.PointF( e.Bounds.Left, e.Bounds.Top + e.Bounds.Height * 0.5f ) );

      if ( ( e.State & ListViewItemStates.Focused ) != 0 )
      {
        e.DrawFocusRectangle();
      }
    }



    private void listChars_ItemActivate( object sender, EventArgs e )
    {
      if ( listChars.SelectedItems.Count == 0 )
      {
        return;
      }
      Types.C64Character character = (Types.C64Character)listChars.SelectedItems[0].Tag;
      BaseDocument doc = Core.MainForm.ActiveDocument;
      if ( doc != null )
      {
        doc.InsertText( "" + character.CharValue );
      }
    }



    public override System.Drawing.Size GetPreferredSize( System.Drawing.Size proposedSize )
    {
      return new System.Drawing.Size( 534, 390 );
    }



  }
}
