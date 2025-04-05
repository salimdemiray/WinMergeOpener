using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WinMergeOpener;

namespace MergerTest
{
    [TestClass]
    public class LoadTest
    {
        [TestMethod]
        public void ZZ_CopyRun_FromTag()
        {
            /*
             
             "\\menu"
"C:\\Uyumsoft\\UyumProjects\\Senfoni\\branches\\release.1.0.60.0\\Senfoni\\Modules\\HRM.WEB\\CustomCode\\SubContractCodes\\EmployeeAllowanceCreateOperation.cs"
45
45
"C:\\Uyumsoft\\UyumProjects\\Senfoni\\branches\\release.1.0.60.0\\Senfoni\\Modules\\HRM\\HRM\\HRM.csproj"*/

            WinMergeOpener.Program.TestCommand("zz_cp1",new string[]{"\\menu",
               @"C:\Uyumsoft\UyumProjects\Senfoni\branches\Tag\Erp.4.2109\170\Senfoni\Senfoni\Senfoni\HRM\BatchSchool.xml","45","45",
               @"C:\Uyumsoft\UyumProjects\Senfoni\branches\Tag\Erp.4.2109\170\Senfoni\Senfoni\Senfoni\Senfoni.csproj" });
            //Main()

        }
        [TestMethod]
        public void ZZ_CopyRun_FromBranch()
        {
            /*
             
             "\\menu"
"C:\\Uyumsoft\\UyumProjects\\Senfoni\\branches\\release.1.0.60.0\\Senfoni\\Modules\\HRM.WEB\\CustomCode\\SubContractCodes\\EmployeeAllowanceCreateOperation.cs"
45
45
"C:\\Uyumsoft\\UyumProjects\\Senfoni\\branches\\release.1.0.60.0\\Senfoni\\Modules\\HRM\\HRM\\HRM.csproj"*/

            WinMergeOpener.Program.TestCommand("zz_cp1", new string[]{"\\menu",
               "C:\\Uyumsoft\\UyumProjects\\Senfoni\\branches\\release.1.0.60.0\\Senfoni\\Modules\\HRM.WEB\\CustomCode\\SubContractCodes\\EmployeeAllowanceCreateOperation.cs","45","45",
               "C:\\Uyumsoft\\UyumProjects\\Senfoni\\branches\\release.1.0.60.0\\Senfoni\\Modules\\HRM\\HRM\\HRM.csproj" });
            //Main()

        }
    }
}
