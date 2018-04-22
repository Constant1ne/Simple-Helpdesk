using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VdkDemo.Domain.Repository;

namespace TestProject
{
    [TestClass]
    public class DBCreatorTest
    {
        [TestMethod]
        public void createDb() {
            var dbName = "Claims";
            var dbCreator = new DBCreator(dbName);

            if (dbCreator.createDataBase()) {
                dbCreator.createTables();
            }
        }
    }
}
