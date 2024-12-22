using System;
using System.Diagnostics;

namespace WinMergeOpener.Command
{

    public class ProjectFileMerge : MergeFile
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.ToLower().Equals("s", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            OpenWinMerger(ArgParam.ProjectFile, ConsoleParam);
        }
    }

}
