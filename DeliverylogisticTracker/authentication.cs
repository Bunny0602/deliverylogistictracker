using System;
using MySql.Data.MySqlClient;

namespace DeliverylogisticTracker
{
    class authentication
    {
        public static void Login()
        {
            Console.Clear();
            Console.WriteLine("+--------------------------------------------------------------------+");
            Console.WriteLine("|                                                                    |");
            Console.WriteLine("|             DELIVERY LOGISTICS TRACKER                             |");
            Console.WriteLine("|                                                                    |");
            Console.WriteLine("|        ██╗      ██████╗  ██████╗ ██╗███╗   ██╗                     |");
            Console.WriteLine("|        ██║     ██╔═══██╗██╔════╝ ██║████╗  ██║                     |");
            Console.WriteLine("|        ██║     ██║   ██║██║  ███╗██║██╔██╗ ██║                     |");
            Console.WriteLine("|        ██║     ██║   ██║██║   ██║██║██║╚██╗██║                     |");
            Console.WriteLine("|        ███████╗╚██████╔╝╚██████╔╝██║██║ ╚████║                     |");
            Console.WriteLine("|        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝                     |");
            Console.WriteLine("|                                                                    |");
            Console.WriteLine("+--------------------------------------------------------------------+");
            Console.Write("| Enter Email : ");
            string email = Console.ReadLine();
            Console.Write("| Enter Password : ");
            string password = Console.ReadLine();
            Console.WriteLine("+--------------------------------------------------------------------+");


            using (var conn = database.GetConnection())
            {
                conn.Open();
                string query = "SELECT role FROM users WHERE email = @Email AND password = @Password";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    string role = result.ToString();
                    Console.WriteLine($"Login successful! Welcome {role}.");
                    Console.WriteLine("Press Any key to enter!");
                    Console.ReadKey();

                    if (role == "Admin") choices.showAdminchoices();
                    else if (role == "User") choices.showUserchoices(email);
                    else if (role == "Driver") choices.showDriverchoices(email);

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(@"
 ██▓ ███▄    █ ██▒   █▓ ▄▄▄       ██▓     ██▓▓█████▄    ▓█████  ███▄ ▄███▓ ▄▄▄       ██▓ ██▓        ▒█████   ██▀███      ██▓███   ▄▄▄        ██████   ██████  █     █░ ▒█████   ██▀███  ▓█████▄      
▓██▒ ██ ▀█   █▓██░   █▒▒████▄    ▓██▒    ▓██▒▒██▀ ██▌   ▓█   ▀ ▓██▒▀█▀ ██▒▒████▄    ▓██▒▓██▒       ▒██▒  ██▒▓██ ▒ ██▒   ▓██░  ██▒▒████▄    ▒██    ▒ ▒██    ▒ ▓█░ █ ░█░▒██▒  ██▒▓██ ▒ ██▒▒██▀ ██▌     
▒██▒▓██  ▀█ ██▒▓██  █▒░▒██  ▀█▄  ▒██░    ▒██▒░██   █▌   ▒███   ▓██    ▓██░▒██  ▀█▄  ▒██▒▒██░       ▒██░  ██▒▓██ ░▄█ ▒   ▓██░ ██▓▒▒██  ▀█▄  ░ ▓██▄   ░ ▓██▄   ▒█░ █ ░█ ▒██░  ██▒▓██ ░▄█ ▒░██   █▌     
░██░▓██▒  ▐▌██▒ ▒██ █░░░██▄▄▄▄██ ▒██░    ░██░░▓█▄   ▌   ▒▓█  ▄ ▒██    ▒██ ░██▄▄▄▄██ ░██░▒██░       ▒██   ██░▒██▀▀█▄     ▒██▄█▓▒ ▒░██▄▄▄▄██   ▒   ██▒  ▒   ██▒░█░ █ ░█ ▒██   ██░▒██▀▀█▄  ░▓█▄   ▌     
░██░▒██░   ▓██░  ▒▀█░   ▓█   ▓██▒░██████▒░██░░▒████▓    ░▒████▒▒██▒   ░██▒ ▓█   ▓██▒░██░░██████▒   ░ ████▓▒░░██▓ ▒██▒   ▒██▒ ░  ░ ▓█   ▓██▒▒██████▒▒▒██████▒▒░░██▒██▓ ░ ████▓▒░░██▓ ▒██▒░▒████▓  ██▓ 
░▓  ░ ▒░   ▒ ▒   ░ ▐░   ▒▒   ▓▒█░░ ▒░▓  ░░▓   ▒▒▓  ▒    ░░ ▒░ ░░ ▒░   ░  ░ ▒▒   ▓▒█░░▓  ░ ▒░▓  ░   ░ ▒░▒░▒░ ░ ▒▓ ░▒▓░   ▒▓▒░ ░  ░ ▒▒   ▓▒█░▒ ▒▓▒ ▒ ░▒ ▒▓▒ ▒ ░░ ▓░▒ ▒  ░ ▒░▒░▒░ ░ ▒▓ ░▒▓░ ▒▒▓  ▒  ▒▓▒ 
 ▒ ░░ ░░   ░ ▒░  ░ ░░    ▒   ▒▒ ░░ ░ ▒  ░ ▒ ░ ░ ▒  ▒     ░ ░  ░░  ░      ░  ▒   ▒▒ ░ ▒ ░░ ░ ▒  ░     ░ ▒ ▒░   ░▒ ░ ▒░   ░▒ ░       ▒   ▒▒ ░░ ░▒  ░ ░░ ░▒  ░ ░  ▒ ░ ░    ░ ▒ ▒░   ░▒ ░ ▒░ ░ ▒  ▒  ░▒  
 ▒ ░   ░   ░ ░     ░░    ░   ▒     ░ ░    ▒ ░ ░ ░  ░       ░   ░      ░     ░   ▒    ▒ ░  ░ ░      ░ ░ ░ ▒    ░░   ░    ░░         ░   ▒   ░  ░  ░  ░  ░  ░    ░   ░  ░ ░ ░ ▒    ░░   ░  ░ ░  ░  ░   
 ░           ░      ░        ░  ░    ░  ░ ░     ░          ░  ░       ░         ░  ░ ░      ░  ░       ░ ░     ░                       ░  ░      ░        ░      ░        ░ ░     ░        ░      ░  
                   ░                          ░                                                                                                                                          ░        ░  ");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        public static void Register()
        {
            Console.Clear();
            Console.WriteLine("+------------------------------------------------------------------------+");
            Console.WriteLine("|                                                                        |");
            Console.WriteLine("|                   DELIVERY LOGISTICS TRACKER                           |");
            Console.WriteLine("|                                                                        |");
            Console.WriteLine("|        ██████╗ ███████╗ ██████╗ ██╗███████╗████████╗███████╗██████╗    |");
            Console.WriteLine("|        ██╔══██╗██╔════╝██╔════╝ ██║██╔════╝╚══██╔══╝██╔════╝██╔══██╗   |");
            Console.WriteLine("|        ██████╔╝█████╗  ██║  ███╗██║███████╗   ██║   █████╗  ██████╔╝   |");
            Console.WriteLine("|        ██╔══██╗██╔══╝  ██║   ██║██║╚════██║   ██║   ██╔══╝  ██╔══██╗   |");
            Console.WriteLine("|        ██║  ██║███████╗╚██████╔╝██║███████║   ██║   ███████╗██║  ██║   |");
            Console.WriteLine("|        ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝   |");
            Console.WriteLine("|                                                                        |");
            Console.WriteLine("+------------------------------------------------------------------------+");
            Console.Write("| Enter Email : ");
            string email = Console.ReadLine();
            Console.Write("| Enter Password : ");
            string password = Console.ReadLine();
            Console.WriteLine("+------------------------------------------------------------------------+");


            using (var conn = database.GetConnection())
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM users WHERE email = @Email";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    Console.WriteLine("Email already exists.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                string insertQuery = "INSERT INTO users (email, password, role) VALUES (@Email, @Password, 'User')";

                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                cmd.ExecuteNonQuery();
                Console.Clear();
                Console.WriteLine(@"
 ██▀███  ▓█████   ▄████  ██▓  ██████ ▄▄▄█████▓ ██▀███   ▄▄▄     ▄▄▄█████▓ ██▓ ▒█████   ███▄    █      ██████  █    ██  ▄████▄   ▄████▄  ▓█████   ██████   ██████   █████▒█    ██  ██▓     ▐██▌ 
▓██ ▒ ██▒▓█   ▀  ██▒ ▀█▒▓██▒▒██    ▒ ▓  ██▒ ▓▒▓██ ▒ ██▒▒████▄   ▓  ██▒ ▓▒▓██▒▒██▒  ██▒ ██ ▀█   █    ▒██    ▒  ██  ▓██▒▒██▀ ▀█  ▒██▀ ▀█  ▓█   ▀ ▒██    ▒ ▒██    ▒ ▓██   ▒ ██  ▓██▒▓██▒     ▐██▌ 
▓██ ░▄█ ▒▒███   ▒██░▄▄▄░▒██▒░ ▓██▄   ▒ ▓██░ ▒░▓██ ░▄█ ▒▒██  ▀█▄ ▒ ▓██░ ▒░▒██▒▒██░  ██▒▓██  ▀█ ██▒   ░ ▓██▄   ▓██  ▒██░▒▓█    ▄ ▒▓█    ▄ ▒███   ░ ▓██▄   ░ ▓██▄   ▒████ ░▓██  ▒██░▒██░     ▐██▌ 
▒██▀▀█▄  ▒▓█  ▄ ░▓█  ██▓░██░  ▒   ██▒░ ▓██▓ ░ ▒██▀▀█▄  ░██▄▄▄▄██░ ▓██▓ ░ ░██░▒██   ██░▓██▒  ▐▌██▒     ▒   ██▒▓▓█  ░██░▒▓▓▄ ▄██▒▒▓▓▄ ▄██▒▒▓█  ▄   ▒   ██▒  ▒   ██▒░▓█▒  ░▓▓█  ░██░▒██░     ▓██▒ 
░██▓ ▒██▒░▒████▒░▒▓███▀▒░██░▒██████▒▒  ▒██▒ ░ ░██▓ ▒██▒ ▓█   ▓██▒ ▒██▒ ░ ░██░░ ████▓▒░▒██░   ▓██░   ▒██████▒▒▒▒█████▓ ▒ ▓███▀ ░▒ ▓███▀ ░░▒████▒▒██████▒▒▒██████▒▒░▒█░   ▒▒█████▓ ░██████▒ ▒▄▄  
░ ▒▓ ░▒▓░░░ ▒░ ░ ░▒   ▒ ░▓  ▒ ▒▓▒ ▒ ░  ▒ ░░   ░ ▒▓ ░▒▓░ ▒▒   ▓▒█░ ▒ ░░   ░▓  ░ ▒░▒░▒░ ░ ▒░   ▒ ▒    ▒ ▒▓▒ ▒ ░░▒▓▒ ▒ ▒ ░ ░▒ ▒  ░░ ░▒ ▒  ░░░ ▒░ ░▒ ▒▓▒ ▒ ░▒ ▒▓▒ ▒ ░ ▒ ░   ░▒▓▒ ▒ ▒ ░ ▒░▓  ░ ░▀▀▒ 
  ░▒ ░ ▒░ ░ ░  ░  ░   ░  ▒ ░░ ░▒  ░ ░    ░      ░▒ ░ ▒░  ▒   ▒▒ ░   ░     ▒ ░  ░ ▒ ▒░ ░ ░░   ░ ▒░   ░ ░▒  ░ ░░░▒░ ░ ░   ░  ▒     ░  ▒    ░ ░  ░░ ░▒  ░ ░░ ░▒  ░ ░ ░     ░░▒░ ░ ░ ░ ░ ▒  ░ ░  ░ 
  ░░   ░    ░   ░ ░   ░  ▒ ░░  ░  ░    ░        ░░   ░   ░   ▒    ░       ▒ ░░ ░ ░ ▒     ░   ░ ░    ░  ░  ░   ░░░ ░ ░ ░        ░           ░   ░  ░  ░  ░  ░  ░   ░ ░    ░░░ ░ ░   ░ ░       ░ 
   ░        ░  ░      ░  ░        ░              ░           ░  ░         ░      ░ ░           ░          ░     ░     ░ ░      ░ ░         ░  ░      ░        ░            ░         ░  ░ ░    ");
            }
        }
    }
}
