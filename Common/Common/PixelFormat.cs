
// plain copy of System.Drawing.Imaging.PixelFormat

namespace GR.Drawing
{
  public enum PixelFormat
  {
    Indexed               = 0x10000,
    Gdi                   = 0x20000,
    Alpha                 = 0x40000,
    PAlpha                = 0x80000,
    Extended              = 0x100000,
    Canonical             = 0x200000,
    Undefined             = 0,
    DontCare              = 0,
    Format1bppIndexed     = 0x30101, 
    Format4bppIndexed     = 0x30402,
    Format8bppIndexed     = 0x30803,
    Format16bppGrayScale  = 0x101004,
    Format16bppRgb555     = 0x21005,
    Format16bppRgb565     = 0x21006,
    Format16bppArgb1555   = 0x61007,
    Format24bppRgb        = 0x21808,
    Format32bppRgb        = 0x22009,
    Format32bppArgb       = 0x26200A,
    Format32bppPArgb      = 0xE200B,
    Format48bppRgb        = 0x10300C,
    Format64bppArgb       = 0x34400D,
    Format64bppPArgb      = 0x1C400E,
    Max                   = 0xF
  }
}
