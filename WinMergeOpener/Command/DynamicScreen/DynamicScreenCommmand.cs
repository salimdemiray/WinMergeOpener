using System;
using System.IO;
using WinMergeOpener.Command.DynamicScreen;

namespace WinMergeOpener.Command
{
    public partial class DynamicScreenCommmand : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.StartsWith("zz_", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            if (ConsoleParam.Command.Equals("zz_cp", StringComparison.InvariantCultureIgnoreCase))
                CopyFileEvent(ArgParam);

            if (ConsoleParam.Command.Equals("zz_dl", StringComparison.InvariantCultureIgnoreCase))
                DeleteOldFileEvent(ArgParam);
        }

        void DeleteOldFileEvent(ArgumentParam ArgParam)
        {
            string fullProjectMasterDirectory = FindProjectMasterDirectoy(ArgParam);
            var PathDynamicCompledCode = @"Senfoni\Senfoni\Senfoni\CustomPrg\SOURCE";
            var DirectoryOfSourceCodes = Path.Combine(fullProjectMasterDirectory, PathDynamicCompledCode);

            DynamicCopyToZzzProject.DeleteDirectoryFilesWithSubDirectort(DirectoryOfSourceCodes);
        }

        static string FindZZZProjectFilePath(string fullProjectMasterDirectory)
        {
            var pathZZZProj = Directory.GetFiles(fullProjectMasterDirectory, "ZZZ.csproj", SearchOption.AllDirectories);

            if (pathZZZProj.Length == 0) return string.Empty;

            return pathZZZProj[0];
        }

        void CopyFileEvent(ArgumentParam ArgParam)
        {
            string fullProjectMasterDirectory = FindProjectMasterDirectoy(ArgParam);

            var PathDynamicCompledCode = @"Senfoni\Senfoni\Senfoni\CustomPrg\SOURCE";
            try
            {
                var fullZZFilePath = FindZZZProjectFilePath(fullProjectMasterDirectory);

                if (string.IsNullOrEmpty(fullZZFilePath)) return;

                var DirectoryOfSourceCodes = Path.Combine(fullProjectMasterDirectory, PathDynamicCompledCode);

                if (!Directory.Exists(DirectoryOfSourceCodes)) return;

                new DynamicCopyToZzzProject(fullZZFilePath, DirectoryOfSourceCodes).CopyFile();
            }
            catch
            {
            }
        }
        string FindProjectMasterDirectoy(ArgumentParam ArgParam)
        {
            var fileVersionData = _VersionManager.VersionInfoFromFileName(ArgParam.FileName);
            var cleanedProjectPath = fileVersionData.Path.Substring(@"branches\\".Length - 1);

            var fullProjectMasterDirectory = Path.Combine(_VersionManager.BranchPath, cleanedProjectPath);
            return fullProjectMasterDirectory;
        }
    }
}
