using Npgsql;
using System.Security.Principal;

namespace DBTest;

class Program
{
    static void Main(string[] args)
    {

        PostgresDataAccess post = new PostgresDataAccess();
        List<BankUserModel> users1 = PostgresDataAccess.OldLoadBankUsers();

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

                    //for (int i = 0; i < user.accounts.Count; i++)
                    //{
                    //    Console.WriteLine($"<{i + 1}. {user.accounts[i].name}: {user.accounts[i].name}");
                    //}

                    foreach (BankAccountModel account in user.accounts)
                    {
                        Console.WriteLine($"ID: {account.id} Account name: {account.name} Balance: {account.balance}\n");
                        Console.WriteLine($"Currency: {account.currency_name} Exchange rate: {account.currency_exchange_rate}\n");
                    }

                    //kristians
                    //for (int i = 0; i < user.accounts.Count; i++)
                    //{
                    //    Console.WriteLine($"\nID : {user.accounts[i].id} Account name: {user.accounts[i].name} Balance: {user.accounts[i].balance}\n");

                    //    for (int j = 0;j < user.accounts[i].transactions.Count; j++) 
                    //    {
                    //        Console.WriteLine($"ppp {user.accounts[i].transactions[j].transaction_name}: {user.accounts[i].transactions[j].transferred_amount}\n");
                    //    }
                       
                    //}---
                   
                    //string useraccountchoice = console.readline();
                    //int accountidtchosen = user.accounts[int32.parse(useraccountchoice) - 1].id;

                    // userAccc... är det account_id som anävndaren har valt
                    // skapa en transactioonModelklass utifrån fälten som finns i databasen
                    // TODO: sjriv en ny funktion i postgresdataaccess som tar in account_id
                    // den ska ställa frågan till SQL:
                    // select * from bank_transactions where from_account_id = 9 OR to_account_id=9 order by timestamps DESC

                    // om to_account_id = 9 innebär det: att det är plus, annars minus
                    // det är antingen en depostit till konto 9 (from_account_id = null) amount ska vara plus
                    // eller så är det en transfer till 9, i så fall är from_account_id INTE null) amount ska vara plus

                    // om from_account_id = 9 innebär
                    // antingen en withdraw från 9 om to_account_id = null, amount ska vara minus
                    // eller en transfer ut från id 9 om to_account_id inte är null, amount ska vara minus


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

                    Console.WriteLine("\n0. See your treansferred history");
                    Console.WriteLine("6. See your deposit history");
                    Console.WriteLine("7. See your withdraw history");
                    string choice= Console.ReadLine();
                    switch (choice)
                    {
                        case "0":
                        //    //see you accounts and balnace
                           PostgresDataAccess.transforHistory(user);
                           break;
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

                        case "6":
                            PostgresDataAccess.transforHistoryDeposit(user);
                            //Console.WriteLine("Transsfer succeeded");
                            break;
                        case "7":
                            PostgresDataAccess.transforHistoryWithdraw(user);
                            //Console.WriteLine("Transsfer succeeded");
                            break;

                    }
                }

            }
            

        }

        
    }
}
