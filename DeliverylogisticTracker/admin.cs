using System;
using MySql.Data.MySqlClient;

namespace DeliverylogisticTracker
{
    class admin
    {
        public static void viewAllorders()
        {
            try
            {
                using (var conn = database.GetConnection())
                {
                    conn.Open();

                    string updateQuery = "UPDATE orders SET status = 'Complete' WHERE status = 'Delivered'";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.ExecuteNonQuery();

                    string query = "SELECT id, email AS user_email, product, pickup_location, pickup_name, " +
                                   "dropoff_location, dropoff_name, driver_email, route, price, status " +
                                   "FROM orders ORDER BY FIELD(status, 'To Ship', 'To Receive', 'Delivered', 'Complete')";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("+-------------------------------------------------------------------------------------------------------------------------------------------------------+");
                    Console.WriteLine("| ID  | User Email           | Product       | Pickup      | Pickup Name     | Dropoff        | Dropoff Name    |      Driver         |   Status        |");
                    Console.WriteLine("+-------------------------------------------------------------------------------------------------------------------------------------------------------+");

                    bool hasOrders = false;
                    while (reader.Read())
                    {
                        hasOrders = true;
                        string driver = reader["driver_email"] == DBNull.Value ? "N/A" : reader["driver_email"].ToString();
                        string pickupName = reader["pickup_name"] == DBNull.Value ? "N/A" : reader["pickup_name"].ToString();
                        string dropoffName = reader["dropoff_name"] == DBNull.Value ? "N/A" : reader["dropoff_name"].ToString();

                        Console.WriteLine($"| {reader["id"],-3} | {reader["user_email"],-20} | {reader["product"],-13} | {reader["pickup_location"],-11} | {pickupName,-15} | {reader["dropoff_location"],-14} | {dropoffName,-15} | {driver,-19} | {reader["status"],-15} |");
                    }

                    reader.Close();

                    if (!hasOrders)
                    {
                        Console.WriteLine("|                                                                          No Orders Found                                                          |");
                    }

                    Console.WriteLine("+-------------------------------------------------------------------------------------------------------------------------------------------------------+");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving orders: " + ex.Message);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }



        public static void viewAllpersonnel()
        {
            try
            {
                using (var conn = database.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id, name, email, status FROM personnel";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("+------+----------------------+------------------------------+------------+");
                    Console.WriteLine("|  ID  |        Name         |           Email              |   Status   |");
                    Console.WriteLine("+------+----------------------+------------------------------+------------+");

                    bool hasPersonnel = false;
                    while (reader.Read())
                    {
                        hasPersonnel = true;
                        Console.WriteLine($"| {reader["id"],-4} | {reader["name"],-20} | {reader["email"],-28} | {reader["status"],-10} |");
                    }

                    if (!hasPersonnel)
                    {
                        Console.WriteLine("|                        No personnel found.                        |");
                    }

                    Console.WriteLine("+--------------------------------------------------------------------+");

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving personnel: " + ex.Message);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void createPersonnel()
        {
            string adminEmail = "bunnygreat0@gmail.com";

            Console.Clear();
            Console.Write("Enter your Email (For Notification): ");
            string notifEmail = Console.ReadLine();

            Console.Write("Enter Personnel Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Personnel Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Personnel Password: ");
            string password = Console.ReadLine();

            using (var conn = database.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM personnel WHERE email = @Email";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    Console.WriteLine("Email already exists.");
                    return;
                }

                string insertQuery = "INSERT INTO personnel (name, email, password, status) VALUES (@Name, @Email, @Password, 'Available')";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Name", name);
                insertCmd.Parameters.AddWithValue("@Email", email);
                insertCmd.Parameters.AddWithValue("@Password", password);
                insertCmd.ExecuteNonQuery();

                string insertUserQuery = "INSERT INTO users (email, password, role) VALUES (@Email, @Password, 'Driver')";
                MySqlCommand insertUserCmd = new MySqlCommand(insertUserQuery, conn);
                insertUserCmd.Parameters.AddWithValue("@Email", email);
                insertUserCmd.Parameters.AddWithValue("@Password", password);
                insertUserCmd.ExecuteNonQuery();

                Console.WriteLine("Personnel created successfully!");

                string subject = "New Personnel Account Created ";
                string body = "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n"
                            + " ██████╗ ██████╗ ███╗   ██╗ ██████╗ ██████╗  █████╗ ████████╗███████╗ \n"
                            + "██╔════╝██╔═══██╗████╗  ██║██╔════╝ ██╔══██╗██╔══██╗╚══██╔══╝██╔════╝ \n"
                            + "██║     ██║   ██║██╔██╗ ██║██║  ███╗██████╔╝███████║   ██║   ███████╗ \n"
                            + "██║     ██║   ██║██║╚██╗██║██║   ██║██╔══██╗██╔══██║   ██║   ╚════██║ \n"
                            + "╚██████╗╚██████╔╝██║ ╚████║╚██████╔╝██║  ██║██║  ██║   ██║   ███████║ \n"
                            + " ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚══════╝ \n"
                            + "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n"
                            + $"🎉 **Congratulations, {name}!** 🎉\n\n"
                            + "You have been successfully added as personnel in our system.\n\n"
                            + "🔹 Name: " + name + "\n"
                            + "🔹 Email: " + email + "\n"
                            + "🔹 Password: " + password + "\n\n"
                            + "Please keep your credentials secure and do not share them with anyone.\n\n"
                            + "Welcome to the team!\n\n"
                            + "**Best Regards,**\n"
                            + "**Your Logistics Team**";


                notification.sendNotification(adminEmail, subject, body);

                if (!string.IsNullOrEmpty(notifEmail))
                {
                    notification.sendNotification(notifEmail, subject, body);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }




        public static void updatePersonnel()
        {
            viewAllpersonnel();

            string adminEmail = "bunnygreat0@gmail.com";

            Console.Write("\nEnter your Email (Legit Email for notification): ");
            string notifEmail = Console.ReadLine();

            Console.Write("Enter Personnel ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            Console.Write("Enter new Email (or press Enter to keep current email): ");
            string newEmail = Console.ReadLine();

            Console.Write("Enter new Password (or press Enter to keep current password): ");
            string newPassword = Console.ReadLine();

            using (var conn = database.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT email FROM personnel WHERE id = @Id";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Id", id);
                object result = checkCmd.ExecuteScalar();

                if (result == null)
                {
                    Console.WriteLine("Personnel not found.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                string oldEmail = result.ToString();
                string userEmail = string.IsNullOrEmpty(newEmail) ? oldEmail : newEmail;

                List<string> updates = new List<string>();
                if (!string.IsNullOrEmpty(newEmail)) updates.Add("email = @NewEmail");
                if (!string.IsNullOrEmpty(newPassword)) updates.Add("password = @NewPassword");

                if (updates.Count == 0)
                {
                    Console.WriteLine("No updates provided.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                string updateQuery = $"UPDATE personnel SET {string.Join(", ", updates)} WHERE id = @Id";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@Id", id);
                if (!string.IsNullOrEmpty(newEmail)) updateCmd.Parameters.AddWithValue("@NewEmail", newEmail);
                if (!string.IsNullOrEmpty(newPassword)) updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);

                updateCmd.ExecuteNonQuery();
                Console.WriteLine("Personnel updated successfully.");

                string subject = "Personnel Account Updated ";
                string body = $"Your personnel details have been updated:\n\n"
                            + "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n"
                            + " █    ██  ██▓███  ▓█████▄  ▄▄▄     ▄▄▄█████▓▓█████ ▓█████▄  \r\n"
                            + " ██  ▓██▒▓██░  ██▒▒██▀ ██▌▒████▄   ▓  ██▒ ▓▒▓█   ▀ ▒██▀ ██▌\r\n"
                            + "▓██  ▒██░▓██░ ██▓▒░██   █▌▒██  ▀█▄ ▒ ▓██░ ▒░▒███   ░██   █▌\r\n"
                            + "▓▓█  ░██░▒██▄█▓▒ ▒░▓█▄   ▌░██▄▄▄▄██░ ▓██▓ ░ ▒▓█  ▄ ░▓█▄   ▌\r\n"
                            + "▒▒█████▓ ▒██▒ ░  ░░▒████▓  ▓█   ▓██▒ ▒██▒ ░ ░▒████▒░▒████▓  \r\n"
                            + "░▒▓▒ ▒ ▒ ▒▓▒░ ░  ░ ▒▒▓  ▒  ▒▒   ▓▒█░ ▒ ░░   ░░ ▒░ ░ ▒▒▓  ▒  \r\n"
                            + "░░▒░ ░ ░ ░▒ ░      ░ ▒  ▒   ▒   ▒▒ ░   ░     ░ ░  ░ ░ ▒  ▒  \r\n"
                            + " ░░░ ░ ░ ░░        ░ ░  ░   ░   ▒    ░         ░    ░ ░  ░ \r\n"
                            + "   ░                 ░          ░  ░           ░  ░   ░       \r\n"
                            + "                   ░                                ░      \r\n"
                            + "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n"
                            + $"Email: {userEmail}\n"
                            + (!string.IsNullOrEmpty(newPassword) ? $"New Password: {newPassword}\n" : "")
                            + "Please keep your credentials secure.\n\nThank you.";


                notification.sendNotification(adminEmail, subject, body);
                if (!string.IsNullOrEmpty(notifEmail))
                {
                    notification.sendNotification(notifEmail, subject, body);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        public static void deletePersonell()
        {
            viewAllpersonnel();

            string adminEmail = "bunnygreat0@gmail.com";

            Console.Write("\nEnter your Email (Legit Email for notification): ");
            string notifEmail = Console.ReadLine();

            Console.Write("Enter Personnel ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            using (var conn = database.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT email FROM personnel WHERE id = @Id";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Id", id);
                object result = checkCmd.ExecuteScalar();

                if (result == null)
                {
                    Console.WriteLine("Personnel not found.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                string email = result.ToString();

                
                Console.Write($"Are you sure you want to delete {email}? (yes/no): ");
                string confirm = Console.ReadLine().ToLower();
                if (confirm != "yes")
                {
                    Console.WriteLine("Deletion cancelled.");
                    return;
                }

                
                string deleteQuery = "DELETE FROM personnel WHERE id = @Id";
                MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn);
                deleteCmd.Parameters.AddWithValue("@Id", id);
                deleteCmd.ExecuteNonQuery();

                Console.WriteLine("Personnel deleted successfully.");

                string subject = "Personnel Account Deleted ";
                string body = "Your personnel account has been removed from the system.\n\n"
                            + "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n"
                            + "▓█████▄ ▓█████  ██▓    ▓█████▄▄▄█████▓▓█████ ▓█████▄  \r\n"
                            + "▒██▀ ██▌▓█   ▀ ▓██▒    ▓█   ▀▓  ██▒ ▓▒▓█   ▀ ▒██▀ ██▌ \r\n"
                            + "░██   █▌▒███   ▒██░    ▒███  ▒ ▓██░ ▒░▒███   ░██   █▌ \r\n"
                            + "░▓█▄   ▌▒▓█  ▄ ▒██░    ▒▓█  ▄░ ▓██▓ ░ ▒▓█  ▄ ░▓█▄   ▌ \r\n"
                            + "░▒████▓ ░▒████▒░██████▒░▒████▒ ▒██▒ ░ ░▒████▒░▒████▓  \r\n"
                            + " ▒▒▓  ▒ ░░ ▒░ ░░ ▒░▓  ░░░ ▒░ ░ ▒ ░░   ░░ ▒░ ░ ▒▒▓  ▒   \r\n"
                            + " ░ ▒  ▒  ░ ░  ░░ ░ ▒  ░ ░ ░  ░   ░     ░ ░  ░ ░ ▒  ▒   \r\n"
                            + " ░ ░  ░    ░     ░ ░      ░    ░         ░    ░ ░  ░  \r\n"
                            + "   ░       ░  ░    ░  ░   ░  ░           ░  ░   ░    \r\n"
                            + " ░                                            ░      \r\n"
                            + "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n"
                                            + $"Email: {email}\n\n"
                            + "If this was a mistake, please contact the administrator.\n\n"
                            + "Thank you.";

                notification.sendNotification(adminEmail, subject, body);
                if (!string.IsNullOrEmpty(notifEmail))
                {
                    notification.sendNotification(notifEmail, subject, body);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

    }
}
