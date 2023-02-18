using System;
namespace DBTest
{
	public class BankAccountModel
	{
		public int id { get; set; }

		public string name { get; set; }

		public decimal balance { get; set; }
		
		public double interest_rate { get; set; }
       
        public int user_id { get; set; }

        public string currency_name { get; set; }
        public int currency_id { get; set; }
        public int currency_id { get; set; }
        public double currency_exchange_rate { get; set; }

        private DateTime transactions_timestamp;


        public List<BankTransactionModel> transactions { get; set; }

        public List<BankTransactionModel> GetTransactionsByAccountId( bool immediate = false)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - transactions_timestamp.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);
            //Console.WriteLine("delta: " + delta);
            //accounts_timestamp = DateTime.UtcNow;
            if (delta > 25 | immediate)
            {
                //Console.WriteLine("Cache expired");
                transactions_timestamp = DateTime.UtcNow;
                transactions = PostgresDataAccess.GetTransactionByAccountId();
                return transactions;
            }
            return transactions;
        }
    }
    //public double currency_exchange_rate { get; set; }

		

}


