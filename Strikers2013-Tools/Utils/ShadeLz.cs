using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StrikersTools.Utils
{

    class ShadeLz
    {
        /* Decompression code for Shade games; adapted from Scarlet which was
         * adapted from dr_dec.py by @BlackDragonHunt & @FireyFly
         * https://github.com/BlackDragonHunt/Danganronpa-Tools
         * 
         * ################################################################################
         * # Copyright © 2016 BlackDragonHunt
         * # This work is free. You can redistribute it and/or modify it under the
         * # terms of the Do What The Fuck You Want To Public License, Version 2,
         * # as published by Sam Hocevar. See the COPYING file for more details.
         * ################################################################################
         */
        public static byte[] Decompress(byte[] compressed)
        {
            var magic = new byte[] { 0xFC, 0xAA, 0x55, 0xA7 };

            var decompressedSize = 0;
            var compressedSize = 0;
            var pos = 0;
            if (compressed.Take(4).SequenceEqual(magic))
            {
                decompressedSize = BitConverter.ToInt32(compressed, 4);
                compressedSize = BitConverter.ToInt32(compressed, 8);
                pos = 12;
            }
            else
            {
                decompressedSize = CalculateDecompressedSize(compressed);
                compressedSize = compressed.Length;
            }
            var decompressed = new byte[decompressedSize];

            int inOffset = pos, outOffset = 0;
            int windowOffset = 0, count = 0, prevOffset = 0;

            while (outOffset < decompressedSize)
            {
                byte flags = compressed[inOffset++];

                if ((flags & 0x80) == 0x80)
                {
                    /* Copy data from the output.
                     * 1xxyyyyy yyyyyyyy
                     * Count -> x + 4
                     * Offset -> y
                     */
                    count = (((flags >> 5) & 0x3) + 4);
                    windowOffset = (((flags & 0x1F) << 8) + compressed[inOffset++]);
                    prevOffset = windowOffset;

                    for (int i = 0; i < count; i++)
                        decompressed[outOffset + i] = decompressed[(outOffset - windowOffset) + i];

                    outOffset += count;
                }
                else if ((flags & 0x60) == 0x60)
                {
                    /* Continue copying data from the output.
                     * 011xxxxx
                     * Count -> x
                     * Offset -> reused from above
                     */
                    count = (flags & 0x1F);
                    windowOffset = prevOffset;

                    for (int i = 0; i < count; i++)
                        decompressed[outOffset + i] = decompressed[(outOffset - windowOffset) + i];

                    outOffset += count;
                }
                else if ((flags & 0x40) == 0x40)
                {
                    /* Insert multiple copies of the next byte.
                     * 0100xxxx yyyyyyyy
                     * 0101xxxx xxxxxxxx yyyyyyyy
                     * Count -> x + 4
                     * Data -> y
                     */
                    if ((flags & 0x10) == 0x00)
                        count = ((flags & 0x0F) + 4);
                    else
                        count = ((((flags & 0x0F) << 8) + compressed[inOffset++]) + 4);

                    byte data = compressed[inOffset++];
                    for (int i = 0; i < count; i++)
                        decompressed[outOffset++] = data;
                }
                else if ((flags & 0xC0) == 0x00)
                {
                    /* Insert raw bytes from the input.
                     * 000xxxxx
                     * 001xxxxx xxxxxxxx
                     * Count -> x
                     */
                    if ((flags & 0x20) == 0x00)
                        count = (flags & 0x1F);
                    else
                        count = (((flags & 0x1F) << 8) + compressed[inOffset++]);

                    for (int i = 0; i < count; i++)
                        decompressed[outOffset++] = compressed[inOffset++];
                }
            }
            return decompressed;
        }

        private static int CalculateDecompressedSize(byte[] compressed)
        {
            var length = 0;

            int inOffset = 0, outOffset = 0;
            int windowOffset = 0, count = 0, prevOffset = 0;

            while (inOffset < compressed.Length)
            {
                byte flags = compressed[inOffset++];

                if ((flags & 0x80) == 0x80)
                {
                    count = (((flags >> 5) & 0x3) + 4);
                    windowOffset = (((flags & 0x1F) << 8) + compressed[inOffset++]);
                    prevOffset = windowOffset;

                }
                else if ((flags & 0x60) == 0x60)
                {
                    count = (flags & 0x1F);
                    windowOffset = prevOffset;
                }

                else if ((flags & 0x40) == 0x40)
                {
                    if ((flags & 0x10) == 0x00)
                        count = ((flags & 0x0F) + 4);
                    else
                        count = ((((flags & 0x0F) << 8) + compressed[inOffset++]) + 4);
                    inOffset += 1;
                }

                else if ((flags & 0xC0) == 0x00)
                {
                    if ((flags & 0x20) == 0x00)
                        count = (flags & 0x1F);
                    else
                        count = (((flags & 0x1F) << 8) + compressed[inOffset++]);

                    inOffset += count;
                }
                length += count;
            }
            return length;
        }

        // Not a real compressor yet, just produces valid files
        public static byte[] Compress(byte[] input)
        {
            var outputStream = new MemoryStream();
            byte[] output;
            using (var bw = new BinaryWriter(outputStream))
            {
                var pos = 0;
                while (pos < input.Length)
                {
                    var count = Math.Min(0x1FFF, input.Length - pos);
                    var controlBytes = new byte[] { (byte)(((count & 0x1F00) >> 8) | 0x20), (byte)(count & 0xFF) };

                    bw.Write(controlBytes);
                    for(var i = 0; i < count; i++)
                    {
                        bw.Write(input[pos + i]);
                    }
                    pos += count;
                }
                bw.BaseStream.Position = 0;
                output = outputStream.ToArray();
            }
            return output;
        }
    }
}
// https://github.com/xdanieldzd/Scarlet/blob/master/Scarlet.IO.CompressionFormats/SpikeDRVita.cs
/* Copy data from the output.
 * 1xxyyyyy yyyyyyyy
 * Count -> x + 4
 * Offset -> y
 */
/* Continue copying data from the output.
 * 011xxxxx
 * Count -> x
 * Offset -> reused from above
 */
/* Insert multiple copies of the next byte.
 * 0100xxxx yyyyyyyy
 * 0101xxxx xxxxxxxx yyyyyyyy
 * Count -> x + 4
 * Data -> y
 */
/* Insert raw bytes from the input.
 * 000xxxxx
 * 001xxxxx xxxxxxxx
 * Count -> x
 */