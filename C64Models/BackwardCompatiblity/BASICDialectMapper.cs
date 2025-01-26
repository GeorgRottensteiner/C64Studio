namespace RetroDevStudio.BackwardCompatibility
{
  public static class BASICDialectMapper
  {
    public enum ObsoleteBasicVersion
    {
      C64_BASIC_V2 = 0, //  Anpassung des VIC BASIC V2 für VC10, C64, C128 (C64-Modus), SX64, PET 64.
      V1_0,             // Version 1.0	Noch recht fehlerbehaftet, erste Versionen im PET 2001
      V2_0,             // August  Version 2.0	Fehlerkorrekturen, eingebaut in weitere Versionen des PET
      V3_0,             // Version 3.0	Leichte Fehlerkorrekturen und Integration des Maschinensprachemonitors TIM, schnelle Garbage Collection. CBM 3000 Serie (Standard) und PET 2001 (angezeigte Versionsnummer dort ist allerdings noch 2).
      V4_0,             // Version 4.0	Neue Befehle für leichtere Handhabung für Diskettenlaufwerke für CBM 4000. Auch durch ROM-Austausch für CBM 3000 Serie und PET 2001 verfügbar.
      V4_1,             // Version 4.1	Fehlerkorrigierte Fassung der Version 4.0 mit erweiterter Bildschirmeditor für CBM 8000. Auch durch ROM-Austausch für CBM 3000/4000 Serie verfügbar.
      V4_2,             // Version 4.2	Geänderte und ergänzte Befehle für den CBM 8096.
      VIC_BASIC_V2,     // Funktionsumfang von V 2.0 mit Korrekturen aus der Version-4-Linie. Einsatz im VC20.
      V4_PLUS,          // (intern bis 4.75)	Neue Befehle und RAM-Unterstützung bis 256 KByte für CBM-II-Serie (CBM 500, 6x0, 7x0). Fortsetzung und Ende der Version-4-Linie.
      V3_5,             // Version 3.5	Neue Befehle für die Heimcomputer C16/116 und Plus/4. Zusammenführung aus C64 BASIC V2 und Version-4-Linie.
      V3_6,             // Version 3.6	Neue Befehle für LCD-Prototypen.
      V7_0,             // Version 7.0	Neue Befehle für den C128/D/DCR. Weiterentwicklung des C16/116 BASIC 3.5 .
      V10_0,            // Version 10 Neue Befehle für C65, beinhaltet sehr viele Fehler, kam aus dem Entwicklungsstadium nicht heraus. Weiterentwicklung des BASIC 7.0.
      BASIC_LIGHTNING,  // BASIC extension
      LASER_BASIC,      // BASIC extension
      SIMONS_BASIC,     // Simons Basic
      SIMONS_BASIC_2    // Simons Basic Extended (SBX)
    }



    public static string BASICDialectKeyFromObsoleteEnum( uint version )
    {
      string  dialectKey = "BASIC V2";
      switch ( (ObsoleteBasicVersion)version )
      {
        case ObsoleteBasicVersion.C64_BASIC_V2:
          break;
        case ObsoleteBasicVersion.BASIC_LIGHTNING:
          dialectKey = "BASIC Lightning";
          break;
        case ObsoleteBasicVersion.LASER_BASIC:
          dialectKey = "Laser BASIC";
          break;
        case ObsoleteBasicVersion.SIMONS_BASIC:
          dialectKey = "Simon's BASIC";
          break;
        case ObsoleteBasicVersion.V3_5:
          dialectKey = "BASIC V3.5";
          break;
        case ObsoleteBasicVersion.V7_0:
          dialectKey = "BASIC V7.0";
          break;
      }
      return dialectKey;
    }



  }



}