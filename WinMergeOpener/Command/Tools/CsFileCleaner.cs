using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMergeOpener.Command.Tools
{
    public static class CsFileCleaner
    {
        public static void Clean(string sourceFile)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile)) return;

                FileInfo fi = new FileInfo(sourceFile);
                if (!fi.Extension.Equals(".cs", StringComparison.CurrentCultureIgnoreCase)) return;

                var fileContent = File.ReadAllText(sourceFile, Encoding.UTF8);
                fileContent = RemoveServerAttributes(fileContent);
                fileContent = ReplaceFlags(fileContent);
                fileContent = SimplifyProperties(fileContent);

                File.WriteAllText(sourceFile, fileContent, Encoding.UTF8);
            }
            catch (Exception err)
            {
                Console.WriteLine("Error: " + err.Message);
            }
        }

        private static string RemoveServerAttributes(string content)
        {
            return content.Replace("\r\n[Server]", "")
                          .Replace("[Server]\r\n", "");
        }

        private static string ReplaceFlags(string content)
        {
            return content.Replace("AllowInInsert = false, AllowInUpdate = false, AllowInSelect = false", "Flags=ColumnFlags.None");
        }

        private static string SimplifyProperties(string content)
        {
            var matchesList = Regex.Matches(content, @"([sg])et\s{[\w\s;=]+\}");

            foreach (Match match in matchesList)
            {
                if (match.Value.Contains("Collection")) continue;
                string changeKey = match.Groups[1].Value;
                content = content.Replace(match.Value, changeKey + "et;");
            }

            return Regex.Replace(content, @"\s+\{\s+([sg]et;\s+)+\}", "{get;set;}");
        }
    }

}
