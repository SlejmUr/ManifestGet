using System;
using System.ComponentModel;
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
        public static T GetParameter<T>(string[] args, string param, T defaultValue = default(T))
        {
            var index = IndexOfParam(args, param);

            if (index == -1 || index == (args.Length - 1))
                return defaultValue;

            var strParam = args[index + 1];

            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                return (T)converter.ConvertFromString(strParam);
            }

            return default(T);
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
