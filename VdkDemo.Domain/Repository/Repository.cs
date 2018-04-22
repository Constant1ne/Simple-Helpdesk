using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using VdkDemo.Domain.Repository.Models;

namespace VdkDemo.Domain.Repository
{
    public class Repository<T> where T : class
    {
        private string connectionString;

        public Repository(string connectionStr) {
            this.connectionString = connectionString;
        }

        private DataModels.ClaimsDB getConnection() {
            return new DataModels.ClaimsDB(connectionString);
        }

        public int Insert<T>(T entity) {
            using (var conn = getConnection()) {
                return conn.Insert<T>(entity);
            }
        }

        public void Delete<T>(T entity) {
            using (var conn = getConnection()) {
                conn.Delete<T>(entity);
            }
        }

        public List<T> GetAll<T>() where T : class {
            using (var conn = getConnection()) {
                return conn.GetTable<T>().ToList();
            }
        }

        /*public T Find<T>(int id) where T : class {
            using (var conn = getConnection()) {
                
            }
        }*/
    }
}
