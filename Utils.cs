using System;
using System.IO;

namespace ManifestGet
{
    class Utils
    {
        public static bool HasParameter(string[] args, string param)
        {
            return IndexOfParam(args, param) > -1;
        }
        public static int IndexOfParam(string[] args, string param)
        {
            for (var x = 0; x < args.Length; ++x)
            {
                if (args[x].Equals(param, StringComparison.OrdinalIgnoreCase))
                    return x;
            }

            return -1;
        }
        public static void WriteToFile(string msg, string filename)
        {
            FileInfo logFileInfo = new FileInfo(filename);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using (FileStream fileStream = new FileStream(filename, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine(DateTime.Now + " | " + msg);
                }
            }
        }
    }
}
