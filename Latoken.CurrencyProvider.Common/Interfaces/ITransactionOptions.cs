using Latoken.CurrencyProvider.Common.Interfaces;

namespace Latoken.CurrencyProvider.Common.Intefaces
{

	/**
	* Параметры необходимые для создания транзакции. Зависит от провайдера
	* @param {OutputAgent} agent - экземпляр агента для отправки
	* @param {TransactionCredentials} credentials - параметры получателя
	* @param {BigInt} value - сумма операции
	*/
	public interface ITransactionOptions
	{
		IOutputAgent agent { get; set; }
		ITransactionCredentials credentials { get; set; }
		long value { get; set; }
	}
}