using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using System.Globalization;
using System.Linq;


using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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








        public static bool TransferMoney()
        {

            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                Console.WriteLine("Enter user_id: ");
                int user_id = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter from_account_id: ");
                int from_account_id = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter to_account_id: ");
                int to_account_id = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());





                string newAmount = amount.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
                try
                {

                    var output = cnn.Query($@"
                    BEGIN TRANSACTION;
                    UPDATE bank_account SET balance = CASE
                       WHEN id = {from_account_id} AND balance >= '{newAmount}' THEN balance - '{newAmount}'
                       WHEN id = {to_account_id} THEN balance + '{newAmount}'
                    END
                    WHERE id IN ({from_account_id}, {to_account_id});
                    INSERT INTO bank_transaction (name, from_account_id, to_account_id, amount) VALUES ('Överföring', {from_account_id}, {to_account_id}, '{newAmount}');
                    COMMIT;
                ", new DynamicParameters());

                    Console.WriteLine("Money transferred successfully.");


                }
                catch (Npgsql.PostgresException e)
                {

                    return false;
                }
                return true;

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
                    //cnn.Open();
                    //Console.WriteLine("Enter your First Name:");
                    //string first_name = Console.ReadLine().ToLower();
                    //Console.WriteLine("Enter your Last Name:");
                    //string last_name = Console.ReadLine().ToLower();
                    
                    
                    //    Console.WriteLine("Select your Role Id. 1. Administrator, 2. Client, 3. ClientAdmin.\n Press in between number.");
                    //    int role_id = int.Parse(Console.ReadLine());
                    //    Console.WriteLine("Select your branch id between 1. Koala, 2. Owl, 3. Panda, 4. Fox, 5.Squid , 6. Lion, 7.Rabbit, 8. Tiger.\n Press in between number.");
                    //    int branch_id = Convert.ToInt32(Console.ReadLine());

                    
                    
                   
                   
                    //Console.WriteLine("Enter your email:");
                    //string email = Console.ReadLine();
                    //Console.WriteLine("Enter your desired password:");
                    //string pin_code = Console.ReadLine();
                    //string check = "SELECT COUNT(*) FROM bank_user WHERE email = @email";
                    //int count = cnn.ExecuteScalar<int>(check, new { email });
                    //if (count > 0)
                    //{
                    //    Console.WriteLine("The email address is already in use.");
                    //    return;
                    //}
                    //string sql = "INSERT INTO bank_user (first_name, last_name, email, pin_code, role_id, branch_id) " +
                    //             "VALUES (@first_name, @last_name, @email, @pin_code, @role_id, @branch_id)";
                    //cnn.Execute(sql, new { first_name, last_name, email, pin_code, role_id, branch_id });
                    //Console.WriteLine("New user created successfully!");
                    //cnn.Close();
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


        public static void Deposit(BankUserModel user)
        {



            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                cnn.Open();
                Console.WriteLine("=========================");
                Console.WriteLine("Select Your user account id:");
                int id = int.Parse(Console.ReadLine());


                Console.WriteLine("Select amount to deposit:");
                decimal deposit_amont = decimal.Parse(Console.ReadLine());


                string depositQuery = "UPDATE bank_account SET balance = balance + @balance WHERE @id = id";
                using (var depositCommand = new NpgsqlCommand(depositQuery, (NpgsqlConnection?)cnn))
                {
                    depositCommand.Parameters.AddWithValue("@id", id);

                    depositCommand.Parameters.AddWithValue("@balance", deposit_amont);

                    depositCommand.ExecuteNonQuery();
                    Console.WriteLine($"deposited {deposit_amont} into account type {id}");
                }



                Console.WriteLine("Select amount to deposit:");
                decimal deposit_amount = decimal.Parse(Console.ReadLine());


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
                    Console.WriteLine("The account information you entered is not belongs to you ");
                }
                else
                {



                    using (var depositCommand = new NpgsqlCommand(depositQuery, (NpgsqlConnection?)cnn))
                    {
                        depositCommand.Parameters.AddWithValue("@id", id);

                        depositCommand.Parameters.AddWithValue("@amount", deposit_amount);


                        depositCommand.ExecuteNonQuery();
                        Console.WriteLine($"Deposit {deposit_amount} into account id: {id} and account email is:{user.email}");
                        Console.WriteLine("Deposit successful:");
                    }

                }
                cnn.Close();
            }
        }

        public static void Withdraw(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {



                Console.WriteLine("Select Your Bank Account 'account_id':");
                int id = int.Parse(Console.ReadLine());

                Console.WriteLine("Select amount to withdraw:");
                decimal withdraw_amount = decimal.Parse(Console.ReadLine());


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
                    Console.WriteLine("The account information you entered is not belogs to you ");
                }
                else
                {

                    string withdrawQuery = "UPDATE bank_account SET balance = balance - @balance WHERE @id = id";
                    using (var withdrawCommand = new NpgsqlCommand(withdrawQuery, (NpgsqlConnection?)cnn))
                    {
                        withdrawCommand.Parameters.AddWithValue("@id", id);

                        withdrawCommand.Parameters.AddWithValue("@balance", withdraw_amount);

                        withdrawCommand.ExecuteNonQuery();
                        Console.WriteLine($"withdrawal {withdraw_amount} into account type {id}");
                        Console.WriteLine("Withdraw successful:");
                    }


                    cnn.Open();
                    string checkSql = "SELECT balance FROM bank_account WHERE id = @id";
                    decimal currentBalance = cnn.Query<decimal>(checkSql, new { id }).SingleOrDefault();
                    if (withdraw_amount > currentBalance)
                    {
                        Console.WriteLine("Insufficient balance");
                        return;

                    }
                    string updateSql = "UPDATE bank_account SET balance = balance - @withdraw_amount WHERE id = @id";
                    cnn.Execute(updateSql, new { withdraw_amount, id });
                    Console.WriteLine("Withdraw successful:");
                    cnn.Close();



                }
            }
        }


        //public static void Transfer(BankUserModel user)
        //{
        //    using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
        //    {
        //        cnn.Open();

        //        Console.WriteLine("Which A/C would you like transfer from? Please write here ID A/C Number");

        //        int fromId = int.Parse(Console.ReadLine());

        //        int count = 0;

        //        Console.WriteLine("How much MONEY would you like to transfer in Swedish krona (SEK)?");
        //        decimal transferMoney = decimal.Parse(Console.ReadLine());

        //        decimal senderTotalAmount = 0;

               

        //            decimal interestCalculation = 0;
        //            decimal totalLoanAbleBalance = 0;

        //            if (user.accounts.Count > 0)
        //            {

        //                Console.WriteLine($"your currency type is :{item.currency_id}");

        //                if (item.currency_id == "USD" || item.currency_id == "EUR")
        //                {
        //                    senderTotalAmount = transferMoney;
        //                }
        //                else

        //                {
        //                    Console.WriteLine($"ID: {account.id} Account name: {account.name} Balance: {account.balance}\n");
        //                    decimal v = totalBalance += account.balance;
        //                    totalLoanAbleBalance = (v * 5);

        //                }

        //                Console.ForegroundColor = ConsoleColor.Green;
        //                Console.WriteLine($"Your total amount is {totalBalance}");
        //                interestCalculation = totalLoanAbleBalance * (interest_rate / 100);
        //                Console.ResetColor();
        //            }

            
        //        if (count == 0)
        //        {
        //            Console.WriteLine("The account information you entered is not belongs to you ");
        //        }
        //        else
        //        {

        //            string transferQuery = "UPDATE bank_account SET balance = balance - @balance WHERE @id = id";

        //            using (var transferCommand = new NpgsqlCommand(transferQuery, (NpgsqlConnection?)cnn))
        //            {
        //                transferCommand.Parameters.AddWithValue("@id", fromId);
        //                transferCommand.Parameters.AddWithValue("@balance", senderTotalAmount);

        //                transferCommand.ExecuteNonQuery();

        //            }


        //            Console.WriteLine("Which A/C to transfer?  Write down your A/C serial Number");
        //            int to_id = int.Parse(Console.ReadLine());

        //            decimal receiverTotalAmount = 0;

        //            BankAccountModel receiver;
        //            var output = cnn.Query<BankAccountModel>($"SELECT *, bank_currency.name AS currency_name, bank_currency.exchange_rate AS currency_exchange_rate FROM bank_account, bank_currency WHERE bank_account.id = '{to_id}' AND bank_account.currency_id = bank_currency.id", new DynamicParameters());
        //            receiver = output.FirstOrDefault();
        //            Console.WriteLine($"receiver account currency type is :{receiver.name}");



        //            transferQuery = "UPDATE bank_account SET balance = balance + @balance WHERE @id = id";

        //            using (var transferCommand = new NpgsqlCommand(transferQuery, (NpgsqlConnection?)cnn))
        //            {
        //                transferCommand.Parameters.AddWithValue("@id", to_id);
        //                transferCommand.Parameters.AddWithValue("@balance", receiverTotalAmount);

        //                transferCommand.ExecuteNonQuery();

        //                Console.WriteLine("Transsfer succeeded");

        //            }


        //        }

        //    }

        //}

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








        //public static void Withdraw(BankUserModel user)
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



        //        cnn.Close();

        //    }

        //}

        

        public static void TransactionHistory(BankUserModel user)
        {
            using (IDbConnection cnn = new NpgsqlConnection(LoadConnectionString()))
            {
                Console.WriteLine("=========================");
                Console.WriteLine("Enter your user id:");
                int id = Convert.ToInt32(Console.ReadLine());

                string userQuery = "SELECT * FROM bank_user WHERE id = @id";
                using (var userCommand = new NpgsqlCommand(userQuery, (NpgsqlConnection?)cnn))
                {
                    userCommand.Parameters.AddWithValue("@id", id);
                    cnn.Open();
                    NpgsqlDataReader userReader = userCommand.ExecuteReader();
                    List<int> accountIds = new List<int>();
                    while (userReader.Read())
                    {
                        accountIds.Add(userReader.GetInt32(0));
                    }
                    cnn.Close();

                    if (accountIds.Count > 0)
                    {
                        string transactionQuery = "SELECT * FROM bank_transactions WHERE from_account_id = @account_id OR to_account_id = @account_id";
                        foreach (int accountId in accountIds)
                        {
                            using (IDbConnection cnn2 = new NpgsqlConnection(LoadConnectionString()))
                            {
                                using (var transactionCommand = new NpgsqlCommand(transactionQuery, (NpgsqlConnection?)cnn2))
                                {
                                    transactionCommand.Parameters.AddWithValue("@account_id", accountId);
                                    cnn2.Open();
                                    NpgsqlDataReader transactionReader = transactionCommand.ExecuteReader();
                                    while (transactionReader.Read())
                                    {
                                        Console.WriteLine("Transaction ID: " + transactionReader.GetInt32(0));
                                        Console.WriteLine("Transaction Name: " + transactionReader.GetString(1));
                                        Console.WriteLine("From Account ID: " + transactionReader.GetInt32(2));
                                        Console.WriteLine("To Account ID: " + transactionReader.GetInt32(3));
                                        Console.WriteLine("Timestamp: " + transactionReader.GetDateTime(4));
                                        Console.WriteLine("Transferred Amount: " + transactionReader.GetDecimal(5));
                                        Console.WriteLine("=========================");
                                    }
                                    cnn2.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No transactions found for the user with this id: " + id);
                    }
                }
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



        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

    }
}






