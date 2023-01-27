using Npgsql;

namespace DBTest;

class Program
{
    static void Main(string[] args)
    {
       // PostgresDataAccess post = new PostgresDataAccess();   
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
        while (true)
        {
            Console.Write("Please enter FirstName: ");
            string firstName = Console.ReadLine();

            Console.Write("Please enter PinCode: ");
            string pinCode = Console.ReadLine();
            List<BankUserModel> checkedUsers = PostgresDataAccess.CheckLogin(firstName, pinCode);
            if (checkedUsers.Count < 1)
            {
                Console.WriteLine("Login failed, please try again");
                continue;
            }
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
                    Console.WriteLine("1. To Create Account:");
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
                    Console.WriteLine("3.Withdraw:");
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
                            Console.WriteLine("Deposite successful:");
                            break;

                        // To withdraw functions
                        case "3":
                            PostgresDataAccess.withdraw();
                            Console.WriteLine("Withdraw successful:");
                            break;

                        //To Transfer functions
                        case "4":
                            PostgresDataAccess.withdraw();
                            PostgresDataAccess.deposite();
                            Console.WriteLine("Transsfer succeeded");
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
