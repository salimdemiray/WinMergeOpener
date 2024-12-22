using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMergeOpener.Command
{

    public class OpenProjectDirectoryCommand : OpenDirectoryCommand
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.Equals("op", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            OpenDirectory(ArgParam.ProjectFile, ConsoleParam);
        }
    }
}
