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
            try
            {
                Console.Write("Enter Item Name: ");
                string item = Console.ReadLine();
                Console.Write("Enter Pickup Location: ");
                string pickup = Console.ReadLine();
                Console.Write("Enter Pickup Person Name: ");
                string pickupName = Console.ReadLine();
                Console.Write("Enter Dropoff Location: ");
                string dropoff = Console.ReadLine();
                Console.Write("Enter Dropoff Person Name: ");
                string dropoffName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(item) || string.IsNullOrWhiteSpace(pickup) ||
                    string.IsNullOrWhiteSpace(pickupName) || string.IsNullOrWhiteSpace(dropoff) ||
                    string.IsNullOrWhiteSpace(dropoffName))
                {
                    Console.WriteLine("Error: All fields are required.");
                    return;
                }

                int orderId = -1;

                using (var conn = database.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO orders (email, product, pickup_location, pickup_name, dropoff_location, dropoff_name, status) " +
                                   "VALUES (@Email, @Product, @Pickup, @PickupName, @Dropoff, @DropoffName, 'To Ship')";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Product", item);
                        cmd.Parameters.AddWithValue("@Pickup", pickup);
                        cmd.Parameters.AddWithValue("@PickupName", pickupName);
                        cmd.Parameters.AddWithValue("@Dropoff", dropoff);
                        cmd.Parameters.AddWithValue("@DropoffName", dropoffName);

                        cmd.ExecuteNonQuery();
                        orderId = (int)cmd.LastInsertedId;
                    }
                }

                if (orderId > 0)
                {
                    Console.WriteLine("Order successfully placed!");
                    trackOrder(email);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void trackOrder(string email)
        {
            try
            {
                using (var conn = database.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT id, status, driver_email, route FROM orders WHERE email = @Email ORDER BY id DESC LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                Console.WriteLine("You haven't placed any orders yet. Please place an order first before tracking it.");
                                Console.WriteLine("Please press any key to go back!!");
                                Console.ReadLine();
                                return;
                            }

                            int orderId = Convert.ToInt32(reader["id"]);
                            string currentStatus = reader["status"]?.ToString();
                            string driverEmail = reader["driver_email"]?.ToString();
                            string route = reader["route"]?.ToString();

                            Console.WriteLine($"\nTracking Order {orderId}.");

                            if (currentStatus == "Delivered")
                            {
                                Console.WriteLine("Your order has been delivered.");
                                return;
                            }

                            if (string.IsNullOrEmpty(driverEmail))
                            {
                                Console.WriteLine("Waiting for a driver to accept your order...");
                                return;
                            }

                            Console.WriteLine($"Driver Assigned: {driverEmail}");
                            Console.WriteLine($"Route: {route}");

                            if (currentStatus == "To Ship")
                            {
                                Console.WriteLine("Order is now in transit...");
                                Thread.Sleep(40000); 
                                updateOrderstatus(orderId, "To Receive");
                            }

                            if (currentStatus == "To Receive")
                            {
                                Console.WriteLine("Order is now ready for receiving...");
                                Thread.Sleep(60000); 
                                updateOrderstatus(orderId, "Delivered");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        static void updateOrderstatus(int orderId, string newStatus)
        {
            try
            {
                using (var conn = database.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE orders SET status=@Status WHERE id=@OrderId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", newStatus);
                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"Order {orderId} updated to {newStatus}");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
