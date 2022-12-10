using Microsoft.Data.SqlClient;

namespace books_store.Configurations
{
    public sealed class DBConnection
    {
        private static DBConnection db = null;
        private static SqlConnection con = null;

        private DBConnection()
        {
            //con = SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Walid\\Documents\\books_store.mdf;Integrated Security=True;Connect Timeout=30");
        }
        public static DBConnection GetInstance
        {
            get
            {
                if(db == null)
                {
                    lock (con)
                    {
                        if(db == null)
                        {
                            db = new DBConnection();
                        }
                    }
                }
                return db;
            }
        }
    }
}
