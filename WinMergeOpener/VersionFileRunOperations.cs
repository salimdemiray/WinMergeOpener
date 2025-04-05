using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinMergeOpener.Command.Tools;

namespace WinMergeOpener
{
    public static class VersionFileRunOperations
    {
        public static void RunEachFiles(string fileName, ConsoleParam consoleParam, VersionManager _VersionManager, Action<string> FileNameAction)
        {

            FileNameAction(fileName);


            if (consoleParam.TargetVersions.Length == 0) return;

            foreach (var prST in consoleParam.TargetVersions)
            {
                if (prST.VersionNo == 1) continue;

                try
                {
                    var f2 = _VersionManager.FindTargetPath(fileName, prST);

                    if (string.IsNullOrEmpty(f2)) continue;


                    FileNameAction(f2);


                }
                catch (Exception ex)
                {
                    // Log the exception and continue processing other files
                    Console.WriteLine($"Error processing file {fileName}: {ex.Message}");
                }
            }

        }
    }
}
