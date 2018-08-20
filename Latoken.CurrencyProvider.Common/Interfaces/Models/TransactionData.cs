using System;
using System.Collections.Generic;
using System.Text;

namespace Latoken.CurrencyProvider.Common.Interfaces.Models
{
	/**
	* Объект, возращаемый при создании транзакции
	* @param {string} data - Данные транзакции в hex представлении
	* @param {object} info - Дополнительные данные о транзакции (зависит от провайдера)
	*/
	public class TransactionData
	{
		public string data { get; set; }
		public object info { get; set; }
	}
}