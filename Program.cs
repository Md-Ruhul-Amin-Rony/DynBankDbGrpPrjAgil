using Npgsql;
using System.Media;
using ManagedBass;

namespace DBTest;

class Program
{
    static void Main(string[] args)
    {
       // PostgresDataAccess post = new PostgresDataAccess();   
        List<BankUserModel> users1 = PostgresDataAccess.OldLoadBankUsers();

        var arr = new[] {
                       @"__          __  _                            _          _ _               _                 _     ",
                       @"\ \        / / | |                          | |        | (_)             | |               | |    ",
                       @" \ \  /\  / /__| | ___ ___  _ __ ___   ___  | |_ ___   | |_  ___  _ __   | |__   __ _ _ __ | | __,",
                       @"  \ \/  \/ / _ \ |/ __/ _ \| '_ ` _ \ / _ \ | __/ _ \  | | |/ _ \| '_ \  | '_ \ / _` | '_ \| |/ / ",
                       @"   \  /\  /  __/ | (_| (_) | | | | | |  __/ | || (_) | | | | (_) | | | | | |_) | (_| | | | |   <  ",
                       @"    \/  \/ \___|_|\___\___/|_| |_| |_|\___|  \__\___/  |_|_|\___/|_| |_| |_.__/ \__,_|_| |_|_|\_\ ",
                       @"",
                       @"",
                       @"                                             ,%%%%%%%,",
                       @"                                           ,%%/\%%%%/\%,",
                       @"                                          ,%%%\c "" J/%%,",
                       @"                     %.                   %%%%/ d  b \%%%",
                       @"                     `%%.         __      %%%%    _  |%%%",
                       @"                      `%%      .-'  `'~--'`%%%%(=_Y_=)%%'",
                       @"                       //    .'     `.     `%%%%`\7/%%%'',____",
                       @"                      ((    /         ;      `%%%%%%% '____)))",
                       @"                      `.`--'         ,'   _,`-._____`-,",
                       @"                        `""'`._____  `--,`          `)))",
                       @"                                   `~'-)))"
            };



        Console.ForegroundColor = ConsoleColor.DarkBlue;

        var maxLength = arr.Aggregate(0, (max, line) => Math.Max(max, line.Length));
        for (int x = 0; x < Console.BufferWidth / 2 - maxLength / 2; x++)
        {
            ConsoleDraw(arr, x, 1);
            Thread.Sleep(100);
        }
        Console.ReadKey();
        Console.ResetColor();


        foreach (BankUserModel item in users1)
        {
            Console.WriteLine($"Id is :{item.id}, name is : {item.first_name}, pincode is :{item.pin_code}");
            Console.WriteLine($" name is : {item.first_name}, pincode is :{item.pin_code}");

        }
        List<BankUserModel> users = PostgresDataAccess.LoadBankUsers();
        Console.WriteLine($"users length: {users.Count}");
        foreach (BankUserModel user in users)
        {
            Console.WriteLine($"Hello {user.first_name} your pincode is {user.pin_code}");
        }
        int tries = 3;
        while (true)
        {
            Console.Write("Please enter FirstName: ");
            string firstName = Console.ReadLine();

            Console.Write("Please enter PinCode: ");
            string pinCode = Console.ReadLine();

            // Possible to login to multuple user. System shouldn't multiple user to login. It should return one unique user, use FirstOrDefault.
            // Prevent duplicate user to register.
            //First check if the new user exists or not.
            List<BankUserModel> checkedUsers = PostgresDataAccess.CheckLogin(firstName, pinCode);
            if (checkedUsers.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nLogin failed, please try again! {0} more tries left.\n", tries);
                Console.ResetColor();



                tries--;
                if (tries == -1)
                {

                    timmer.timer();

                    tries = 2;


                }

                continue;
            }
            // Remove foreach because logged in user must be one
            foreach (BankUserModel user in checkedUsers)
            {
                user.accounts = PostgresDataAccess.GetUserAccounts(user.id);
                Console.WriteLine($"Logged in as {user.first_name} your pincode is {user.pin_code} and the id is {user.id}\n");
                Console.WriteLine($"role_id: {user.role_id} branch_id: {user.branch_id}\n");
                Console.WriteLine($"is_admin: {user.is_admin} is_client: {user.is_client}\n");
                Console.WriteLine($"User account list length: {user.accounts}\n");
                if (user.accounts.Count > 0)
                {
                    foreach (BankAccountModel account in user.accounts)
                    {
                        Console.WriteLine($"ID: {account.id} Account name: {account.name} Balance: {account.balance}\n");
                        Console.WriteLine($"Currency: {account.currency_name} Exchange rate: {account.currency_exchange_rate}\n");
                    }
                }
                if (user.role_id == 1 || user.role_id==3)
                {
                    Console.WriteLine("Hello !! You are a administrator and you have the right to create an account:");
                    Console.WriteLine("Select the menu below:");
                    Console.WriteLine("1. To Create user:");
                    Console.WriteLine("2. Exit");
                    string choice= Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            PostgresDataAccess.CreateUsers();
                            break;

                            case "2":
                            break;
                        
                    }

                }
                if (user.role_id==2)
                {
                    Console.WriteLine("Welcome to your Banksystem:");
                    Console.WriteLine("Select the menu below to perform your task:");
                    Console.WriteLine("1. Create Accounts:");
                    Console.WriteLine("2. To Deposit:");
                    Console.WriteLine("3. Withdraw:");
                    Console.WriteLine("4. To Transfer");
                    Console.WriteLine("5. To Logout");
                    string choice= Console.ReadLine();
                    switch (choice)
                    {
                        case "1":

                            PostgresDataAccess.CreateAccounts();
                            break;

                        // To deposit functions
                        case "2":
                            PostgresDataAccess.deposite();
                            //Console.WriteLine("Deposite successful:");
                            break;

                        // To withdraw functions
                        case "3":
                            PostgresDataAccess.withdraw(user);
                            //Console.WriteLine("Withdraw successful:");
                            break;

                        //To Transfer functions
                        case "4":
                            PostgresDataAccess.Transfer(user);
                            //Console.WriteLine("Transsfer succeeded");
                            break;


                        // To Log out
                         case "5":

                            break;

                    }
                }

            }
        }

        
    }
    static void ConsoleDraw(IEnumerable<string> lines, int x, int y)
    {
        if (x > Console.WindowWidth) return;
        if (y > Console.WindowHeight) return;

        var trimLeft = x < 0 ? -x : 0;
        int index = y;

        x = x < 0 ? 0 : x;
        y = y < 0 ? 0 : y;

        var linesToPrint =
            from line in lines
            let currentIndex = index++
            where currentIndex > 0 && currentIndex < Console.WindowHeight
            select new
            {
                Text = new String(line.Skip(trimLeft).Take(Math.Min(Console.WindowWidth - x, line.Length - trimLeft)).ToArray()),
                X = x,
                Y = y++
            };

        Console.Clear();
        foreach (var line in linesToPrint)
        {
            Console.SetCursorPosition(line.X, line.Y);
            Console.Write(line.Text);
        }
    }

}
