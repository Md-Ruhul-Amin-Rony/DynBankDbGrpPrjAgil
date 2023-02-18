using System.Media;

namespace DBTest
{
    public class StartProgramBase
    {
        public static void ProgramStart()
        {

            SoundPlayer player = new SoundPlayer("C:\\Users\\hasan\\OneDrive\\Skrivbord\\BankApp-20230212\\DynBankDbGrpPrjAgil\\06 - Relaxing Harp.wav");
            player.PlayLooping();
            Console.WriteLine("The music is playing. Press Enter to stop.");
            Console.ReadLine();
            player.Stop();

            //PostgresDataAccess post = new PostgresDataAccess();


            //List<BankUserModel> users1 = PostgresDataAccess.OldLoadBankUsers();

            ////        string title = @"
            ////$$\       $$\                           
            ////$$ |      \__|                          
            ////$$ |      $$\  $$$$$$\  $$$$$$$\        
            ////$$ |      $$ |$$  __$$\ $$  __$$\       
            ////$$ |      $$ |$$ /  $$ |$$ |  $$ |      
            ////$$ |      $$ |$$ |  $$ |$$ |  $$ |      
            ////$$$$$$$$\ $$ |\$$$$$$  |$$ |  $$ |      
            ////\________|\__| \______/ \__|  \__|    ";

            ////        Console.Write(title);
            ////        Console.WriteLine("\n\n\n\nsWelcome to Our bank");
            ////        //Console.ReadKey();
            //// Demo onlys



            //string title = @" 
            //                                     \|\||
            //                                    -' ||||/
            //                                   /7   |||||/
            //                                  /    |||||||/`-.____________
            //                                  \-' |||||||||               `-._
            //                                   -|||||||||||               |` -`.
            //                                     ||||||               \   |   `\\
            //                                      |||||\  \______...---\_  \    \\
            //                                         |  \  \           | \  |    ``-.__--.
            //                                         |  |\  \         / / | |       ``---'
            //                                       _/  /_/  /      __/ / _| |
            //                                      (,__/(,__/      (,__/ (,__/

            // ";
            //Console.ForegroundColor = ConsoleColor.Blue;
            //Console.WriteLine(title);
            //Console.ForegroundColor = ConsoleColor.DarkGreen;
            //Console.WriteLine("Welcome to Our bank\n\n");
            //Console.ResetColor();
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



            Console.ForegroundColor = ConsoleColor.Blue;

            var maxLength = arr.Aggregate(0, (max, line) => Math.Max(max, line.Length));
            for (int x = 0; x < Console.BufferWidth / 2 - maxLength / 2; x++)
            {
                ConsoleDraw(arr, x, 1);
                Thread.Sleep(100);
            }
            Console.ReadKey();
            Console.ResetColor();

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
            // End Ascii
            //foreach (BankUserModel item in users1)
            //{
            //    Console.WriteLine($"Id is :{item.id}, email is : {item.email}, pincode is :{item.pin_code}");
            //    Console.WriteLine($" name is : {item.first_name}, pincode is :{item.pin_code}");
            //}
            List<BankUserModel> users = PostgresDataAccess.LoadBankUsers();
            Console.WriteLine($"users length: {users.Count}");
            Console.WriteLine("\nChose a user from the list to login.");
            foreach (BankUserModel user in users)
            {
                //Console.WriteLine($"\nHello {user.first_name}, your email is {user.email} your pincode is {user.pin_code}");
                Console.WriteLine($"\nHello {user.first_name}, your email is {user.email} your pincode is {user.pin_code}");
            }
            int tries = 3;
            while (true)
            {
                Console.Write("Please enter email address: ");
                string email = Console.ReadLine();

                Console.Write("Please enter PinCode: ");
                string pinCode = Console.ReadLine();

                // Possible to login to multuple user. System shouldn't multiple user to login. It should return one unique user, use FirstOrDefault.
                // Prevent duplicate user to register.
                //First check if the new user exists or not.
                List<BankUserModel> checkedUsers = PostgresDataAccess.CheckLogin(email, pinCode);
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nLogged in as {user.first_name} ID : {user.id}");
                    Console.ResetColor();
                    //Console.WriteLine($"Logged in as {user.first_name},your email is {user.email} your pincode is {user.pin_code} and the id is {user.id}\n");
                    //Console.WriteLine($"role_id: {user.role_id} branch_id: {user.branch_id}\n");
                    //Console.WriteLine($"is_admin: {user.is_admin} is_client: {user.is_client}\n");
                    //Console.WriteLine($"User account list length: {user.GetAccounts().Count}\n");// here need to add 'count' method to get the exact number.
                    //Console.WriteLine("please select an account from the list:");

                    //if (user.GetAccounts().Count > 0)
                    //{

                    //    decimal totalBalance = 0;

                    //    foreach (BankAccountModel account in user.accounts)
                    //    {
                    //        Console.WriteLine($"ID: {account.id} Account name: {account.name} Balance: {account.balance}\n");
                    //        // Console.WriteLine($"Currency: {account.currency_id} Exchange rate: {account.currency_id.exchange_rate}\n");
                    //        totalBalance += account.balance;
                    //    }
                    //    Console.WriteLine($"Total balance is : {totalBalance}");
                    //}
                    if (user.GetAccounts().Count > 0)
                    {
                        for (int i = 0; i < user.GetAccounts().Count; i++)
                        {

                        }

                    }

                    if (user.role_id == 1)

                    {

                        Console.WriteLine("Hello !! You are a administrator and you have the right to create an account:");
                        Console.WriteLine("Select the menu below:");
                        Console.WriteLine("1. To Create user:");
                        Console.WriteLine("2. Exit");
                        string choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "1":
                                PostgresDataAccess.CreateUsers();
                                break;

                            case "2":
                                break;

                        }

                    }

                    if (user.role_id == 3)
                    {
                        bool cont3 = true;
                        while (cont3)
                        {
                        mainmenu:
                            Console.WriteLine("Hello !! You are an administrator and client and you have the right to create an account and use banksystem:");
                            Console.WriteLine("Select the menu below:");
                            Console.WriteLine("1. To Create user:");
                            Console.WriteLine("2. Create Accounts:");
                            Console.WriteLine("3. To Deposit:");
                            Console.WriteLine("4. Withdraw:");
                            Console.WriteLine("5. To Transfer");
                            Console.WriteLine("6. To Logout");
                            Console.WriteLine("7. Transaction History");
                            Console.WriteLine("8. Transfer K");
                            Console.WriteLine("9. To Transactionstory");
                            Console.WriteLine("10. Loan Department");
                            string choice = Console.ReadLine();
                            switch (choice)
                            {
                                case "1":
                                    PostgresDataAccess.CreateUsers();

                                    break;

                                case "2":

                                    PostgresDataAccess.CreateAccounts(user);
                                    break;

                                case "3":
                                    PostgresDataAccess.deposite(user);

                                    break;


                                case "4":
                                    PostgresDataAccess.Withdraw(user);

                                    break;


                                case "5":
                                    PostgresDataAccess.Transfer(user);

                                    break;


                                // To Log out
                                case "6":

                                    cont3 = false;
                                    break;
                                case "7":
                                    PostgresDataAccess.transforHistory(user);
                                    break;
                                case "8":
                                    PostgresDataAccess.Transfer(user);
                                    break;
                                case "9":
                                    PostgresDataAccess.GetTransactionByAccountId();
                                    break;
                                case "10":
                                    loanMenu:
                                    Console.WriteLine("---------------------------------------------");
                                    Console.WriteLine("\nWelcome to loan department in LION's Bank!\n");
                                    Console.WriteLine("   1. Loan calculation. Please PRESS 1");
                                    Console.WriteLine("   2. How much loan You will get from Lion's Bank. Please press 2");
                                    Console.WriteLine("   3. Goto Main menu!");
                                    Console.WriteLine("---------------------------------------------");

                                    string loanOption = Console.ReadLine();

                                    switch (loanOption)
                                    {
                                        case "1":
                                            PostgresDataAccess.LoanCalculation();
                                            goto loanMenu;
                                        case "2":
                                            PostgresDataAccess.LoanWithNormalTim_Query(user);
                                            goto loanMenu; ;
                                        case "3":
                                            goto mainmenu;
                                        default:
                                            Console.WriteLine("Invalid input. Please select 1 OR 2");
                                            goto loanMenu; ;
                                    }
                                    break;
                            }
                        }
                    }
                    if (user.role_id == 2)
                    {
                    mainmenu:
                        Console.WriteLine("Welcome to your Banksystem:");
                        Console.WriteLine("Select the menu below to perform your task:");
                        Console.WriteLine("0. See your acounts");
                        Console.WriteLine("1. Create Accounts:");
                        Console.WriteLine("2. Deposit:");
                        Console.WriteLine("3. Withdraw:");
                        Console.WriteLine("4. Transfer");
                        Console.WriteLine("5. Loan Department");
                        Console.WriteLine("6. Transactions history");
                        Console.WriteLine("7. Logout");
                        string choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "0":
                                PostgresDataAccess.seeAccounts(user);
                                Console.WriteLine($"\nPres enter to get back to main menu.");
                                Console.ReadLine();
                                goto mainmenu;
                            case "1":

                                PostgresDataAccess.CreateAccounts(user);
                                break;

                            // To deposit functions
                            case "2":
                                PostgresDataAccess.deposite(user);
                                goto mainmenu;
                            //Console.WriteLine("Deposite successful:");


                            // To withdraw functions
                            case "3":
                                PostgresDataAccess.Withdraw(user);
                                goto mainmenu;
                            //Console.WriteLine("Withdraw successful:");


                            case "4":
                                PostgresDataAccess.Transfer(user);
                                goto mainmenu;

                            case "5":
                            returnToLoanDepartment:
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("\nWelcome to loan department in LION's Bank!\n");
                                Console.WriteLine("   1. Loan calculation. Please PRESS 1");
                                Console.WriteLine("   2. How much loan You will get from Lion's Bank. Please press 2");
                                Console.WriteLine("   3. Goto Main menu!");
                                Console.WriteLine("---------------------------------------------");

                                string loanOption = Console.ReadLine();


                                switch (loanOption)
                                {
                                    case "1":
                                        PostgresDataAccess.LoanCalculation();

                                        goto mainmenu;
                                    case "2":
                                        PostgresDataAccess.LoanWithNormalTim_Query(user);
                                        goto mainmenu;
                                    case "3":
                                        goto mainmenu;
                                    default:
                                        Console.WriteLine("Invalid input. Please select 1 OR 2");
                                        goto mainmenu;
                                }
                                break;


                            case "6":
                                Console.Clear();
                                Console.WriteLine("1. Deposit history");
                                Console.WriteLine("2. Withdraw history");
                                Console.WriteLine("3. Transfer history");
                                Console.WriteLine("4. Back to main menu");
                                string witch = Console.ReadLine();
                                switch (witch)
                                {
                                    case "1":
                                        PostgresDataAccess.depositHistory(user);
                                        goto mainmenu;

                                    case "2":
                                        PostgresDataAccess.WithdrawHistory(user);
                                        goto mainmenu;

                                    case "3":
                                        PostgresDataAccess.transforHistory(user);
                                        goto mainmenu;


                                    case "4":

                                        goto mainmenu;

                                }
                                goto mainmenu;

                                // To Log out


                        }
                    }

                }


            }
        }
    }
}