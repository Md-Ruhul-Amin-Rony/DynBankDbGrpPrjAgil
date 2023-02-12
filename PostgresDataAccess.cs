using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using System.Globalization;
using System.Linq;


using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Xml.Linq;
using Dapper;
using Microsoft.VisualBasic;
using Npgsql;
using NpgsqlTypes;

namespace DBTest
{
    public class PostgresDataAccess

    {
        public static List<BankTransactionModel> GetTransactionByAccountId()

        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                Console.WriteLine("Enter your bank account id:");
                int account_id = int.Parse(Console.ReadLine());

                var output = cnn.Query<BankTransactionModel>($"SELECT * FROM bank_transaction WHERE from_account_id = {account_id} OR to_account_id = {account_id} ORDER BY timestamp DESC", new DynamicParameters());
                return output.ToList();
            }
            foreach (var transaction in GetTransactionByAccountId())
            {
                Console.WriteLine("Transaction Name: " + transaction.name);
                Console.WriteLine("From Account ID: " + transaction.from_account_id);
                Console.WriteLine("To Account ID: " + transaction.to_account_id);
                //Console.WriteLine("Timestamp: " + transaction.);
                Console.WriteLine("Amount: " + transaction.amount);
                Console.WriteLine("-----------------------------");
            }


        }




        
        public static List<BankUserModel> OldLoadBankUsers()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))

            {


                cnn.Open();




                var output = cnn.Query<BankUserModel>("SELECT * FROM bank_user", new DynamicParameters());



                return output.ToList();

            }

        }

        public static void CreateUsers()
        {
            {
                using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
                {
                    try
                    {
                        cnn.Open();
                        Console.WriteLine("Enter your First Name:");
                        string first_name = Console.ReadLine().ToLower();
                        Console.WriteLine("Enter your Last Name:");
                        string last_name = Console.ReadLine().ToLower();

                        again:
                        Console.WriteLine("Select your Role Id. 1. Administrator, 2. Client, 3. ClientAdmin.\n Press in between number.");
                        int role_id = int.Parse(Console.ReadLine());
                        if (role_id < 1 || role_id > 3)
                        {
                            Console.WriteLine("Invalid role id. Please select a number between 1 and 3.");
                            goto again;
                        }
                        else
                        {
                            again1:
                            Console.WriteLine("Select your branch id between 1. Koala, 2. Owl, 3. Panda, 4. Fox, 5.Squid , 6. Lion, 7.Rabbit, 8. Tiger.\n Press in between number.");
                            int branch_id = Convert.ToInt32(Console.ReadLine());
                           
                            if (branch_id < 1 || branch_id > 8)
                            {
                                Console.WriteLine("Invalid branch id. Please select a number between 1 and 8.");
                                goto again1;
                            }
                            else
                            {






                                Console.WriteLine("Enter your email:");
                                string email = Console.ReadLine();
                                Console.WriteLine("Enter your desired password:");
                                string pin_code = Console.ReadLine();
                                string check = "SELECT COUNT(*) FROM bank_user WHERE email = @email";
                                int count = cnn.ExecuteScalar<int>(check, new { email });
                                if (count > 0)
                                {
                                    Console.WriteLine("The email address is already in use.");
                                    return;
                                }
                                string sql = "INSERT INTO bank_user (first_name, last_name, email, pin_code, role_id, branch_id) " +
                                             "VALUES (@first_name, @last_name, @email, @pin_code, @role_id, @branch_id)";
                                cnn.Execute(sql, new { first_name, last_name, email, pin_code, role_id, branch_id });
                                Console.WriteLine("New user created successfully!");
                                cnn.Close();

                            }
                        }
                    }
                    catch (FormatException e)
                    {

                        Console.WriteLine("Invalid input. Please enter a valid input");
                    }
                   
                }

            }
        }


        public static void CreateAccounts(BankUserModel user)



        {
            using (NpgsqlConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                cnn.Open();
                again2:
                Console.WriteLine("Enter your Account Type: \n 1.Savings, 2.Salary, 3.ISK, 4.Pension, 5.Family A/C, 6.Child A/C");
                try
                {
                    int accountType = Convert.ToInt32(Console.ReadLine());

                    if (accountType<1 || accountType>6)
                    {
                        Console.WriteLine("Invalid input!! please select between 1-8");
                        goto again2;
                    }

                    string accountName = "";
                    double interestRate = 0;



                    switch (accountType)
                    {
                        case 1:
                            accountName = "Savings";
                            interestRate = 1.5;
                            Console.WriteLine("You have opened a savings account and you will get 1.5% interest yearly.");
                            break;
                        case 2:
                            accountName = "Salary";
                            interestRate = 0;
                            break;
                        case 3:
                            accountName = "ISK";
                            interestRate = 5;
                            break;
                        case 4:
                            accountName = "Pension";
                            interestRate = 1.5;
                            break;
                        case 5:
                            accountName = "Family A/C";
                            interestRate = 0;
                            break;
                        case 6:
                            accountName = "Child A/C";
                            interestRate = 0;
                            break;
                        default:
                            Console.WriteLine("Invalid account type");
                            break;
                    }
                    Console.WriteLine("Enter your user_id: ");
                    int user_id = Convert.ToInt32(Console.ReadLine());
                    int count = 0;
                   foreach (BankAccountModel item in user.accounts)
                    {

                        if (item.user_id == user_id)
                        {
                            count++;
                        }

                    }

                    if (count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("The account id you entered is not belongs to you");
                        Console.ResetColor();
                        return;
                    }
                    again4:
                    Console.WriteLine("Enter your currency_id: Type 1 for SEK; 2 for USD; 3 for EUR ");
                    int currencyId = Convert.ToInt32(Console.ReadLine());
                    if (currencyId<1||currencyId>3)
                    {
                        Console.WriteLine("Invalid input!! please select between 1-3!! ");
                        goto again4;

                    }
                    Console.WriteLine("Enter your balance: ");
                    decimal balance = Convert.ToDecimal(Console.ReadLine());


                    using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO bank_account (interest_rate, name, user_id, currency_id, balance) VALUES (@interest_rate, @name, @user_id,@currency_id, @balance)", cnn))


                    {
                        command.Parameters.AddWithValue("@name", accountName);
                        command.Parameters.AddWithValue("@interest_rate", interestRate);
                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@currency_id", currencyId);
                        command.Parameters.AddWithValue("@balance", balance);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine("Successfully created account");
                        Console.Clear();
                        cnn.Close();
                    }

                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");

                    return;
                }


            }

        }

        //read accounts
        public static void seeAccounts(BankUserModel user)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n        ======================================0===============0=====================================\n");
            Console.ResetColor();
            foreach (BankAccountModel item in user.accounts)
            {
               
                //decimal totalBalance = 0;
                Console.WriteLine("  ID: {0}  |        Account name: {1}        |        Balance: {2}  {3}", item. id, item.name, item.balance, item.currency_name);
                //totalBalance += item.balance;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                //Console.WriteLine($"                                                                               Total balance is : {totalBalance}");
                Console.ResetColor();

              

            }
        }

        // Deposit method
        public static void deposite(BankUserModel user)
        {

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {


                cnn.Open();
                int id;
                decimal deposit_amont;
                //bool sucsses = false;
                //while (!sucsses)
                //{
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n        ======================================0===============0=====================================\n");
                Console.ResetColor();



                foreach (BankAccountModel item in user.accounts)
                {
                    Console.WriteLine("Id: {0}  |        account: {1}        |        balance: {2}  {3}", item.id, item.name, item.balance, item.currency_name);

                }
                Console.WriteLine("\nChose one account above to deposit.      /by Id/  ");
                id = int.Parse(Console.ReadLine());
                Console.WriteLine("Select amount to deposit to:        /Swedish SEK/");

                deposit_amont = decimal.Parse(Console.ReadLine());
                int count = 0;
                foreach (BankAccountModel item in user.accounts)
                {
                    //for (int i = 0; i < user.accounts.Count; i++)
                    //{

                    //    Console.WriteLine("press");
                    //    Console.ReadLine();


                    if (item.id == id)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Account Id you selected Not exist. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine("Pess enter to go to main menu.");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    try
                    {
                        string depositQuery = "UPDATE bank_account SET balance = balance +@balance WHERE @id = id ";// AND @name =    name";
                        using (var depositCommand = new NpgsqlCommand(depositQuery, (NpgsqlConnection?)cnn))
                        {
                            //NpgsqlCommand dptcommand = new NpgsqlCommand("insert into bank_account(transaction_name,to_account_id, timestamps,transferred_amount) values (@transaction_name, @to_account_id,@timestamps,@transferred_amount)", (NpgsqlConnection?)cnn);

                            depositCommand.Parameters.AddWithValue("@balance", deposit_amont);
                            depositCommand.ExecuteNonQuery();
                            //insert to >> bank_transactions >> history
                            NpgsqlCommand insertcommand = new NpgsqlCommand("insert into bank_transactions(transaction_name,to_account_id, timestamps,transferred_amount) values (@transaction_name, @to_account_id,@timestamps,@transferred_amount);", (NpgsqlConnection?)cnn);
                            //insertcommand.Parameters.AddWithValue("@id", fromId);
                            insertcommand.Parameters.AddWithValue("@transaction_name", "Deposit");
                            //insertcommand.Parameters.AddWithValue("@balance", deposit_amont);

                            insertcommand.Parameters.AddWithValue("@to_account_id", id);
                            insertcommand.Parameters.AddWithValue("@timestamps", DateAndTime.Now);
                            insertcommand.Parameters.AddWithValue("@transferred_amount", deposit_amont);

                            insertcommand.ExecuteNonQuery();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine($"Deposit successfull!");
                            Console.ResetColor();
                            //foreach (BankAccountModel item in user.accounts) { if (item.id == id) { Console.WriteLine("\"Id: {0}  |        account: {1}        |       New balance: {2}  {3}\n", item.id, item.name, item.balance, item.currency_name); } }
                            using (var cmd = new NpgsqlCommand($"SELECT * FROM bank_transactions WHERE to_account_id = {id}", (NpgsqlConnection?)cnn))
                            {
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Deposit amount: "  +"+"+reader["transferred_amount"].ToString () +  " " +"TO account: " +reader["to_account_id"].ToString() +" "+ "Date/Time: " + reader["timestamps"].ToString());
                                     
                                        Console.ResetColor();
                                    }
                                }
                                // foreach (BankAccountModel item in user.accounts) { }
                                Console.WriteLine("\n1. See your deposit histories.");
                                Console.WriteLine("Pess enter to go to main menu.");
                           
                                string dp_hoistory = Console.ReadLine();
                                if (dp_hoistory == "1")
                                {
                                    depositHistory(user);
                                }

                                return;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Console.ResetColor();
                        Console.WriteLine("Invalid Input!");

                        Console.ReadLine();

                        Console.WriteLine("Pess enter to go to main menu.");
                        return;
                    }
                }



            }



        }


        // Read deposit history 
        public static void depositHistory(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n        ======================================0===============0=====================================\n");
                Console.ResetColor();
                foreach (BankAccountModel item in user.accounts)
                {
                    Console.WriteLine("Id: {0}  |        account: {1}        |        balance: {2}  {3}", item.id, item.name, item.balance, item.currency_name);

                }
                Console.WriteLine("\nChose one account above to see deposit historys.      /by Id/  ");
                int id = int.Parse(Console.ReadLine());

                int count = 0;
                foreach (BankAccountModel item in user.accounts)
                {

                    if (item.id == id)
                    {
                        count++;
                    }
                    //Console.WriteLine(item);
                }
                if (count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("The account id you entered is not belogs to you ");
                    Console.ResetColor();
                }
                else
                {

                    using (var cmd = new NpgsqlCommand($"SELECT * FROM bank_transactions WHERE to_account_id = {id}", (NpgsqlConnection?)cnn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                
                                Console.WriteLine(" Transaction id: {0}\n Transaction name: {1} \n Deposit : {2} \n To account: {3} \n Date/Time: {4} \n Amount: +{5:N2}\n", reader.GetInt32(0), reader.GetString(1), reader.IsDBNull(2), reader.GetInt32(3), reader.GetDateTime(4), reader.GetDouble(5));
                                Console.ResetColor();
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nYou have No more deposit history in this account yet.");
                        Console.ResetColor();
                        Console.WriteLine("Press enter get back to main menu.");
                        Console.ReadLine();

                    }
                }



            }
        }


        //Withdraw mathod

        public static void withdraw(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n        ======================================0===============0=====================================\n");
                Console.ResetColor();
                foreach (BankAccountModel item in user.accounts)
                {
                    Console.WriteLine("Id: {0}  |        account: {1}        |        balance: {2}  {3}", item.id, item.name, item.balance, item.currency_name);

                }
                Console.WriteLine("\nChose one account above to withdraw.      /by Id/  ");
                int id = int.Parse(Console.ReadLine());

                //Console.WriteLine(" Enter your Account Type: \n Savings, Salary, ISK, Pension, Family A/C, Child A/C "); // Account Type.
                //Console.WriteLine("Select Your account name:");
                //string account_Name = Console.ReadLine().ToLower();

                Console.WriteLine("Select amount to withdraw.        /Swedish SEK/");
                decimal withdraw_amont = decimal.Parse(Console.ReadLine());


                int count = 0;
                foreach (BankAccountModel item in user.accounts)
                {
                    if (item.id == id)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Account Id you selected Not exist. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine("Pess enter to go to main menu.");
                    Console.ReadLine();
                    return;
                }
                else
                {

                    

                    try
                    {
                        string withdrwQuery = "UPDATE bank_account SET balance = balance - @balance WHERE @id = id ";
                        using (var withdrawCommand = new NpgsqlCommand(withdrwQuery, (NpgsqlConnection?)cnn))
                        {
                            withdrawCommand.Parameters.AddWithValue("@balance", withdraw_amont);
                            withdrawCommand.ExecuteNonQuery();
                            //insert to >> bank_transactions >> history
                            NpgsqlCommand insertcommand = new NpgsqlCommand("insert into bank_transactions(transaction_name,from_account_id, timestamps,transferred_amount) values (@transaction_name, @from_account_id,@timestamps,@transferred_amount);", (NpgsqlConnection?)cnn);
                            //insertcommand.Parameters.AddWithValue("@id", fromId);
                            insertcommand.Parameters.AddWithValue("@transaction_name", "Withdraw");
                            //insertcommand.Parameters.AddWithValue("@from_account_id", id);

                            insertcommand.Parameters.AddWithValue("@from_account_id", id);
                            insertcommand.Parameters.AddWithValue("@timestamps", DateAndTime.Now);
                            insertcommand.Parameters.AddWithValue("@transferred_amount", withdraw_amont);

                            insertcommand.ExecuteNonQuery();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Withdrw successfull!");
                            Console.ResetColor();
                            //foreach (BankAccountModel item in user.accounts) { if (item.id == id) { Console.WriteLine("\"Id: {0}  |        account: {1}        |       New balance: {2}  {3}\n", item.id, item.name, item.balance, item.currency_name); } }
                            using (var cmd = new NpgsqlCommand($"SELECT * FROM bank_transactions WHERE from_account_id = {id}", (NpgsqlConnection?)cnn))
                            {
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                                        Console.WriteLine("Withdrw amount: " + "-" + reader["transferred_amount"].ToString() + " " + "From account: " + reader["from_account_id"].ToString() + " " + "Date/Time: " + reader["timestamps"].ToString());

                                        Console.ResetColor();
                                    }
                                }
                                // foreach (BankAccountModel item in user.accounts) { }
                                Console.WriteLine("\n1. See your withdrw histories.");

                                string wd_hoistory = Console.ReadLine();
                                Console.WriteLine("Pess enter to go to main menu.");
                                if (wd_hoistory == "1")
                                {
                                    WithdrawHistory(user);
                                }


                                return;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Console.ResetColor();
                        Console.WriteLine("Invalid Input!");

                        Console.ReadLine();

                        Console.WriteLine("Pess enter to go to main menu.");
                        return;

                    }
                    
                }

              

            }

        }
        // Read withdrw history 
        public static void WithdrawHistory(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("\n        ======================================0===============0=====================================\n");
                Console.ResetColor();
                foreach (BankAccountModel item in user.accounts)
                {
                    Console.WriteLine("Id: {0}  |        account: {1}        |        balance: {2}  {3}", item.id, item.name, item.balance, item.currency_name);

                }
                Console.WriteLine("\nChose one account above to see withdraw historys.      /by Id/  ");
                int id = int.Parse(Console.ReadLine());

                int count = 0;
                foreach (BankAccountModel item in user.accounts)
                {

                    if (item.id == id)
                    {
                        count++;
                    }
                    //Console.WriteLine(item);
                }
                if (count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("The account id you entered is not belogs to you ");
                    Console.ResetColor();
                }
                else
                {

                    using (var cmd = new NpgsqlCommand($"SELECT * FROM bank_transactions WHERE from_account_id = {id} ", (NpgsqlConnection?)cnn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine(" Transaction id: {0}\n Transaction name: {1} \n From account : {2} \n Withdraw: {3} \n Date/Time: {4} \n Amount: -{5:N2}\n", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.IsDBNull(3), reader.GetDateTime(4), reader.GetDouble(5));
                                Console.ResetColor();
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nYou have No more Withdraw history in this account yet.");
                        Console.ResetColor();
                        Console.WriteLine("Press enter get back to main menu.");
                        Console.ReadLine();
                    }
                }



            }
        }

       

        public static void Transfer(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n        ======================================0===============0=====================================\n");
                Console.ResetColor();
                foreach (BankAccountModel item in user.accounts)
                {
                    Console.WriteLine("Id: {0}  |        account: {1}        |        balance: {2}  {3}", item.id, item.name, item.balance, item.currency_name);

                }
                Console.WriteLine("\nChose one account above to transfer from.      /by Id/  ");

                int fromId = int.Parse(Console.ReadLine());

                int count = 0;

                Console.WriteLine("Select amount you want to transfer.      /SEK/");
                double transferMoney = double.Parse(Console.ReadLine());

                double senderTotalAmount = 0;

                foreach (BankAccountModel item in user.accounts)
                {

                    if (item.id == fromId)
                    {
                        Console.WriteLine($"Currency typ :{item.currency_name}");

                        if (item.currency_name == "USD" || item.currency_name == "EUR")
                        {
                            senderTotalAmount = transferMoney / item.currency_exchange_rate;
                        }
                        else
                        {
                            senderTotalAmount = transferMoney;
                        }
                        count++;
                    }
                }
                if (count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nThe account information you entered is not belogs to you\n ");
                    Console.ResetColor();
                    Console.WriteLine("Press enter to get back to main menu. ");
                }
                else
                {

                    string transferQuery = "UPDATE bank_account SET balance = balance - @balance WHERE @id = id";

                    using (var transferCommand = new NpgsqlCommand(transferQuery, (NpgsqlConnection?)cnn))
                    {
                        transferCommand.Parameters.AddWithValue("@id", fromId);
                        transferCommand.Parameters.AddWithValue("@balance", senderTotalAmount);

                        transferCommand.ExecuteNonQuery();
                        //Console.WriteLine($"deposited {transferMoney} into account for user {id} to account name {Acount_name} ");
                    }


                    Console.WriteLine("Which A/C to transfer?  Write down your A/C serial Number");
                    int to_id = int.Parse(Console.ReadLine());

                    //Console.WriteLine("Give a name to your transaction.");
                    //string transaction_name = Console.ReadLine();



                    double receiverTotalAmount = 0;

                    BankAccountModel receiver;
                    var output = cnn.Query<BankAccountModel>($"SELECT *, bank_currency.name AS currency_name, bank_currency.exchange_rate AS currency_exchange_rate FROM bank_account, bank_currency WHERE bank_account.id = '{to_id}' AND bank_account.currency_id = bank_currency.id", new DynamicParameters());
                    receiver = output.FirstOrDefault();
                    Console.WriteLine($"receiver account currency type is :{receiver.currency_name}");

                    if (receiver.currency_name == "USD" || receiver.currency_name == "EUR")
                    {
                        receiverTotalAmount = transferMoney / receiver.currency_exchange_rate;
                    }
                    else
                    {
                        receiverTotalAmount = transferMoney;
                    }

                    transferQuery = "UPDATE bank_account SET balance = balance + @balance WHERE @id = id";

                    using (var transferCommand = new NpgsqlCommand(transferQuery, (NpgsqlConnection?)cnn))
                    {
                        transferCommand.Parameters.AddWithValue("@id", to_id);
                        transferCommand.Parameters.AddWithValue("@balance", receiverTotalAmount);

                        transferCommand.ExecuteNonQuery();
                        Console.WriteLine("{0:N2} {1} has been transfer from {2} to {3}", receiverTotalAmount, receiver.currency_name, fromId, to_id);
                        Console.WriteLine("Transsfer succeeded");
                        //Console.WriteLine("Du har inte tillräckligt med pengar din balance är {0:N2} {1} försök igen med lägre summa.", lBalance, sek);

                        //old
                        //transferquery = "insert into bank_transactions(id, transaction_name,from_account_id,to_account_id, timestamps)  (@id, @transaction_name,@from_account_id, @to_account_id,@timestamps";
                        //insetr into transaction 

                        NpgsqlCommand insertcommand = new NpgsqlCommand("insert into bank_transactions(transaction_name,from_account_id,to_account_id, timestamps,transferred_amount) values (@transaction_name, @from_account_id, @to_account_id,@timestamps,@transferred_amount);", (NpgsqlConnection?)cnn);
                        //insertcommand.Parameters.AddWithValue("@id", fromId);
                        insertcommand.Parameters.AddWithValue("@transaction_name", "Transfer");
                        insertcommand.Parameters.AddWithValue("@from_account_id", fromId);

                        insertcommand.Parameters.AddWithValue("@to_account_id", to_id);
                        insertcommand.Parameters.AddWithValue("@timestamps", DateAndTime.Now);
                        insertcommand.Parameters.AddWithValue("@transferred_amount", transferMoney);

                        insertcommand.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nTransfer successfull.");

                        Console.ResetColor();

                        using (var cmd = new NpgsqlCommand($"SELECT * FROM bank_transactions WHERE from_account_id = {fromId} AND to_account_id= {to_id}", (NpgsqlConnection?)cnn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("Transferred amount: " + "-" + reader["transferred_amount"].ToString() + " " + "From account: " + reader["from_account_id"].ToString() + "To account: " + reader["to_account_id"].ToString() + " " + "Date/Time: " + reader["timestamps"].ToString());

                                    Console.ResetColor();
                                }
                            }
                            // foreach (BankAccountModel item in user.accounts) { }
                            Console.WriteLine("\n1. See your Transfer histories.");
                            Console.WriteLine("Pess enter to go to main menu.");
                           
                            string wd_hoistory = Console.ReadLine();
                            if (wd_hoistory == "1")
                            {
                                transforHistory(user);
                            }

                          

                            return;
                        }
                    }

                   
                }
                cnn.Close();
            }

        }

        


        public static void transforHistory(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n        ======================================0===============0=====================================\n");
                Console.ResetColor();
                foreach (BankAccountModel item in user.accounts)
                {
                    Console.WriteLine("Id: {0}  |        account: {1}        |        balance: {2}  {3}", item.id, item.name, item.balance, item.currency_name);

                }
                Console.WriteLine("\nChose one account above to see the history.      /by Id/  ");

           
                int id = int.Parse(Console.ReadLine());

                int count = 0;
                foreach (BankAccountModel item in user.accounts)
                {

                    if (item.id == id)
                    {
                        count++;
                    }
                    //Console.WriteLine(item);
                }

                if (count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("The account id you entered is not belogs to you ");
                    Console.ResetColor();
                }
                else
                {
                    try
                    {
                        using (var cmd = new NpgsqlCommand($"SELECT * FROM bank_transactions WHERE  from_account_id = {id} And to_account_id IS NOT NULL", (NpgsqlConnection?)cnn))
                        {

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(" Transaction id: {0}\n Transaction name: {1} \n From account: {2} \n To account: {3} \n Date/Time: {4} \n Amount: {5:N2}\n", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetDateTime(4), reader.GetDouble(5));
                                    Console.ResetColor();
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("\nYou have no more trasfeerred history in this account yet.");
                            Console.ResetColor();
                            Console.WriteLine("Press enter get back to main menu.");
                            Console.ReadLine();
                        }
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("SomThing went worong. Please try again later.");
                        Console.ResetColor();
                    }
                   



                }


            }
        }

        public static void LoanWithNormalTim_Query(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
            takingInputName:
                Console.WriteLine("Enter your Loan Type: \nPERSONAL, HOUSE, STUDENT, CAR"); // Account Type.
                string name = Console.ReadLine().ToUpper();

                if (name == "PERSONAL" || name == "HOUSE" || name == "STUDENT" || name == "CAR")
                {


                    Console.WriteLine("Enter your Interest Rate: \nPERSONAL = 2,5, HOUSE = 1,5, STUDENT = 0.5, CAR = 1,25");
                //decimal interest_rate = decimal.Parse(Console.ReadLine());
                takingInteresteInputAgain:
                    var inputInteresetRateConverted = decimal.TryParse(Console.ReadLine(), out var interest_rate);
                    if (!inputInteresetRateConverted)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nInvalid Input! Please put only spacific number.\n");
                        Console.ResetColor();
                        goto takingInteresteInputAgain;
                    }

                    Console.WriteLine("Enter your USER ID, which is existing in the Bank.");
                    int user_id = int.Parse(Console.ReadLine());

                    string postgres = "INSERT INTO bank_loan (name, interest_rate, user_id) " +
                                 "VALUES (@name, @interest_rate, @user_id)";
                    cnn.Execute(postgres, new { name, interest_rate, user_id });

                    decimal interestCalculation = 0;
                    decimal totalLoanAbleBalance = 0;

                    if (user.accounts.Count > 0)
                    {
                        decimal totalBalance = 0;


                        foreach (BankAccountModel account in user.accounts)
                        {
                            Console.WriteLine($"ID: {account.id} Account name: {account.name} Balance: {account.balance}\n");
                            decimal v = totalBalance += account.balance;
                            totalLoanAbleBalance = (v * 5);

                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Your total amount is {totalBalance}");
                        interestCalculation = totalLoanAbleBalance * (interest_rate / 100);
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"We have calculated 5 times of your total deposit in the bank {totalLoanAbleBalance} SEK.");
                    //Console.WriteLine("Your {0} loan is {1:N4} and Interest amount (per month) will {2:N4}", name, totalLoanAbleBalance, interestCalculation);
                    Console.WriteLine($"Your {name} Loan is {totalLoanAbleBalance} and Interest Amount (Yearly)will {interestCalculation:N2}.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid Input! Please follow the following NAME.");
                    Console.ResetColor();
                    goto takingInputName;
                }

            }

        }

        public static void LoanCalculation()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
            inputAccountType:
                Console.WriteLine("Enter your Loan Type number: \n1. PERSONAL(2.5%), 2.HOUSE(1.5%), 3.STUDENT(0.5%) , 4.CAR(1.25%)");

                var inputAccountTypeConverted = int.TryParse(Console.ReadLine(), out var accountType);
                if (!inputAccountTypeConverted)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nInvalid Input! Please put only spacific number.\n");
                    Console.ResetColor();
                    goto inputAccountType;
                }

                string accountName = "";
                double interestRate = 0;

                switch (accountType)
                {
                    case 1:
                        accountName = "Personal-Loan";
                        interestRate = 2.5;
                        Console.WriteLine("You have chossen Personal-Loan and It's Interest Rate is 2.5% Yearly");
                        break;

                    case 2:
                        accountName = "House-Loan";
                        interestRate = 1.5;
                        Console.WriteLine("You have chossen House-Loan and It's Interest Rate is 1.5% Yearly");
                        break;
                    case 3:
                        accountName = "Student-Loan";
                        interestRate = 0.5;
                        Console.WriteLine("You have chossen Student-Loan and It's Interest Rate is 0.5% Yearly");
                        break;
                    case 4:
                        accountName = "CAR-Loan";
                        interestRate = 1.25;
                        Console.WriteLine("You have chossen Student-Loan and It's Interest Rate is 1.25% Yearly");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid Account Type. Please press 1 - 4 number.\n");
                        Console.ResetColor();
                        goto inputAccountType;
                }

            takingBalanceInputAgain:
                Console.WriteLine("How much loan you want take?");

                var inputBalanceConverted = double.TryParse(Console.ReadLine(), out var balance);
                if (!inputBalanceConverted)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nInvalid Input! Please put only Desire Amount.\n");
                    Console.ResetColor();
                    goto takingBalanceInputAgain;
                }

                double interestCalculation = 0;

                interestCalculation = balance * (interestRate / 100);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\nYour {accountName} is {balance:N2} and Interest rate is {interestRate}% Amount(Yearly) will {interestCalculation:N2} SEK.\n");
                Console.ResetColor();
            }
        }
        public static List<BankUserModel> LoadBankUsers()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                var output = cnn.Query<BankUserModel>("select * from bank_user", new DynamicParameters());



                return output.ToList();
                cnn.Close();
            }

        }
        public static List<BankUserModel> CheckLogin(string email, string pinCode)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<BankUserModel>($"SELECT bank_user.*, bank_role.is_admin, bank_role.is_client FROM bank_user, bank_role WHERE email= '{email}' AND pin_code = '{pinCode}' AND bank_user.role_id = bank_role.id", new DynamicParameters());

                return output.ToList();

            }
        }



        public static List<BankAccountModel> GetUserAccounts(int user_id)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<BankAccountModel>($"SELECT bank_account.*, bank_currency.name AS currency_name, bank_currency.exchange_rate AS currency_exchange_rate FROM bank_account, bank_currency WHERE user_id = '{user_id}' AND bank_account.currency_id = bank_currency.id", new DynamicParameters());

                return output.ToList();
            }


        }


        public static void SaveBankUser(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into bank_users (first_name, last_name, pin_code) values (@first_name, @last_name, @pin_code)", user);

            }
        }




        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

    }
}






