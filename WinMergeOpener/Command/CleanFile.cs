using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace WinMergeOpener.Command
{
    //\[.*TitleCaption\(\"(?<a>.*)\".*\s.*public\s(?<t>\w*)\s(?<d>\w*)
    public class CleanFile : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.Equals("c", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            CleanOperation(ArgParam.FileName, ConsoleParam);
        }

        protected void CleanOperation(string fileName, ConsoleParam consoleParam)
        {
            CleanXmlTrueFalse(fileName);
            CleanCsFile(fileName);

            if (consoleParam.TargetVersions.Length == 0) return;

            foreach (var prST in consoleParam.TargetVersions)
            {
                if (prST.VersionNo == 1) continue;

                try
                {
                    var f2 = _VersionManager.FindTargetPath(fileName, prST);

                    if (string.IsNullOrEmpty(f2)) continue;

                    CleanXmlTrueFalse(f2);
                    CleanCsFile(f2);
                }
                catch
                {
                }
            }
        }

        void CleanXmlTrueFalse(string sourceFile)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFile)) return;
                if (!File.Exists(sourceFile)) return;
                FileInfo fi = new FileInfo(sourceFile);
                if (!fi.Extension.Equals(".xml", StringComparison.CurrentCultureIgnoreCase)) return;
                var K1 = File.ReadAllText(sourceFile, Encoding.UTF8);

                K1 = K1.Replace("\"True\"", "\"true\"").Replace("\"False\"", "\"false\"");
                K1 = K1.Replace(" Focused=\"true\"", "").Replace(" ControlEnabled=\"true\"", "");
                K1 = K1.Replace(" CaptionVisibility=\"true\"", "").Replace(" CaptionVisible=\"true\"", "")
                    .Replace(" ControlRequired=\"false\"", "").Replace(" Visibility=\"true\"", "")
                    .Replace(" ControlVisible=\"true\"", "").Replace(" ControlVisible=\"true\"", "")
                    .Replace(" ControlSingleLine=\"true\"", "").Replace(" ControlEditEnabled=\"true\"", "")
                    .Replace(" ServerAttribute=\"\"", "").Replace(" DecimalPlaces=\"2\"", "")
                    .Replace(" DecimalPlaces=\"0\"", " DecimalPlaces=\"Sıfır\"")
                    .Replace("Caption=\"\"", "").Replace("SourceType=\"Command\"", "").Replace("ProcessTypeMode=\"1\"", "")
                    .Replace("colspan=\"1\"", "").Replace("ListPropertyTable=\"\"", "").Replace("DefaultValue=\"\"", "");

                Regex regIfadeArasiTemizle = new Regex(@"([\w\""])([ ]{2,25})(\w)");
                K1 = regIfadeArasiTemizle.Replace(K1, "$1 $3");

                Regex regDisXmlTagBoslukTemizlik = new Regex(@"(""[ ]+)(\/\>)");
                K1 = regDisXmlTagBoslukTemizlik.Replace(K1, "\"$2");

                Regex regIcXmlTagBoslukTemizlik = new Regex(@"([\""\w])([ ]+)(\>)");
                K1 = regIcXmlTagBoslukTemizlik.Replace(K1, "$1$3");

                File.WriteAllText(sourceFile, K1, Encoding.UTF8);
            }
            catch (Exception err)
            {
                Console.WriteLine("Hata:" + err.Message);
                Console.ReadLine();
            }
        }

        void CleanCsFile(string sourceFile)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFile)) return;
                if (!File.Exists(sourceFile)) return;
                FileInfo fi = new FileInfo(sourceFile);
                if (!fi.Extension.Equals(".cs", StringComparison.CurrentCultureIgnoreCase)) return;
                var K1 = File.ReadAllText(sourceFile, Encoding.UTF8);
                K1 = K1.Replace("\r\n[Server]", "");
                K1 = K1.Replace("[Server]\r\n", "");
                K1 = K1.Replace("AllowInInsert = false, AllowInUpdate = false, AllowInSelect = false", "Flags=ColumnFlags.None");

                var matchesList = Regex.Matches(K1, @"([sg])et\s{[\w\s;=]+\}");

                foreach (Match mt in matchesList)
                {
                    if (mt.Value.Contains("Collection")) continue;
                    string changeKey = mt.Groups[1].Value;
                    K1 = K1.Replace(mt.Value, changeKey + "et;");
                }
                K1 = Regex.Replace(K1, @"\s+\{\s+([sg]et;\s+)+\}", "{get;set;}");
                File.WriteAllText(sourceFile, K1, Encoding.UTF8);
            }
            catch (Exception err)
            {
                Console.WriteLine("Hata:" + err.Message);
                Console.ReadLine();
            }
        }

    }
}
