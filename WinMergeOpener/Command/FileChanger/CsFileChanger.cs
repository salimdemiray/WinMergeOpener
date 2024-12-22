using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinMergeOpener.Command.FileChanger.CS;

namespace WinMergeOpener.Command
{
    public class CsFileChanger : CommandBase
    {
        public override bool IsRun(ConsoleParam ConsoleParam)
        {
            var kom = ConsoleParam.Command.ToLower();
            return kom.Equals("cs", StringComparison.CurrentCultureIgnoreCase);
        }

        public override void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam)
        {
            Console.Clear();

            TablePrint tableCommand = new TablePrint();


            var st1 = tableCommand.AddColumn("Cs Dosya İşlemleri");
            var OperationList = FindOperations("CS");

            foreach (var opItem in OperationList)
            {
                st1.AddHeaderAndValue(opItem.OptionKey, opItem.OptionDesc);
            }

            tableCommand.Print();

            string cmdListText = Console.ReadLine().Trim();

            var findOp = OperationList.FirstOrDefault(x => cmdListText.Equals(x.OptionKey, StringComparison.InvariantCultureIgnoreCase));


            if (findOp == null) return;

            findOp.ChangeFile(ArgParam.FileName);

            Console.WriteLine("ddd");
        }

        private static IFileChanger[] FindOperations(string directoryName)
        {
            Type interfaceType = typeof(IFileChanger);
            directoryName = directoryName.ToUpper().Trim();
            return typeof(LoadCollectionFromCollType_GlobalObj_Load_Ops).Assembly.GetTypes()
                   .Where(x => interfaceType.IsAssignableFrom(x) &&
                   x.IsClass &&
                   x.FullName.ToUpper().Contains($".{directoryName}.")

                   ).Select(x => Activator.CreateInstance(x) as IFileChanger)
                   .ToArray();
        }

        public static void OpenFileCs(string sourceFile,bool isAfterSave,Func<string,string> changeOps)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFile)) return;
                if (!File.Exists(sourceFile)) return;
                FileInfo fi = new FileInfo(sourceFile);
                if (!fi.Extension.Equals(".cs", StringComparison.CurrentCultureIgnoreCase)) return;
                var K1 = File.ReadAllText(sourceFile, Encoding.UTF8);

                if(changeOps != null) K1= changeOps(K1);


                if(isAfterSave)
                {
                    File.WriteAllText(sourceFile, K1, Encoding.UTF8);
                }
            }
            catch { }
        }
    }
}
