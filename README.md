

   ![Alt Text](             https://github.com/Md-Ruhul-Amin-Rony/DynBankDbGrpPrjAgil/blob/ee31f072b0380e5e2afefad20c2835f7e12f7017/gif%20branch/S1hnYM98.gif)

   ![Alt Text](                            https://github.com/Md-Ruhul-Amin-Rony/DynBankDbGrpPrjAgil/blob/newReaz/gif%20branch/S1hnYM98.gif)

___
> *Welcome to our simple banking application! This banking application is a school group project in **C#** and **Postgres database**. This project made in Visual Studio and it is a C# console application. The main target user of this group project was to learn OOP in C# and learn how to connect and perform read/write operations to the database. We have used three different connections and much understandable logics witch may help you much in future and not facing same problem as we did during the project.* 


## More Details:

> This project is a fully functional bank system that has been implemented in a C# console application. This bank system has Admin section where admin can create user and even admin can be the user of the banksystem also. Beside this there is a client or user section where its allows user to create accounts with different currencies, make deposits and withdrawals, transfer funds between accounts and between different user, and view account balances and with currency conversion. Bank System has a Loan section where user can take a loan about 5 times on their original balance. If ueser want to take loan more than 5 times of original amount then loan will not get approved. In Loan section user will able to see the interest rate respectively.



## Features
Our dynamic bank system has several features that make it stand out:

Customizable Account Types: Our bank system allows users to create customized account types, such as 1.Savings, 2.Salary, 3.ISK, 4.Pension, 5.Family A/C, 6.Child A/C. This allows users to have greater control over their finances and better manage their money. When user will open a savings account they will be informed about their interest rate which they will get yearly.

Flexible Transaction Options: User can make transactions in a variety of ways, including online banking, mobile banking, and ATM transactions. Our system is designed to be flexible and convenient, allowing users to access their accounts whenever and wherever they need to. When user will transfer amount from one account to another acccount which has different cuurency, our system will convert it automatically.
Real-Time Account Balances: Our bank system provides user with real-time updates on their account balances, so they can always stay up-to-date on their financial status. For exception handling we have used TryPrase() and Try Catch().
___

## Features which is not completed yet:

- Our project has been done by SIMPLE INTEREST RATE. But we havn't done COMPOUND INTEREST Rate on our project.
- The user should be able to get a menu and chose if he/she wants to **transfer money** between him/her accounts or to other users accounts. And your are able to this but yout don't get the menu to chose between accounts.
___

## Bugs which is not fixed yet:
- It is posible to Transfer, deposit, withdraw a negetive amount. witch is in progress to being fixed.

___

## Demo:
___
<img src="https://user-images.githubusercontent.com/113901667/220205024-6360162d-c51c-4b85-83c2-78083be904ed.gif">




## Using the App:
> To use the bank system, follow these steps:
Open the application and log in as administrator and select "Create User" to create a new user.
Open the application and select "Create Account" to create a new account.
Chose the account type and enter the account details, and the account id Will be generated automatically and put your initial balance.
Use the "Deposit" and "Withdraw" options to make transactions on the account.
Use the "Transfer" option to transfer funds from one account to another.
Use the "See Accounts" option to view the current accounts with balance of the user.
Use the "View Transaction History" option to view the history of transactions on the account.
Use the "Loan section" option to view the all functionality of our Loan Department.

## Technology stack:
- C# 
- Postgress database
- .NET 6

# Exception handling
Handling of exception is important to prevent crashing the program and how to deal with unexpected secenarios.

```
TryPrase()
```

```
try 
{
  //  Block of code to try
}
catch (Exception e)
{
  //  Block of code to handle errors
}
```
___

## Reflection
We have faced some problem during the project which are teaches us a lot. { get; set; } was the vital role playing method in this project where OOP(Object Orienting Programming) and OOAD(Object Orienting Analysis and Design)logic help us to develop this dynamic Bank application. We have used some CLASSES like BankAccountModel, BankTransactionModel, BankUserModel, UserModel etc. User can input data on console where console will through data till PostgreSQL Database to store. Without authentication user unable to use our secure database. Even though 3 times failed logging our system will block that user for 3 minutes or more.
 
## Key Word:
C#, { get; set; }, data-oriented, using Npgsql, using NpgsqlTypes, using Dapper,
___

## Geting started

> To run this project, you will need to have the following Software Installed on your machine:

-.NET Core SDK
-Visual Studio or another code editor which will support C#.
> Once you have installed these dependencies, you can follow these steps to run the project:
-Clone the repository to your local machine.
-Open the project in Visual Studio Code or another code editor.
-Build and run the project.
___


## Why this way of creating a bank system with classes and using C#, Postgres is a good ?

1 - Object-oriented design: Using classes to represent user and accounts which are taken from the database it allows for a clear and organized structure of the system. It makes it easy to understand the different components of the system and how they relate to each other.

2 - Reusability: All classes can be reused and create multiple object of them, allowing for the representation of multiple users, accounts and transactions in the system.

3 - The use of properties with public GETTERS and SETTERS makes it easy to access the properties of the objects. It also makes it easy to add or remove properties in the future without affecting the rest of the system.

4 - The nested Account class within the User class is logical and easy to understand, it represents the accounts that a user has.

5 -The structure uses basic concepts of Object-Oriented programming, which makes it easy to implement and understand.

___




> The project described above is designed and implemented by students of Chas Academy which is functional but not production ready solution. During the project we have performed Object oriented Analysis and Design (OOAD) approach to analysis the requirements and design the application. In addition to that, we have followed agile methodologies during project work.

We have tried to build a 3-layer application by separating UI, Business logic and data access layer. Our application is built in a way that can be extend in future. There are still lot of scope available to improve in future, for example we can work with cache data to improve performance as well as data validation and security.  

## UML Design:
<img src="https://user-images.githubusercontent.com/113901667/220205448-97382726-3632-4b21-84ee-07a029d7be98.jpeg"  height="500">




___

## Kanban Board:
- [Kanban Board](https://trello.com/b/mv1IUL0B/kanban-broard)

## Collaborators who worked in this project:
   - [Ruhul Amin](https://github.com/Md-Ruhul-Amin-Rony)
   - [Reza Eskandar](https://github.com/Rezaeskandar/ATM)
   - [MK Hasan](https://github.com/chasmkhasan)
   - [Tasmia Zahin](https://github.com/tasmiazahin)
   - [Suhagan Mostahid](https://github.com/suhagan)
___
## Contributing
> If you would like to contribute to this project, please fork the repository and submit a pull request with your changes. We welcome all contributions, including bug fixes and new features. 
> 

