using System;
using WinMergeOpener.Command.Tools;

namespace WinMergeOpener.Command
{
    public partial class CleanFile : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.Equals("c", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            CleanOperation(ArgParam.FileName, ConsoleParam);
        }

        protected void CleanOperation(string fileName, ConsoleParam consoleParam)
        {
            VersionFileRunOperations.RunEachFiles(fileName, consoleParam, _VersionManager, f =>
            {
                XmlFileCleaner.Clean(f);
                CsFileCleaner.Clean(f);
            });
        }

        //protected void CleanOperation(string fileName, ConsoleParam consoleParam)
        //{
        //    XmlFileCleaner.Clean(fileName);
        //    CsFileCleaner.Clean(fileName);

        //    if (consoleParam.TargetVersions.Length == 0) return;

        //    foreach (var prST in consoleParam.TargetVersions)
        //    {
        //        if (prST.VersionNo == 1) continue;

        //        try
        //        {
        //            var f2 = _VersionManager.FindTargetPath(fileName, prST);

        //            if (string.IsNullOrEmpty(f2)) continue;

        //            XmlFileCleaner.Clean(f2);
        //            CsFileCleaner.Clean(f2);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log the exception and continue processing other files
        //            Console.WriteLine($"Error processing file {fileName}: {ex.Message}");
        //        }
        //    }
        //}


    }
}
