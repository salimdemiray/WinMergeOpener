using System;
using System.IO;

namespace WinMergeOpener.Command
{
    public class DeleteQuickFiles : CommandBase
    {
        const string QUICK_LOAD_PATH = @"C:\UyumQuickLoad";

        FileInfo ProjectFileInfo;
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.Equals("z", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            ProjectFileInfo = new FileInfo(ArgParam.ProjectFile);
            DeleteQuickFile();
            DeleteNavFiles();
            DeleteCustomPageXml();
        }

        void DeleteCustomPageXml()
        {
            try
            {
                var masterProjectDirectory = GetSolutionDirectory();
                if (string.IsNullOrWhiteSpace(masterProjectDirectory)) return;
                var searhDir = Path.Combine(masterProjectDirectory, @"Senfoni\Senfoni\Senfoni", @"CustomPrg\\XML\\");
                var navFiles = Directory.GetFiles(searhDir, "*.xml", SearchOption.AllDirectories);

                Array.ForEach(navFiles, File.Delete);
            }
            catch
            {
            }
        }

        string GetSolutionDirectory()
        {
            var currentSelectedVersion = _VersionManager.VersionInfoFromFileName(ProjectFileInfo.FullName) ?? new VersionInfo();
            if (currentSelectedVersion == null) return string.Empty;

            var masterPathIndex = ProjectFileInfo.FullName.IndexOf(currentSelectedVersion.Path, StringComparison.CurrentCultureIgnoreCase);
            if (masterPathIndex < 0) return string.Empty;

            return ProjectFileInfo.FullName.Substring(0, masterPathIndex) + currentSelectedVersion.Path;
        }

        void DeleteNavFiles()
        {
            try
            {
                var masterProjectDirectory = GetSolutionDirectory();
                if (string.IsNullOrWhiteSpace(masterProjectDirectory)) return;

                var searhDir = Path.Combine(masterProjectDirectory, @"Senfoni\Senfoni\Senfoni");
                var navFiles = Directory.GetFiles(searhDir, "Navigation*.xml", SearchOption.AllDirectories);

                Array.ForEach(navFiles, File.Delete);
            }
            catch { }
        }

        static void DeleteQuickFile()
        {
            Array.ForEach(Directory.GetFiles(QUICK_LOAD_PATH), File.Delete);
        }
    }
}
