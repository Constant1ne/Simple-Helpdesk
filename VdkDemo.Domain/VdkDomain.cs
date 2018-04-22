using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdkDemo.Domain.Repository.Models;
using LinqToDB;

namespace VdkDemo.Domain
{
    public class VdkDomain
    {
        public void Insert(Claim claim, History status) {
            var str = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                $"Initial Catalog=Claims;" +
                "Integrated Security = True;" +
                "Connection Timeout=20;";
            using (var conn = new DataModels.ClaimsDB(str)) {
                conn.BeginTransaction();

                claim.History.Add(status);
                status.ParentClaim = claim;

                var id = conn.Insert(claim.toDB());
                status.ClaimId = id;
                var sid = conn.Insert(status.toDB());

                conn.CommitTransaction();
            }
        }
    }
}
