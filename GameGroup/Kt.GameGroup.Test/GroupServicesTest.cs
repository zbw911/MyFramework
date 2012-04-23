using Kt.GameGroup.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Kt.GameGroup.Data;
using Kt.GameGroup.Tests.NinjectTester;
using Ninject;
using Kt.GameGroup.Model.ViewModel;
using System.Collections.Generic;

namespace Kt.GameGroup.Test
{


    /// <summary>
    ///这是 GroupServicesTest 的测试类，旨在
    ///包含所有 GroupServicesTest 单元测试
    ///</summary>
    [TestClass()]
    public class GroupServicesTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion




        /// <summary>
        ///CreateGroup 的测试
        ///</summary>
        [TestMethod()]
        public void CreateGroupTest1()
        {

            GroupServices target = TesterInit.Kernel.Get<GroupServices>();
            Decimal uId = 111; // TODO: 初始化为适当的值
            int gameId = 1; // TODO: 初始化为适当的值
            int platFormId = 2; // TODO: 初始化为适当的值
            int gameserverid = 3; // TODO: 初始化为适当的值
            string gName = "塔顶adsfasdf"; // TODO: 初始化为适当的值
            string descript = "de j8 script"; // TODO: 初始化为适当的值
            bool expected = true; // TODO: 初始化为适当的值
            bool actual;
            bool viewPerm = true;
            int joinPerm = 1;
            actual = target.CreateGroup(uId, gameId, platFormId, gameserverid, gName, descript, joinPerm, viewPerm);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///HotGroup 的测试
        ///</summary>
        [TestMethod()]
        public void HotGroupTest()
        {
            //IRepository<group_Infor> GroupInforRepository = null; // TODO: 初始化为适当的值
            //IRepository<group_gameStat> GroupGameStatRepository = null; // TODO: 初始化为适当的值
            //IRepository<group_member> GroupMemberRepository = null;
            //GroupServices target = new GroupServices(GroupInforRepository, GroupGameStatRepository, GroupMemberRepository); // TODO: 初始化为适当的值
            //int topgame = 0; // TODO: 初始化为适当的值
            //int tophotgroup = 0; // TODO: 初始化为适当的值
            //IEnumerable<HotGameGroupInfo> expected = null; // TODO: 初始化为适当的值
            //IEnumerable<HotGameGroupInfo> actual;
            //actual = target.HotGroup(topgame, tophotgroup);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ModifyGroup 的测试
        ///</summary>
        [TestMethod()]
        public void ModifyGroupTest()
        {
            //IRepository<group_Infor> GroupInforRepository = null; // TODO: 初始化为适当的值
            //IRepository<group_gameStat> GroupGameStatRepository = null; // TODO: 初始化为适当的值
            //IRepository<group_member> GroupMemberRepository = null;
            //GroupServices target = new GroupServices(GroupInforRepository, GroupGameStatRepository, GroupMemberRepository); // TODO: 初始化为适当的值
            //ModifyGroupInfo GroupInfo = null; // TODO: 初始化为适当的值
            //bool expected = false; // TODO: 初始化为适当的值
            //bool actual;
            //actual = target.ModifyGroup(GroupInfo);
            //Assert.AreEqual(expected, actual);
         
        }
    }
}
