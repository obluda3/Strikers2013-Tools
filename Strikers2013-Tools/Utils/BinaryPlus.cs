using System.IO;

namespace StrikersTools.Utils
{
    static class BinaryPlus
    {
        public static void PadWith(this BinaryWriter bw, byte padByte, long count)
        {
            for (var i = 0; i < count; i++)
                bw.Write(padByte);
        }

        public static uint PeekUInt32(this BinaryReader br)
        {
            var bkpos = br.BaseStream.Position;
            var output = br.ReadUInt32();
            br.BaseStream.Position = bkpos;
            return output;
        }
        public static uint ReadNibble(this BinaryReader br)
        {
            var bkpos = br.BaseStream.Position;
            var output = br.ReadUInt32();
            br.BaseStream.Position = bkpos;
            return output;
        }

        public static void WriteAlignment(this BinaryWriter bw, int alignment, byte padByte)
        {
            if (bw.BaseStream.Position % alignment == 0)
                return;

            var count = ((bw.BaseStream.Position / alignment) + 1) * alignment;
            bw.PadWith(padByte, count);
        }
    }
}
