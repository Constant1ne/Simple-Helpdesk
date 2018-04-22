using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdkDemo.Domain.Repository.Models
{
    public class History {
        public EClaimStatus Status;
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public Claim ParentClaim { get; set; }
        public EClaimStatus CurrentStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comment { get; set; }

        public History() { }
        public History(DataModels.History dbObject) {
            Id = dbObject.Id;
            ClaimId = dbObject.ClaimId;
            ParentClaim = new Claim(dbObject.Claim);
            CurrentStatus = (EClaimStatus)dbObject.Status;
            CreatedDate = dbObject.CreatedDate;
            Comment = dbObject.Comment;
        }
        public DataModels.History toDB() {
            return new DataModels.History() {
                Id = this.Id,
                ClaimId = this.ClaimId,
                //Claim = ParentClaim.toDB(),
                CreatedDate = this.CreatedDate,
                Status = (int)this.CurrentStatus,
                Comment = this.Comment
            };
        }
    }

    public enum EClaimStatus : int {
        //[LinqToDB.Mapping.MapValue(Value = 0)]
        Opened = 0,
        //[LinqToDB.Mapping.MapValue(Value = 1)]
        Solved,
        //[LinqToDB.Mapping.MapValue(Value = 2)]
        Returned,
        //[LinqToDB.Mapping.MapValue(Value = 3)]
        Closed
    }
}
