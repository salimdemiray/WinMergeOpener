using System;
using System.Diagnostics;

namespace WinMergeOpener.Command
{
    public class MergeFile : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return string.IsNullOrEmpty(kom) || kom.ToLower().Equals("m", StringComparison.CurrentCultureIgnoreCase);

        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            OpenWinMerger(ArgParam.FileName, ConsoleParam);
        }

        protected void OpenWinMerger(string fl, ConsoleParam consoleParam)
        {
            foreach (var prST in consoleParam.TargetVersions)
            {
                try
                {
                    RunWinMerger(fl, prST);
                }
                catch
                {
                }
            }
        }

        void RunWinMerger(string fl, TargetVersion VersionNo)
        {
            var f2 = _VersionManager.FindTargetPath(fl, VersionNo);
            if (VersionNo.NoTagDirectory(f2)) return;

            if (string.IsNullOrEmpty(f2)) return;
            Process.Start(_VersionManager.WinMergePath, fl + " " + f2);
        }

        protected void RunWinMerger2File(string fl, string f2)
        {
            Process.Start(_VersionManager.WinMergePath, fl + " " + f2);
        }

        protected void RunWinMerge3File(string fl, string f2, string f3)
        {
            var arg = string.Format("{0} {1} {2}", fl, f2, f3);
            Process.Start(_VersionManager.WinMergePath, arg);

            // /dl Sol /dr sağ /dm orta 
            /*"C:\Program Files (x86)\WinMerge\WinMergeU.exe" "D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Modules\HRM\HRM\PersonalData\Leave.cs" "D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Modules\HRM\HRM\FastPayroll\FastPayEmp.cs" "D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.cs"*/
        }

    }

}
