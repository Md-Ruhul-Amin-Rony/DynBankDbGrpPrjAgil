using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
                cnn.Close();
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
                Console.WriteLine(" Enter your desired username:");
                string first_name = Console.ReadLine().ToLower();
                Console.WriteLine("Enter your last name:");
                string last_name = Console.ReadLine();
                Console.WriteLine("select your role id between 1-3");
                int role_id = int.Parse(Console.ReadLine());
                Console.WriteLine("Select your branch id between 1-3");
                int branch_id = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine(" Enter your desired password:");
                string desiredPassword = Console.ReadLine();


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
        public static void CreateAccounts()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {


                cnn.Open();
                Console.WriteLine(" Enter your AccountID:");
                int account_id = int.Parse(Console.ReadLine());
                
                Console.WriteLine(" Enter your desired username:");
                string account_name = Console.ReadLine().ToLower();
                Console.WriteLine("Enter your interest rate (savings= 1.5 and salary= 0 )");
                decimal interest_rate = decimal.Parse(Console.ReadLine());
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
                int id = int.Parse(Console.ReadLine().ToLower());
               
                Console.WriteLine("Select Your account name:");
                string Acount_name = Console.ReadLine().ToLower();
                Console.WriteLine("Select amount to deposit:");
                decimal deposit_amont = decimal.Parse(Console.ReadLine().ToLower());
               
                // Create a parameterized query to deposit money into the user's account
                string depositQuery = "UPDATE bank_account SET balance = balance + @balance WHERE @id = id AND @name = name";
                using (var depositCommand = new NpgsqlCommand(depositQuery, (NpgsqlConnection?)cnn))
                {
                    depositCommand.Parameters.AddWithValue("@id", id);
                    depositCommand.Parameters.AddWithValue("@name", Acount_name);
                    depositCommand.Parameters.AddWithValue("@balance", deposit_amont);
                    
                    depositCommand.ExecuteNonQuery();
                    Console.WriteLine($"deposited {deposit_amont} into account for user {id} to account name {Acount_name} ");
                }

    //            UPDATE accounts SET balance = balance - 100.00
    //WHERE name = 'Alice';
    //            UPDATE branches SET balance = balance - 100.00
    //WHERE name = (SELECT branch_name FROM accounts WHERE name = 'Alice');
    //            UPDATE accounts SET balance = balance + 100.00
    //WHERE name = 'Bob';
    //            UPDATE branches SET balance = balance + 100.00
    //WHERE name = (SELECT branch_name FROM accounts WHERE name = 'Bob');

                cnn.Close();

            }
        }

        //Withdraw mathod

        public static void withdraw()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.WriteLine("=========================");
                Console.WriteLine("Select Your user account id:");
                int id = int.Parse(Console.ReadLine().ToLower());

                Console.WriteLine("Select Your account name:");
                string Acount_name = Console.ReadLine().ToLower();
                Console.WriteLine("Select amount to withdraw:");
                decimal withdraw_amont = decimal.Parse(Console.ReadLine().ToLower());

                // Create a parameterized query to withdraw money into the user's account
                string withdrawQuery = "UPDATE bank_account SET balance = balance - @balance WHERE @id = id AND @name = name";
                using (var withdrawCommand = new NpgsqlCommand(withdrawQuery, (NpgsqlConnection?)cnn))
                {
                    withdrawCommand.Parameters.AddWithValue("@id", id);
                    withdrawCommand.Parameters.AddWithValue("@name", Acount_name);
                    withdrawCommand.Parameters.AddWithValue("@balance", withdraw_amont);

                    withdrawCommand.ExecuteNonQuery();
                    Console.WriteLine($"withdrawal {withdraw_amont} into account for user {id} to account name {Acount_name}");
                }

                cnn.Close();

            }
        }

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



