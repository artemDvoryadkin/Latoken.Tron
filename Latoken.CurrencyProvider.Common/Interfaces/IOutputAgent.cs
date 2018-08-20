namespace Latoken.CurrencyProvider.Common.Interfaces
{
	/**
	* Агент - сущность, отвечающая за 1 вывод в единицу времени
	* @param {string} address - Agent address (отправка с данного адреса - если применимо)
	* @param {string} privateKey - Private Key или API ключ
	*/
	public interface IOutputAgent
	{
		string address { get; set; }
		string privateKey {get;set;}
	}
}