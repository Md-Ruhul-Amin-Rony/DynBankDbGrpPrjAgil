using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Dapper;
using Microsoft.VisualBasic;
using Npgsql;

namespace DBTest
{
    public  class PostgresDataAccess
    {
        public static List<BankUserModel> OldLoadBankUsers()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            { 
               
                
                    cnn.Open();
                    
                


                var output = cnn.Query<BankUserModel>("SELECT * FROM bank_user", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
          
            }
            // Kopplar upp mot DB:n
            // läser ut alla Users
            // Returnerar en lista av Users
        }
        public static void CreateUsers()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {


                cnn.Open();
                Console.WriteLine(" Enter your userID:");
                int userID = int.Parse(Console.ReadLine());
                // How to detect that id is available or not ?
                //if (userID==BankUserModel.id)
                //{
                //    Console.WriteLine();

                //}
                Console.WriteLine(" Enter your First Name:"); //first_name
                string first_name = Console.ReadLine().ToLower();
                Console.WriteLine("Enter your Last Name:");
                string last_name = Console.ReadLine().ToLower();
                Console.WriteLine("select your Role Id. 1. Administrator, 2. Client, 3. ClientAdmin.\n Press in between number.");
                int role_id = int.Parse(Console.ReadLine());
                Console.WriteLine("Select your branch id between 1. Stockholm, 2. Malmö, 3. Dhaka.\n Press in between number.");
                int branch_id = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine(" Enter your desired password:");
                string desiredPassword = Console.ReadLine();

                // Prevent duplicate user to register.
                //First check if the new user exists or not.

                NpgsqlCommand insertCommand = new NpgsqlCommand("INSERT INTO bank_user(id, first_name,last_name,pin_code,role_id, branch_id) VALUES (@id, @first_name,@last_name, @pin_code,@role_id,@branch_id);", (NpgsqlConnection?)cnn);
                insertCommand.Parameters.AddWithValue("@id", userID);
                insertCommand.Parameters.AddWithValue("@first_name", first_name);
                insertCommand.Parameters.AddWithValue("@last_name", last_name);

                insertCommand.Parameters.AddWithValue("@pin_code", desiredPassword);
                insertCommand.Parameters.AddWithValue("@role_id", role_id);
                insertCommand.Parameters.AddWithValue("@branch_id", branch_id);
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("Registration successful");



               // var output = cnn.Query<BankUserModel>("SELECT * FROM bank_user", new DynamicParameters());
                //Console.WriteLine(output);
               // return output.ToList();
                cnn.Close();
            }

        }

       
        // Create Acounts method
        public static void CreateAccounts()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {


                cnn.Open();
                Console.WriteLine(" Enter your AccountID:");
                int account_id = int.Parse(Console.ReadLine());

                // it looks like account type but not account name
                //user can put even 'xyz'
                // better to use same as currency for example 1 for savings_account , 2 for salary_account
                // may be account number ??
                Console.WriteLine(" Enter your Account Type: \n Savings, Salary, ISK, Pension, Family A/C, Child A/C "); // Account Type.
                // Option will be serial number. Like 1. Saving, 2. Salary. etc.
                string account_name = Console.ReadLine().ToLower();


                Console.WriteLine("Enter your interest rate \n Savings= 1.5 Salary= 0, ISK = 5, Pension = 0.5, Family = 0.75, Child = 1.25");
                decimal interest_rate = decimal.Parse(Console.ReadLine());

                // user id should take it from logged in user.
                // no need to take input
                Console.WriteLine("Enter your bank user id:");
                int user_id = int.Parse(Console.ReadLine());


                Console.WriteLine("Select your currency id between 1 for SEK, 2 for-USD, 3 for-EUR");
                int currency_id = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine(" Enter your initial balance::");
                decimal account_balance = decimal.Parse(Console.ReadLine());




                NpgsqlCommand insertCommand = new NpgsqlCommand("INSERT INTO bank_account(id, name,interest_rate,user_id,currency_id, balance) VALUES (@id, @name,@interest_rate, @user_id,@currency_id,@balance);", (NpgsqlConnection?)cnn);
                insertCommand.Parameters.AddWithValue("@id", account_id);
                insertCommand.Parameters.AddWithValue("@name", account_name);
                insertCommand.Parameters.AddWithValue("@interest_rate", interest_rate);

                insertCommand.Parameters.AddWithValue("@user_id", user_id);
                insertCommand.Parameters.AddWithValue("@currency_id", currency_id);
                insertCommand.Parameters.AddWithValue("@balance", account_balance);
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("Registration successful");



                // var output = cnn.Query<BankUserModel>("SELECT * FROM bank_user", new DynamicParameters());
                //Console.WriteLine(output);
                // return output.ToList();
                cnn.Close();
            }

        }

        // Deposit method
        public static void deposite()
        {

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {


                cnn.Open();
                Console.WriteLine("=========================");
                Console.WriteLine("Select Your user account id:");
                int id = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter your Account Type: \n SAVINGS, SALARY, ISK, PENSION, FAMILY A/C, CHILD A/C "); // Account Type.
                string Acount_name = Console.ReadLine().ToUpper();

                Console.WriteLine("Enter your Interest Rate: \n SAVINGS = 1.5 SALARY= 0, ISK = 5, PENSION = 0.5, FAMILY A/C = 0.75, CHILD A/C = 1.25");
                double Input_interest_rate = double.Parse(Console.ReadLine());

                Console.WriteLine("Select amount to deposit:");
                double deposit_amont = double.Parse(Console.ReadLine());

                //Console.WriteLine($"You have chosen {id} which is {Acount_name} and your deposit is {deposit_amont}.");

                // Create a parameterized query to deposit money into the user's account
                double depositTotalAmount = 0;

                BankAccountModel receiver;
                var output = cnn.Query<BankAccountModel>($"SELECT bank_account.*, bank_account.name AS account_type, bank_account.interest_rate AS interest_rate FROM bank_account WHERE bank_account.id = '{id}' ", new DynamicParameters());

                receiver = output.FirstOrDefault();
                Console.WriteLine($"receiver account type is :{receiver.account_type}");
                Console.WriteLine($"receiver account interest is :{receiver.interest_rate}");

                //if (receiver.account_type == "Saving, Salary, ISK" && receiver.interest_rate == )
                if (receiver.account_type == "SAVINGS" || receiver.account_type == "SALARY" || receiver.account_type == "ISK" || receiver.account_type == "PENSION" || receiver.account_type == "FAMILY A/C" || receiver.account_type == "CHILD A/C")
                {
                    depositTotalAmount = (deposit_amont * (receiver.interest_rate / 100) / 12); // + deposit_amont;
                }
                else
                {
                    depositTotalAmount = deposit_amont;
                }
                string depositQuery = "UPDATE bank_account SET balance = balance + @balance WHERE @id = id AND @name = name";
                using (var depositCommand = new NpgsqlCommand(depositQuery, (NpgsqlConnection?)cnn))
                {
                    depositCommand.Parameters.AddWithValue("@id", id);
                    depositCommand.Parameters.AddWithValue("@name", Acount_name);
                    depositCommand.Parameters.AddWithValue("@balance", deposit_amont);

                    depositCommand.ExecuteNonQuery();
                    Console.WriteLine($"Your Deposit is {deposit_amont} and You will get interest is {depositTotalAmount} but not in the balance.");
                    Console.WriteLine($"Your Deposit is {deposit_amont} into account is {id} to account name {Acount_name} ");

                    //insert to >> bank_transactions >> history
                    NpgsqlCommand insertcommand = new NpgsqlCommand("insert into bank_transactions(transaction_name,to_account_id, timestamps,transferred_amount) values (@transaction_name, @to_account_id,@timestamps,@transferred_amount);", (NpgsqlConnection?)cnn);
                    //insertcommand.Parameters.AddWithValue("@id", fromId);
                    insertcommand.Parameters.AddWithValue("@transaction_name", "Deposit");
                    //insertcommand.Parameters.AddWithValue("@from_account_id", id);

                    insertcommand.Parameters.AddWithValue("@to_account_id", id);
                    insertcommand.Parameters.AddWithValue("@timestamps", DateAndTime.Now);
                    insertcommand.Parameters.AddWithValue("@transferred_amount", deposit_amont);

                    insertcommand.ExecuteNonQuery();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\ntransaction history successfully saved.");

                    Console.ResetColor();
                    Console.WriteLine($"Deposit successfull!");
                    
                }
                // read transaction history for deposit 



                    cnn.Close();

            }
        }


       // Read deposit history 
        public static void transforHistoryDeposit(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.WriteLine("Select Your Bank Account ID to see your Deposit history (account_id):");
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
                Console.WriteLine("=========================");
                Console.WriteLine("Select Your Bank Account 'account_id':");
                int id = int.Parse(Console.ReadLine());

                //Console.WriteLine(" Enter your Account Type: \n Savings, Salary, ISK, Pension, Family A/C, Child A/C "); // Account Type.
                //Console.WriteLine("Select Your account name:");
                //string account_Name = Console.ReadLine().ToLower();

                Console.WriteLine("Select amount to withdraw:");
                decimal withdraw_amont = decimal.Parse(Console.ReadLine());
                

                int count = 0;
                foreach (BankAccountModel item in user.accounts)
                {
                    
                    if (item.id == id ) //|| item.name == account_Name)
                    {
                        count++;
                    }
                }
                if (count==0)
                {
                    Console.WriteLine("The account information you entered is not belogs to you ");
                }
                else
                {
                    // Create a parameterized query to withdraw money into the user's account
                    string withdrawQuery = "UPDATE bank_account SET balance = balance - @balance WHERE @id = id"; // AND @name = name";
                    using (var withdrawCommand = new NpgsqlCommand(withdrawQuery, (NpgsqlConnection?)cnn))
                    {
                        withdrawCommand.Parameters.AddWithValue("@id", id);
                        //withdrawCommand.Parameters.AddWithValue("@name", account_Name);
                        withdrawCommand.Parameters.AddWithValue("@balance", withdraw_amont);

                        withdrawCommand.ExecuteNonQuery();
                        Console.WriteLine($"withdrawal {withdraw_amont} into account type {id}"); //to account name {Account_name}
                        Console.WriteLine("Withdraw successful:");

                        //insert to bank_transaction >> withdraw 
                        NpgsqlCommand insertcommand = new NpgsqlCommand("insert into bank_transactions(transaction_name,from_account_id, timestamps,transferred_amount) values (@transaction_name, @from_account_id,@timestamps,@transferred_amount);", (NpgsqlConnection?)cnn);
                        //insertcommand.Parameters.AddWithValue("@id", fromId);
                        insertcommand.Parameters.AddWithValue("@transaction_name", "Withdraw");
                        //insertcommand.Parameters.AddWithValue("@from_account_id", id);

                        insertcommand.Parameters.AddWithValue("@from_account_id", id);
                        insertcommand.Parameters.AddWithValue("@timestamps", DateAndTime.Now);
                        insertcommand.Parameters.AddWithValue("@transferred_amount", withdraw_amont);

                        insertcommand.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\ntransaction history successfully saved.");
                        Console.ResetColor();
                    }
                }

                cnn.Close();

            }
           
        }
        // Read withdrw history 
        public static void transforHistoryWithdraw(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.WriteLine("Select Your Bank Account ID to see your withdraw history (account_id):");
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
                    }
                }



            }
        }
        // Transfor in between accounts and other users accounts
        public static void Transfer(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                
                Console.WriteLine("Which A/C would you like transfer from? Please write here ID A/C Number");

                int fromId = int.Parse(Console.ReadLine());

                int count = 0;

                Console.WriteLine("How much MONEY would you like to transfer in Swedish krona (SEK)?");
                double transferMoney = double.Parse(Console.ReadLine());

                double senderTotalAmount = 0;

                foreach (BankAccountModel item in user.accounts)
                {

                    if (item.id == fromId)
                    {
                        Console.WriteLine($"your currency type is :{item.currency_name}");

                        if (item.currency_name=="USD" || item.currency_name == "EUR")
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
                    Console.WriteLine("The account information you entered is not belogs to you ");
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
                        Console.WriteLine("{0:N2} {1} has been transfer from {2} to {3}",receiverTotalAmount,receiver.currency_name,fromId,to_id);
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
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("\ntransaction history successfully saved.");

                        Console.ResetColor();
                    }

                    Console.WriteLine("0. See your transaction history. ");
                    Console.WriteLine("Press ENTER to Exit.");
                    string history = Console.ReadLine();

                    if (history == "0")
                    {
                        transforHistory(user);
                    }
                    else
                    {
                        return;
                    }
                    //old
                }
                cnn.Close();
            }

        }

        //history

        //N kristan code
        //public static List<bankTransactionsModel> GetTransactionByAccountId(int account_id)
        //{
        //    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
        //    {
        //        cnn.Open();
        //        var output = cnn.Query<bankTransactionsModel>($"SELECT * FROM bank_transactions WHERE from_account_id = {account_id} OR to_account_id = {account_id} ORDER BY timestamp DESC", new DynamicParameters());
        //        //Console.WriteLine(output);
        //        return output.ToList();
        //    }
        //    // denna funktion ska leta upp användarens konton från databas och returnera dessa som en lista
        //    // vad behöver denna funktion för information för att veta vems konto den ska hämta
        //    // vad har den för information att tillgå?
        //    // vilken typ av sql-query bör vi använda, INSERT, UPDATE eller SELECT?
        //    // ...?

        //}
        //public static bool bank_transactions(int user_id , string transaction_name, int from_account_id, int to_account_id, double transferred_amount)
        //{


        //    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
        //    {
        //        cnn.Open();
        //        string to_string_amount = transferred_amount.ToString();


        //        try
        //        {

        //            var output = cnn.Query($@"BEGIN TRANSACTION;
        //                 UPDATE bank_account SET balance = CASE 
        //                   WHEN id = {from_account_id} AND balance >= '{to_string_amount}' THEN balance - '{to_string_amount}'
        //                   WHEN id = '{to_account_id}'THEN balance + '{to_string_amount}'
        //                    END
        //                     WHERE id IN ({from_account_id}, {to_account_id})
        //                    INSERT INTO bank_transactions (transaction_name, from_account_id, to_account_id, transfferd_amount) VALUES ('överföring', {from_account_id}, {to_account_id}, '{to_string_amount}')", new DynamicParameters());
        //            //Console.WriteLine(output);

        //        }
        //        catch (Exception)
        //        {
        //            return false;

        //        }
        //        return true;


        //    }


        //}

        //old
        public static void transforHistory(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.WriteLine("Select Your Bank Account ID to see all your trasfers (account_id):");
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
                    //string query2 = $"SELECT * FROM bank_transactions WHERE to_account_id = {id}";
                    //bool query3 = true;
                   
                        using (var cmd = new NpgsqlCommand($"SELECT * FROM bank_transactions WHERE from_account_id = {id}", (NpgsqlConnection?)cnn))
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
                        }
                   
                        //{

                        //    Console.ForegroundColor = ConsoleColor.Red;
                        //    Console.WriteLine("Somthing went wrong. Please try agin later.");
                        //    Console.ResetColor();
                        //} 
                               
                            
                            
                            
                    
                    
                    // if (query3)
                    //{
                    //    using (var cmd = new NpgsqlCommand(query2, (NpgsqlConnection?)cnn))
                    //    {

                    //        using (var reader = cmd.ExecuteReader())
                    //        {
                    //            while (reader.Read())
                    //            {
                    //                Console.ForegroundColor = ConsoleColor.DarkYellow;
                    //                Console.WriteLine(" Transaction id: {0}\n Transaction name: {1}  \n To account: {2} \n Amount: {3:N2}\n", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDouble(3));
                    //                Console.ResetColor();
                    //            }
                    //        }
                    //    }
                    //}
                   
                }



            }
        }
            // Making withdraw function:
            //public static void Withdraw()
            //{

            //    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            //    {


            //        cnn.Open();
            //        Console.WriteLine("=========================");
            //        Console.WriteLine("Select Your user account id:");
            //        int id = int.Parse(Console.ReadLine().ToLower());
            //        Console.WriteLine("Select Your account user Id:");
            //        string Acount_userid = Console.ReadLine().ToLower();
            //        Console.WriteLine("Select amount to deposit:");
            //        decimal withdraw_amont = decimal.Parse(Console.ReadLine().ToLower());

            //        // Create a parameterized query to deposit money into the user's account
            //        string depositQuery = "UPDATE bank_account SET balance = (balance - @depositAmount) WHERE @id = @id AND @user_id =@user_id";
            //        if (true)
            //        {

            //        }
            //        using (var depositCommand = new NpgsqlCommand(depositQuery, (NpgsqlConnection?)cnn))
            //        {
            //            depositCommand.Parameters.AddWithValue("@id", id);
            //            depositCommand.Parameters.AddWithValue("@user_id", Acount_userid);
            //            depositCommand.Parameters.AddWithValue("@depositAmount", withdraw_amont);

            //            depositCommand.ExecuteNonQuery();
            //            Console.WriteLine($"deposited {withdraw_amont} into account for user {id} to account user Id {Acount_userid}");
            //        }

            //        //            UPDATE accounts SET balance = balance - 100.00
            //        //WHERE name = 'Alice';
            //        //            UPDATE branches SET balance = balance - 100.00
            //        //WHERE name = (SELECT branch_name FROM accounts WHERE name = 'Alice');
            //        //            UPDATE accounts SET balance = balance + 100.00
            //        //WHERE name = 'Bob';
            //        //            UPDATE branches SET balance = balance + 100.00
            //        //WHERE name = (SELECT branch_name FROM accounts WHERE name = 'Bob');

            //        cnn.Close();

            //    }

            //}

            //public static void Transfer()
            //{
            //    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            //    {
            //        cnn.Open();
            //        Console.WriteLine("=========================");
            //        Console.WriteLine("Select Your user account id to transfer from:");
            //        int from_id = int.Parse(Console.ReadLine());
            //        Console.WriteLine("Select Your account user Id to transfer to:");
            //        int to_id = int.Parse(Console.ReadLine());
            //        Console.WriteLine("Select amount to transfer:");
            //        decimal trans_amount = decimal.Parse(Console.ReadLine());

            //        using (var transaction = cnn.BeginTransaction())
            //        {
            //            try
            //            {
            //                string transferQuery = "UPDATE bank_account SET balance = balance - @amount WHERE id = @from_id; " +
            //                                       "UPDATE bank_account SET balance = balance + @amount WHERE id = @to_id";
            //                using (var transferCommand = new NpgsqlCommand(transferQuery, (NpgsqlConnection?)cnn))
            //                {
            //                    transferCommand.Parameters.AddWithValue("@from_id", from_id);
            //                    transferCommand.Parameters.AddWithValue("@to_id", to_id);
            //                    transferCommand.Parameters.AddWithValue("@amount", trans_amount);
            //                    transferCommand.ExecuteNonQuery();
            //                }
            //                transaction.Commit();
            //                Console.WriteLine("Transaction Successful!");
            //            }
            //            catch (Exception ex)
            //            {
            //                transaction.Rollback();
            //                Console.WriteLine("Transaction Failed: " + ex.Message);
            //            }
            //        }

            //        cnn.Close();
            //    }
            //}


        public static List<BankUserModel> LoadBankUsers()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<BankUserModel>("select * from bank_user", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
            // Kopplar upp mot DB:n
            // läser ut alla Users
            // Returnerar en lista av Users
        }
        public static List<BankUserModel> CheckLogin(string firstName, string pinCode)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            { 
                var output = cnn.Query<BankUserModel>($"SELECT bank_user.*, bank_role.is_admin, bank_role.is_client FROM bank_user, bank_role WHERE first_name = '{firstName}' AND pin_code = '{pinCode}' AND bank_user.role_id = bank_role.id", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
                //return output.FirstOrDefault();
            }
            // Kopplar upp mot DB:n
            // läser ut alla Users
            // Returnerar en lista av Users
        }

        //public static List<BankAccountModel> checkId(int id , string bank_account, )
        //{
        //    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
        //    {

        //        //var output = cnn.Query<BankUserModel>($"SELECT bank_user.*, bank_role.is_admin, bank_role.is_client FROM bank_user, bank_role WHERE first_name = '{firstName}' AND pin_code = '{pinCode}' AND bank_user.role_id = bank_role.id", new DynamicParameters());
        //        //Console.WriteLine(output);
        //        var output = cnn.Query<BankUserModel>($"SELECT bank_account.* new DynamicParameters());
        //        //Console.WriteLine(output);
        //        return output.ToList();
        //    }
        //    // Kopplar upp mot DB:n
        //    // läser ut alla Users
        //    // Returnerar en lista av Users
        //}
        public static List<BankAccountModel> GetUserAccounts(int user_id)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<BankAccountModel>($"SELECT bank_account.*, bank_currency.name AS currency_name, bank_currency.exchange_rate AS currency_exchange_rate FROM bank_account, bank_currency WHERE user_id = '{user_id}' AND bank_account.currency_id = bank_currency.id", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();
            }
            // denna funktion ska leta upp användarens konton från databas och returnera dessa som en lista
            // vad behöver denna funktion för information för att veta vems konto den ska hämta
            // vad har den för information att tillgå?
            // vilken typ av sql-query bör vi använda, INSERT, UPDATE eller SELECT?
            // ...?

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



