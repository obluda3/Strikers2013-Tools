using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopaEditor.Dictionaries
{
    public static class SpecialChars
    {
        public static Dictionary<string, string> FrenchAccents = new Dictionary<string, string>()
        {
            ["À"] = "$",
            ["à"] = "@",
            ["É"] = "&",
            ["é"] = "*",
            ["è"] = "+",
            ["Ê"] = "[",
            ["ê"] = "<",
            ["î"] = "]",
            ["ï"] = "^",
            ["ô"] = "_",
            ["ù"] = ">",
            ["Ç"] = "{",
            ["ç"] = "|",
            ["Ö"] = "}",
            ["ă"] = @"\",
        };
    }
}
