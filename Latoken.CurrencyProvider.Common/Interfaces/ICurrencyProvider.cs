using System;
using System.Collections.Generic;
using Latoken.CurrencyProvider.Common.Interfaces.Models;

namespace Latoken.CurrencyProvider.Common.Interfaces
{
	public interface ICurrencyProvider
	{
		/* Validations */
		/**
	    * Проверяет валидность адреса для данного провайдера
	    * @param {string} address - Адрес для проверки
	    * @returns {boolean} адрес является верным
		*/
		// ReSharper disable once InconsistentNaming
		Boolean validateAddress(String address);

		/**
		* Проверка статуса транзакции (валидность receipt-а)
		* @param {object} address - Объект чека транзакции
		* @returns {boolean} Вадидность
		*/
		// ReSharper disable once InconsistentNaming
		bool validateReceipt(object any);

		/**
		* Проверка входных данных на валидность и возможность создания транзакции
		* @param {object} options - параметры для создания транзакции. Отличаются для каждого платежного провайдера
		* @returns {boolean} успешность проверки параметров
		*/
		// принимает тип не тот, должен приимать интерфейс, но нельзя принимать интерфейс так как по нему создается тип 
		// rpc json. по этому переиграли историю.
		// ReSharper disable once InconsistentNaming
		bool validateTransactionOptions(TransactionOptions options);

		/* Balances */
		/**
		* Получить доступный баланс по идентификатору (Адрес)
		* @param {string} address - Адрес для проверки баланса
		* @returns {BigInt} баланс счета в минимальных единицах валюты
		*/
		// ReSharper disable once InconsistentNaming
		long balanceByAddress(string address);

		/**
		* Получить доступный баланс по идентификатору (Адрес) из БД
		* @param {string} address - Адрес для проверки баланса
		* @returns {BigInt} баланс счета в минимальных единицах валюты
		*/
		// ReSharper disable once InconsistentNaming
		long? balanceByAddressDB(string address);

		/**
		* Получить доступный баланс базовой валюты к текущей по адресу (ETH для ERC20 / NEO для NEP5 / ...)
		* @param {string} address - Адрес для проверки мета-баланса
		* @returns {Promise<BigInt>} баланс счета в минимальных единицах валюты
		*/
		// ReSharper disable once InconsistentNaming
		long? balanceMetaByAddress(string address);

		/* Transactions */
		/**
		* Функция определяет доступно ли локальное вычисление id (хэша) транзакции
		*/
		// ReSharper disable once InconsistentNaming
		bool txOfflineExternalIdAvailable();

		/**
		* Вычисление хэша транзакции до отправки в сеть
		* @param txHex - транзакция в hex представлении
		* @returns {string} id (hash) транзакции
		*/
		// ReSharper disable once InconsistentNaming
		string txGetOfflineExternalID(string txHex);

		/**
		* Создание транзакции и сериализация её в виде пригодном для отправки в сеть
		* @param {object} options - параметры для создания транзакции. Отличаются для каждого платежного провайдера
		* @returns {TransactonData} сериализованная транзакция в hex + дополнительные данные
		*/
		// ReSharper disable once InconsistentNaming
		TransactionData txCreate(TransactionOptions options);

		/**
		* Отправка подготовленной транзакции в сеть
		* @param txHex - транзакция в hex представлении
		* @param txHash - id транзакции
		* @returns {boolean} успешность отправки
		*/
		// ReSharper disable once InconsistentNaming
		TransactionSendResult txTransferRaw(string txHex, string txHash);

		/**
		* Отправка транзакции в сеть. Выполняется checkTransactionParams + makeTransaction + transferRaw
		* @param {object} options - параметры для создания транзакции. Отличаются для каждого платежного провайдера
		* @returns {boolean} успешность отправки
		*/
		// ReSharper disable once InconsistentNaming
		TransactionSendResult txTransfer(TransactionOptions options);

		/**
		* Получить объект со статусом отправленной транзакции (receipt)
		* @returns {Promise<TransactionSendResult>} Объект с информацией о результате отправки
		*/
		// ReSharper disable once InconsistentNaming
		ITransactionSendResult txGetSendingResult(string txid);

		/**
		* Получить информацию о транзакции (receipt)
		* @returns {object} Объект с информацией о транзакции
		*/
		// ReSharper disable once InconsistentNaming
		object txGetReceipt(string txid);

		/**
		* Получить список транзакций по адресу
		* @param {string} address
		* @returns {TransactionReceipt} - объект описание транзакции
		*/
		// ReSharper disable once InconsistentNaming
		List<Object> txListByAddress(string address);

		/**
		* Получить список хэшей транзакций по адресу
		* @param {string} address
		* @returns {string} - ID/Hash транзакции
		*/
		//TODO: вытаскивет только 100 нужно сделать пейджинг.
		// ReSharper disable once InconsistentNaming
		List<string> txListHashByAddress(string address);

		// Utils
		/**
		* Получить значение в минимальных единицах валюты
		* @param {string} base - значение для конвертации в десятичном представлении
		* @returns {BigInt} значение в минимальных единицах валюты
		*/
		// ReSharper disable once InconsistentNaming
		long toNativeNumber(string @base);

		/**
		* Получить значение в минимальных единицах валюты
		* @param {string} base - значение для конвертации
		* @returns {BigInt} значение в десятичном представлении
		*/
		// ReSharper disable once InconsistentNaming
		decimal fromNativeNumber(string @base);

		/**
		* Получить идентификатор API RPC узла (url или hostname)
		* @returns {string} URL RPC узла
		*
		* getProviderAPIHost
		*/
		// ReSharper disable once InconsistentNaming
		string getProviderAPIHost();
	}
}