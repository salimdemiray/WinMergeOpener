using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;


namespace WinMergeOpener
{
    public class ConsoleParam
    {
        public ConsoleParam()
        {
            TargetVersions = new TargetVersion[0];
        }

        public bool Success { get; set; }
        public string Command { get; set; }
        public string ExtraNumberParams { get; set; }
        public int VersionNo { get; set; }

        public TargetVersion[] TargetVersions { get; set; }

        public override string ToString()
        {
            return Command;
        }


    }

    public class TargetVersion
    {
        public int VersionNo { get; set; }
        public int TagNo { get; set; }
        public string TargetName { get; set; }

        public override string ToString()
        {
            return string.Join("-", VersionNo, TagNo);
        }

        /// <summary>
        /// İşlem Versiyon Tagı için ve  Bu taga ait Dizin Yok mu
        /// </summary>
        /// <param name="f2"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool NoTagDirectory(string f2)
        {
            if (TagNo <= 0) return false;
            var tagPath = $"\\{TagNo}\\";

            int indexTag= f2.IndexOf(tagPath);

            if(indexTag==-1) return false;

            var drt = f2.Substring(0,indexTag + tagPath.Length);

            return !System.IO.Directory.Exists(drt);
        }
    }

    public class TaskInfo
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string Yol { get; set; } = "";
    }

    public class VersionInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Yol { get; set; }
        public string Tag { get; set; }
        public int VersionNo { get; set; }

        public VersionInfo()
        {
            Tag = "";
        }

        public override string ToString()
        {
            return string.Join("-", new[] { Name, Path });
        }
    }

    public class ConsoleParamReader
    {
        ConsoleParam ParseWithCommaTag(string cmd)
        {
            var m1 = Regex.Match(cmd, @"(?<kom>\w*)(?<val>(\d+:\d+|\d+)[,;]?)+", RegexOptions.IgnorePatternWhitespace);


            if (!m1.Success) return new ConsoleParam();
            var kommand = m1.Groups["kom"].Value;

            var prmText = cmd;

            if (!string.IsNullOrWhiteSpace(kommand)) prmText = cmd.Replace(kommand, "");

            var sd = prmText.Split(',', ';', ' ');
            var versionList = sd.SelectMany(SelectOp).ToList();
            
            return new ConsoleParam
            {
                Command = kommand,
                Success = true,
                //ExtraNumberParams = prmText,
                TargetVersions = versionList.ToArray()
            };
        }

        static TargetVersion[] ConverRangedTagParematers(string cmd)
        {
            List<TargetVersion> versionList = new List<TargetVersion>();

            var opText = cmd;

            if (!opText.Contains(":") || !opText.Contains("..")) return versionList.ToArray();
            var multiRangeRegex = Regex.Match(cmd, @"(?<kom>\w*)(?<ver>(\d+)):(?<val1>(\d+))\.\.(?<val2>(\d+))", RegexOptions.IgnorePatternWhitespace);

            if (!multiRangeRegex.Success) return versionList.ToArray();

            var s1 = Convert.ToInt32(multiRangeRegex.Groups["val1"].Value);
            var s2 = Convert.ToInt32(multiRangeRegex.Groups["val2"].Value);


            var version = Convert.ToInt32(multiRangeRegex.Groups["ver"].Value);

            var indexStart = Math.Min(s1, s2);
            var indexEnd = Math.Max(s1, s2);

            if (indexEnd - indexStart > 10) return versionList.ToArray();


            for (int v1 = indexStart; v1 <= indexEnd; v1++)
            {
                versionList.Add(new TargetVersion { TagNo = v1, VersionNo = version });
            }

            return versionList.ToArray();
        }

        static TargetVersion[] SelectOp(string m)
        {

            var res = ConverRangedTagParematers(m);

            if (res.Length > 0) return res;

            var sd = m.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            TargetVersion t = new TargetVersion();

            if (int.TryParse(sd[0], out int Version))
                t.VersionNo = Version;

            if (sd.Length > 1 && int.TryParse(sd[1], out int TagNo))
                t.TagNo = TagNo;

            return new[] { t };

        }

        ConsoleParam ParseWithComma(string cmd)
        {
            var m1 = Regex.Match(cmd, @"(?<kom>\w*)(?<val>\d+[,;][\d,;]+)");
            if (!m1.Success) return new ConsoleParam();

            var sd = m1.Groups["val"].Value.Split(',', ';');
            var versionList = sd.Select(SelectOp).ToArray();

            return new ConsoleParam
            {
                Command = m1.Groups["kom"].Value,
                Success = true,
                //ExtraNumberParams = m1.Groups["val"].Value,
                TargetVersions = versionList[0]
            };
        }

        //ConsoleParam ParseCommandAndNumber(string cmd)
        //{
        //    var m = Regex.Match(cmd, @"(?<kom>\w*)(?<val>\d+)\s?(?<tar>\d*)");
        //    if (!m.Success) return new ConsoleParam();

        //    var verID = int.Parse(m.Groups["val"].Value);
        //    int.TryParse(m.Groups["tar"].Value, out int targetNo);
        //    return new ConsoleParam
        //    {
        //        Command = m.Groups["kom"].Value,
        //        VersionNo = verID,
        //        Success = true,
        //        TargetVersions = new TargetVersion[] {
        //            new TargetVersion{VersionNo=targetNo }
        //        }
        //    };
        //}

        public ConsoleParam Parse(string cmd)
        {
            var resultTag = ParseWithCommaTag(cmd);
            if (resultTag.Success) return resultTag;

            var result = ParseWithComma(cmd);
            return result;

            //return ParseCommandAndNumber(cmd);
        }
    }
}
