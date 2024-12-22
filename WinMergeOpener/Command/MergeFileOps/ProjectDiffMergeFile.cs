using System;
using System.Diagnostics;

namespace WinMergeOpener.Command
{

    public class ProjectDiffMergeFile : DiffMergeFile
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.ToLower().Equals("dp", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            DiffMerge(ArgParam.ProjectFile, ConsoleParam);
        }

    }

}
