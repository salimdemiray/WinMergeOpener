using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using WinMergeOpener.Command;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Reflection;
using System.Xml.Linq;

namespace WinMergeOpener
{
    /*
     \menu D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Modules\HRM\HRM\PersonalData\Leave.cs 12 46 D:\UyumProjects\Senfoni\branches\release.1.0.60.0.Live\Senfoni\Senfoni\Senfoni\Senfoni.csproj
         */

    /*
     \menu D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Senfoni\Senfoni\HRM\FinReceiptPayroll\MonthlyFinRecCostCenDistM.xml 12 46 D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Senfoni\Senfoni\Senfoni.csproj
     
        \menu D:\UyumProjects\Senfoni\branches\release.1.0.60.0.LiveNew\Senfoni\Senfoni\Senfoni\HRM\PayrollFinRelM.xml 12 46 D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Senfoni\Senfoni\Senfoni.csproj


       ** \menu D:\UyumProjects\Senfoni\branches\Tag\Erp.4.2109\24\Senfoni\Modules\HRM\HRM\General\AddressType.cs l 12 46 D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Senfoni\Senfoni\Senfoni.csproj

         D:\UyumProjects\Senfoni\branches\Tag\Erp.4.2109\24\Senfoni\Modules\HRM\HRM\General\AddressType.cs 


         */

    /*
     \menu C:\Uyumsoft\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Modules\HRM\HRM\FinReceiptPayroll\MonthlyFinRecCostCenDistD.cs 12 46 C:\Uyumsoft\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Modules\HRM\HRM\HRM.csproj
     */


    partial class Program
    {
        public const string tr = "trunk";
        //public const string wm = @"C:\Program Files (x86)\WinMerge\WinMergeU.exe";
        public const string VS = @"C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe";

        const string ts = @"D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Senfoni\Senfoni\HRM\Absence.xml";
        //const string ts1 = @"D:\UyumProjects\Senfoni\trunk\Senfoni\Senfoni\Senfoni\HRM\Absence.xml";
        //const string ts2 = @"D:\UyumProjects\Senfoni\branches\release.1.0.60.0.live\Senfoni\Modules\HRM.WEB\CustomCode\IBBIntegrationCodes\IBBGeneralTransferCode.cs";
        //const string ts3 = @"D:\UyumProjects\Senfoni\branches\release.1.0.60.0.11g.Eko.Ortak\Senfoni\Modules\HRM\HRM\PersonalData\Leave.cs";

        //const string tsT = @"D:\UyumProjects\Senfoni\branches\release.1.0.60.0\Senfoni\Modules\HRM.WEB\CustomCode\VisitCode.cs";

        //const string Test = @"branches\release.1.0.51.0";
        //const string Test49_2 = @"branches\release.1.0.49.2";
        //const string Test50_1 = @"branches\release.1.0.50.1";



        //static void XmlValidate()
        //{
        //https://uyg.sgk.gov.tr/SgkTescil4a/ornekler/istencikis.xsd
        //XmlReaderSettings settings = new XmlReaderSettings();
        //settings.Schemas.Add("", @"C:\Users\salim.demiray\Desktop\Temp\istencikis.xsd");
        //settings.ValidationType = ValidationType.Schema;

        //XmlReader reader = XmlReader.Create(@"C:\Users\salim.demiray\Desktop\Temp\03P186-KAHRAMANMARAS PIAZZA AVM-247710101105339504601-07_06_2024_17_06_35-ISTEN CIKIS.xml", settings);
        //XmlDocument document = new XmlDocument();
        //document.Load(reader);

        //ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

