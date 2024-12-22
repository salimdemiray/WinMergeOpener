using System;
using System.Diagnostics;

namespace WinMergeOpener.Command
{

    public class NotePadOpen : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.ToLower().Equals("n", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            RunWNotePad(ArgParam.FileName);
        }

        protected void RunWNotePad(string fl)
        {
            Process.Start(_VersionManager.NotePadPath, fl );
        }
    }

}
