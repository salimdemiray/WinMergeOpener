using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMergeOpener.Command
{

    public class ProperyGiven : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.Equals("p", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {

            if (ConsoleParam.TargetVersions.Length == 0) return;
            var sourceFile = ArgParam.FileName;

            if (string.IsNullOrEmpty(sourceFile)) return;
            if (!File.Exists(sourceFile)) return;
            FileInfo fi = new FileInfo(sourceFile);
            if (!fi.Extension.Equals(".cs", StringComparison.CurrentCultureIgnoreCase)) return;
            var K1 = File.ReadAllText(sourceFile, Encoding.UTF8);

            var K2 = "";

            var ver = ConsoleParam.TargetVersions[0].VersionNo;
            switch (ver)
            {
                case 1:
                    K2 = Temiz1(K1);
                    break;
                case 2:
                    K2 = Temiz2(K1);
                    break;
                case 3:
                    K2 = Temiz3(K1);
                    break;
                case 4:
                    K2 = PropertyList(K1);
                    break;
                default:
                    break;
            }

            OpenNotePad(K2);
        }

        public static void OpenNotePad(string textContent,string FileName="show.txt")
        {
            var tmpDirect = Path.GetTempPath();

            var wFile = Path.Combine(tmpDirect, FileName);

            File.WriteAllText(wFile, textContent, Encoding.UTF8);
            Process.Start(wFile);
        }

        public static string Temiz1(string K1)
        {
            K1 = Regex.Replace(K1, @"\[TitleCaption\(.*\)\]", "");
            K1 = Regex.Replace(K1, @"\[Data.*\(.*\)\]", "");
            K1 = Regex.Replace(K1, @"\[UyumIndex.*\(.*\)\]", "");
            K1 = Regex.Replace(K1, @"\[Guid.*\(.*\)\]", "");
            K1 = Regex.Replace(K1, @"\[UyumDetailObject.*\(.*\)\]", "");
            K1 = Regex.Replace(K1, @"get\s+\{.*\}", "");
            K1 = Regex.Replace(K1, @"set\s+\{.*\}", "");
            K1 = Regex.Replace(K1, @"\{\s+get.*\}", "");
            K1 = K1.Replace("[UyumPrimaryKey(0)]", "");

            var LineList = K1.Split('\n').ToList();

            for (int i = 0; i < LineList.Count; i++)
            {
                var ln = LineList[i].Trim().ToLower();

                bool remove = false;

                if (ln.StartsWith("using", StringComparison.InvariantCultureIgnoreCase)) remove = true;
                else if (ln.StartsWith("namespace", StringComparison.InvariantCultureIgnoreCase)) remove = true;
                else remove |= string.IsNullOrEmpty(ln);
                if (remove)
                {
                    LineList.RemoveAt(i);
                    i--;
                }
            }

            var K2 = string.Join("\n", LineList.ToArray());
            return Regex.Replace(K2, @"\{\s+\}", "", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        }

        public static string Temiz2(string K1)
        {
            return Regex.Replace(K1, @"([\w\<\>,]+)\s(\w+)", "$2");
        }

        public static string Temiz3(string K1)
        {
            var ress = Regex.Matches(K1, @"public\sstring\s?(?<a>\w+)\s?\{");
            const int ItemCount = 10;


            StringBuilder sb = new StringBuilder();
            List<string> ls = new List<string>();
            foreach (Match rs in ress)
            {
                if (!rs.Success) continue;

                ls.Add(rs.Groups["a"].Value);
            }

            for (int i = 0; i < ls.Count; i += ItemCount)
            {
                var degL = ls.Skip(i).Take(ItemCount).ToArray();
                sb.Append(string.Join("=", degL)).AppendLine("=\"\";");
            }

            return sb.ToString();
        }

        public static string PropertyList(string K1)
        {
            var ress = Regex.Matches(K1, @"public\s*(new)?(override)?\s*(?<t>[\d\w]+)\s?(?<a>[\w_]+).*");            
            var SkipList = new [] { "UpdateDate", "CreateUserId", "UpdateUserId", "CreateDate", "class" };

            StringBuilder sb = new StringBuilder();
            List<string> ls = new List<string>();
            foreach (Match rs in ress)
            {
                if (!rs.Success) continue;
                var fieldName = rs.Groups["a"].Value.Trim();
                var allTextt = rs.Value;

                if (fieldName.Contains("Collection") || SkipList.Contains(fieldName) || allTextt.Contains("(")) continue;               
                ls.Add(fieldName);
            }

            for (int i = 0; i < ls.Count; i += 1)
            {
                sb.AppendLine(ls[i]);
            }

            return sb.ToString();
        }
    }

}
