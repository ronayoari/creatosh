using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WCFAzureDALServiceWebRole.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            WCFAzureDALServiceWebRole.IService1 service = new WCFAzureDALServiceWebRole.Service1();
            WCFAzureDALServiceWebRole.GameSequenceEntityX entity1 = new WCFAzureDALServiceWebRole.GameSequenceEntityX("1", "0", "None");
            WCFAzureDALServiceWebRole.GameSequenceEntityX entity2 = new WCFAzureDALServiceWebRole.GameSequenceEntityX("1", "1", "None");
            WCFAzureDALServiceWebRole.GameSequenceEntityX entity3 = new WCFAzureDALServiceWebRole.GameSequenceEntityX("1", "2", "None");
            WCFAzureDALServiceWebRole.GameSequenceEntityX entity4 = new WCFAzureDALServiceWebRole.GameSequenceEntityX("1", "3", "None");
            List<WCFAzureDALServiceWebRole.GameSequenceEntityX> entities = new List<WCFAzureDALServiceWebRole.GameSequenceEntityX>();
            GameSequenceEntityX[] arrayEntites = new GameSequenceEntityX[entities.Count];
            entities.CopyTo(arrayEntites);
            Console.WriteLine(arrayEntites);
            Assert.AreEqual("SUCCESS - game sequence was added succesfully", service.AddEntitiesToTable(arrayEntites, "games"));
            
            List<GameSequenceEntityX> arrayEntitiesRetrieved = service.RetrieveAllGameSequenceEntities("1", "games");
            if (arrayEntitiesRetrieved == null)
            {
                Console.WriteLine("retrived error");
            }
            Assert.AreEqual(entities, arrayEntitiesRetrieved);

            Assert.AreEqual("SUCCESS - Game sequence 0 was deleted", service.DeleteEntityInTable("1", "0", "games"));
            Assert.AreEqual("SUCCESS - Game sequence 1 was deleted", service.DeleteEntityInTable("1", "1", "games"));
            Assert.AreEqual("SUCCESS - Game sequence 2 was deleted", service.DeleteEntityInTable("1", "2", "games"));
            Assert.AreEqual("SUCCESS - Game sequence 3 was deleted", service.DeleteEntityInTable("1", "3", "games"));

            Assert.AreEqual(new GameSequenceEntityX[0], service.RetrieveAllGameSequenceEntities("1", "games"));

        }

    }
}
