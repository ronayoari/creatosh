using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Entity;
*/

namespace CreatoshDAL.Tests
{
    [TestClass]
    public class ServiceGameTests
    {
        CreatoshDAL.ServiceGame service = new CreatoshDAL.ServiceGame();
        CreatoshDBEntities model = new CreatoshDBEntities();
        int gameID=0;

        [TestInitialize]
        public void testInitGame()
        {


            if(model.Users.Find(306487249)==null)
            {
                User added = model.Users.Add(new User()
                {
                    user_ID = 306487249,
                    name = "Rona yoari",
                    password = "24fD3cF5",
                    type = "Student",
                    activities_Num =0
                });
                model.SaveChanges();
            }



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
        public void testGame()
        {
           string gameName = "Game 6";
            string description = "..........";
            DateTime dt = DateTime.Now;
            int creator_ID = 306487249;
            int grade_level = 4;
            string template_name = "Multiple choice";
            string subject_name = "English";
            int status = 0;
            string inputString = service.AddGame(gameName, description, dt, creator_ID, grade_level, template_name, subject_name);
            model.SaveChanges();
            
            bool parsed = Int32.TryParse(inputString, out gameID);

            Assert.IsTrue(parsed);
            if (parsed)
            {
                Assert.AreNotEqual(model.Games.Find(gameID),null);
                Console.WriteLine("Test Pass");
            }
            
        }

        [TestMethod]
        public void testPublishGame()
        {
        }

        [TestMethod]
        public void testReturnGameForReCreate()
        {
        }



        [TestCleanup]
        public void testCleanupGame()
        {
            Game g =model.Games.Find(gameID);
            if (g != null)
                model.Games.Remove(g);
            model.SaveChanges();
        }




    }



}
