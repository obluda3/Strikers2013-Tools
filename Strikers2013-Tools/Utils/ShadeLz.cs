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
                    count = (((flags >> 5) & 0x3) + 4);
                    windowOffset = (((flags & 0x1F) << 8) + compressed[inOffset++]);
                    prevOffset = windowOffset;

                    for (int i = 0; i < count; i++)
                        decompressed[outOffset + i] = decompressed[(outOffset - windowOffset) + i];

                    outOffset += count;
                }
                else if ((flags & 0x60) == 0x60)
                {
                    count = (flags & 0x1F);
                    windowOffset = prevOffset;

                    for (int i = 0; i < count; i++)
                        decompressed[outOffset + i] = decompressed[(outOffset - windowOffset) + i];

                    outOffset += count;
                }
                else if ((flags & 0x40) == 0x40)
                {
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
                    if ((flags & 0x20) == 0x00)
                        count = flags;
                    else
                        count = (((flags & 0x1F) << 8) + compressed[inOffset++]);
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
                    decompressedSize += ((flag >> 5) & 0x3) + 4;
                    inOffset++;
                }
                else if ((flag & 0x60) == 0x60)
                {
                    decompressedSize += flag & 0x1F;
                }
                else if ((flag & 0x40) == 0x40)
                {
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

            int rawLength, runLength, longestMatchLen, matchOffset, pos, length;
            rawLength = runLength = longestMatchLen = matchOffset = pos = 0;
            length = data.Length;
            bool match = false;

            Dictionary<int, List<int>> PatternHistory = new Dictionary<int, List<int>>();

            while (pos + rawLength < length)
            {
                var currentPos = pos + rawLength;
                byte curByte = data[currentPos];

                int hash;

                if (length - currentPos < 3) hash = -1;
                else
                {
                    byte b1 = curByte, b2 = data[currentPos + 1], b3 = data[currentPos + 2];
                    hash = b1 | (b2 << 8) | (b3 << 16);
                }
                List<int> listPos;

                var alreadyExists = PatternHistory.TryGetValue(hash, out listPos);
                if (!alreadyExists) listPos = new List<int>();

                listPos.Add(currentPos);
                PatternHistory[hash] = listPos;


                if (!match && length - currentPos > 4)
                {
                    // check LZ Match
                    if (alreadyExists)
                    {
                        var prevPositions = PatternHistory[hash];
                        var longestMatchPos = 0;
                        longestMatchLen = 0;
                        for (var i = prevPositions.Count - 2; i >= 0; i--) // skips the last one cuz we just added it
                        {
                            var currentMatch = prevPositions[i];
                            if (currentPos + longestMatchLen >= length)
                                break;
                            if (currentPos - currentMatch > 0x1FFF)
                            {
                                prevPositions.RemoveAll(x => x <= currentMatch);
                                PatternHistory[hash] = prevPositions;
                                break;
                            }

                            var curMatchLen = 0;
                            // If it's not at least as long as the current match, it's useless
                            if (data[currentMatch + longestMatchLen] == data[currentPos + longestMatchLen])
                            {
                                while (data[currentPos + curMatchLen] == data[currentMatch + curMatchLen])
                                {
                                    curMatchLen++;
                                    if (currentPos + curMatchLen >= data.Length)
                                        break;
                                }
                                if (curMatchLen > longestMatchLen)
                                {
                                    longestMatchPos = currentMatch;
                                    longestMatchLen = curMatchLen;
                                }
                            }
                        }
                        matchOffset = currentPos - longestMatchPos;
                    }
                    // checking rle
                    runLength = 1;
                    while (currentPos + runLength < length)
                    {
                        if (data[currentPos + runLength] != curByte)
                        {
                            break;
                        }
                        runLength++;
                    }
                    match = runLength >= 4 || longestMatchLen >= 4;
                }
                if (match)
                {
                    if (rawLength > 0)
                    {
                        var rawBytes = new byte[rawLength];
                        Array.Copy(data, pos, rawBytes, 0, rawLength);
                        EncodeRawBytes(rawBytes, rawLength, output);
                        pos += rawLength;
                        rawLength = 0;
                    }

                    int copyLen = runLength;
                    if (longestMatchLen > runLength) // LZ
                    {
                        copyLen = longestMatchLen;
                        EncodeMatch(longestMatchLen, matchOffset, output);
                        pos += longestMatchLen;
                    }
                    else // RLE
                    {
                        EncodeRun(runLength, curByte, output);
                        pos += runLength;
                    }
                    // update history
                    if (currentPos + copyLen + 1 < length)
                    {
                        for (var i = currentPos + 1; i < currentPos + copyLen; i++)
                        {
                            byte b1 = data[i], b2 = data[i + 1], b3 = data[i + 2];
                            hash = b1 | (b2 << 8) | (b3 << 16);

                            List<int> prevPositions;

                            var listExists = PatternHistory.TryGetValue(hash, out prevPositions);
                            if (!listExists) prevPositions = new List<int>();

                            prevPositions.Add(i);
                            PatternHistory[hash] = prevPositions;
                        }
                    }
                }
                else rawLength++;

                match = false;
                runLength = longestMatchLen = matchOffset = 0;

            }
            if (rawLength > 0)
            {
                var rawBytes = new byte[rawLength];
                Array.Copy(data, pos, rawBytes, 0, rawLength);
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

        private static void EncodeRun(int runLength, byte repeatedByte, Stream output)
        {
            var curLen = runLength;
            while (curLen - 4 > 0xFFF)
            {
                output.WriteByte(0x5F);
                output.WriteByte(0xFF);
                output.WriteByte(repeatedByte);
                curLen -= 0x1003;
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

        private static void EncodeRawBytes(byte[] rawBytes, int rawLength, Stream output)
        {
            if (rawLength < 0x20)
            {
                output.WriteByte((byte)rawLength);
                output.Write(rawBytes.ToArray(), 0, rawLength);
            }
            else
            {
                var pos = 0;
                var curLen = rawLength;
                while (curLen > 0x1FFF)
                {

                    output.WriteByte(0x3F);
                    output.WriteByte(0xFF);
                    for (var i = pos; i < pos + 0x1fff; i++) output.WriteByte(rawBytes[i]);
                    pos += 0x1FFF;
                    curLen -= 0x1FFF;

                }
                if (curLen < 0x1f)
                {
                    output.WriteByte((byte)curLen);
                    for (var i = pos; i < pos + curLen; i++) output.WriteByte(rawBytes[i]);
                }
                else
                {
                    output.WriteByte((byte)(0x20 | ((0x1F00 & curLen) >> 8)));
                    output.WriteByte((byte)(curLen & 0xFF));
                    for (var i = pos; i < pos + curLen; i++) output.WriteByte(rawBytes[i]);
                }
            }
        }

        
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
