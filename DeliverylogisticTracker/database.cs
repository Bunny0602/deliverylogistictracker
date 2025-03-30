using System;
using MySql.Data.MySqlClient;

namespace DeliverylogisticTracker
{
    class database
    {
        private static string connString = "server=localhost;database=deliverylogistics;user=root;password=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connString);
        }
    }
}
