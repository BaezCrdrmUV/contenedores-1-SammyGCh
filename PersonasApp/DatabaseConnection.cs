using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PersonasApp
{
    public class DatabaseConnection
    {
        private string infoConnection;
        private MySqlConnection connection;

        private void Connect()
        {
            try
            {
                infoConnection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                // infoConnection = "server=localhost;port=3320;user=adminPersonas;password=practica1;database=personas";
                Console.WriteLine("DBStr {0}", infoConnection);
                
                connection = new MySqlConnection(infoConnection);
                connection.Open();
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public MySqlConnection OpenConnection()
        {
            try
            {
                Connect();
            }
            catch (MySqlException)
            {
                throw;
            }

            return connection;
        }

        public void CloseConnection()
        {
            if (connection != null)
            {
                try
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }
    }
}
