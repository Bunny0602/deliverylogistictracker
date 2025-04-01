using System;
using MySql.Data.MySqlClient;

namespace DeliverylogisticTracker
{
    class driver
    {
        public static void viewPendingorders(string driverEmail)
        {
            using (var conn = database.GetConnection())
            {
                conn.Open();
                string query = "SELECT id, email, product, pickup_location, pickup_name, dropoff_location, dropoff_name FROM orders WHERE status='To Ship'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("+--------------------------------------------------------------------------------------------------+");
                Console.WriteLine("|                                           Pending Orders                                         |");
                Console.WriteLine("+------+-----------------+--------------------+----------+--------------------+------------+-------+");
                Console.WriteLine("|  ID  |     Product     |   Pickup Location  |  Sender  |  Dropoff Location  |  Receiver  |  Fee  |");
                Console.WriteLine("+------+-----------------+--------------------+----------+--------------------+------------+-------+");

                bool hasOrder = false;
                List<int> availableOrderIds = new List<int>();

                while (reader.Read())
                {
                    hasOrder = true;
                    int orderId = Convert.ToInt32(reader["id"]);
                    string product = reader["product"].ToString();
                    string pickup = reader["pickup_location"].ToString();
                    string pickupName = reader["pickup_name"].ToString();
                    string dropoff = reader["dropoff_location"].ToString();
                    string dropoffName = reader["dropoff_name"].ToString();
                    int price = new Random().Next(100, 500);

                    availableOrderIds.Add(orderId);

                    Console.WriteLine($"| {orderId,-4} | {product,-15} | {pickup,-18} | {pickupName,-8} | {dropoff,-18} | {dropoffName,-10} | {price,5} |");
                }

                reader.Close();

                if (!hasOrder)
                {
                    Console.WriteLine("|                                        No Pending Orders                                         |");
                    Console.WriteLine("+--------------------------------------------------------------------------------------------------+");
                    Console.WriteLine("Press any key to go back...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("+--------------------------------------------------------------------------------------------------+");

                int orderID;
                while (true)
                {
                    Console.Write("Enter Order ID to accept (or 0 to cancel): ");
                    if (int.TryParse(Console.ReadLine(), out orderID))
                    {
                        if (orderID == 0) return;
                        if (availableOrderIds.Contains(orderID)) break;
                    }
                    Console.WriteLine(@"
██╗███╗   ██╗██╗   ██╗ █████╗ ██╗     ██╗██████╗      ██████╗ ██████╗ ██████╗ ███████╗██████╗     ██╗██████╗                                    
██║████╗  ██║██║   ██║██╔══██╗██║     ██║██╔══██╗    ██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗    ██║██╔══██╗                                   
██║██╔██╗ ██║██║   ██║███████║██║     ██║██║  ██║    ██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝    ██║██║  ██║                                   
██║██║╚██╗██║╚██╗ ██╔╝██╔══██║██║     ██║██║  ██║    ██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗    ██║██║  ██║                                   
██║██║ ╚████║ ╚████╔╝ ██║  ██║███████╗██║██████╔╝    ╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║    ██║██████╔╝██╗                                
╚═╝╚═╝  ╚═══╝  ╚═══╝  ╚═╝  ╚═╝╚══════╝╚═╝╚═════╝      ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝    ╚═╝╚═════╝ ╚═╝                                
                                                                                                                                                
██████╗ ██╗     ███████╗ █████╗ ███████╗███████╗    ███████╗███╗   ██╗████████╗███████╗██████╗      █████╗                                      
██╔══██╗██║     ██╔════╝██╔══██╗██╔════╝██╔════╝    ██╔════╝████╗  ██║╚══██╔══╝██╔════╝██╔══██╗    ██╔══██╗                                     
██████╔╝██║     █████╗  ███████║███████╗█████╗      █████╗  ██╔██╗ ██║   ██║   █████╗  ██████╔╝    ███████║                                     
██╔═══╝ ██║     ██╔══╝  ██╔══██║╚════██║██╔══╝      ██╔══╝  ██║╚██╗██║   ██║   ██╔══╝  ██╔══██╗    ██╔══██║                                     
██║     ███████╗███████╗██║  ██║███████║███████╗    ███████╗██║ ╚████║   ██║   ███████╗██║  ██║    ██║  ██║                                     
╚═╝     ╚══════╝╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝    ╚══════╝╚═╝  ╚═══╝   ╚═╝   ╚══════╝╚═╝  ╚═╝    ╚═╝  ╚═╝                                     
                                                                                                                                                
██╗   ██╗ █████╗ ██╗     ██╗██████╗     ██████╗ ███████╗███╗   ██╗██████╗ ██╗███╗   ██╗ ██████╗      ██████╗ ██████╗ ██████╗ ███████╗██████╗    
██║   ██║██╔══██╗██║     ██║██╔══██╗    ██╔══██╗██╔════╝████╗  ██║██╔══██╗██║████╗  ██║██╔════╝     ██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗   
██║   ██║███████║██║     ██║██║  ██║    ██████╔╝█████╗  ██╔██╗ ██║██║  ██║██║██╔██╗ ██║██║  ███╗    ██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝   
╚██╗ ██╔╝██╔══██║██║     ██║██║  ██║    ██╔═══╝ ██╔══╝  ██║╚██╗██║██║  ██║██║██║╚██╗██║██║   ██║    ██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗   
 ╚████╔╝ ██║  ██║███████╗██║██████╔╝    ██║     ███████╗██║ ╚████║██████╔╝██║██║ ╚████║╚██████╔╝    ╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║██╗
  ╚═══╝  ╚═╝  ╚═╝╚══════╝╚═╝╚═════╝     ╚═╝     ╚══════╝╚═╝  ╚═══╝╚═════╝ ╚═╝╚═╝  ╚═══╝ ╚═════╝      ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝");
                }

                string chosenRoute = chooseRoute();
                int finalPrice = new Random().Next(100, 500);

                string updateQuery = "UPDATE orders SET driver_email=@Driver, status='To Ship', route=@Route, price=@Price WHERE id=@orderId";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@Driver", driverEmail);
                updateCmd.Parameters.AddWithValue("@orderId", orderID);
                updateCmd.Parameters.AddWithValue("@Route", chosenRoute);
                updateCmd.Parameters.AddWithValue("@Price", finalPrice);

                int rowsAffected = updateCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Order {orderID} accepted successfully. Route taken: {chosenRoute}.");
                    Console.WriteLine($"Delivery Fee: P{finalPrice}");
                    setDriverstatus(driverEmail, "Delivering");
                }
                else
                {
                    Console.WriteLine("Failed to accept order.");
                }

                Console.WriteLine("Press any key to go back...");
                Console.ReadKey();
            }
        }



        static string chooseRoute()
        {
            string[] routes =
            {

            "Mabilis na Ruta: EDSA via Skyway – Pinakamabilis pero may toll fees",
            "Walang Trapik: C5 via BGC – Iwas trapiko, mas mabilis",
            "Relax na Biyahe: Coastal Road via Cavite – Maganda ang tanawin pero mas mahaba",
            "Expressway: NLEX-SLEX Connector – Pinakamaganda para sa malalayong biyahe",
            "Ligtas na Ruta: Ortigas hanggang Maynila via Mabini – Mas kaunting aksidente",
            "Panlungsod na Ruta: Makati papuntang QC via Roxas Blvd – Mainam para sa mga delivery sa lungsod"

            };

            Random rand = new Random();
            return routes[rand.Next(routes.Length)];
        }

        public static void viewDeliverhistory(string driverEmail)
        {
            using (var conn = database.GetConnection())
            {
                conn.Open();

                string activeQuery = "SELECT id, product, pickup_location, dropoff_location, route, price, status " +
                                     "FROM orders WHERE driver_email = @Driver AND status IN ('To Ship', 'To Receive') " +
                                     "ORDER BY FIELD(status, 'To Ship', 'To Receive')";

                MySqlCommand activeCmd = new MySqlCommand(activeQuery, conn);
                activeCmd.Parameters.AddWithValue("@Driver", driverEmail);
                MySqlDataReader activeReader = activeCmd.ExecuteReader();

                Console.WriteLine("+-------------------------------------------------------------------------------------------------------------------------------------------------------+");
                Console.WriteLine("|                                                              Active Deliveries                                                                        |");
                Console.WriteLine("+-----+------------+-----------------------+-----------------------+--------------------------------------------------------------+---------+-----------+");
                Console.WriteLine("| ID  |  Product   |  Pickup Location      |  Dropoff Location     |           Route Description                                  |  Price  |  Status   |");
                Console.WriteLine("+-----+------------+-----------------------+-----------------------+--------------------------------------------------------------+---------+-----------+");

                bool hasActiveOrders = false;

                while (activeReader.Read())
                {
                    hasActiveOrders = true;
                    string route = activeReader["route"].ToString();
                    string formattedRoute = route.Length > 60 ? route.Substring(0, 57) + "..." : route;

                    Console.WriteLine($"| {activeReader["id"],-3} | {activeReader["product"],-10} | {activeReader["pickup_location"],-21} | {activeReader["dropoff_location"],-21} | {formattedRoute,-60} | ₱{activeReader["price"],-6} | {activeReader["status"],-9} |");
                }
                activeReader.Close();

                if (!hasActiveOrders)
                {
                    Console.WriteLine("|                                                              No Active Deliveries Found                                                               |");
                }
                Console.WriteLine("+-------------------------------------------------------------------------------------------------------------------------------------------------------+\n");

                string completeQuery = "SELECT id, product, pickup_location, pickup_name, dropoff_location, dropoff_name, price, route, status " +
                                       "FROM orders WHERE driver_email = @Driver AND status IN ('Complete', 'Delivered')";

                MySqlCommand completeCmd = new MySqlCommand(completeQuery, conn);
                completeCmd.Parameters.AddWithValue("@Driver", driverEmail);

                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                MySqlDataReader completedReader = completeCmd.ExecuteReader();

                Console.WriteLine("+----------------------------------------------------------------------------------------------------------------------------------------------------------------------+");
                Console.WriteLine("|                                                                     Completed Deliveries                                                                             |");
                Console.WriteLine("+-----+------------+-----------------------+-------------+-----------------------+---------------+------------------------------------------------+--------+-----------+");
                Console.WriteLine("| ID  | Product    | Pickup Location       | Sender      | Dropoff Location      | Receiver      | Route Description                              |  Fee   |  Status   |");
                Console.WriteLine("+-----+------------+-----------------------+-------------+-----------------------+---------------+------------------------------------------------+--------+-----------+");

                bool hasCompletedOrders = false;
                while (completedReader.Read())
                {
                    hasCompletedOrders = true;
                    string route = completedReader["route"].ToString();
                    string formattedRoute = route.Length > 48 ? route.Substring(0, 45) + "..." : route;

                    Console.WriteLine($"| {completedReader["id"],-3} | {completedReader["product"],-10} | {completedReader["pickup_location"],-21} | {completedReader["pickup_name"],-11} | {completedReader["dropoff_location"],-21} | {completedReader["dropoff_name"],-11} | {formattedRoute,-48} | ₱{completedReader["price"],-5} | {completedReader["status"],-9} |");
                }
                completedReader.Close();

                if (!hasCompletedOrders)
                {
                    Console.WriteLine("|                                                                       No Completed Deliveries Found                                                                |");
                }
                Console.WriteLine("+----------------------------------------------------------------------------------------------------------------------------------------------------------------------+\n");

                if (!checkPendingorder(driverEmail, conn))
                {
                    setDriverstatus(driverEmail, "Available");
                }

                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
            }
        }
        static bool checkPendingorder(string driverEmail, MySqlConnection conn)
        {
            string query = "SELECT COUNT(*) FROM orders WHERE driver_email=@Driver AND status IN ('To Ship', 'To Receive')";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Driver", driverEmail);

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

        static void setDriverstatus(string driverEmail, string status)
        {
            using (var conn = database.GetConnection())
            {
                conn.Open();
                string query = "UPDATE personnel SET status=@Status WHERE email=@Driver";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Driver", driverEmail);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Driver status updated to {status}.");
            }
        }
    }
}
