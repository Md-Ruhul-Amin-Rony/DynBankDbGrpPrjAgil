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
                Console.WriteLine($"Logged in as {user.first_name} your pincode is {user.pin_code} and the id is {user.id}");
                Console.WriteLine($"role_id: {user.role_id} branch_id: {user.branch_id}");
                Console.WriteLine($"is_admin: {user.is_admin} is_client: {user.is_client}");
                Console.WriteLine($"User account list length: {user.accounts}");
                if (user.accounts.Count > 0)
                {
                    foreach (BankAccountModel account in user.accounts)
                    {
                        Console.WriteLine($"ID: {account.id} Account name: {account.name} Balance: {account.balance}");
                        Console.WriteLine($"Currency: {account.currency_name} Exchange rate: {account.currency_exchange_rate}");
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

                            //Console.WriteLine(" Enter your userID:");
                            //int userID = int.Parse(Console.ReadLine());
                            //Console.WriteLine(" Enter your desired username:");
                            //string first_name = Console.ReadLine().ToLower();
                            //Console.WriteLine("Enter your last name:");
                            //string last_name = Console.ReadLine();
                            //Console.WriteLine("select your role id between 1-3");
                            //int role_id = int.Parse(Console.ReadLine());
                            //Console.WriteLine("Select your branch id between 1-3");
                            //int branch_id = Convert.ToInt32(Console.ReadLine());

                            //Console.WriteLine(" Enter your desired password:");
                            //string desiredPassword = Console.ReadLine();


                            //NpgsqlCommand insertCommand = new NpgsqlCommand("INSERT INTO bank_user(id, first_name,last_name,pin_code,role_id, branch_id) VALUES (@id, @first_name,@last_name, @pin_code,@role_id,@branch_id);", (NpgsqlConnection?)cnn);
                            //insertCommand.Parameters.AddWithValue("@id", userID);
                            //insertCommand.Parameters.AddWithValue("@first_name", first_name);
                            //insertCommand.Parameters.AddWithValue("@last_name", last_name);

                            //insertCommand.Parameters.AddWithValue("@pin_code", desiredPassword);
                            //insertCommand.Parameters.AddWithValue("@role_id", role_id);
                            //insertCommand.Parameters.AddWithValue("@branch_id", branch_id);
                            //insertCommand.ExecuteNonQuery();
                            //Console.WriteLine("Registration successful");
                            break;
                            case "2":
                            break;
                        
                    }

                }

            }
        }

        
    }
}
