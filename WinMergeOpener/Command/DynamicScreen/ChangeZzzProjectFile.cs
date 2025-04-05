using System.Collections.Generic;
using System.Xml;

namespace WinMergeOpener.Command.DynamicScreen
{

    public class ChangeZzzProjectFile
    {
        readonly XmlDocument doc = new XmlDocument();
        public List<string> AddProjectFile { get; set; }
        public string CodesDirectory { get; set; }
        public string DirectoryOfSourceCodes { get; set; }
        public string FullZZFilePath { get; set; }
        public void Change()
        {
            doc.Load(FullZZFilePath);

            var compileNode = FindCompileNode();
            AddFiles(compileNode);
            Save();
        }

        void AddFiles(XmlNode compileNode)
        {
            compileNode.InnerXml = string.Empty;

            foreach (var file in AddProjectFile)
            {
                var f1 = file.Replace(CodesDirectory, "");
                var compile = doc.CreateElement("Compile");
                compile.SetAttribute("Include", "Codes" + f1);
                compileNode.AppendChild(compile);
            }
        }

        XmlNode FindCompileNode()
        {
            XmlNode ItemGroupXml = null;

            foreach (XmlNode item in doc.DocumentElement)
            {
                if (!item.Name.Equals("ItemGroup")) continue;

                if (!item.InnerXml.Contains("Compile Include")) continue;

                if (ItemGroupXml == null)
                {
                    ItemGroupXml = item;
                    continue;
                }

                item.RemoveAll();


            }

            if (ItemGroupXml == null)
            {
                var itemGroup = doc.CreateElement("ItemGroup");
                doc.DocumentElement.AppendChild(itemGroup);
                return itemGroup;
            }

            return ItemGroupXml;
        }

        void Save()
        {
            doc.InnerXml = doc.InnerXml.Replace("xmlns=\"\"", "");
            doc.Save(FullZZFilePath);
        }
    }
}
