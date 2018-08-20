using System;
using System.Collections.Generic;
using System.Text;

namespace Latoken.CurrencyProvider.Common.Interfaces
{
	public interface ITransactionSendResult
	{
		TransactionStatus status { get; set; }
		string error { get; set; } //Взят тип как строка, в спецификации тип Error, но не описан.
		Info info { get; set; }
	}
}
