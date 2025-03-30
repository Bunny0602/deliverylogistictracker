using System;
using System.Threading;
using MySql.Data.MySqlClient;

namespace DeliverylogisticTracker
{
    class user
    {
        public static void createOrder(string email)
        {
            Console.Clear();
            Console.Write("Enter Item Name: ");
            string item = Console.ReadLine();
            Console.Write("Enter Puckup Location: ");
            string pickup = Console.ReadLine();
            Console.Write("Enter Pickup Person Name: ");
            string pickupName = Console.ReadLine();
            Console.Write("Enter Dropoff Location: ");
            string dropoff = Console.ReadLine();
            Console.Write("Enter Dropoff Person Name: ");
            string dropoffName = Console.ReadLine();

            int orderId = -1;

            using (var conn = database.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO orders (email, product, pickup_location, pickup_name, dropoff_location, dropoff_name, status) VALUES (@Email, @Product, @Pickup, @PickupName, @Dropoff, @DropoffName, 'To Ship')";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Product", item);
                cmd.Parameters.AddWithValue("@Pickup", pickup);
                cmd.Parameters.AddWithValue("@PickupName", pickupName);
                cmd.Parameters.AddWithValue("@Dropoff", dropoff);
                cmd.Parameters.AddWithValue("@DropoffName", dropoffName);

                cmd.ExecuteNonQuery();
                orderId = (int)cmd.LastInsertedId;

                Console.WriteLine(@"
 ██████╗ ██████╗ ██████╗ ███████╗██████╗      ██████╗██████╗ ███████╗ █████╗ ████████╗███████╗██████╗     ███████╗██╗   ██╗ ██████╗ ██████╗███████╗███████╗███████╗███████╗██╗   ██╗██╗     ██╗  ██╗   ██╗
██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗    ██╔════╝██╔══██╗██╔════╝██╔══██╗╚══██╔══╝██╔════╝██╔══██╗    ██╔════╝██║   ██║██╔════╝██╔════╝██╔════╝██╔════╝██╔════╝██╔════╝██║   ██║██║     ██║  ╚██╗ ██╔╝
██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝    ██║     ██████╔╝█████╗  ███████║   ██║   █████╗  ██║  ██║    ███████╗██║   ██║██║     ██║     █████╗  ███████╗███████╗█████╗  ██║   ██║██║     ██║   ╚████╔╝ 
██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗    ██║     ██╔══██╗██╔══╝  ██╔══██║   ██║   ██╔══╝  ██║  ██║    ╚════██║██║   ██║██║     ██║     ██╔══╝  ╚════██║╚════██║██╔══╝  ██║   ██║██║     ██║    ╚██╔╝  
╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║    ╚██████╗██║  ██║███████╗██║  ██║   ██║   ███████╗██████╔╝    ███████║╚██████╔╝╚██████╗╚██████╗███████╗███████║███████║██║     ╚██████╔╝███████╗███████╗██║██╗
 ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝     ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═════╝     ╚══════╝ ╚═════╝  ╚═════╝ ╚═════╝╚══════╝╚══════╝╚══════╝╚═╝      ╚═════╝ ╚══════╝╚══════╝╚═╝╚═╝");
            }

            if (orderId > 0)
            {
                trackOrder(email);
            }
        }

