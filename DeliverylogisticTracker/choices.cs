using System;

namespace DeliverylogisticTracker
{
    class choices
    {
        public static void showMainchoices()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("+----------------------------------------+");
                    Console.WriteLine("|    DELIVERY LOGISTICS TRACKER          |");
                    Console.WriteLine("+----------------------------------------+");
                    Console.WriteLine("|  [1] Login                             |");
                    Console.WriteLine("|  [2] Register                          |");
                    Console.WriteLine("|  [3] Exit                              |");
                    Console.WriteLine("+----------------------------------------+");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            authentication.Login();
                            break;
                        case "2":
                            authentication.Register();
                            break;
                        case "3":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static void showAdminchoices()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("+-------------------------------------------+");
                    Console.WriteLine("|               ADMIN MENU                  |");
                    Console.WriteLine("+-------------------------------------------+");
                    Console.WriteLine("|  [1] View All Orders                      |");
                    Console.WriteLine("|  [2] View All Personnel                   |");
                    Console.WriteLine("|  [3] Create Personnel                     |");
                    Console.WriteLine("|  [4] Update Personnel                     |");
                    Console.WriteLine("|  [5] Delete Personnel                     |");
                    Console.WriteLine("|  [6] Logout                               |");
                    Console.WriteLine("+-------------------------------------------+");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            admin.viewAllorders();
                            break;
                        case "2":
                            admin.viewAllpersonnel();
                            break;
                        case "3":
                            admin.createPersonnel();
                            break;
                        case "4":
                            admin.updatePersonnel();
                            break;
                        case "5":
                            admin.deletePersonnel();
                            break;
                        case "6":
                            return;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static void showUserchoices(string email)
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("+----------------------------+");
                    Console.WriteLine("|        USER MENU           |");
                    Console.WriteLine("+----------------------------+");
                    Console.WriteLine("|  [1] Create Order          |");
                    Console.WriteLine("|  [2] Track Order           |");
                    Console.WriteLine("|  [3] Logout                |");
                    Console.WriteLine("+----------------------------+");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            user.createOrder(email);
                            break;
                        case "2":
                            user.trackOrder(email);
                            break;
                        case "3":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static void showDriverchoices(string email)
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("+--------------------------------+");
                    Console.WriteLine("|         DRIVER MENU            |");
                    Console.WriteLine("+--------------------------------+");
                    Console.WriteLine("|  [1] View Pending Orders       |");
                    Console.WriteLine("|  [2] View Delivery History     |");
                    Console.WriteLine("|  [3] Logout                    |");
                    Console.WriteLine("+--------------------------------+");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                        driver.viewPendingorders(email);
                    else if (choice == "2")
                        driver.viewDeliverhistory(email);
                    else if (choice == "3")
                        return;
                    else
                        Console.WriteLine("Invalid choice.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
