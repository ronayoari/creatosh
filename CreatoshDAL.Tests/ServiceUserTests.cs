using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreatoshDAL.Tests
{
    [TestClass]
    public class ServiceUserTests
    {
        CreatoshDAL.ServiceUser service = new CreatoshDAL.ServiceUser();
        CreatoshDBEntities model = new CreatoshDBEntities();

        [TestInitialize]
        public void testInitUser()
        {
            if (model.Users.Find(301246987) == null)
            {
                User added = model.Users.Add(new User()
                {
                    user_ID = 301246987,
                    name = "Dan",
                    password = "7gD4fg4R",
                    type = "Student",
                    activities_Num = 0,
                    online = 0
                });
                model.SaveChanges();
            }
        }


        [TestMethod]
        public void testLogin()
        {
            User dan = model.Users.Find(301246987);
            string online = service.login(dan.user_ID);
            dan = model.Users.Find(301246987);
            Assert.IsTrue(dan.online == 1);
        }

        [TestMethod]
        public void testLogout()
        {
            User dan = model.Users.Find(301246987);
            service.login(dan.user_ID);
            string online = service.logout(dan.user_ID);
            dan = model.Users.Find(301246987);
            Assert.IsTrue(dan.online == 0);
        }

        [TestCleanup]
        public void testCleanupUser()
        {

            User dan = model.Users.Find(301246987);
            if ( dan != null)
            {
                model.Users.Remove(dan);
            }

        }
    }
}