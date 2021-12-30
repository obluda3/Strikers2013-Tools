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

        public static void WriteAlignment(this BinaryWriter bw, int alignment, byte padByte = 0)
        {
            if (bw.BaseStream.Position % alignment == 0)
                return;

            var count = (((bw.BaseStream.Position / alignment) + 1) * alignment) - bw.BaseStream.Position;
            bw.PadWith(padByte, count);
        }

        public static void SeekAlignment(this BinaryReader br, int alignment)
        {
            if (br.BaseStream.Position % alignment == 0)
                return;

            br.BaseStream.Position = ((br.BaseStream.Position / alignment) + 1) * alignment;
        }
        public static void SeekAlignment(this BinaryWriter bw, int alignment)
        {
            if (bw.BaseStream.Position % alignment == 0)
                return;

            bw.BaseStream.Position = ((bw.BaseStream.Position / alignment) + 1) * alignment;
        }

        public static byte[] Padded(this byte[] input, int alignment)
        {
            var length = input.Length;
            if (length % alignment == 0) return input;

            var newLength = ((length / alignment) + 1) * alignment;
            var output = new byte[(int)newLength];
            input.CopyTo(output, 0);
            return output;
        }
    }
}