        public static void trackOrder(string email)
        {
            using (var conn = database.GetConnection()) 
            {
                conn.Open();
                string query = "SELECT id, status, driver_email, route FROM orders WHERE email = @Email ORDER BY id DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read()) 
                {
                    Console.WriteLine(@"
███╗   ██╗ ██████╗      █████╗  ██████╗████████╗██╗██╗   ██╗███████╗     ██████╗ ██████╗ ██████╗ ███████╗██████╗ ███████╗    ███████╗ ██████╗ ██╗   ██╗███╗   ██╗██████╗                                        
████╗  ██║██╔═══██╗    ██╔══██╗██╔════╝╚══██╔══╝██║██║   ██║██╔════╝    ██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗██╔════╝    ██╔════╝██╔═══██╗██║   ██║████╗  ██║██╔══██╗                                       
██╔██╗ ██║██║   ██║    ███████║██║        ██║   ██║██║   ██║█████╗      ██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝███████╗    █████╗  ██║   ██║██║   ██║██╔██╗ ██║██║  ██║                                       
██║╚██╗██║██║   ██║    ██╔══██║██║        ██║   ██║╚██╗ ██╔╝██╔══╝      ██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗╚════██║    ██╔══╝  ██║   ██║██║   ██║██║╚██╗██║██║  ██║                                       
██║ ╚████║╚██████╔╝    ██║  ██║╚██████╗   ██║   ██║ ╚████╔╝ ███████╗    ╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║███████║    ██║     ╚██████╔╝╚██████╔╝██║ ╚████║██████╔╝██╗                                    
╚═╝  ╚═══╝ ╚═════╝     ╚═╝  ╚═╝ ╚═════╝   ╚═╝   ╚═╝  ╚═══╝  ╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝    ╚═╝      ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝╚═════╝ ╚═╝                                    
                                                                                                                                                                                                                
██████╗ ██╗     ███████╗ █████╗ ███████╗███████╗     ██████╗██████╗ ███████╗ █████╗ ████████╗███████╗     █████╗ ███╗   ██╗     ██████╗ ██████╗ ██████╗ ███████╗██████╗     ███████╗██╗██████╗ ███████╗████████╗
██╔══██╗██║     ██╔════╝██╔══██╗██╔════╝██╔════╝    ██╔════╝██╔══██╗██╔════╝██╔══██╗╚══██╔══╝██╔════╝    ██╔══██╗████╗  ██║    ██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗    ██╔════╝██║██╔══██╗██╔════╝╚══██╔══╝
██████╔╝██║     █████╗  ███████║███████╗█████╗      ██║     ██████╔╝█████╗  ███████║   ██║   █████╗      ███████║██╔██╗ ██║    ██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝    █████╗  ██║██████╔╝███████╗   ██║   
██╔═══╝ ██║     ██╔══╝  ██╔══██║╚════██║██╔══╝      ██║     ██╔══██╗██╔══╝  ██╔══██║   ██║   ██╔══╝      ██╔══██║██║╚██╗██║    ██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗    ██╔══╝  ██║██╔══██╗╚════██║   ██║   
██║     ███████╗███████╗██║  ██║███████║███████╗    ╚██████╗██║  ██║███████╗██║  ██║   ██║   ███████╗    ██║  ██║██║ ╚████║    ╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║    ██║     ██║██║  ██║███████║   ██║██╗
╚═╝     ╚══════╝╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝     ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝    ╚═╝  ╚═╝╚═╝  ╚═══╝     ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝    ╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝   ╚═╝╚═╝");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey();
                    return;
                }

                int orderId = Convert.ToInt32(reader["id"]);
                string currentStatus = reader["status"]?.ToString();
                string driverEmail = reader["driver_email"]?.ToString();
                string route = reader["route"]?.ToString();
                reader.Close();

                Console.WriteLine($"\nTracking Order {orderId}.");

                if (currentStatus == "Delivered")
                {
                    Console.WriteLine(@"
 ██████╗ ██████╗ ██████╗ ███████╗██████╗      █████╗ ██╗     ██████╗ ███████╗ █████╗ ██████╗ ██╗   ██╗    ██████╗ ███████╗██╗     ██╗██╗   ██╗███████╗██████╗ ███████╗██████╗ ██╗    
██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗    ██╔══██╗██║     ██╔══██╗██╔════╝██╔══██╗██╔══██╗╚██╗ ██╔╝    ██╔══██╗██╔════╝██║     ██║██║   ██║██╔════╝██╔══██╗██╔════╝██╔══██╗██║    
██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝    ███████║██║     ██████╔╝█████╗  ███████║██║  ██║ ╚████╔╝     ██║  ██║█████╗  ██║     ██║██║   ██║█████╗  ██████╔╝█████╗  ██║  ██║██║    
██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗    ██╔══██║██║     ██╔══██╗██╔══╝  ██╔══██║██║  ██║  ╚██╔╝      ██║  ██║██╔══╝  ██║     ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗██╔══╝  ██║  ██║╚═╝    
╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║    ██║  ██║███████╗██║  ██║███████╗██║  ██║██████╔╝   ██║       ██████╔╝███████╗███████╗██║ ╚████╔╝ ███████╗██║  ██║███████╗██████╔╝██╗    
 ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝    ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═════╝    ╚═╝       ╚═════╝ ╚══════╝╚══════╝╚═╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝╚══════╝╚═════╝ ╚═╝    
                                                                                                                                                                                     
███╗   ██╗ ██████╗     ████████╗██████╗  █████╗  ██████╗██╗  ██╗██╗███╗   ██╗ ██████╗     ███╗   ██╗███████╗███████╗██████╗ ███████╗██████╗                                          
████╗  ██║██╔═══██╗    ╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██║ ██╔╝██║████╗  ██║██╔════╝     ████╗  ██║██╔════╝██╔════╝██╔══██╗██╔════╝██╔══██╗                                         
██╔██╗ ██║██║   ██║       ██║   ██████╔╝███████║██║     █████╔╝ ██║██╔██╗ ██║██║  ███╗    ██╔██╗ ██║█████╗  █████╗  ██║  ██║█████╗  ██║  ██║                                         
██║╚██╗██║██║   ██║       ██║   ██╔══██╗██╔══██║██║     ██╔═██╗ ██║██║╚██╗██║██║   ██║    ██║╚██╗██║██╔══╝  ██╔══╝  ██║  ██║██╔══╝  ██║  ██║                                         
██║ ╚████║╚██████╔╝       ██║   ██║  ██║██║  ██║╚██████╗██║  ██╗██║██║ ╚████║╚██████╔╝    ██║ ╚████║███████╗███████╗██████╔╝███████╗██████╔╝██╗                                      
╚═╝  ╚═══╝ ╚═════╝        ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝ ╚═════╝     ╚═╝  ╚═══╝╚══════╝╚══════╝╚═════╝ ╚══════╝╚═════╝ ╚═╝");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                if (string.IsNullOrEmpty(driverEmail))
                {
                    Console.WriteLine("Waiting for a driver to accept your order...");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"Driver Assigned: {driverEmail}");
                Console.WriteLine($"Route: {route}");

                if (currentStatus == "To Ship")
                {
                    Console.WriteLine("Order is now in transit...");
                    Thread.Sleep(40000);
                    updateOrderstatus(orderId, "To Receive");

                    currentStatus = "To Receive";
                    Console.WriteLine($"Order {orderId} updated: {currentStatus}");
                }

                if (currentStatus == "To Receive")
                {
                    Console.WriteLine("Order is now ready for receiving...");
                    Thread.Sleep(60000);
                    updateOrderstatus(orderId, "Delivered");

                    currentStatus = "Delivered";
                    Console.WriteLine($"Order {orderId} updated: {currentStatus}");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }


        static void updateOrderstatus(int orderId, string newStatus)
        {
            using (var conn = database.GetConnection())
            {
                conn.Open();
                string query = "UPDATE orders SET status=@Status WHERE id=@OrderId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

