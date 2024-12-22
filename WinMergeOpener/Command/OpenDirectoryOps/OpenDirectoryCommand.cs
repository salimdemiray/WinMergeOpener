using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMergeOpener.Command
{
    public class OpenDirectoryCommand : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.Equals("o", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            OpenDirectory(ArgParam.FileName, ConsoleParam);
        }

        protected void OpenDirectory(string fl, ConsoleParam ConsoleParam)
        {

            if (ConsoleParam.TargetVersions.Length == 0) return;
            var f2 = _VersionManager.FindTargetPath(fl, ConsoleParam.TargetVersions[0]);

            if (string.IsNullOrEmpty(f2)) return;
            Process.Start("explorer", "/select," + f2);
        }
    }
}
