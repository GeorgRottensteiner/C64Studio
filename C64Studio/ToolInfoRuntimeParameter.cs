namespace RetroDevStudio
{
    public class ToolInfoRuntimeParameter
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public ToolInfoRuntimeParameter() : this(string.Empty, string.Empty) { }

        public ToolInfoRuntimeParameter(string name, string value, bool enabled = true)
        {
            Name = name;
            Value = value;
            Enabled = enabled;
        }

        public override string ToString()
        {
            return $"{Name} : {Value}";
        }

        public bool ToChunk(GR.IO.FileChunk chunk)
        {
            if (chunk == null) return false;

            if (chunk.Type != FileChunkConstants.SETTINGS_TOOL) return false;
            if (string.IsNullOrEmpty(Name)) return false;
            if (string.IsNullOrEmpty(Value)) return false;

            chunk.AppendString(Name);
            chunk.AppendString(Value);
            chunk.AppendU8(Enabled ? (byte)1 : (byte)0);

            return true;
        }

        public bool FromChunk(GR.IO.IReader reader)
        {
            if (reader == null) return false;
            if (reader.Position >= reader.Size) return false;
            Name = reader.ReadString();
            Value = reader.ReadString();
            Enabled = (reader.ReadUInt8() > 0);
            return true;
        }
    }
}
