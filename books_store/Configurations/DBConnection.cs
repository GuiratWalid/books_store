using Microsoft.Data.SqlClient;

namespace books_store.Configurations
{
    public sealed class DBConnection
    {
        private static DBConnection db = null;
        private static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Walid\\Documents\\books_store.mdf;Integrated Security=True;Connect Timeout=30";
        private static SqlConnection sqlConnection = null;

        public static SqlConnection GetInstance
        {
            get
            {
                if(db == null)
                {
                    lock (sqlConnection)
                    {
                        if(db == null)
                        {
                            db = new DBConnection();
                            sqlConnection = new SqlConnection(connectionString);
                            Console.WriteLine(sqlConnection);
                        }
                    }
                }
                return sqlConnection;
            }
        }
    }
}
