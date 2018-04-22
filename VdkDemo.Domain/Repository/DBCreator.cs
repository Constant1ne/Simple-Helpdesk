using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VdkDemo.Domain.Repository
{
    public class DBCreator
    {
        private string connToSqlServer;
        private string connToDatabase;
        private string filePath;
        private string dbName;

        public string ConnStrToDB { get { return connToDatabase; } }
        public DBCreator(string dbName) {
            this.dbName = dbName;
            filePath = Directory.GetCurrentDirectory() + $"\\{dbName}";
            //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
            connToSqlServer = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                "Integrated Security = True;" +
                "Connection Timeout=20;";

            connToDatabase = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                $"Initial Catalog={dbName};" +
                "Integrated Security = True;" +
                "Connection Timeout=20;";
        }

        public bool createDataBase() {
            var createDBQuery = $"CREATE DATABASE {dbName} ON PRIMARY" +
                $" (NAME = {dbName}," +
                $" FILENAME = '{filePath}.mdf'," +
                " SIZE = 3MB, MAXSIZE = 100MB, FILEGROWTH = 10%)" +

                " LOG ON" +
                $" (NAME = {dbName}_log," +
                $" FILENAME = '{filePath}.ldf'," +
                " SIZE = 1MB, MAXSIZE = 5MB, FILEGROWTH = 10%);";

            return execQuery(createDBQuery, connToSqlServer);
        }

        public bool createTables() {
            var claimsTable = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Repository\DBScripts\Claims.sql");
            var historyTable = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Repository\DBScripts\History.sql");

            return execQuery(claimsTable, connToDatabase) 
                && execQuery(historyTable, connToDatabase);
        }

        private bool execQuery(string query, string connStr) {
            using (var conn = new SqlConnection(connStr))
            using (var command = new SqlCommand(query, conn)) {
                try {
                    conn.Open();
                    command.ExecuteNonQuery();
                    return true;
                } catch (Exception ex) {
                    return false;
                } finally {
                    if ((conn.State == ConnectionState.Open)) {
                        conn.Close();
                    }
                }
            }
        }
    }
}
