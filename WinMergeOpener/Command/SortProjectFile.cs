using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace WinMergeOpener.Command
{
    public partial class UyumCommand
    {
        public class SortProjectFile
        {
            readonly ArgumentParam argParam;
            readonly List<XmlNode> CompileNodeList;

            public SortProjectFile(ArgumentParam argParam)
            {
                this.argParam = argParam;
                CompileNodeList = new List<XmlNode>();
            }

            public void Run()
            {
                var document = new XmlDocument();
                document.Load(argParam.ProjectFile);

                if (!(document["Project"] is XmlNode projectNode)) return;

                foreach (XmlNode ndItemGroup in projectNode.ChildNodes)
                {
                    if (ndItemGroup.Name != "ItemGroup") continue;
                    SortItemGroup(ndItemGroup);
                }

                document.Save(argParam.ProjectFile);
            }

            void SortItemGroup(XmlNode ndItemGroup)
            {
                CompileNodeList.Clear();
                foreach (XmlNode ndCompile in ndItemGroup.ChildNodes)
                {
                    if (ndCompile.Name != "Compile") continue;
                    CompileNodeList.Add(CleanSubTypeCode(ndCompile));
                }

                if (CompileNodeList.Count == 0) return;

                for (int i = 0; i < ndItemGroup.ChildNodes.Count; i++)
                {
                    XmlNode ndCompile = ndItemGroup.ChildNodes[i];
                    if (ndCompile.Name != "Compile") continue;
                    ndItemGroup.RemoveChild(ndCompile);
                    i--;
                }
                var orderedList = CompileNodeList
                      .Select(n => new { Text = n.Attributes["Include"].Value, Node = n })
                      .OrderBy(n => n.Text)
                      .ToArray();

                foreach (var item in orderedList)
                {
                    ndItemGroup.AppendChild(item.Node);
                }
            }

            XmlNode CleanSubTypeCode(XmlNode ndCompile)
            {
                if (!ndCompile.HasChildNodes) return ndCompile;

                if (!(ndCompile["SubType"] is XmlElement SubTypeNode)) return ndCompile;

                if (SubTypeNode.InnerText != "Code") return ndCompile;

                ndCompile.RemoveChild(SubTypeNode);
                ndCompile.Normalize();
                (ndCompile as XmlElement).IsEmpty = true;
                return ndCompile;
            }
        }
    }
}
