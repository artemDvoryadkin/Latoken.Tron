using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TronToken.BussinesLogic.Model.TronModel
{
    public class ResultSearchTransactionModel
    {
        public long Total { get; set; }
        public List<TransactionModel> Data {get;set;}
    }
}
