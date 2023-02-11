using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest
{
    public class BankTransactionModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public int from_account_id { get; set; }

        public int to_account_id { get; set; }

        public decimal amount { get; set; }

        public string GetSignedAmount(int account_id)
        {
            if (account_id == from_account_id)
            {
                return $"-{amount}";
            }
            return amount.ToString();
        }
    }
}
