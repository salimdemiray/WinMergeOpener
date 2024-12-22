using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace WinMergeOpener
{
    public class VersionManager
    {
        public VersionManager()
        {
            Versions = new List<VersionInfo>();
            Tasks = new List<TaskInfo>();
        }

        public void LoadVersions()
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("Versions.xml");

                XmlElement xMain = xDoc["Main"];
                Versions.Clear();
                Tasks.Clear();
                if (xMain == null) return;

                var winMergePath = xMain.GetAttribute("WinMergePath");
                var notePadPath = xMain.GetAttribute("NotePadPath");
                var branchPath = xMain.GetAttribute("BranchPath");

                if (string.IsNullOrEmpty(winMergePath))
                {
                    winMergePath = @"C:\Program Files(x86)\WinMerge\WinMergeU.exe";
                }

                if (string.IsNullOrEmpty(NotePadPath))
                {
                    NotePadPath = @"C:\Program Files\Notepad++\notepad++.exe";
                }

                if (string.IsNullOrEmpty(branchPath))
                {
                    branchPath = @"C:\Uyumsoft\UyumProjects\Senfoni\branches\";
                }

                this.WinMergePath = winMergePath;
                this.NotePadPath = notePadPath;
                this.BranchPath = branchPath;
                Versions.Add(new VersionInfo { Name = "trunk", Path = Program.tr });
                int versionNo = 1;
                foreach (XmlElement el in xMain.GetElementsByTagName("Version"))
                {
                    Versions.Add(new VersionInfo
                    {
                        Name = el.GetAttribute("Name"),
                        Path = el.GetAttribute("Path"),
                        VersionNo = versionNo++
                    });
                }

                foreach (XmlElement el in xMain.GetElementsByTagName("Task"))
                {
                    Tasks.Add(new TaskInfo { Name = el.GetAttribute("Name"), Yol = el.GetAttribute("Yol"), Path = el.GetAttribute("Arg") });
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Version.xml Dosya Hatası:{0}", err.Message);
            }
        }

        public List<VersionInfo> Versions { get; private set; }
        public List<TaskInfo> Tasks { get; private set; }

        public string WinMergePath { get; private set; }
        public string NotePadPath { get; private set; }
        public string BranchPath { get; private set; }

        T NoFinder<T>(List<T> findList, int verID)
        {
            var findIndex = verID - 1;
            if (findIndex < 0 || findIndex >= findList.Count) return default(T);
            return findList[findIndex];
        }

        public VersionInfo FindVersionNo(int verID)
        {
            return NoFinder(Versions, verID);
        }

        public TaskInfo FindTaskNo(ConsoleParam verID)
        {
            if (verID.TargetVersions.Length == 0) return null;
            return NoFinder(Tasks, verID.TargetVersions[0].VersionNo);
        }

        public TaskInfo FindTaskNo(int verID)
        {
            return NoFinder(Tasks, verID);
        }


        public VersionInfo VersionInfoFromFileName(string fl)
        {
            var findVerisyon = Versions.Find(n => fl.IndexOf(n.Path + "\\", StringComparison.OrdinalIgnoreCase) >= 0);
            if (findVerisyon != null) return findVerisyon;
            var findTag = Versions.Find(n => FindTagPath(n, fl));

            if (findTag == null) return null;

            var fxVerPath = $"{findTag.Path.Split('\\')[1]}\\";

            var i1 = fl.IndexOf(fxVerPath, StringComparison.OrdinalIgnoreCase) + fxVerPath.Length;
            var F = fl.Substring(i1);

            var tagText = F.Split('\\')[0];

            return new VersionInfo
            {
                Name = findTag.Name + " " + tagText,
                Path = TagPathChange(findTag.Path) + "\\" + tagText + "\\",
                Yol = findTag.Yol,
                Tag = tagText
            };
        }

        bool FindTagPath(VersionInfo n, string fl)
        {
            string VerPath = TagPathChange(n);
            return fl.IndexOf(VerPath + "\\", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        static string TagPathChange(VersionInfo n)
        {
            return TagPathChange(n.Path);
        }

        static string TagPathChange(string Path)
        {
            return Path.Replace(@"branches\", @"branches\Tag\");
        }

        static string TagPathChangeReverse(string Path)
        {
            return Path.Replace(@"branches\Tag\", @"branches\");
        }

        public string FindTargetPath(string file, TargetVersion target)
        {
            string f2 = "";
            var currentFileVerInfo = VersionInfoFromFileName(file);
            var verTarget = FindVersionNo(target.VersionNo);

            var fl = file;

            if (currentFileVerInfo.Tag.Length > 0)
            {
                fl = TagPathChangeReverse(file).Replace("\\" + currentFileVerInfo.Tag + "\\", "\\");
            }

            var t1 = Regex.Match(fl, @"branches\\([\.\w]+)\\", RegexOptions.IgnorePatternWhitespace);

            if (t1.Success)
            {
                currentFileVerInfo = new VersionInfo
                {
                    Name = "n",
                    Path = "branches\\" + t1.Groups[1].Value
                };
            }
            string TargetPath = "";

            if (verTarget != null) TargetPath = verTarget.Path;

            if (target.TagNo > 0)
            {
                TargetPath = TagPathChange(TargetPath) + "\\" + target.TagNo.ToString();
            }

            f2 = fl.Replace(currentFileVerInfo.Path, TargetPath);
            f2 = f2.Replace("ı", "i");

            if (target.NoTagDirectory(f2)) return string.Empty;

            return f2;
        }

        //public string FindTargetPath(string fl, VersionInfo verTarget)
        //{
        //    string f2 = "";
        //    var verInfo = VersionInfoFromFileName(fl);

        //    var t1 = Regex.Match(fl, @"(release[\.\w]+)\\", RegexOptions.IgnorePatternWhitespace);

        //    if (t1.Success)
        //    {
        //        verInfo = new VersionInfo
        //        {
        //            Name = "n",
        //            Path = "branches\\" + t1.Groups[1].Value
        //        };
        //    }
        //    string TargetPath = "";

        //    if (verTarget != null) TargetPath = verTarget.Path;

        //    if (verInfo != null)
        //    {
        //        f2 = fl.Replace(verInfo.Path, TargetPath);
        //        f2 = f2.Replace("ı", "i");
        //    }
        //    return f2;
        //}

    }
}

/*
 D:\UyumProjects\Senfoni\branches\Tag\release.1.0.60.0.LiveNew\215\Senfoni\Modules\HRM\HRM\PersonalData\Leave.cs 12 46 D:\UyumProjects\Senfoni\branches\Tag\release.1.0.60.0.LiveNew\215\Senfoni\Senfoni\Senfoni\Senfoni.csproj
     */
