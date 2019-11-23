using System.IO;

namespace DataLoggerApp.FileHelpers
{
    public static class FileHelper
    {
        public static void WriteTextFile(string filePath, string text, bool append)
        {
            using StreamWriter tw = new StreamWriter(filePath, append);
            tw.WriteLine(text);
        }
    }
}