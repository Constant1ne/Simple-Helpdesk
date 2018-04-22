using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels;

namespace VdkDemo.Domain.Repository.Models
{
    public class Claim {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<History> History;

        public Claim() { }
        public Claim(DataModels.Claim dbObject) {
            Id = dbObject.Id;
            Name = dbObject.Name;
            this.History = dbObject.Histories.Select(item => new History(item)).ToList();
        }

        public DataModels.Claim toDB() {
            return new DataModels.Claim() {
                Id = this.Id,
                Name = this.Name,
                Histories = History.ConvertAll<DataModels.History>(item => item.toDB())
            };
        }
    }
}
