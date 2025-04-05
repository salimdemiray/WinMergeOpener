using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMergeOpener.Command.Tools
{
    public static class XmlFileCleaner
    {
        public static void Clean(string sourceFile)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile)) return;

                FileInfo fi = new FileInfo(sourceFile);
                if (!fi.Extension.Equals(".xml", StringComparison.CurrentCultureIgnoreCase)) return;

                var fileContent = File.ReadAllText(sourceFile, Encoding.UTF8);
                fileContent = ReplaceTrueFalseValues(fileContent);
                fileContent = RemoveUnnecessaryAttributes(fileContent);
                fileContent = CleanExtraSpaces(fileContent);

                File.WriteAllText(sourceFile, fileContent, Encoding.UTF8);
            }
            catch (Exception err)
            {
                Console.WriteLine("Error: " + err.Message);
            }
        }

        private static string ReplaceTrueFalseValues(string content)
        {
            return content.Replace("\"True\"", "\"true\"")
                          .Replace("\"False\"", "\"false\"");
        }

        private static string RemoveUnnecessaryAttributes(string content)
        {
            return content.Replace(" Focused=\"true\"", "")
                          .Replace(" ControlEnabled=\"true\"", "")
                          .Replace(" CaptionVisibility=\"true\"", "")
                          .Replace(" CaptionVisible=\"true\"", "")
                          .Replace(" ControlRequired=\"false\"", "")
                          .Replace(" Visibility=\"true\"", "")
                          .Replace(" ControlVisible=\"true\"", "")
                          .Replace(" ControlSingleLine=\"true\"", "")
                          .Replace(" ControlEditEnabled=\"true\"", "")
                          .Replace(" ServerAttribute=\"\"", "")
                          .Replace(" DecimalPlaces=\"2\"", "")
                          .Replace(" DecimalPlaces=\"0\"", " DecimalPlaces=\"Sıfır\"")
                          .Replace("Caption=\"\"", "")
                          .Replace("SourceType=\"Command\"", "")
                          .Replace("ProcessTypeMode=\"1\"", "")
                          .Replace("colspan=\"1\"", "")
                          .Replace("ListPropertyTable=\"\"", "")
                          .Replace("DefaultValue=\"\"", "");
        }

        private static string CleanExtraSpaces(string content)
        {
            content = Regex.Replace(content, @"([\w\""])([ ]{2,25})(\w)", "$1 $3");
            content = Regex.Replace(content, @"(""[ ]+)(\/\>)", "\"$2");
            content = Regex.Replace(content, @"([\""\w])([ ]+)(\>)", "$1$3");
            return content;
        }
    }
}
