using System;
using System.Windows.Forms;
using RetroDevStudio.Types;
using GR.Image;
using RetroDevStudio;



namespace RetroDevStudio.Documents
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

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
      _DefaultFont = new System.Drawing.Font( listPETSCII.Font.FontFamily, listPETSCII.Font.Size * 96.0f / DPIHandler.DPIY, listPETSCII.Font.Style );

      listPETSCII.ItemWidth = (int)( 80 * GR.Image.DPIHandler.DPIX / 96.0f );
      listPETSCII.ItemHeight = (int)( 40 * GR.Image.DPIHandler.DPIY / 96.0f );

      listPETSCII.SetDisplaySize( listPETSCII.ClientSize.Width, listPETSCII.ClientSize.Height );
      listPETSCII.DisplayPage.Create( 120, 120, GR.Drawing.PixelFormat.Format24bppRgb );
      listPETSCII.PixelFormat = GR.Drawing.PixelFormat.Format24bppRgb;
      listPETSCII.Font = Core.Imaging.FontFromMachine( MachineType.C64, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      foreach ( Types.C64Character character in ConstantData.PetSCIIToChar.Values )
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

      System.Drawing.Brush  brushBackground = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) ) );
      System.Drawing.Pen  penBorder = new System.Drawing.Pen( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) ) );

      g.FillRectangle( brushBackground, 0, 0, 80, 40 );
      g.DrawRectangle( penBorder, 0, 0, 79, 39 );

      System.Drawing.Brush  brush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) ) );

      g.DrawString( "" + character.CharValue, listPETSCII.Font, brush, new System.Drawing.PointF( 2, 4 ) );
      g.DrawString( character.PetSCIIValue.ToString( "X02" ), _DefaultFont, brush, new System.Drawing.PointF( 40, 4 ) );
      g.DrawString( character.Desc, _DefaultFont, brush, new System.Drawing.PointF( 1, 24 ) );

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
      this.listPETSCII.PixelFormat = GR.Drawing.PixelFormat.DontCare;
      this.listPETSCII.SelectedIndex = -1;
      this.listPETSCII.Size = new System.Drawing.Size(534, 390);
      this.listPETSCII.TabIndex = 1;
      this.listPETSCII.VisibleAutoScrollHorizontal = true;
      this.listPETSCII.VisibleAutoScrollVertical = false;
      this.listPETSCII.KeyPressed += new System.Windows.Forms.KeyPressEventHandler(this.listPETSCII_KeyPressed);
      this.listPETSCII.SizeChanged += new System.EventHandler(this.listPETSCII_SizeChanged);
      this.listPETSCII.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listPETSCII_MouseDoubleClick);
      // 
      // PetSCIITable
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
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
      InsertPETSCII();
    }



    private void InsertPETSCII()
    {
      if ( listPETSCII.SelectedIndex == -1 )
      {
        return;
      }
      Types.C64Character character = (Types.C64Character)listPETSCII.Items[listPETSCII.SelectedIndex].Value;
      if ( ( Core.Navigating.LastActiveCodeDocument != null )
      &&   ( Core.Navigating.LastActiveCodeDocument.BaseDoc != null ) )
      {
        BaseDocument doc = Core.Navigating.LastActiveCodeDocument.BaseDoc;
        if ( doc != null )
        {
          doc.InsertText( "" + character.CharValue );
        }
      }
    }



    private void listPETSCII_KeyPressed( object sender, KeyPressEventArgs e )
    {
      if ( e.KeyChar == '\r' )
      {
        InsertPETSCII();
      }
    }



  }
}
