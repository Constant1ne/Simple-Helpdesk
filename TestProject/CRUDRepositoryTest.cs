using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VdkDemo.Domain.Repository;
using VdkDemo.Domain;
using VdkDemo.Domain.Repository.Models;

namespace TestProject
{
    [TestClass]
    public class CRUDRepositoryTest
    {
        [TestMethod]
        public void TestMethod1() {
            //using (var conn = new Repository(dbCreator.ConnStrToDB)) { }
            //var tmp = new VdkDemo.Domain.Repository.Models.Claim();
            var dom = new VdkDomain();
            var claim = new Claim() {
                Name = "Название заявки",
                History = new System.Collections.Generic.List<History>()
            };
            var status = new History() {
                Comment = "Комментарий к статусу",
                CreatedDate = DateTime.Now,
                CurrentStatus = EClaimStatus.Opened,
            };
            dom.Insert(claim, status);
        }
    }
}
