using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

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
                fileContent = FormatXml(fileContent);

                File.WriteAllText(sourceFile, fileContent, Encoding.UTF8);
            }
            catch (Exception err)
            {
                Console.WriteLine("Error: " + err.Message);
            }
        }

        static string ReplaceTrueFalseValues(string content)
        {
            return content.Replace("\"True\"", "\"true\"")
                          .Replace("\"False\"", "\"false\"");
        }

        static string RemoveUnnecessaryAttributes(string content)
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
                          .Replace(" DecimalPlaces=\"0\"", " DecimalPlaces=\"Sýfýr\"")
                          .Replace("Caption=\"\"", "")
                          .Replace("SourceType=\"Command\"", "")
                          .Replace("ProcessTypeMode=\"1\"", "")
                          .Replace("colspan=\"1\"", "")
                          .Replace("ListPropertyTable=\"\"", "")
                          .Replace("DefaultValue=\"\"", "");
        }

        static string CleanExtraSpaces(string content)
        {
            content = Regex.Replace(content, @"([\w\""])([ ]{2,25})(\w)", "$1 $3");
            content = Regex.Replace(content, @"(""[ ]+)(\/\>)", "\"$2");
            content = Regex.Replace(content, @"([\""\w])([ ]+)(\>)", "$1$3");
            return content;
        }

        static string FormatXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml); // XML stringini parse eder

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = " ", // 2 boþluk
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,                
                Encoding = System.Text.Encoding.UTF8,
                OmitXmlDeclaration = false // <?xml ... ?> satýrý kalsýn mý?
            };

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                doc.Save(xmlWriter);
                return stringWriter.ToString(); // Formatlanmýþ XML stringi
            }
        }
    }
}
