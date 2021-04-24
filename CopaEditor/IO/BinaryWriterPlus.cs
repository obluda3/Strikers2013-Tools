using System.IO;

namespace StrikersTools.IO
{
    static class BinaryWriterPlus
    {
        public static void PadWith(this BinaryWriter bw, byte padByte, long count)
        {
            for (var i = 0; i < count; i++)
                bw.Write(padByte);
        }

    }
}
