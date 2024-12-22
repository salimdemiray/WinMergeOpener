using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinMergeOpener.Command.FileChanger.CS
{
    public interface IFileChanger
    {
        string OptionKey { get; }
        string OptionDesc { get; }
        void ChangeFile(string file);

    }
    public class LoadCollectionFromCollType_GlobalObj_Load_Ops : IFileChanger
    {
        public string OptionKey => "L1";

        public string OptionDesc => "LoadCollectionFromCollType -> GlobalObj.Load";

        public void ChangeFile(string file)
        {
            CsFileChanger.OpenFileCs(file, true, SaveOps);
        }

        string SaveOps(string content)
        {
            string pattern = @"\((\w+)\)gc\.LoadCollectionFromCollType\(typeof\(\w+\),?\s*(\w*)\)";
            string substitution = @"GlobalObj.Load<$1>($2)";

            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;

            Regex regex = new Regex(pattern, options);
            string result = regex.Replace(content, substitution);

            return result;
        }
    }
}
