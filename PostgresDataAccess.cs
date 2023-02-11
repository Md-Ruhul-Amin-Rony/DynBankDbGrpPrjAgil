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
                    Console.WriteLine($"We have calculated 5 times of your total deposit in the bank {totalLoanAbleBalance}");
                    //Console.WriteLine("Your {0} loan is {1:N4} and Interest amount (per month) will {2:N4}", name, totalLoanAbleBalance, interestCalculation);
                    Console.WriteLine($"Your {name} Loan is {totalLoanAbleBalance} and Interest Amount (per month)will {interestCalculation:N2}.");
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



