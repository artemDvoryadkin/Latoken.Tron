using System;
using System.Collections.Generic;
using System.Text;

namespace Latoken.CurrencyProvider.Common.Interfaces.Models
{
    public class TransactionSendResult : ITransactionSendResult
	{
		public TransactionSendResult(TransactionStatus status, string error, Info info)
		{
			this.status = status;
			this.error = error;
			this.info = info;
		}

		public TransactionStatus status { get; set; }
		public string error { get; set; }
		public Info info { get; set; }
	}
}