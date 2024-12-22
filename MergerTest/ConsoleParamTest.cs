using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinMergeOpener;
using System.Windows;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace MergerTest
{
    [TestClass]
    public class ConsoleParamTest
    {
        ConsoleParamReader paramReader;
        [TestInitialize()]
        public void Init()
        {

            paramReader = new ConsoleParamReader();
        }

        [TestMethod]
        public void ParseM1_Merge1()
        {
            //var res = paramReader.Parse("M1");
            //Assert.AreEqual(res.Command, "M");
            //Assert.AreEqual(res.VersionNo, 1);
            //Assert.AreEqual(res.TargetNo, 0);
        }

        [TestMethod]
        public void Parse1_Merge1()
        {
            //var res = paramReader.Parse("1");
            //Assert.AreEqual(res.Command, "");
            //Assert.AreEqual(res.VersionNo, 1);
            //Assert.AreEqual(res.TargetNo, 0);
        }

        [TestMethod]
        public void Parse1_3_Merge1Target3()
        {
            //var res = paramReader.Parse("1 3");
            //Assert.AreEqual(res.Command, "");
            //Assert.AreEqual(res.VersionNo, 1);
            //Assert.AreEqual(res.TargetNo, 3);
        }

        [TestMethod]
        public void ParseM1_3_Merge1Target3()
        {
            //var res = paramReader.Parse("M1 3");
            //Assert.AreEqual(res.Command, "M");
            //Assert.AreEqual(res.VersionNo, 1);
            //Assert.AreEqual(res.TargetNo, 3);
        }

        [TestMethod]
        public void ParseMK1_MustBeMK_1()
        {
            //var res = paramReader.Parse("MK1");
            //Assert.AreEqual(res.Command, "MK");
            //Assert.AreEqual(res.VersionNo, 1);
            //Assert.AreEqual(res.TargetNo, 0);
        }

        [TestMethod]
        public void ParseMK1_3_MustBeMK_1_3()
        {
            //var res = paramReader.Parse("MK1 3");
            //Assert.AreEqual(res.Command, "MK");
            //Assert.AreEqual(res.VersionNo, 1);
            //Assert.AreEqual(res.TargetNo, 3);
        }

        [TestMethod]
        public void Parse_Multi_M1_2_3_4_1()
        {

            string tx = "";
            var Lines = tx.Split('\n');
            var newLines = new List<string>();

            foreach (var line in Lines)
            {
                var sd = line.Trim('\r', ' ', '\t', ';', '/').Trim().Split('=');
                newLines.Add(string.Format("{0}={1};", sd[1], sd[0]));
            }


            var resultText = string.Join("\r\n", newLines);



        }


        [TestMethod]
        public void Parse_Multi_M1_2_3_4_5()
        {
            var res = paramReader.Parse("MK1,2;3;4;5");
            Assert.AreEqual(res.Command, "MK");
            Assert.AreEqual(res.VersionNo, 1);
            //Assert.AreEqual(res.TargetNo, 3);
            //Assert.AreEqual(res.ExtraNumberParams, "1,2;3;4;5");
        }

        [TestMethod]
        public void Parse_Multi_M1_2_3_4_5_XX()
        {


            //
            //            var files = new string[]{
            //@"/branches/release.1.0.60.0/Senfoni/Modules/HRM/HRM/_CustomCode/CommonOps.PayrollOps.cs"
            //};

            var files = new string[]{
@"Senfoni/Modules/HRM.WEB/CustomCode/ConstDataManagment.cs"
};
            //string SolutionPath = @"D:\UyumProjects\Senfoni\branches\release.1.0.60.0";
            //D:\UyumProjects\Senfoni\branches\release.1.0.60.0Senfoni\Modules\HRM.WEB\CustomCode\ConstDataManagment.cs
            //D:\UyumProjects\Senfoni\branches\release.1.0.60.0.Live\Senfoni\Modules\HRM.WEB\CustomCode\ConstDataManagment.cs
            //var basePath = FindBasePath(files[0], SolutionPath);
            //foreach (string file in files)
            //{
            //    string newFileName = files[0].Replace("/", "\\");
            //    string FullPath = basePath + newFileName;
            //    Guid.NewGuid().ToString();
            //    System.Threading.Thread.Sleep(1000);
            //}

        }

        [TestMethod]
        public void Parse_Multi_M1_2_3_4_6_XX()
        {
            var files = new string[]{
@"\branches\release.1.0.60.0.Live\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.PayrollOps.cs"


};

            string SolutionPath = @"D:\UyumProjects\Senfoni\branches\Tag\Erp.4.2109\38";
            //var basePath = FindBasePath(files[0], SolutionPath);



            //foreach (string file in files)
            //{
            //    string newFileName = files[0].Replace("/", "\\");
            //    string FullPath = basePath + newFileName;
            //    Guid.NewGuid().ToString();
            //    System.Threading.Thread.Sleep(1000);
            //}

            var results = new Finder(SolutionPath).GetList(files);
            //D:\UyumProjects\Senfoni\branches\Tag\release.1.0.60.0.Live\387\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.PayrollOps.cs

        }

        [TestMethod]
        public void Test_AyniSolutionDosyasi()
        {
            var files = new string[] { @"\branches\release.1.0.60.0.Live\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.PayrollOps.cs" };
            string SolutionPath = @"D:\UyumProjects\Senfoni\branches\release.1.0.60.0.Live";
            var results = new Finder(SolutionPath).GetList(files);
            var isSame = string.Equals(results[0], @"D:\UyumProjects\Senfoni\branches\release.1.0.60.0.Live\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.PayrollOps.cs"
, StringComparison.InvariantCultureIgnoreCase);
            Assert.IsTrue(isSame);
        }

        [TestMethod]
        public void Test_BaskaSolutionDosyasi()
        {
            var files = new string[] { @"\branches\release.1.0.60.0.Live\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.PayrollOps.cs" };
            string SolutionPath = @"D:\UyumProjects\Senfoni\branches\Erp.4.2109";
            var results = new Finder(SolutionPath).GetList(files);

            var isSame = string.Equals(results[0], @"D:\UyumProjects\Senfoni\branches\Erp.4.2109\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.PayrollOps.cs"
, StringComparison.InvariantCultureIgnoreCase);

            Assert.IsTrue(isSame);
        }


        [TestMethod]
        public void Test_BaskaSolutionDosyasi1()
        {
            var files = new string[] { @"Senfoni\Modules\HRM.WEB\CustomCode\PayrollFastInput.cs" };
            string SolutionPath = @"D:\UyumProjects\Senfoni\branches\Erp.4.2109";
            var results = new Finder(SolutionPath).GetList(files);

            var isSame = string.Equals(results[0], @"D:\UyumProjects\Senfoni\branches\Erp.4.2109\Senfoni\Modules\HRM.WEB\CustomCode\PayrollFastInput.cs"
, StringComparison.InvariantCultureIgnoreCase);

            Assert.IsTrue(isSame);
        }

        [TestMethod]
        public void Test_UYus()
        {
            var tt = WinMergeOpener.Command.UyumCommand.CleanEmptyAttribute(",LeftAlias = \"\", RightAlias = \"\",     Order = 0, CustomSql = \"\"");

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void Tag0()
        {
            /*
            2 D: \UyumProjects\Senfoni\branches\Tag\Erp.4.2109\38 -
\branches\release.1.0.60.0.Live\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.PayrollOps.cs

            2 D:\UyumProjects\Senfoni\branches\Tag\Erp.4.2109\38-
\branches\release.1.0.60.0.Live\Senfoni\Modules\HRM\HRM\_CustomCode\CommonOps.PayrollOps.cs

                */

            var files = new string[] { @"Senfoni\Modules\HRM.WEB\CustomCode\PayrollFastInput.cs" };
            string SolutionPath = @"D:\UyumProjects\Senfoni\branches\Tag\Erp.4.2109\38";
            var results = new Finder(SolutionPath).GetList(files);

            var isSame = string.Equals(results[0], @"D:\UyumProjects\Senfoni\branches\Erp.4.2109\Senfoni\Modules\HRM.WEB\CustomCode\PayrollFastInput.cs"
, StringComparison.InvariantCultureIgnoreCase);

            Assert.IsTrue(isSame);
        }



        class Finder
        {
            readonly string _SolutionPath;
            bool IsSolutionTag;

            public Finder(string SolutionPath)
            {
                _SolutionPath = SolutionPath;
            }

            public string[] GetList(string[] filenames)
            {
                if (filenames.Length == 0) return new string[0];
                List<string> openList = new List<string>();

                foreach (string file in filenames)
                {
                    openList.Add(GetFilePath(file));
                }
                return openList.ToArray();
            }

            string GetFilePath(string file)
            {
                bool isTag = file.IndexOf(@"\tag\", StringComparison.InvariantCultureIgnoreCase) >= 0;
                var reg = @"(\\branches\\[\w\._]+\\senfoni|senfoni)";

                var targetFile = "";

                var m1 = Regex.Match(file, @"\\branches\\[\w\._]+\\senfoni|senfoni", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    targetFile = file.Replace(m1.Value, "senfoni");
                    return Connect(_SolutionPath, targetFile);
                }

                return "";
            }

            string Connect(string p1, string p2)
            {
                return p1.Trim('\\') + "\\" + p2.Trim('\\');

            }

        }


    }




}
