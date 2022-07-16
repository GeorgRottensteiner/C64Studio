using System.Collections.Generic;

namespace RetroDevStudio
{
    public class ToolInfo
    {
        public enum ToolType
        {
            UNKNOWN,
            ASSEMBLER,
            EMULATOR
        };

        public ToolType Type = ToolType.UNKNOWN;
        public string Name = "";
        public string Filename = "";
        public string WorkPath = "";
        public string PrgArguments = "";
        public string CartArguments = "";
        public string DebugArguments = "";
        public string TapeArguments = "";
        public string DiskArguments = "";
        //        public string TrueDriveOnArguments = "";
        //        public string TrueDriveOffArguments = "";
        public bool PassLabelsToEmulator = true;
        public List<ToolInfoRuntimeParameter> AdditionalParameters;

        public ToolInfo()
        {
            AdditionalParameters = new List<ToolInfoRuntimeParameter>();
        }

        public GR.IO.FileChunk ToChunk()
        {
            GR.IO.FileChunk chunk = new GR.IO.FileChunk(FileChunkConstants.SETTINGS_TOOL);

            chunk.AppendU32((uint)Type);
            chunk.AppendString(Name);
            chunk.AppendString(Filename);
            chunk.AppendString(PrgArguments);
            chunk.AppendString(DebugArguments);
            chunk.AppendString(WorkPath);
            chunk.AppendString(CartArguments);
            chunk.AppendString(TapeArguments);
            chunk.AppendString(DiskArguments);
            chunk.AppendU8(PassLabelsToEmulator ? (byte)0 : (byte)1);
            foreach(var param in AdditionalParameters)
            {
                param.ToChunk(chunk);
            }

            return chunk;
        }

        public bool FromChunk(GR.IO.FileChunk Chunk)
        {
            if (Chunk.Type != FileChunkConstants.SETTINGS_TOOL)
            {
                return false;
            }

            GR.IO.IReader reader = Chunk.MemoryReader();

            Type = (ToolType)reader.ReadUInt32();
            Name = reader.ReadString();
            Filename = reader.ReadString();
            PrgArguments = reader.ReadString();
            DebugArguments = reader.ReadString();
            WorkPath = reader.ReadString();
            CartArguments = reader.ReadString();
            TapeArguments = reader.ReadString();
            DiskArguments = reader.ReadString();
            PassLabelsToEmulator = (reader.ReadUInt8() == 0);
            while (reader.Position < reader.Size)
            {
                var param = new ToolInfoRuntimeParameter();
                if (param.FromChunk(reader)) AdditionalParameters.Add(param);
            }    
            return true;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
