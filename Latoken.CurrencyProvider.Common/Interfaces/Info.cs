using System;
using System.Collections.Generic;
using System.Text;

namespace Latoken.CurrencyProvider.Common.Interfaces
{
	public class Info
	{
		public Info(string id, string message, DateTime sentTime)
		{
			this.id = id;
			this.message = message;
			this.sentTime = sentTime;
		}

		public string id { get; set; }
		public string message { get; set; }
		public DateTime sentTime { get; set; }
	}
}