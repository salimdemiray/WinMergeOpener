using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WinMergeOpener.Command
{
    public class RunTaskCommand : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.Equals("t", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            var taskInfo = _VersionManager.FindTaskNo(ConsoleParam);
            TaskRun(ArgParam.FileName, taskInfo, ConsoleParam);
        }

        void TaskRun(string fl, TaskInfo v, ConsoleParam consoleParam)
        {
            var verInfo = _VersionManager.VersionInfoFromFileName(fl);
            if (v == null) v = new TaskInfo ();

            var fi = fl.IndexOf(verInfo.Path, StringComparison.CurrentCulture);
            if (fi == -1) return;

            var curVersionDirectory = fl.Substring(0, fi);
            var versionPath = curVersionDirectory + verInfo.Path;
            var targetFileName = v?.Path ?? "";
            targetFileName = targetFileName.Replace("{VersionPath}", versionPath);

            if (IsRunInnerCommand(v, targetFileName, consoleParam)) return;

            RunExternalCommand(v, targetFileName);
        }

        bool IsRunInnerCommand(TaskInfo v, string targetFileName, ConsoleParam consoleParam)
        {
            if (!(v.Yol.Contains("{{") && v.Yol.Contains("}}"))) return false;
            if (consoleParam.TargetVersions.Length == 0) return false;

            //var kalanP = consoleParam.ExtraNumberParams.Substring(consoleParam.VersionNo.ToString().Length + 1);
            var willRunningCommand = v.Yol.Replace("{{", "").Replace("}}", "");
            //if (com.Equals("t")) return true;

            var newConsoleParam = new ConsoleParam { Command = willRunningCommand, TargetVersions = consoleParam.TargetVersions.Skip(1).ToArray() };
            var runCommand = _CommandFactory.FindCommand(newConsoleParam);

            if (runCommand != null) runCommand.Run(new ArgumentParam(new[] { "", targetFileName }), newConsoleParam);
            return true;
        }

        void RunExternalCommand(TaskInfo v, string targetFileName)
        {
            if (!string.IsNullOrEmpty(v.Yol))
            {
                if (!File.Exists(v.Yol))
                {
                    Console.WriteLine("Dosya Bulunamadı:" + v.Yol);
                    return;
                }
                Process.Start(v.Yol, targetFileName);
            }
            else
            {
                Process.Start(targetFileName);
            }
        }
    }
}
