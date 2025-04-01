using System;
using System.Drawing;
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

                    Console.WriteLine("+----------------------------------------------------------------------------------------------------------------------------------------------------------+");
                    Console.WriteLine("| ID  | User Email           | Product       | Pickup      | Pickup Name     | Dropoff        | Dropoff Name    | Driver                 | Status          |");
                    Console.WriteLine("+----------------------------------------------------------------------------------------------------------------------------------------------------------+");

                    bool hasOrders = false;
                    while (reader.Read())
                    {
                        hasOrders = true;
                        string driver = reader["driver_email"] == DBNull.Value ? "N/A" : reader["driver_email"].ToString();
                        string pickupName = reader["pickup_name"] == DBNull.Value ? "N/A" : reader["pickup_name"].ToString();
                        string dropoffName = reader["dropoff_name"] == DBNull.Value ? "N/A" : reader["dropoff_name"].ToString();

                        Console.WriteLine($"| {reader["id"],-3} | {reader["user_email"],-20} | {reader["product"],-13} | {reader["pickup_location"],-11} | {pickupName,-15} | {reader["dropoff_location"],-14} | {dropoffName,-15} | {driver,-23} | {reader["status"],-14} |");
                    }

                    reader.Close();

                    if (!hasOrders)
                    {
                        Console.WriteLine("|                                                                          No Orders Found                                                             |");
                    }

                    Console.WriteLine("+----------------------------------------------------------------------------------------------------------------------------------------------------------+");
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
                    Console.WriteLine("|  ID  |        Name          |           Email              |   Status   |");
                    Console.WriteLine("+------+----------------------+------------------------------+------------+");

                    bool hasPersonnel = false;
                    while (reader.Read())
                    {
                        hasPersonnel = true;
                        Console.WriteLine($"| {reader["id"],-4} | {reader["name"],-20} | {reader["email"],-28} | {reader["status"],-10} |");
                    }

                    if (!hasPersonnel)
                    {
                        Console.WriteLine("|                        No personnel found.                          |");
                    }

                    Console.WriteLine("+-------------------------------------------------------------------------+");

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

            Console.WriteLine("Create the account personnel!!");

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

                string subject = "🎉 New Personnel Account Created 🎉";

                string body = $@"
                                <html>
                                <head>
                                      <style>
                                          body {{
                                              font-family: Arial, sans-serif;
                                              background-color: #F0E3CA;
                                              padding: 20px;
                                              text-align: center;
                                          }}
                                          .container {{
                                              max-width: 800px;
                                              margin: auto;
                                              padding: 15px;
                                              border-radius: 10px;
                                              background-color: #F0E3CA;
                                              max-height: 400px;
                                          }}
                                          .header {{
                                              display: flex;
                                              align-items: center;
                                              padding: 10px;
                                          }}
                                           .header img {{
                                              width: 40px;
                                              height: 40px;
                                              margin-right: 10px;
                                          }}
                                          .content-box {{
                                              background-color: #F3EBD7;
                                              padding: 20px;
                                              border-radius: 20px;
                                              text-align: left;
                                              max-width: 600px;
                                              margin: auto;
                                          }}
                                          .logo-center {{
                                              display: block;
                                              margin: 0 auto 10px;
                                              width: 40px;
                                              height: 40px;
                                          }}
                                          .footer {{
                                              font-size: 9px;
                                              color: #555;
                                              margin-top: 10px;
                                              text-align: left;
                                              padding-left: 20px;
                                          }}
                                      </style>
                                </head>
                                <body>
                                    <div class='container'>
                                        <div class='header'>
                                            <img src='https://drive.google.com/uc?export=view&id=12_sG-vxVf6cJ4wJQ2zCfdgxu-QxuAUy5' alt='#'>
                                            <p style='font-size: 12px; font-weight: bold; margin: 0;'>New Personnel Account Created</p>
                                        </div>
                
                                        <div class='content-box'>
                                            <img src='https://drive.google.com/uc?export=view&id=12_sG-vxVf6cJ4wJQ2zCfdgxu-QxuAUy5' class='logo-center' alt='#'>
                    
                                            <p><strong>🎉 Welcome, {name}! 🎉</strong></p>
                                            <p>You are now registered as personnel in our system.</p>
                                            <p>📛 <strong>Name:</strong> {name}</p>
                                            <p>📧 <strong>Email:</strong> {email}</p>
                                            <p>🔑 <strong>Password:</strong> {password}</p>
                                            <p>Please keep your credentials secure.</p>
                                        </div>
                
                                        <p class='footer'>Best Regards,<br>Your Logistics Team</p>
                                    </div>
                                </body>
                                </html>";

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

            Console.WriteLine("Choose who you want to update!!");

            Console.Write("Enter Personnel ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID. Operation cancelled.");
                return;
            }

            Console.Write("\nEnter your Email (For Notification): ");
            string notifEmail = Console.ReadLine();

            Console.Write("Enter new Name (or press Enter to keep current name): ");
            string newName = Console.ReadLine();

            Console.Write("Enter new Email (or press Enter to keep current email): ");
            string newEmail = Console.ReadLine();

            Console.Write("Enter new Password (or press Enter to keep current password): ");
            string newPassword = Console.ReadLine();

            using (var conn = database.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT name, email FROM personnel WHERE id = @Id";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = checkCmd.ExecuteReader();

                if (!reader.Read())
                {
                    Console.WriteLine("Personnel not found.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                string oldName = reader["name"].ToString();
                string oldEmail = reader["email"].ToString();
                reader.Close();

                List<string> updates = new List<string>();
                if (!string.IsNullOrEmpty(newName)) updates.Add("name = @NewName");
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
                if (!string.IsNullOrEmpty(newName)) updateCmd.Parameters.AddWithValue("@NewName", newName);
                if (!string.IsNullOrEmpty(newEmail)) updateCmd.Parameters.AddWithValue("@NewEmail", newEmail);
                if (!string.IsNullOrEmpty(newPassword)) updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);

                updateCmd.ExecuteNonQuery();
                Console.WriteLine("Personnel updated successfully.");

                string htmlBody = $@"
                                    <html>
                                    <head>
                                    <style>
                                          body {{
                                              font-family: Arial, sans-serif;
                                              background-color: #E5E0E0;
                                              padding: 20px;
                                              text-align: center;
                                          }}
                                          .container {{
                                              max-width: 800px;
                                              margin: auto;
                                              padding: 15px;
                                              border-radius: 10px;
                                              background-color: #E1D9C7;
                                              max-height: 400px;
                                          }}
                                          .header {{
                                              display: flex;
                                              align-items: center;
                                              padding: 10px;
                                          }}
                                           .header img {{
                                              width: 40px;
                                              height: 40px;
                                              margin-right: 10px;
                                          }}
                                          .content-box {{
                                              background-color: #F3EBD7;
                                              padding: 20px;
                                              border-radius: 20px;
                                              text-align: left;
                                              max-width: 600px;
                                              margin: auto;
                                          }}
                                          .logo-center {{
                                              display: block;
                                              margin: 0 auto 10px;
                                              width: 40px;
                                              height: 40px;
                                          }}
                                          .footer {{
                                              font-size: 9px;
                                              color: #555;
                                              margin-top: 10px;
                                              text-align: left;
                                              padding-left: 20px;
                                          }}
                                     </style>
                                    </head>
                                    <body>
                                        <div class='container'>
                                            <div class='header'>
                                                <img src='https://drive.google.com/uc?export=view&id=12_sG-vxVf6cJ4wJQ2zCfdgxu-QxuAUy5' alt='#'>
                                                <p style='font-size: 12px; font-weight: bold; margin: 0;'>Personnel Account Updated</p>
                                            </div>

                                            <div class='content-box'>
                                            <img src='https://drive.google.com/uc?export=view&id=12_sG-vxVf6cJ4wJQ2zCfdgxu-QxuAUy5' class='logo-center' alt='#'>

                                                <p>Your personnel details have been updated:</p>
                                                <p><strong>📛 Name:</strong> {(string.IsNullOrEmpty(newName) ? oldName : newName)}</p>
                                                <p><strong>📧 Email:</strong> {(string.IsNullOrEmpty(newEmail) ? oldEmail : newEmail)}</p>
                                                {(string.IsNullOrEmpty(newPassword) ? "" : $"<p><strong>🔑 New Password:</strong> {newPassword}</p>")}
                                                <p>Please keep your credentials secure.</p>
                                                <p>Thank you.</p>
                                            </div>

                                            <div class='footer'>
                                                <p><strong>Best Regards,</strong><br>Your Logistics Team</p>
                                            </div>
                                        </div>
                                    </body>
                                    </html>";

                notification.sendNotification(adminEmail, "Personnel Account Updated", htmlBody);
                if (!string.IsNullOrEmpty(notifEmail))
                {
                    notification.sendNotification(notifEmail, "Personnel Account Updated", htmlBody);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }



        public static void deletePersonnel()
        {
            viewAllpersonnel();

            string adminEmail = "bunnygreat0@gmail.com";

            Console.WriteLine("Choose who you want to delete!!");

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

                string subject = "Personnel Account Deleted";

                string htmlBody = $@"
                                    <html>
                                    <head>
                                        <style>
                                            body {{
                                                padding: 20px; 
                                                text-align: center;
                                            }}
                                            .container {{
                                                max-width: 800px;
                                                margin: auto;
                                                padding: 15px;
                                                border-radius: 10px;
                                                background-color: #F0E3CA;
                                                max-height: 400px;
                                            }}
                                            .header {{
                                                display: flex;
                                                align-items: center;
                                                padding: 10px;
                                            }}
                                            .header img {{
                                                width: 40px;
                                                height: 40px;
                                                margin-right: 10px;
                                            }}
                                            .content-box {{
                                                background-color: #F3EBD7;
                                                padding: 20px;
                                                border-radius: 20px;
                                                text-align: left;
                                                max-width: 600px;
                                                margin: auto;
                                            }}
                                            .logo-center {{
                                                display: block;
                                                margin: 0 auto 10px;
                                                width: 40px;
                                                height: 40px;
                                            }}
                                            .footer {{
                                                font-size: 9px;
                                                color: #555;
                                                margin-top: 10px;
                                                text-align: left;
                                                padding-left: 20px;
                                            }}
                                        </style>
                                    </head>
                                        <div class='container'>
                                            <div class='header'>
                                                <img src='https://drive.google.com/uc?export=view&id=12_sG-vxVf6cJ4wJQ2zCfdgxu-QxuAUy5' alt='#'>
                                                <p style='font-size: 12px; font-weight: bold; margin: 0;'>Personnel Account Deleted</p>
                                            </div>

                                            <div class='content-box'>
                                                <img src='https://drive.google.com/uc?export=view&id=12_sG-vxVf6cJ4wJQ2zCfdgxu-QxuAUy5' class='logo-center' alt='#'>
                                                <p>Your personnel account has been removed from the system.</p>
                                                <p><strong>Email:</strong> {email}</p>
                                                <p>If this was a mistake, please contact the administrator.</p>
                                                <p>Thank you.</p>
                                            </div>

                                            <div class='footer'>
                                                <p><strong>Best Regards,</strong><br>Your Logistics Team</p>
                                            </div>
                                        </div>
                                    </body>
                                    </html>";

                notification.sendNotification(adminEmail, subject, htmlBody);
                if (!string.IsNullOrEmpty(notifEmail))
                {
                    notification.sendNotification(notifEmail, subject, htmlBody);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

    }
}
