﻿using Npgsql;
using System.Security.Principal;

namespace DBTest;

class Program
{
    static void Main(string[] args)
    {

        PostgresDataAccess post = new PostgresDataAccess();
        List<BankUserModel> users1 = PostgresDataAccess.OldLoadBankUsers();

        //        string title = @"
        //$$\       $$\                           
        //$$ |      \__|                          
        //$$ |      $$\  $$$$$$\  $$$$$$$\        
        //$$ |      $$ |$$  __$$\ $$  __$$\       
        //$$ |      $$ |$$ /  $$ |$$ |  $$ |      
        //$$ |      $$ |$$ |  $$ |$$ |  $$ |      
        //$$$$$$$$\ $$ |\$$$$$$  |$$ |  $$ |      
        //\________|\__| \______/ \__|  \__|    ";

        //        Console.Write(title);
        //        Console.WriteLine("\n\n\n\nsWelcome to Our bank");
        //        //Console.ReadKey();
        // Demo onlys
       


        string title = @" \|\||
  -' ||||/
 /7   |||||/
/    |||||||/`-.____________
\-' |||||||||               `-._
 -|||||||||||               |` -`.
   ||||||               \   |   `\\
    |||||\  \______...---\_  \    \\
       |  \  \           | \  |    ``-.__--.
       |  |\  \         / / | |       ``---'
     _/  /_/  /      __/ / _| |
    (,__/(,__/      (,__/ (,__/

         ";
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(title);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Welcome to Our bank\n\n");
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

                    //for (int i = 0; i < user.accounts.Count; i++)
                    //{
                    //    Console.WriteLine($"<{i + 1}. {user.accounts[i].name}: {user.accounts[i].name}");
                    //}

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
                    Console.WriteLine("5. To Loan Calculation");
                    Console.WriteLine("6. Able to Loan");
                    Console.WriteLine("7. Approved Loan with out Balance - with Normal_Query");
                    Console.WriteLine("8. Approved Loan with out Balance - with Tim_Query");


                    Console.WriteLine("9. See your deposit history");
                    Console.WriteLine("10.see you withdraw history");
                    Console.WriteLine("\n11. See your treansferred history");
                    Console.WriteLine("12. To Logout");
                    //Console.WriteLine("7. See your withdraw history");




                    string choice = Console.ReadLine();
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

                        case "5":
                            PostgresDataAccess.LoanCalculation();
                            break;

                        case "6":
                            PostgresDataAccess.Loan(user);
                            break;

                        case "7":
                            PostgresDataAccess.LoanWithNormal_Query(user);
                            break;
                        case "8":
                            PostgresDataAccess.LoanWithNormalTim_Query(user);
                            break;
                        case "9":
                            PostgresDataAccess.transforHistoryDeposit(user);
                            break;


                        case "10":
                            PostgresDataAccess.transforHistoryWithdraw(user);
                            //Console.WriteLine("Transsfer succeeded");
                            break;
                        case "11":
                            PostgresDataAccess.transforHistory(user);
                            break;

                        case "12":

                            break;
                    }

                }
             }

            
            

        }

        
    }
}
