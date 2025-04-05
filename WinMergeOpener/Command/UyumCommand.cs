using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMergeOpener.Command
{
    public partial class UyumCommand : CommandBase
    {
        public const string MenuText = "uy_in,uy_tb,pg,dt,st,bk,ps,sl";
        //const string branhesPaths = @"C:\Uyumsoft\UyumProjects\Senfoni\branches\";
        ArgumentParam ArgParam;

        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.StartsWith("uy_", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            this.ArgParam = ArgParam;
            //https://k23-software.net/csharp-dynamic-dll-loading-tutorial#full-source-code

            if (ConsoleParam.Command.Equals("uy_in", StringComparison.InvariantCultureIgnoreCase))
            {
                var r = GetFileModule(ArgParam.FileName);
                RunClassInit(r, false);
            }
            else if (ConsoleParam.Command.Equals("uy_is", StringComparison.InvariantCultureIgnoreCase))
            {
                var r = GetFileModule(ArgParam.FileName);
                RunClassInit(r, true);
            }
            else if (ConsoleParam.Command.Equals("uy_tb", StringComparison.InvariantCultureIgnoreCase))
            {
                var r = GetFileModule(ArgParam.FileName);
                RunClassProbHelper(r, "TableColumnXml");
            }
            else if (ConsoleParam.Command.Equals("uy_pg", StringComparison.InvariantCultureIgnoreCase))
            {
                var r = GetFileModule(ArgParam.FileName);
                RunClassProbHelper(r, "PageControlXml");
            }
            else if (ConsoleParam.Command.Equals("uy_dt", StringComparison.InvariantCultureIgnoreCase))
            {
                var r = GetFileModule(ArgParam.FileName);
                RunClassProbHelper(r, "ModelToDataTable");
            }
            else if (ConsoleParam.Command.Equals("uy_st", StringComparison.InvariantCultureIgnoreCase))
            {
                var r = GetFileModule(ArgParam.FileName);
                RunClassProbHelper(r, "ToStringMessage");
            }
            else if (ConsoleParam.Command.Equals("uy_bk", StringComparison.InvariantCultureIgnoreCase))
            {
                DeleteBakFiles(ArgParam, ConsoleParam);
            }
            else if (ConsoleParam.Command.Equals("uy_ps", StringComparison.InvariantCultureIgnoreCase))
            {
                ProjectSort(ArgParam);
            }
            else if (ConsoleParam.Command.Equals("uy_cj", StringComparison.InvariantCultureIgnoreCase))
            {
                CollectionJoinClean(ArgParam);
            }
            else if (ConsoleParam.Command.Equals("uy_sl", StringComparison.InvariantCultureIgnoreCase))
            {
                OpenSvnLog(ArgParam);
            }
        }

        static void OpenSvnLog(ArgumentParam argParam)
        {
            var fl = argParam.FileName;
            Process.Start("TortoiseProc.exe", string.Format("/command:log /path:\"{0}\"", fl));
        }

        string GetRunClassProbHelperText(ModuleResponse r, string operationType)
        {
            Assembly asmHrm = LoadHrmAms(ArgParam, _VersionManager);
            var typeCommonOps = asmHrm.GetType("HRM.CommonOps");
            var typeEnum = typeCommonOps.GetNestedType("ReadClassPropertiesHelperType");
            var typeClass = GetOtherType(r);
            var enumValue = Enum.Parse(typeEnum, operationType);

            var staticMethod = typeCommonOps.GetMethod("ReadClassPropertiesHelper");
            return staticMethod.Invoke(null, new[] { typeClass, enumValue, false }).ToString();
        }

        void RunClassProbHelper(ModuleResponse r, string operationType)
        {
            var res = GetRunClassProbHelperText(r, operationType);
            var newFileName = string.Format("{0}_{1}.txt", r.ClassName, operationType);
            ProperyGiven.OpenNotePad(res, newFileName);
        }

        public static void PrepareMenu(TablePrint tableUyum)
        {
            var col1 = tableUyum.AddColumn("");
            var col2 = tableUyum.AddColumn("");

            col1.AddHeaderAndValue("uy_in", "Class init string", "[1]");
            col2.AddHeaderAndValue("uy_tb", "Xml Tablo", "[1]");
            col1.AddHeaderAndValue("uy_pg", "Xml Sayfa", "[1]");
            col2.AddHeaderAndValue("uy_dt", "Model DataTable", "[1]");
            col1.AddHeaderAndValue("uy_st", "Model ToString", "[1]");
            col2.AddHeaderAndValue("uy_bk", "Bak Dosyası Sil", "[1]");
            col1.AddHeaderAndValue("uy_is", "Class init string & ToString ", "[1]");
            col2.AddHeaderAndValue("uy_ps", "Proje Dosyaları Sırala", "[1]");
            col2.AddHeaderAndValue("uy_cj", "Col. Join Temizleme", "[1]");
            col1.AddHeaderAndValue("uy_sl", "Svn History", "[1]");
        }

        Type GetOtherType(ModuleResponse r)
        {
            Assembly asmOth = LoadAsm(ArgParam, r.ModuleName, _VersionManager);
            return asmOth.GetType(r.GetAsm);
        }

        #region Run İnit Kode
        void RunClassInit(ModuleResponse r, bool WithToString)
        {
            if (!r.Success) return;

            string ExtraMessage = "";
            if (WithToString)
            {
                ExtraMessage = "public override string ToString() {\r\n" + GetRunClassProbHelperText(r, "ToStringMessage") + "\r\n}";
            }

            RunClassInit(r.ModuleName, r.ClassName, ExtraMessage);
        }

        void RunClassInit(string Module, string TypeName, string ExtraMessage)
        {
            Assembly asmHrm = LoadHrmAms(ArgParam, _VersionManager);
            var tCommonOps = asmHrm.GetType("HRM.CommonOps");
            Type t1 = null;

            if (Module == "HRM")
            {
                t1 = asmHrm.GetType(Module + "." + TypeName);
            }
            else
            {
                Assembly asmOth = LoadAsm(ArgParam, Module, _VersionManager);
                t1 = asmOth.GetType(Module + "." + TypeName);
            }

            var staticMethod = tCommonOps.GetMethod("InitClassString");
            var res = staticMethod.Invoke(null, new[] { t1 }).ToString();

            if (!string.IsNullOrWhiteSpace(ExtraMessage))
            {
                res += "\r\n\r\n\r\n\r\n\r\n" + ExtraMessage;
            }

            ProperyGiven.OpenNotePad(res, TypeName + "_show.txt");
        }

        static void DeleteBakFiles(ArgumentParam ArgParam, ConsoleParam consoleParam)
        {
            var fullPath = Path.GetDirectoryName(ArgParam.ProjectFile);

            if (consoleParam.TargetVersions?.Length > 0 && consoleParam.TargetVersions[0].VersionNo == 2)
            {
                string masterParentDir = GetParentDirectoryNameHasFile(fullPath, "*.sln");
                if (!string.IsNullOrWhiteSpace(masterParentDir)) fullPath = masterParentDir;
            }

            var willDeledetFiles = Directory.GetFiles(fullPath, "*.bak", SearchOption.AllDirectories);
            foreach (var deleteFile in willDeledetFiles)
            {
                try
                {
                    File.Delete(deleteFile);
                }
                catch
                {

                }
            }
        }

        static string GetParentDirectoryNameHasFile(string fullPath, string searchPattern)
        {
            try
            {
                var pd = Directory.GetParent(fullPath);
                if (pd == null || !pd.Exists) return string.Empty;
                var findFiles = Directory.GetFiles(fullPath, searchPattern);
                if (findFiles.Length > 0) return fullPath;

                return GetParentDirectoryNameHasFile(pd.FullName, searchPattern);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        public static ModuleResponse GetFileModule(string FileName)
        {
            var res = new ModuleResponse();
            if (!File.Exists(FileName)) return res;
            var textContent = File.ReadAllText(FileName, Encoding.UTF8);

            var regUyumBase = @"public(\spartial)?\sclass\s+(\w+)\s?";
            var regNameSpace = @"namespace\s(\w+)\s?";

            var matClass = Regex.Match(textContent, regUyumBase);
            var matNameSpace = Regex.Match(textContent, regNameSpace);

            if (!matClass.Success || !matNameSpace.Success) return res;

            res.Success = true;
            res.ClassName = matClass.Groups[2].Value;
            res.ModuleName = matNameSpace.Groups[1].Value;

            return res;
        }

        public class ModuleResponse
        {
            public bool Success { get; set; }
            public string ModuleName { get; set; }
            public string ClassName { get; set; }

            public string GetAsm => $"{ModuleName}.{ClassName}";
        }

        #region Load Asm

        static Assembly LoadHrmAms(ArgumentParam ArgParam, VersionManager _VersionManager)
        {
            return LoadAsm(ArgParam, "HRM", _VersionManager);
        }

        static Assembly LoadAsm(ArgumentParam ArgParam, string ModName, VersionManager _VersionManager)
        {

            var pt = _VersionManager.BranchPath;

            //var drvs = DriveInfo.GetDrives();

            //if (!drvs.Any(n => n.Name == "D:\\"))
            //{
            //    pt = pt.Replace("D:\\", "C:\\");
            //}

            var x = ArgParam.ProjectFile.Replace(pt, "");
            var versiyonPath = x.Split('\\')[0];
            var commonDllPath = Path.Combine(pt + versiyonPath, @"Senfoni\CommonDll\Debug");

            var hrmDllPath = Path.Combine(commonDllPath, ModName + ".DLL");
            return Assembly.UnsafeLoadFrom(hrmDllPath);
        }
        #endregion

        static void ProjectSort(ArgumentParam argParam)
        {
            new SortProjectFile(argParam).Run();
        }

        static void CollectionJoinClean(ArgumentParam argParam)
        {
            if (!argParam.FileName.Contains("Collection.cs")) return;

            //LeftFieldName\s*=\s*([\"\w\.]+)\,?
            var pattern = @"\s*=\s*([""\.\s\w]+)\s*";
            RegexOptions options = RegexOptions.Singleline | RegexOptions.CultureInvariant;

            var searchList = new[] { "JoinType", "LeftTable", "LeftFieldName", "RightTable", "RightFieldName" };

            var sb = new StringBuilder();

            var values = new Dictionary<string, string>();

            try
            {

                if (!File.Exists(argParam.FileName)) return;
                FileInfo fi = new FileInfo(argParam.FileName);
                if (!fi.Extension.Equals(".cs", StringComparison.CurrentCultureIgnoreCase)) return;
                var K1 = File.ReadAllText(argParam.FileName, Encoding.UTF8);

                if (!K1.Contains("Collection")) return;
                int startSeracj = 0;
                while (true)
                {

                    int starjoinIx = K1.IndexOf("[UyumJoin(", startSeracj, StringComparison.Ordinal);
                    if (starjoinIx < 0) break;
                    int endjoinIx = K1.IndexOf(")]", starjoinIx, StringComparison.Ordinal);
                    if (endjoinIx < 0) break;


                    var captu = K1.Substring(starjoinIx, (endjoinIx - starjoinIx) + 2);

                    var clean1 = captu.Replace("\r\n", "");
                    if (!clean1.Contains("LeftFieldName"))
                    {
                        startSeracj = endjoinIx;
                        continue;
                    }
                    values.Clear();

                    foreach (var se in searchList)
                    {
                        var innerPattern = @",?\s*" + se + pattern;
                        var mList = Regex.Match(clean1, innerPattern, options);
                        values[se] = "";

                        if (!mList.Success) continue;

                        values[se] = mList.Groups[1].Value;
                        clean1 = Regex.Replace(clean1, innerPattern, "");
                    }

                    clean1 = clean1.Replace("[UyumJoin(", "").Replace(")]", "").Trim();
                    clean1 = CleanEmptyAttribute(clean1);

                    sb.Clear();
                    sb.Append("[UyumJoin(")
                        .Append(string.Join(",", values.Values))
                        .Append(clean1)
                        .Append(")]");
                    K1 = K1.Replace(captu, sb.ToString());
                }


                File.WriteAllText(argParam.FileName, K1, Encoding.UTF8);
                //ProperyGiven.OpenNotePad(K1, "dddd" + "_show.txt");
            }
            catch (Exception err)
            {
                Console.WriteLine("Hata:" + err.Message);
                Console.ReadLine();
            }
        }

        public static string CleanEmptyAttribute(string text)
        {
            //   //, RightAlias = "" -> \s*,?\s*RightAlias\s*=\s*\"s*"
            var cleanAttributes = new[] { "RightAlias", "LeftAlias", "CustomSql", "DeleteCustomMessage " };
            var clean1 = text;
            foreach (var atr in cleanAttributes)
            {
                var intPattern = @"\s*,?\s*" + atr + @"\s*=\s*\""s*""";
                clean1 = Regex.Replace(clean1, intPattern, "");
            }

            return clean1;

        }
    }
}
