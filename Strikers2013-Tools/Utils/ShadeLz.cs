using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
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

            var decompressedSize = 0L;
            var compressedSize = 0L;
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

                    Console.Write($"(LZ <-{windowOffset:X},{count:X}>) ");
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

                    Console.Write($"(LZ <-{windowOffset:X},{count:X}>) ");
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
                    Console.Write($"(RLE <{data.ToString("X2")}, {count}>) ");
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
                        count = flags;
                    else
                        count = (((flags & 0x1F) << 8) + compressed[inOffset++]);

                    var buffer = compressed.Skip(inOffset).Take(count).ToArray();
                    Console.Write(BitConverter.ToString(buffer).Replace('-', ' '));
                    Console.Write(" ");
                    for (int i = 0; i < count; i++)
                        decompressed[outOffset++] = compressed[inOffset++];
                }
            }
            return decompressed;
        }

        private static long CalculateDecompressedSize(byte[] input)
        {
            // https://github.com/FanTranslatorsInternational/Kuriimu2/blob/dev/src/Kompression/Implementations/Decoders/Headerless/ShadeLzHeaderlessDecoder.cs
            var decompressedSize = 0L;
            var inOffset = 0L;
            while (inOffset < input.Length)
            {
                var flag = input[inOffset++];

                if ((flag & 0x80) == 0x80)
                {
                    // Lz match start
                    decompressedSize += ((flag >> 5) & 0x3) + 4;
                    inOffset++;
                }
                else if ((flag & 0x60) == 0x60)
                {
                    // Lz match continue
                    decompressedSize += flag & 0x1F;
                }
                else if ((flag & 0x40) == 0x40)
                {
                    // Rle data
                    int length;
                    if ((flag & 0x10) == 0x00)
                        length = (flag & 0xF) + 4;
                    else
                        length = ((flag & 0xF) << 8) + input[inOffset++] + 4;

                    inOffset++;
                    decompressedSize += length;
                }
                else
                {
                    // Raw data
                    int length;
                    if ((flag & 0x20) == 0x00)
                        length = flag;
                    else
                        length = ((flag & 0x1F) << 8) + input[inOffset++];

                    inOffset += length;
                    decompressedSize += length;
                }
            }

            return decompressedSize;
        }
        public static byte[] Compress(byte[] data, bool needsHeader)
        {
            var output = new MemoryStream();

            if (needsHeader)
                for (var i = 0; i < 12; i++) output.WriteByte(0);

            var pos = 0;
            var length = data.Length;
            var rawLength = 0;
            int runLength = 0;
            int matchLength = 0;
            int matchOffset = 0;
            MatchType match = MatchType.None;

            int[] LastSeenPosition = new int[0x100];
            Dictionary<int, int> BackPos = new Dictionary<int, int>();
            List<byte> rawBytes = new List<byte>();

            while (pos + rawLength < length)
            {
                var currentPos = pos + rawLength;
                byte curByte = data[currentPos];
                if (match == MatchType.None)
                {
                    if (length - currentPos > 4)
                    {
                        // check LZ Match
                        if (LastSeenPosition[curByte] != 0)
                        {
                            var prevPos = LastSeenPosition[curByte];
                            match = MatchType.LZ;
                            if (currentPos - prevPos < 0x1FFF)
                            {
                                matchLength = 0;
                                var curMatchLen = 0;
                                while (data[currentPos + curMatchLen] == data[prevPos + curMatchLen])
                                {
                                    curMatchLen++;
                                    if (currentPos + curMatchLen >= data.Length)
                                        break;
                                }

                                var longestMatchIndex = prevPos;
                                matchLength = curMatchLen;

                                while (BackPos.ContainsKey(prevPos) && BackPos[prevPos] != 0)
                                {
                                    curMatchLen = 0;
                                    prevPos = BackPos[prevPos];
                                    if (currentPos - prevPos > 0x1FFF) break;
                                    while (data[currentPos + curMatchLen] == data[prevPos + curMatchLen])
                                    {
                                        curMatchLen++;
                                        if (currentPos + curMatchLen >= data.Length)
                                            break;
                                    }
                                    longestMatchIndex = curMatchLen > matchLength ? prevPos : longestMatchIndex;
                                    matchLength = Math.Max(matchLength, curMatchLen);
                                }
                                matchOffset = currentPos - longestMatchIndex;
                                if (matchLength < 4) match = MatchType.None;
                            }
                            else match = MatchType.None;
                        }
                        // checking rle
                        match = MatchType.RLE;
                        runLength = 1;
                        for (var i = currentPos + 1; i < currentPos + 4; i++)
                        {
                            if (data[i] != curByte)
                            {
                                match = MatchType.None;
                                break;
                            }
                            runLength++;
                        }
                        if (match == MatchType.RLE)
                        {
                            while (currentPos + runLength < length)
                            {
                                if (data[currentPos + runLength] != curByte)
                                    break;
                                runLength++;
                            }
                        }

                        if (match == MatchType.None && matchLength >= 4) match = MatchType.LZ;

                    }

                }
                if (match != MatchType.None)
                {
                    if (rawLength > 0)
                    {
                        EncodeRawBytes(rawBytes, rawLength, output);
                        pos += rawLength;
                        rawBytes.Clear();
                        rawLength = 0;
                    }

                    if (matchLength > runLength) match = MatchType.LZ;
                    else match = MatchType.RLE;
                    if (match == MatchType.RLE)
                    {
                        EncodeRun(runLength, curByte, output);
                        for(var i = currentPos; i < currentPos + runLength; i++)
                        {
                            var oldPrevPos = LastSeenPosition[curByte];

                            LastSeenPosition[curByte] = i;
                            BackPos[i] = oldPrevPos;
                        }
                        pos += runLength;
                    }
                    else if (match == MatchType.LZ)
                    {
                        EncodeMatch(matchLength, matchOffset, output);

                        // update positions
                        for (var i = currentPos; i < currentPos + matchLength; i++)
                        {
                            var currentByte = data[i];
                            var oldPrevPos = LastSeenPosition[currentByte];

                            LastSeenPosition[currentByte] = i;
                            BackPos[i] = oldPrevPos;
                        }
                        pos += matchLength;
                    }

                    match = MatchType.None;
                }
                else
                {
                    rawBytes.Add(curByte);
                    var oldPrevPos = LastSeenPosition[curByte];
                    LastSeenPosition[curByte] = currentPos;
                    BackPos[currentPos] = oldPrevPos;
                    rawLength++;
                }
                match = MatchType.None;
                runLength = matchLength = matchOffset = 0;

            }
            if (rawLength > 0)
            {
                EncodeRawBytes(rawBytes, rawLength, output);
            }

            if (needsHeader)
            {
                output.Position = 0;
                var magic = new byte[] { 0xFC, 0xAA, 0x55, 0xA7 };
                var decompressedSize = BitConverter.GetBytes(length);
                var compressedSize = BitConverter.GetBytes(output.Length);

                output.Write(magic, 0, 4);
                output.Write(decompressedSize, 0, 4);
                output.Write(compressedSize, 0, 4);

            }

            return output.ToArray();
        }

        // Not a real compressor yet, just produces valid files
        private static void EncodeRun(int runLength, byte repeatedByte, Stream output)
        {
            var curLen = runLength;
            while (curLen - 4 > 0xFFF)
            {
                output.WriteByte(0x5F);
                output.WriteByte(0xFF);
                output.WriteByte(repeatedByte);
                curLen -= 0xFFF;
            }
            if (curLen - 4 < 0x10)
            {
                output.WriteByte((byte)(0x40 | (curLen - 4)));
                output.WriteByte(repeatedByte);
            }
            else
            {
                output.WriteByte((byte)(0x50 | ((0xF00 & (curLen - 4)) >> 8)));
                output.WriteByte((byte)((curLen - 4) & 0xFF));
                output.WriteByte(repeatedByte);
            }
        }
        private static void EncodeMatch(int matchLength, int matchOffset, Stream output)
        {
            if (matchLength < 8)
            {
                output.WriteByte((byte)(0x80 | ((matchLength - 4) << 5) | ((matchOffset & 0x1F00) >> 8)));
                output.WriteByte((byte)(matchOffset & 0xFF));
            }
            else
            {
                var curMatchLen = matchLength;
                output.WriteByte((byte)(0xE0 | ((matchOffset & 0x1F00) >> 8)));
                output.WriteByte((byte)(matchOffset & 0xFF));

                curMatchLen -= 7;
                while (curMatchLen > 0x1f)
                {
                    output.WriteByte(0x7F);
                    curMatchLen -= 0x1F;
                }
                output.WriteByte((byte)(0x60 | (curMatchLen & 0x1F)));
            }
        }

        private static void EncodeRawBytes(List<byte> rawBytes, int rawLength, Stream output)
        {
            if (rawBytes.Count != rawLength)
                Console.WriteLine("wtf");
            if (rawLength < 0x20)
            {
                output.WriteByte((byte)rawLength);
                output.Write(rawBytes.ToArray(), 0, rawLength);
                rawBytes.Clear();
            }
            else
            {
                var curLen = rawLength;
                while (curLen > 0x1FFF)
                {

                    output.WriteByte(0x3F);
                    output.WriteByte(0xFF);
                    output.Write(rawBytes.Take(0x1FFF).ToArray(), 0, 0x1FFF);
                    rawBytes.RemoveRange(0, 0x1FFF);
                    curLen -= 0x1FFF;

                }
                if (curLen < 0x1f)
                {
                    output.WriteByte((byte)rawLength);
                    output.Write(rawBytes.Take(curLen).ToArray(), 0, curLen);
                    rawBytes.RemoveRange(0, curLen);
                }
                else
                {
                    output.WriteByte((byte)(0x20 | ((0x1F00 & curLen) >> 8)));
                    output.WriteByte((byte)(curLen & 0xFF));
                    output.Write(rawBytes.Take(curLen).ToArray(), 0, curLen);
                    rawBytes.RemoveRange(0, curLen);
                }
            }
        }
        private enum MatchType { None, LZ, RLE }


        /*
        public static byte[] LegacyCompress(byte[] input)
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
                    for (var i = 0; i < count; i++)
                    {
                        bw.Write(input[pos + i]);
                    }
                    pos += count;
                }
                bw.BaseStream.Position = 0;
                output = outputStream.ToArray();
            }
            return output;
        }*/
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

// if !wasMatchFound

// Check for LZ Match (table of last usage and everything)
// if there is, get offset and length

// Check for RLE Run
// if there is, get length and byte

// Increase raw length

// if wasMatchFound
// Raw output
// If LZ
// Output lz match
// reset everything
// If RLE
// Output RLE run