        //// the following call to Validate succeeds.
        //document.Validate(eventHandler);
        //}

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }
        }

        public class CsFileDocumentationReader
        {
            private readonly string _filePath;

            public CsFileDocumentationReader(string filePath)
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"C# source file not found: {filePath}");

                _filePath = filePath;
            }

            public void PrintDocumentation()
            {
                Console.WriteLine($"Reading documentation from: {_filePath}\n");

                using (StreamReader reader = new StreamReader(_filePath))
                {
                    string line;
                    bool inCommentBlock = false;
                    string currentTag = null;
                    string commentContent = "";

                    string currentMethod = null; // Mevcut metot adı
                    string currentParams = "";   // Mevcut parametreler

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Yorum satırları başlıyor mu?
                        if (line.Trim().StartsWith("///"))
                        {
                            inCommentBlock = true;
                            string content = line.Trim().Substring(3).Trim();

                            // XML tag başlangıcını kontrol et
                            var tagMatch = Regex.Match(content, @"^<(\w+)>");
                            if (tagMatch.Success)
                            {
                                currentTag = tagMatch.Groups[1].Value;
                                content = content.Substring(tagMatch.Length).Trim();
                            }

                            // XML tag kapanışını kontrol et
                            var closeTagMatch = Regex.Match(content, $"</{currentTag}>$");
                            if (closeTagMatch.Success)
                            {
                                content = content.Substring(0, closeTagMatch.Index).Trim();
                                inCommentBlock = false;
                            }

                            commentContent += content + " ";

                            // Eğer blok bitti ise ekrana yazdır ve sıfırla
                            if (!inCommentBlock)
                            {
                                PrintTagContent(currentTag, commentContent.Trim());
                                commentContent = "";
                                currentTag = null;
                            }
                        }
                        else
                        {
                            // Metot satırı bulma
                            var methodMatch = Regex.Match(line.Trim(), @"^(public|private|protected|internal|static|\s)*\s*\w+\s+(\w+)\(([^)]*)\)");
                            if (methodMatch.Success)
                            {
                                currentMethod = methodMatch.Groups[2].Value;
                                currentParams = methodMatch.Groups[3].Value;

                                Console.WriteLine($"\nMethod: {currentMethod}");
                                Console.WriteLine($"Parameters: {currentParams}");
                            }
                        }
                    }
                }
            }

            private void PrintTagContent(string tag, string content)
            {
                switch (tag)
                {
                    case "summary":
                        Console.WriteLine("Summary:");
                        break;
                    case "remarks":
                        Console.WriteLine("Remarks:");
                        break;
                    case "example":
                        Console.WriteLine("Example:");
                        break;
                    default:
                        Console.WriteLine($"{tag}:");
                        break;
                }
                Console.WriteLine(content);
                Console.WriteLine();
            }
        }


        static void TestRead()
        {

            string csFilePath = @"C:\Uyumsoft\UyumProjects\Senfoni\branches\Erp.4.2109\Senfoni\Modules\HRM.WEB\CustomCode\PayrollCodes\MainClasses\PayrollTransfer\CheckEmployeeForPreparePayrollOps.cs"; // Okumak istediğiniz C# dosyasının tam yolu

            var reader = new CsFileDocumentationReader(csFilePath);
            reader.PrintDocumentation();
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            //TestRead();
            //return;
            try
            {
                var ops = new Operation();
                ops.Run(args);

                if (ops.HasError) Console.ReadLine();
            }
            catch (Exception err)
            {
                ConsoleTool.WriteError("Hata", err.Message);
                Console.ReadLine();
            }
        }




        public class Operation
        {
            const string BuildPath = @"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe";
            readonly VersionManager _VersionManager;

            public CommandFactory CommandFactory { get; }
            public bool HasError { get; private set; }

            public Operation()
            {
                _VersionManager = new VersionManager();
                _VersionManager.LoadVersions();
                CommandFactory = new CommandFactory(_VersionManager);
            }

            public void Run(string[] args)
            {
                //\menu $(ItemPath) $(CurLine) $(CurCol)
                if (args.Length == 0)
                {
                    args = new[] { @"\menu", ts };
                }
                var argParam = new ArgumentParam(args);
                if (argParam.IsMenu)
                {
                    if (args.Length < 2) return;
                    ShowMenu(argParam);
                }
            }

            void ShowMenu(ArgumentParam argParam)
            {
                MenuPrinter(argParam);

                string cmdListText = Console.ReadLine().Trim();

                ConsoleParamReader consoleParamReader = new ConsoleParamReader();

                var commands = cmdListText.Split('|').Select(n => n.Trim());

                foreach (var cmd in commands)
                {
                    try
                    {
                        var consoleParam = consoleParamReader.Parse(cmd);

                        if (!consoleParam.Success) return;

                        RunCmd(argParam, consoleParam);

                    }
                    catch (Exception errRun)
                    {
                        ConsoleTool.WriteErrorAll(cmd, errRun);
                        HasError = true;
                    }
                }
            }

            void MenuPrinter(ArgumentParam argParam)
            {
                new MenuPrintOperation(_VersionManager, CommandFactory).Print(argParam);
            }

            void RunCmd(ArgumentParam argParam, ConsoleParam consoleParam)
            {
                var kom = consoleParam.Command;
                var sourceFile = argParam.FileName;
                var findEvent = CommandFactory.FindCommand(consoleParam);

                if (findEvent != null)
                {
                    findEvent.Run(argParam, consoleParam);
                    return;
                }

                var firstParam = consoleParam.TargetVersions.FirstOrDefault();
                if (firstParam == null) return;
                if (kom.ToLower().Equals("b", StringComparison.CurrentCultureIgnoreCase)) MsBuild(sourceFile, firstParam);
                else if (kom.ToLower().Equals("v", StringComparison.CurrentCultureIgnoreCase)) OpenDevEnv(sourceFile, consoleParam.VersionNo);
                else if (kom.ToLower().Equals("cp", StringComparison.CurrentCultureIgnoreCase)) CopyFileOtherVersion(argParam, consoleParam);
            }

            string FindTargetPath(string fl, int VersionNo)
            {
                string f2 = "";
                var verInfo = _VersionManager.VersionInfoFromFileName(fl);
                var verTarget = _VersionManager.FindTaskNo(VersionNo);

                var t1 = Regex.Match(fl, @"(release[\.\w]+)\\", RegexOptions.IgnorePatternWhitespace);

                if (t1.Success)
                {
                    verInfo = new VersionInfo
                    {
                        Name = "n",
                        Path = "branches\\" + t1.Groups[1].Value
                    };
                }
                string TargetPath = tr;

                if (verTarget != null) TargetPath = verTarget.Path;

                if (verInfo != null)
                {
                    f2 = fl.Replace(verInfo.Path, TargetPath);
                    f2 = f2.Replace("ı", "i");
                }
                return f2;
            }

            void OpenDevEnv(string fl, int VersionNo)
            {
                var f2 = FindTargetPath(fl, VersionNo);

                if (string.IsNullOrEmpty(f2)) return;
                Process.Start(VS, f2 + " /Edit");
            }

            void MsBuild(string fl, TargetVersion firstParam)
            {
                var f2 = FindTargetPath(fl, firstParam.VersionNo);
                if (string.IsNullOrEmpty(f2)) return;
                var verTarget = _VersionManager.FindTaskNo(firstParam.VersionNo);

                int i = f2.IndexOf(verTarget.Path, StringComparison.CurrentCulture);

                string FullAdr = f2.Substring(0, i) + verTarget.Path;

                string prm = string.Format(" /t:Build \"{0}\\Senfoni\\Senfoni\\Senfoni\\Senfoni.csproj\" /p:Configration=Debug", FullAdr);

                ProcessStartInfo ps = new ProcessStartInfo(BuildPath, prm)
                {
                    CreateNoWindow = false,
                    UseShellExecute = true,
                    RedirectStandardOutput = true
                };

                Process.Start(ps);
            }

            public static void AddMultiLang(string fl, string ParamValue)
            {
                var pi = fl.IndexOf("Senfoni", 20, StringComparison.CurrentCulture);

                if (pi == -1) return;
                var ffind1 = ParamValue;
                var sd = ffind1.Split("._".ToCharArray());

                if (sd.Length < 3) return;

                var p1 = fl.Substring(0, pi) + "Senfoni\\Modules\\" + sd[0] + "\\";
                var csName = sd[1].Replace("Collection", "");

                var files = Directory.GetFiles(p1, csName + ".cs", SearchOption.AllDirectories);

                if (files.Length == 0) return;

                var content = File.ReadAllLines(files[0]);
                bool isFind = false;

                for (int i = 0; i < content.Length; i++)
                {
                    var cn = content[i];

                    if (cn.Contains(" " + sd[2] + " "))
                    {
                        for (int il = 0; il < 3; il++)
                        {
                            var ix = i - il - 1;
                            var fSn = content[ix].Trim();

                            if (fSn.StartsWith("[Data", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var lastIndex = fSn.IndexOf(")", StringComparison.InvariantCultureIgnoreCase);

                                var ixMulti = fSn.IndexOf("IsMultilingual", StringComparison.InvariantCultureIgnoreCase);

                                if (ixMulti == -1)
                                {
                                    var snNew = fSn.Insert(lastIndex, ",IsMultilingual=true");

                                    var setV = content[ix].Replace(fSn, snNew);

                                    content[ix] = setV;
                                    isFind = true;
                                    break;
                                }
                            }
                        }

                        if (isFind)
                            break;
                    }
                }

                if (isFind)
                {
                    File.WriteAllLines(files[0], content);
                }
                Console.WriteLine(isFind ? "Değişti" : "Sabit Kaldı");
            }

            void CopyFileOtherVersion(ArgumentParam argParam, ConsoleParam consoleParam)
            {
                var fl = argParam.FileName;

                ConsoleTool.WriteInfo("Ana Dosya", fl);
                ConsoleTool.WriteInfo("Dosya Kopyalamak İstediğinize Emin misiniz.?", "(E/H)");

                Console.ReadLine();

                CopyExtraParamValues(consoleParam, fl);

                Console.WriteLine("Devam Etmek için Enter a Basın");
                Console.ReadLine();
            }

            void CopyExtraParamValues(ConsoleParam consoleParam, string fl)
            {
                if (consoleParam == null || consoleParam.TargetVersions.Length == 0) return;

                foreach (var prST in consoleParam.TargetVersions)
                {
                    try
                    {
                        var f2Y = _VersionManager.FindTargetPath(fl, prST);

                        if (string.IsNullOrEmpty(f2Y)) continue;
                        if (prST.NoTagDirectory(f2Y)) continue;

                        FileInfo fiTarget = new FileInfo(f2Y);

                        if (!Directory.Exists(fiTarget.DirectoryName))
                        {
                            Directory.CreateDirectory(fiTarget.DirectoryName);
                        }

                        File.Copy(fl, f2Y, true);
                        Console.WriteLine(prST + " Kopyalandı");
                    }
                    catch (Exception err)
                    {
                        ConsoleTool.WriteInfo(prST + " Hata", err.Message);
                    }
                }
            }
        }
    }
}

