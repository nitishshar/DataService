using System;
using System.Data.Odbc;

namespace DataService.Helpers
{
    public class DBConnector : IDisposable
    { 
        public OdbcConnection Connection { get; }

        public DBConnector(string connectionString)
        {
            Connection = new OdbcConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}