using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace TextRendering
{
    internal class FileExplorer
    {
        public static string GetCurrentDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public static string FilePath(string path)
        {
            return Path.Combine(GetCurrentDirectory(), path);
        }
    }
}
