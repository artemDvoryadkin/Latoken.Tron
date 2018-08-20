using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Latoken.CurrencyProvider.Common;
using Latoken.CurrencyProvider.Common.Interfaces;

namespace TronToken.BussinesLogic.Model
{
	public class OutputAgent : IOutputAgent
	{
		public OutputAgent(string address)
		{
			address = address;
		}

		public OutputAgent(string address, string privateKey)
		{
			address = address;
			privateKey = privateKey;
		}

		public string address { get; set; }
		public string privateKey { get; set; }
	}
}