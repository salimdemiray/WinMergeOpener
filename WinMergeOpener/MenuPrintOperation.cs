using System;
using System.Collections.Generic;
using System.Linq;
using WinMergeOpener.Command;

namespace WinMergeOpener
{
    partial class Program
    {
        public class MenuPrintOperation
        {
            readonly VersionManager _VersionManager;
            readonly CommandFactory CommandFactory;

            public MenuPrintOperation(VersionManager _VersionManager, CommandFactory commandFactory)
            {
                this._VersionManager = _VersionManager;
                CommandFactory = commandFactory;
            }

            public void Print(ArgumentParam argParam)
            {
                //Console.WriteLine("Ana Dosya:" + argParam.FileName);
                //Console.WriteLine("Project:" + argParam.ProjectFile);
                ConsoleTool.WriteInfo("Ana Dosya", argParam.FileName);
                ConsoleTool.WriteInfo("Project", argParam.ProjectFile);

                var currentSelectedVersion = _VersionManager.VersionInfoFromFileName(argParam.FileName) ?? new VersionInfo();

                TablePrint tableCommand = new TablePrint();

                var st1 = tableCommand.AddColumn("");
                var st2 = tableCommand.AddColumn("");
                var st3 = tableCommand.AddColumn("");


                st1.AddHeaderAndValue("M", "Merge", "[1][1,2]");
                st2.AddHeaderAndValue("O", "Open Directory", "[1]");
                st3.AddHeaderAndValue("S", "Merge Proje File", "[1][1,2]");


                st1.AddHeaderAndValue("DM", "Farklı Merge", "[3 4]");
                st2.AddHeaderAndValue("DP", "Farklı Proje Merge", "[3 4]");
                st3.AddHeaderAndValue("CP", "Dosyayı Versiyona Kopyala", "[1]");

                st1.AddHeaderAndValue("B", "Msbuild");
                st2.AddHeaderAndValue("P", "UyumObject Propertyleri", "[1]");
                st3.AddHeaderAndValue("C", "Clean File", "[1][1,2,3]");

                st1.AddHeaderAndValue("L", "Multi Language");
                st2.AddHeaderAndValue("Z", "Quick Load Sil");
                st3.AddHeaderAndValue("A", "3 Dosya Merge");

                st1.AddHeaderAndValue("OP", "Project File Directory", "");
                st2.AddHeaderAndValue("CS", "Cs Dosya İşlemleri", "[1]");

                tableCommand.Print();

                TablePrint tableUyum = new TablePrint();


                UyumCommand.PrepareMenu(tableUyum);
                tableUyum.Print();

                //Console.WriteLine(CommandFactory.GetMenus());

                //Console.WriteLine("Komutlar- M merge , O open Directory ,S Merge Proje File,   B msbuild,P UyumObject Propertyleri, C True\\False Değişimi Yap , L Multi Language değişim ");
                //Console.WriteLine("z -> Quick Load Sil, A 3 Dosya Merge ,p Uyum özellik Temizle ");
                Console.WriteLine("Örnek M1=1. Secenekteki dosya ile merge et O1= 1. yerdeki dizini aç");

                Console.WriteLine("Artık | işaret ile birden fazla Komut Girişi Yapılabilir Örnek : C1|uy_is1");


                TablePrint tableVersion = new TablePrint();
                var tbVersion = tableVersion.AddColumn("Versiyonlar");
                var tbTasks = tableVersion.AddColumn("Task");

                for (int i = 0; i < _VersionManager.Versions.Count; i++)
                {
                    var verItem = _VersionManager.Versions[i];
                    var versionCell = new TablePrint.TitleCell((i + 1).ToString(), verItem.Name);

                    if (currentSelectedVersion.Name == verItem.Name)
                    {
                        versionCell.Style = TablePrint.CellStyle.Type1;
                    }

                    tbVersion.AddRow(versionCell);

                }

                for (int i = 0; i < _VersionManager.Tasks.Count; i++)
                {
                    tbTasks.AddRow(new TablePrint.TitleCell((i + 1).ToString(), _VersionManager.Tasks[i].Name));
                }

                tableVersion.Print();
            }

        }




    }

    public class TablePrint
    {
        private List<ColumnData> _colums;

        public TablePrint()
        {
            _colums = new List<ColumnData>();
        }

        public ColumnData AddColumn(string Header)
        {
            var col = new ColumnData(Header);
            _colums.Add(col);
            return col;
        }

        public void Print()
        {
            var colWidths = _colums.Select(n => n.DataLength()).ToArray();

            var MaxRowCount = _colums.Select(n => n.Rows.Count).Max();

            PrintHeader(colWidths);

            for (int i = 0; i < MaxRowCount; i++)
            {
                PrintRow(i, colWidths);
            }
        }

        void PrintHeader(int[] colWidths)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < _colums.Count; i++)
            {
                Console.Write(_colums[i].Header.PadLeft(colWidths[i]) + "  ");
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        void PrintRow(int rowIndex, int[] colWidths)
        {
            for (int i = 0; i < _colums.Count; i++)
            {
                _colums[i].WriteCell(rowIndex, colWidths[i]);
                Console.Write("  ");
            }
            Console.WriteLine();


        }

        public enum CellStyle
        {
            NoStyle = 0,
            Type1
        }

        public abstract class CellBase
        {
            public abstract int DataLength();
            public abstract void PrintData(int width);
            public CellStyle Style { get; set; }
        }

        public class TitleCell : CellBase
        {
            const int CommandWidth = 10;
            public TitleCell(string header, string value, string parametre = "")
            {
                Header = header;
                Value = value;
                Parametre = parametre;
            }

            public string Header { get; private set; }
            public string Value { get; private set; }
            public string Parametre { get; set; }

            public override int DataLength()
            {
                return /*Header.Length + 2*/CommandWidth + 1 + Value.Length;
            }

            public override void PrintData(int width)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.Write(Header.PadRight(CommandWidth));
                Console.Write(Header);

                Console.ForegroundColor = ConsoleColor.Green;

                var TotalParamLen = Math.Max(0, CommandWidth - Header.Length);
                Console.Write(Parametre.PadRight(TotalParamLen));
                Console.Write(":");


                Console.ResetColor();

                if (Style == CellStyle.Type1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                Console.Write(Value + "".PadLeft(width - DataLength()));


                Console.ResetColor();

                //throw new NotImplementedException();
            }
        }


        public class ColumnData
        {
            public List<CellBase> Rows { get; private set; }
            public string Header { get; private set; }

            public ColumnData(string Header)
            {
                Rows = new List<CellBase>();
                this.Header = Header;
            }

            public void AddRow(CellBase row)
            {
                Rows.Add(row);
            }

            public virtual int DataLength()
            {
                if (Rows.Count == 0) return 0;
                return Rows.Max(n => n.DataLength());
            }

            public void WriteCell(int rowIndex, int width)
            {
                if (Rows.Count <= rowIndex)
                {
                    Console.Write("".PadLeft(width));
                    return;
                }
                Rows[rowIndex].PrintData(width);
            }

            public void AddHeaderAndValue(string header, string value, string parameter = "")
            {
                AddRow(new TitleCell(header, value, parameter));
            }
        }


    }

}

