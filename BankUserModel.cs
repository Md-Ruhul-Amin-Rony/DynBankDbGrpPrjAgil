using System;
namespace DBTest
{
	public class BankUserModel
	{
		public int id { get; set; }

		public string first_name { get; set; }

		public string last_name { get; set; }
		
		public string email { get; set; }

		public string pin_code { get; set; }

		public int role_id { get; set; }

		public int branch_id { get; set; }

		public bool is_admin { get; set; }

		public bool is_client { get; set; }

		
        private DateTime accounts_timestamp;

        public List<BankAccountModel> accounts { get; set; }

        public List<BankAccountModel> GetAccounts(bool immediate = false)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - accounts_timestamp.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);
            //Console.WriteLine("delta: " + delta);
            //accounts_timestamp = DateTime.UtcNow;
            if (delta > 20 | immediate)
            {
                //Console.WriteLine("Cache expired");
                accounts_timestamp = DateTime.UtcNow;
                accounts = PostgresDataAccess.GetUserAccounts(id);
                return accounts;
            }
            return accounts;
        }
    }
}

