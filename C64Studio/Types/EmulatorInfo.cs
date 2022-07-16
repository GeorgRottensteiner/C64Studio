using RetroDevStudio.Types.ASM;
using RetroDevStudio;



namespace RetroDevStudio.Types
{
    public static class EmulatorInfo
    {
        public static bool IsMega65Family(ToolInfo Tool)
        {
            string uppercaseFilename = System.IO.Path.GetFileNameWithoutExtension(Tool.Filename).ToUpper();

            return uppercaseFilename.StartsWith("XMEGA65");
        }

        public static bool IsAtariFamily(ToolInfo Tool)
        {
            string uppercaseFilename = System.IO.Path.GetFileNameWithoutExtension(Tool.Filename).ToUpper();

            return uppercaseFilename.StartsWith("STELLA") || uppercaseFilename.StartsWith("ALTIRRA");
        }



        public static bool IsVICEFamily(ToolInfo Tool)
        {
            string uppercaseFilename = System.IO.Path.GetFileNameWithoutExtension(Tool.Filename).ToUpper();

            return ((uppercaseFilename.StartsWith("X64"))
                  || (uppercaseFilename.StartsWith("XSCPU64"))
                  || (uppercaseFilename.StartsWith("X128"))
                  || (uppercaseFilename.StartsWith("XCBM"))
                  || (uppercaseFilename.StartsWith("XPET"))
                  || (uppercaseFilename.StartsWith("XPLUS4"))
                  || (uppercaseFilename.StartsWith("XVIC")));
        }



        public static bool SupportsDebugging(ToolInfo Tool)
        {
            // currently only the VICE family is supported
            if ((IsVICEFamily(Tool))
            || (IsMega65Family(Tool)))
            {
                return true;
            }
            return false;
        }



        public static void SetDefaultRunArguments(ToolInfo Tool)
        {
            Tool.WorkPath = "\"$(RunPath)\"";
            Tool.Type = ToolInfo.ToolType.EMULATOR;
            Tool.PrgArguments = "\"$(RunFilename)\"";
            Tool.CartArguments = "";
            Tool.DebugArguments = "";
            Tool.TapeArguments = "\"$(RunFilename)\"";
            Tool.DiskArguments = "\"$(RunFilename)\"";
            Tool.PassLabelsToEmulator = false;

            string upperCaseFilename = System.IO.Path.GetFileNameWithoutExtension(Tool.Filename).ToUpper();

            if (IsVICEFamily(Tool))
            {
                // VICE
                Tool.Name = "WinVICE";
                Tool.PrgArguments = "\"$(RunFilename)\"";
                Tool.CartArguments = "-cartcrt \"$(RunFilename)\"";
                Tool.DebugArguments = "-initbreak 0x$(DebugStartAddressHex) -remotemonitor";
                Tool.TapeArguments = "\"$(RunFilename)\"";
                Tool.DiskArguments = "\"$(RunFilename)\"";
                Tool.AdditionalParameters.Add(new ToolInfoRuntimeParameter("True Drive On", "+truedrive -virtualdev"));
                Tool.AdditionalParameters.Add(new ToolInfoRuntimeParameter("True Drive Off", "-truedrive +virtualdev"));

                Tool.PassLabelsToEmulator = true;
            }
            else if (upperCaseFilename.StartsWith("CCS64"))
            {
                // CCS64
                Tool.Name = "CCS64";
                Tool.PassLabelsToEmulator = false;
            }
            else if (IsMega65Family(Tool))
            {
                // XMEGA65
                Tool.Name = "XMEGA65";
                Tool.PrgArguments = "-prg \"$(RunFilename)\"";
            }
            else if (upperCaseFilename.StartsWith("STELLA"))
            {
                // Atari Stella
                Tool.Name = "Stella";
            }
            else if (upperCaseFilename.StartsWith("ALTIRRA"))
            {
                // Atari Stella
                Tool.Name = "Altirra";
            }
            else if (upperCaseFilename.StartsWith("ORIC"))
            {
                // Atari Stella
                Tool.Name = "Oric";
            }
            else
            {
                // fallback
                Tool.Name = upperCaseFilename;
            }
        }



        public static LabelFileFormat LabelFormat(ToolInfo Tool)
        {
            string upperCaseFilename = System.IO.Path.GetFileNameWithoutExtension(Tool.Filename).ToUpper();

            if (upperCaseFilename.StartsWith("C64DEBUGGER"))
            {
                return LabelFileFormat.C64DEBUGGER;
            }
            return LabelFileFormat.VICE;
        }



        public static MachineType DetectMachineType(ToolInfo Tool)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(Tool.Filename).ToUpper();

            if ((filename.StartsWith("X64"))
            || (filename.StartsWith("XSCPU64"))
            || (filename.StartsWith("CCS64"))
            || (filename.StartsWith("C64DEBUGGER")))
            {
                return MachineType.C64;
            }
            else if (filename.StartsWith("XVIC"))
            {
                return MachineType.VIC20;
            }
            else if (filename.StartsWith("X128"))
            {
                return MachineType.C128;
            }
            else if (filename.StartsWith("XCBM"))
            {
                return MachineType.CBM;
            }
            else if (filename.StartsWith("XPET"))
            {
                return MachineType.PET;
            }
            else if (filename.StartsWith("XPLUS4"))
            {
                return MachineType.PLUS4;
            }
            else if (filename.StartsWith("STELLA"))
            {
                return MachineType.ATARI2600;
            }
            else if (filename.StartsWith("XMEGA65"))
            {
                return MachineType.MEGA65;
            }
            return MachineType.UNKNOWN;
        }
    }
}
