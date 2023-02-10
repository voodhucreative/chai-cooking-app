using System;
using NUnit.Framework;

namespace ChaiCooking.Tests.ModelTests
{
    [TestFixture]
    public class UserModelTests
    {
        [Test]
        public void CreateUserTest()
        {
            Models.User user = new Models.User();
        }

        [Test]
        public void RegisterUserTest()
        {
            //Models.User user = new Models.User();
            //bool res = Services.ApiBridge.Register(user).Result;
            //Assert.IsTrue(res);
        }

        [Test]
        public void LoginUserTest()
        {
            //Models.User user = new Models.User();
            //bool res = Services.ApiBridge.Login(user).Result;
            //Assert.IsTrue(res);
        }

        [Test]
        public void LogoutUserTest()
        {
            //Models.User user = new Models.User();
            //bool res = Services.ApiBridge.LogOut(user).Result;
            //Assert.IsTrue(res);
        }

        [Test]
        public void PopulateList() // arbitrary for now
        {

        }
    }
}
