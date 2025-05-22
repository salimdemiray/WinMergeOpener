using System.Collections.Generic;
using System.IO;

namespace WinMergeOpener.Command.DynamicScreen
{

    public class DynamicCopyToZzzProject
    {
        readonly List<string> _AddProjectFile = new List<string>();
        readonly string _CodesDirectory;
        readonly string _DirectoryOfSourceCodes;
        readonly string _FullZZFilePath;
        public DynamicCopyToZzzProject(string fullZZFilePath, string directoryOfSourceCodes)
        {
            _FullZZFilePath = fullZZFilePath;
            _DirectoryOfSourceCodes = directoryOfSourceCodes;

            FileInfo fiProject = new FileInfo(_FullZZFilePath);

            _CodesDirectory = Path.Combine(fiProject.DirectoryName, "Codes");
        }

        public void CopyFile()
        {
            if (!Directory.Exists(_CodesDirectory))
                Directory.CreateDirectory(_CodesDirectory);

            var sourceCsFileDirectories = Directory.GetDirectories(_DirectoryOfSourceCodes, "*", SearchOption.AllDirectories);

            foreach (var sourceDirectoryName in sourceCsFileDirectories)
            {
                PrepareForDirectory(sourceDirectoryName);
            }

            AddToProjectFile();
        }



        public static void DeleteDirectoryFiles(string targetDirectory)
        {
            var willDeletedFiles = Directory.GetFiles(targetDirectory);

            foreach (var deleteFile in willDeletedFiles) File.Delete(deleteFile);
        }

        public static void DeleteDirectoryFilesWithSubDirectort(string targetDirectory)
        {
            var willDeletedFiles = Directory.GetFiles(targetDirectory, "*.*", SearchOption.AllDirectories);

            foreach (var deleteFile in willDeletedFiles) File.Delete(deleteFile);
        }

        void AddToProjectFile()
        {
            new ChangeZzzProjectFile
            {
                FullZZFilePath = _FullZZFilePath,
                DirectoryOfSourceCodes = _DirectoryOfSourceCodes,
                CodesDirectory = _CodesDirectory,
                AddProjectFile = _AddProjectFile
            }.Change();
        }

        void PrepareForDirectory(string sourceDirectoryName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectoryName);

            var targetDirectory = Path.Combine(_CodesDirectory, directoryInfo.Name);

            if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

            var sourceCsFiles = Directory.GetFiles(sourceDirectoryName, "*.cs");

            DeleteDirectoryFiles(targetDirectory);

            foreach (var sourceFile in sourceCsFiles)
            {
                var fi = new FileInfo(sourceFile);
                var targetFile = Path.Combine(targetDirectory, fi.Name);

                File.Copy(sourceFile, targetFile, true);
                _AddProjectFile.Add(targetFile);
            }
        }
    }
}
