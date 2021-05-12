using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFilesServer.Helper
{
    public static class FileHelper
    {
        public static string[] ReadAllLines(string path)
        {
            if (File.Exists(path))
            {
                List<string> lines = new List<string>();
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                        lines.Add(reader.ReadLine());
                }
                return lines.ToArray();
            }
            return null;
        }
    }
}
