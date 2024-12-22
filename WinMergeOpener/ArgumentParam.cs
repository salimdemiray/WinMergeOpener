using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMergeOpener
{
    public class ArgumentParam
    {
        public ArgumentParam(string[] args)
        {
            int _LineNo = 0, _ColumnNo = 0;
            IsMenu = args[0] == @"\menu";
            if (args.Length >= 2) FileName = args[1];
            if (args.Length >= 3) int.TryParse(args[2], out _LineNo);
            if (args.Length >= 4) int.TryParse(args[3], out _ColumnNo);
            if (args.Length >= 5) ProjectFile = args[4];


            LineNo = _LineNo;
            ColumnNo = _ColumnNo;
            CommandLine = string.Join(" ", args);
        }

        public bool IsMenu { get; set; }
        public string FileName { get; set; }
        public int LineNo { get; set; }
        public int ColumnNo { get; set; }
        public string ProjectFile { get; set; }
        public string ReadSelectedText { get; set; }
        public string CommandLine { get; set; }
    }
}
