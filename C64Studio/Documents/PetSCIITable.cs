using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using C64Studio.Types;
using GR.Image;

namespace C64Studio
{
  public class PetSCIITable : BaseDocument
  {
    private GR.Forms.ImageListbox listPETSCII;
    private System.Drawing.Font             _DefaultFont;


  
    public PetSCIITable( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;
      InitializeComponent();

      _DefaultFont = listPETSCII.Font;

      listPETSCII.ItemWidth = 80;
      listPETSCII.ItemHeight = 40;
      listPETSCII.SetDisplaySize( listPETSCII.ClientSize.Width, listPETSCII.ClientSize.Height );
      listPETSCII.DisplayPage.Create( 120, 120, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
      listPETSCII.PixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
      listPETSCII.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize );

      foreach ( Types.C64Character character in Types.ConstantData.PetSCIIToChar.Values )
      {
        if ( character.HasChar )
        {
          listPETSCII.Items.Add( CreateItem( character ) );
        }
      }
    }



    private GR.Forms.ImageListbox.ImageListItem CreateItem( C64Character character )
    {
      var item = new GR.Forms.ImageListbox.ImageListItem( listPETSCII );

      item.Value = character;
      item.Image = CreateImageForCharacter( character );

      return item;
    }



    private System.Drawing.Image CreateImageForCharacter( C64Character character )
    {
      var bitmap = new System.Drawing.Bitmap( 80, 40, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
      var g = System.Drawing.Graphics.FromImage( bitmap );

      g.FillRectangle( System.Drawing.SystemBrushes.Info, 0, 0, 80, 40 );
      g.DrawRectangle( System.Drawing.SystemPens.InactiveBorder, 0, 0, 80, 40 );

      System.Drawing.Brush  brush = System.Drawing.SystemBrushes.WindowText;

      g.DrawString( "" + character.CharValue, listPETSCII.Font, brush, new System.Drawing.PointF( 2, 4 ) );
      g.DrawString( character.PetSCIIValue.ToString( "X02" ), _DefaultFont, brush, new System.Drawing.PointF( 40, 4 ) );
      g.DrawString( character.Desc, _DefaultFont, brush, new System.Drawing.PointF( 0, 24 ) );

      g.Dispose();

      return bitmap;
    }



    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PetSCIITable));
      this.listPETSCII = new GR.Forms.ImageListbox();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.SuspendLayout();
      // 
      // listPETSCII
      // 
      this.listPETSCII.AutoScroll = true;
      this.listPETSCII.AutoScrollHorizontalMaximum = 100;
      this.listPETSCII.AutoScrollHorizontalMinimum = 0;
      this.listPETSCII.AutoScrollHPos = 0;
      this.listPETSCII.AutoScrollVerticalMaximum = 100;
      this.listPETSCII.AutoScrollVerticalMinimum = 0;
      this.listPETSCII.AutoScrollVPos = 0;
      this.listPETSCII.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listPETSCII.EnableAutoScrollHorizontal = true;
      this.listPETSCII.EnableAutoScrollVertical = true;
      this.listPETSCII.HottrackColor = ((uint)(2151694591u));
      this.listPETSCII.ItemHeight = 13;
      this.listPETSCII.ItemWidth = 203;
      this.listPETSCII.Location = new System.Drawing.Point(0, 0);
      this.listPETSCII.Name = "listPETSCII";
      this.listPETSCII.PixelFormat = System.Drawing.Imaging.PixelFormat.DontCare;
      this.listPETSCII.SelectedIndex = -1;
      this.listPETSCII.Size = new System.Drawing.Size(534, 390);
      this.listPETSCII.TabIndex = 1;
      this.listPETSCII.VisibleAutoScrollHorizontal = true;
      this.listPETSCII.VisibleAutoScrollVertical = false;
      this.listPETSCII.SizeChanged += new System.EventHandler(this.listPETSCII_SizeChanged);
      this.listPETSCII.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listPETSCII_MouseDoubleClick);
      // 
      // PetSCIITable
      // 
      this.ClientSize = new System.Drawing.Size(534, 390);
      this.Controls.Add(this.listPETSCII);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "PetSCIITable";
      this.Text = "PETSCII";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.ResumeLayout(false);

    }



    public override System.Drawing.Size GetPreferredSize( System.Drawing.Size proposedSize )
    {
      return new System.Drawing.Size( 534, 390 );
    }



    private void listPETSCII_SizeChanged( object sender, EventArgs e )
    {
      listPETSCII.SetDisplaySize( listPETSCII.ClientSize.Width, listPETSCII.ClientSize.Height );
    }



    private void listPETSCII_MouseDoubleClick( object sender, MouseEventArgs e )
    {
      if ( listPETSCII.SelectedIndex == -1 )
      {
        return;
      }
      Types.C64Character character = (Types.C64Character)listPETSCII.Items[listPETSCII.SelectedIndex].Value;
      BaseDocument doc = Core.MainForm.ActiveDocument;
      if ( doc != null )
      {
        doc.InsertText( "" + character.CharValue );
      }
    }
  }
}
