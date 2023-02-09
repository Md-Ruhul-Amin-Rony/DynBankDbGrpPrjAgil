using Npgsql;
using System.Media;

namespace DBTest;

class Program
{
    static void Main(string[] args)
    {
        SoundPlayer player = new SoundPlayer("C:\\Users\\adiar\\OneDrive\\Desktop\\BankAppDB\\DBTest\\06 - Relaxing Harp.wav");
        player.PlayLooping();
        Console.WriteLine("The music is playing. Press Enter to stop.");
        // Console.ReadLine();
        // player.Stop();
        // PostgresDataAccess post = new PostgresDataAccess();   
        List<BankUserModel> users1 = PostgresDataAccess.OldLoadBankUsers();

        foreach (BankUserModel item in users1)
        {
            Console.WriteLine($"Id is :{item.id}, email is : {item.email}, pincode is :{item.pin_code}");
            Console.WriteLine($" name is : {item.first_name}, pincode is :{item.pin_code}");

        }
        List<BankUserModel> users = PostgresDataAccess.LoadBankUsers();
        Console.WriteLine($"users length: {users.Count}");
        foreach (BankUserModel user in users)
        {
            Console.WriteLine($"Hello {user.first_name}, your email is {user.email} your pincode is {user.pin_code}");
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
                Console.WriteLine($"Logged in as {user.first_name},your email is {user.email} your pincode is {user.pin_code} and the id is {user.id}\n");
                Console.WriteLine($"role_id: {user.role_id} branch_id: {user.branch_id}\n");
                Console.WriteLine($"is_admin: {user.is_admin} is_client: {user.is_client}\n");
                Console.WriteLine($"User account list length: {user.GetAccounts().Count}\n");// here need to add 'count' method to get the exact number.
                Console.WriteLine("please select an account from the list:");

                if (user.GetAccounts().Count > 0)
                {
                    decimal totalBalance = 0;
                    foreach (BankAccountModel account in user.accounts)
                    {
                        Console.WriteLine($"ID: {account.id} Account name: {account.name} Balance: {account.balance}\n");
                        // Console.WriteLine($"Currency: {account.currency_id} Exchange rate: {account.currency_id.exchange_rate}\n");
                        totalBalance += account.balance;
                    }
                    Console.WriteLine($"Total balance is : {totalBalance}");
                }
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
                                PostgresDataAccess.Deposit(user);

                                break;


                            case "4":
                                PostgresDataAccess.withdraw(user);

                                break;


                            case "5":
                                PostgresDataAccess.Transfer(user);

                                break;


                            // To Log out
                            case "6":

                                cont3 = false;
                                break;
                            case "7":
                                PostgresDataAccess.TransactionHistory(user);
                                break;
                            case "8":
                                PostgresDataAccess.TransferMoney();
                                break;
                            case "9":
                                PostgresDataAccess.GetTransactionByAccountId();
                                break;
                        }
                    }
                }
                if (user.role_id == 2)
                {

                    Console.WriteLine("Welcome to your Banksystem:");
                    Console.WriteLine("Select the menu below to perform your task:");
                    Console.WriteLine("1. Create Accounts:");
                    Console.WriteLine("2. To Deposit:");
                    Console.WriteLine("3. Withdraw:");
                    Console.WriteLine("4. To Transfer");
                    Console.WriteLine("5. To Logout");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":

                            PostgresDataAccess.CreateAccounts(user);
                            break;

                        // To deposit functions
                        case "2":
                            PostgresDataAccess.Deposit(user);
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
}
