using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrikersTools.Dictionaries
{
    public static class TextDecoder
    {
        public static Dictionary<byte, char> CustomChars = new Dictionary<byte, char>()
        {
            [0xA8] = 'Á',
            [0xA9] = 'á',
            [0xAA] = 'À',
            [0xAB] = 'à',
            [0xAC] = 'Ă',
            [0xAD] = 'ă',
            [0xAE] = 'Â',
            [0xAF] = 'â',
            [0xB0] = 'Ä',
            [0xB1] = 'ä',
            [0xB2] = 'Ç',
            [0xB3] = 'ç',
            [0xB4] = 'É',
            [0xB5] = 'é',
            [0xB6] = 'È',
            [0xB7] = 'è',
            [0xB8] = 'Ê',
            [0xB9] = 'ê',
            [0xBA] = 'Ë',
            [0xBB] = 'ë',
            [0xBC] = 'ß',
            [0xBD] = '¡',
            [0xBE] = 'Í',
            [0xBF] = 'í',
            [0xC0] = 'Ì',
            [0xC1] = 'ì',
            [0xC2] = 'Î',
            [0xC3] = 'î',
            [0xC4] = 'Ï',
            [0xC5] = 'ï',
            [0xC6] = '¿',
            [0xC7] = 'Ñ',
            [0xC8] = 'ñ',
            [0xC9] = 'Ó',
            [0xCA] = 'ó',
            [0xCB] = 'Ò',
            [0xCC] = 'ò',
            [0xCD] = 'Ô',
            [0xCE] = 'ô',
            [0xCF] = 'Ö',
            [0xD0] = 'ö',
            [0xD1] = 'Ú',
            [0xD2] = 'ú',
            [0xD3] = 'Ù',
            [0xD4] = 'ù',
            [0xD5] = 'Û',
            [0xD6] = 'û',
            [0xD7] = 'Ü',
            [0xD8] = 'ü',
            [0xD9] = 'Ã',
            [0xDA] = 'ã',
            [0xDB] = 'Õ',
            [0xDC] = 'õ',
            [0xDD] = '°',
        };

        public static string Decode(byte[] input) 
        {
            var i = 0;
            var output = "";
            if (input.Length < 1)
                return "";

            do
            {
                // HalfWidth char
                if(input[i] < 0x80)
                {
                    if (input[i] == 0x0a)
                        output += "{returnline}";
                    else
                        output += Encoding.GetEncoding("sjis").GetString(input.Skip(i).Take(1).ToArray());
                    i += 1;
                }
                
                // FullWidth char
                else if ((input[i] > 0x80 && input[i] < 0xA0) || input[i] >= 0xe0)
                {
                    output += Encoding.GetEncoding("sjis").GetString(input.Skip(i).Take(2).ToArray());
                    i += 2;
                }

                // Custom accent
                else
                {
                    output += CustomChars[input[i]];
                    i += 1;
                }
                if (i == input.Length)
                    break;
            }
            while (input[i] != 0);

            return output;
        }

        public static byte[] Encode(string input)
        {
            if (input == "") return new byte[0];
            input = input.Replace("{returnline}", "\n");
            var output = new List<byte>();
            foreach(var character in input)
            {
                // This is the worst way of checking that kind of thing but i can't think of something else
                var isSpecialChar = false;
                var customEncoded = (byte)0;
                foreach(var kvp in CustomChars)
                {
                    if(kvp.Value == character)
                    {
                        isSpecialChar = true;
                        customEncoded = kvp.Key;
                        output.Add(customEncoded);
                        break;
                    }
                }

                if(!isSpecialChar)
                {
                    var sjisEncoded = Encoding.GetEncoding("sjis").GetBytes(character.ToString());
                    sjisEncoded.ToList().ForEach(x => output.Add(x));
                }

            }

            if (output.Count == 0)
                output.Add(0);

            return output.ToArray();
        }
    }
}
