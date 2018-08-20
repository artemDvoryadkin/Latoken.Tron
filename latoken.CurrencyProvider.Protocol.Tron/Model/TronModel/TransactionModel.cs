using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TronToken.BussinesLogic.Model.TronModel
{
	public class TransactionModel
	{
		public String Id { get; set; }
		public String TransactionHash { get; set; }
		public int Block { get; set; }
		public Int64 Timestamp { get; set; }
		public String TransferFromAddress { get; set; }
		public String TransferToAddress { get; set; }
		public Int64 Amount { get; set; }
		public String TokenName { get; set; }
		public Boolean Confirmed { get; set; }
	}

	public class TransactionModel2
	{
		public string Hash { get; set; }
		public int Block { get; set; }
		public long Timestamp { get; set; }
		public Boolean Confirmed { get; set; }
		public String OwnerAddress { get; set; }
		public String ToAddress { get; set; }
		public int ContractType { get; set; }
		public String Data { get; set; }
	}
}