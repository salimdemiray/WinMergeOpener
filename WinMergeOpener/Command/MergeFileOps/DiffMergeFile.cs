using System;
using System.Diagnostics;

namespace WinMergeOpener.Command
{

    public class DiffMergeFile : MergeFile
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.ToLower().Equals("dm", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            DiffMerge(ArgParam.FileName, ConsoleParam);
        }

        protected void DiffMerge(string FileName, ConsoleParam consoleParam)
        {
            //var ver1 = _VersionManager.FindVersionNo(consoleParam.VersionNo);
            //var ver2 = _VersionManager.FindVersionNo(consoleParam.TargetNo);

            //if (ver1 == null || ver2 == null) return;

            if (consoleParam.TargetVersions.Length != 2) return;
            var f1 = _VersionManager.FindTargetPath(FileName, consoleParam.TargetVersions[0]);
            var f2 = _VersionManager.FindTargetPath(FileName, consoleParam.TargetVersions[1]);

            if (string.IsNullOrEmpty(f1) || string.IsNullOrEmpty(f2)) return;
            RunWinMerger2File(f1, f2);
        }
    }

}
