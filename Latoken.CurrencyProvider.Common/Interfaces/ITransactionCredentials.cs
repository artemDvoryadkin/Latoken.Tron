namespace Latoken.CurrencyProvider.Common.Interfaces
{
	/**
	* Объект с параметрами получателя
	* @param {string} address - Blockchain адрес, или номер счета
	*/
	public interface ITransactionCredentials
	{
		string address { get; set; }
	}
}