using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
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
                
                  //  cnn.Open();
               
                var output = cnn.Query<BankUserModel>("SELECT * FROM bank_user", new DynamicParameters());
                //var output = cnn.Query<BankUserModel>("SELECT * FROM users", new DynamicParameters());
                //Console.WriteLine(output);
                return output.ToList();

            }
            // Kopplar upp mot DB:n
            // läser ut alla Users
            // Returnerar en lista av Users
        }
       

       
     

        public static void LoanCalculation()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {


                takingInputName:
                Console.WriteLine("Enter your Loan Type: \nPERSONAL, HOUSE, STUDENT, CAR"); // Account Type.
                string name = Console.ReadLine().ToUpper();

                if (name == "PERSONAL" || name == "HOUSE" || name == "STUDENT" || name == "CAR")
                {

                    takingInterestRateInputAgain:
                    Console.WriteLine("Enter your Interest Rate: \nPERSONAL = 2,5, HOUSE = 1,5, STUDENT = 0.5, CAR = 1,25");
                    //double interest_rate = double.Parse(Console.ReadLine());

                    var inputInteresetRateConverted = double.TryParse(Console.ReadLine(), out var interest_rate);
                    if (!inputInteresetRateConverted)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nInvalid Input! Please put only spacific number.\n");
                        Console.ResetColor();
                        goto takingInterestRateInputAgain;
                    }

                    takingBalanceInputAgain:
                    Console.WriteLine("How much loan you want take?");
                    //double balance = double.Parse(Console.ReadLine());

                    var inputBalanceConverted = double.TryParse(Console.ReadLine(), out var balance);
                    if (!inputBalanceConverted)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nInvalid Input! Please put only spacific number.\n");
                        Console.ResetColor();
                        goto takingBalanceInputAgain;
                    }

                    double interestCalculation = 0;

                    if (name == "PERSONAL" || name == "HOUSE" || name == "STUDENT" || name == "CAR")
                    {
                        interestCalculation = balance * (interest_rate / 100) / 12;
                    }
                    else
                        return;

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Your {name} Loan is {balance} and Interest Amount(Per Month) is {interestCalculation}.");
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

        public static void Loan(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                
                takingInputName:    
                Console.WriteLine("Enter your Loan Type: \nPERSONAL, HOUSE, STUDENT, CAR"); // Account Type.
                string name = Console.ReadLine().ToUpper();

                if (name == "PERSONAL" || name == "HOUSE" || name == "STUDENT" || name == "CAR")
                {
                    Console.WriteLine("Enter your Interest Rate: \nPERSONAL = 2,5, HOUSE = 1,5, STUDENT = 0.5, CAR = 1,25");
                    //decimal inputInteresetRateConverted = decimal.Parse(Console.ReadLine());
                    takingInteresteInputAgain:
                    var inputInteresetRateConverted = decimal.TryParse(Console.ReadLine(), out var interest_rate);
                    if (!inputInteresetRateConverted)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nInvalid Input! Please put only spacific number.\n");
                        Console.ResetColor();
                        goto takingInteresteInputAgain;
                    }

                    decimal interestCalculation = 0;
                    decimal interestCalculationYear = 0;
                    decimal interestCalculationMoreYear = 0;
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

                        Console.WriteLine($"\nYour total deposit in Lion's Bank are {totalBalance}\n");

                        interestCalculation = totalLoanAbleBalance * (interest_rate / 100) / 12;
                        //interestCalculationYear = totalLoanAbleBalance * (interest_rate / 100);
                        //interestCalculationMoreYear = totalLoanAbleBalance * ((interest_rate / 100) * 5);
                    }

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("We have calculated 5 times of your total deposit in the bank.");
                    Console.WriteLine($"Your {name} Loan is {totalLoanAbleBalance} and Interest Amount(Per Month) will {interestCalculation}"); // \nInterest Amount(One Year) will {interestCalculationYear} \nInterest Amount(Five Year) will {interestCalculationMoreYear}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid Input! Please follow the following NAME.");
                    Console.ResetColor();
                    goto takingInputName;
                }

                cnn.Close();
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

                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine($"Your total amount is {totalBalance}");
                        interestCalculation = totalLoanAbleBalance * (interest_rate / 100) / 12;
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("We have calculated 5 times of your total deposit in the bank.");
                    Console.WriteLine($"Your {name} Loan is {totalLoanAbleBalance} and Interest Amount (per month)will {interestCalculation}.");
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


        public static List<BankUserModel> LoadBankUsers()
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {

                var output = cnn.Query<BankUserModel>("select * from bank_user", new DynamicParameters());
                //var output = cnn.Query<BankUserModel>("select * from users", new DynamicParameters());
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

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

    }
}



