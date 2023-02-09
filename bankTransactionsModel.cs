using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest
{
    public class bankTransactionsModel
    {
        public int id { get; set; }
        public string transaction_name { get; set; }
        public string from_account_id { get; set; }
        public int to_account_id { get; set; }
        public double transferred_amount { get; set; }
        public string get_signed(int acount_id)
        {
            if (acount_id.ToString() == from_account_id)
            {
                return $" -{transferred_amount}"; 
            }
            return transferred_amount.ToString();
        } 
    }
}
