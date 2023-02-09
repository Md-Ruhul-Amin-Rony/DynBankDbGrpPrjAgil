using System;
namespace DBTest
{
	public class BankAccountModel
	{
		public int id { get; set; }

		public string name { get; set; }

		public decimal balance { get; set; }

        public string account_type { get; set; }

        public double interest_rate { get; set; }

		public string currency_name { get; set; }

		public double currency_exchange_rate { get; set; }

        //private DateTime transactions_timestamp;
        //public List<bankTransactionsModel> transactions { get; set; }

        //public List<bankTransactionsModel> GetTransactionsByAccountId(int account_id, bool immediate = false)
        //{
        //    var ts = new TimeSpan(DateTime.UtcNow.Ticks - transactions_timestamp.Ticks);
        //    double delta = Math.Abs(ts.TotalSeconds);
        //    //Console.WriteLine("delta: " + delta);
        //    //accounts_timestamp = DateTime.UtcNow;
        //    if (delta > 25 | immediate)
        //    {
        //        //Console.WriteLine("Cache expired");
        //        transactions_timestamp = DateTime.UtcNow;
        //        transactions = PostgresDataAccess.GetTransactionByAccountId(account_id);
        //        return transactions;
        //    }
        //    return transactions;
        //}
    }
}

