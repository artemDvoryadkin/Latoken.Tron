using System;
using System.Collections.Generic;
using JsonRpc.Standard.Contracts;
using JsonRpc.Standard.Server;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Interfaces;
using Latoken.CurrencyProvider.Common.Interfaces.Models;
using Microsoft.Extensions.Logging;

namespace Latoken.CurrencyProvider.JsonRpcApplication
{
    public class TronServiceTrovider : JsonRpcService, ICurrencyProvider
	{
	    private readonly ILogger logger;
		private Protocol.Tron.CurrencyProvider _currencyProvider;

		public TronServiceTrovider() 
		{
			ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(TypeNet.Main, "fullnode", 50051, 30, 5000);
			_currencyProvider = new Protocol.Tron.CurrencyProvider(protocolConfiguration);
		}

		public TronServiceTrovider(ILoggerFactory loggerFactory) : this()
		{
			// Inject loggerFactory from constructor.
			if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
			logger = loggerFactory.CreateLogger<TronServiceTrovider>();
		}

		private void LogMrthodeInforamation(string methodeName, string message)
		{
			logger.LogInformation($":::::::: {methodeName} START");
			logger.LogInformation(message);
			logger.LogInformation($":::::::: {methodeName} END");
		}

		/*
		 * ICurrencyProvider
		 *
		 * старт описания интерфейса 
		 */

		[JsonRpcMethod]
		public bool validateAddress(string address)
		{
			string methodeName = "ValidateAddress";
			try
			{
				bool validateAddress = _currencyProvider.validateAddress(address);
				int t = 0;
				LogMrthodeInforamation(methodeName, $"address {address} :::::: результат {validateAddress}");

				return validateAddress;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр address={address}");
				throw;
			}
		}

		[JsonRpcMethod]
		public bool validateReceipt(object any)
		{
			throw new NotImplementedException();
		}

		[JsonRpcMethod]
		public bool validateTransactionOptions(TransactionOptions options)
		{
			string methodeName = "validateTransactionOptions";
			try
			{
				bool validateAddress = _currencyProvider.validateTransactionOptions(options);
				LogMrthodeInforamation(methodeName, $"options {options} :::::: результат {validateAddress}");

				return validateAddress;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр options={options}");
				throw;
			}
		}

		[JsonRpcMethod]
		public long balanceByAddress(string address)
		{
			string methodeName = "balanceByAddress";
			try
			{
				long balance = _currencyProvider.balanceByAddress(address);

				LogMrthodeInforamation(methodeName, $"address {address} :::::: Баланс {balance}");

				return balance;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр address={address}");
				throw;
			}
		}

		[JsonRpcMethod]
		public long? balanceByAddressDB(string address)
		{
			string methodeName = "balanceByAddressDB";
			try
			{
				long? balance = _currencyProvider.balanceByAddressDB(address);

				LogMrthodeInforamation(methodeName, $"address {address} :::::: Баланс {balance}");

				return balance;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр address={address}");
				throw;
			}
		}

		[JsonRpcMethod]
		public long? balanceMetaByAddress(string address)
		{
			string methodeName = "balanceMetaByAddress";
			try
			{
				long? balance = _currencyProvider.balanceMetaByAddress(address);

				LogMrthodeInforamation(methodeName, $"address {address} :::::: Баланс {balance}");

				return balance;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр address={address}");
				throw;
			}
		}

		[JsonRpcMethod]
		public bool txOfflineExternalIdAvailable()
		{
			string methodeName = "txOfflineExternalIdAvailable";
			try
			{
				bool validateAddress = _currencyProvider.txOfflineExternalIdAvailable();

				LogMrthodeInforamation(methodeName, $":::::: результат {validateAddress}");

				return validateAddress;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName}");
				throw;
			}
		}

		[JsonRpcMethod]
		public string txGetOfflineExternalID(string txHex)
		{
		
			string methodeName = "txGetOfflineExternalID";
			try
			{
				string result = _currencyProvider.txGetOfflineExternalID(txHex);

				LogMrthodeInforamation(methodeName, $"address {txHex} :::::: результат {result}");

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр txHex={txHex}");
				throw;
			}
		}

		[JsonRpcMethod]
		public TransactionData txCreate(TransactionOptions options)
		{
			throw new NotImplementedException();
		}

		[JsonRpcMethod]
		public TransactionSendResult txTransferRaw(string txHex, string txHash)
		{
			throw new NotImplementedException();
		}

		[JsonRpcMethod]
		public TransactionSendResult txTransfer(TransactionOptions options)
		{
			throw new NotImplementedException();
		}

		[JsonRpcMethod]
		public ITransactionSendResult txGetSendingResult(string txid)
		{
			string methodeName = "txGetSendingResult";
			try
			{
				ITransactionSendResult transactionSendResult = _currencyProvider.txGetSendingResult(txid);

				LogMrthodeInforamation(methodeName, $"txid {txid} :::::: результат {transactionSendResult}");

				return transactionSendResult;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр address={txid}");
				throw;
			}
		}

		[JsonRpcMethod]
		public object txGetReceipt(string txid)
		{
			throw new NotImplementedException();
		}

		[JsonRpcMethod]
		public List<object> txListByAddress(string address)
		{
			string methodeName = "txListByAddress";
			try
			{
				List<object> result = _currencyProvider.txListByAddress(address);

				LogMrthodeInforamation(methodeName, $"address {address} :::::: результат {result}");

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр address={address}");
				throw;
			}
		}

		[JsonRpcMethod]
		public List<string> txListHashByAddress(string address)
		{
			string methodeName = "txListHashByAddress";
			try
			{
				List<string> result = _currencyProvider.txListHashByAddress(address);

				LogMrthodeInforamation(methodeName, $"address {address} :::::: результат {result}");

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр address={address}");
				throw;
			}
		}

		[JsonRpcMethod]
		public long toNativeNumber(string @base)
		{
			string methodeName = "toNativeNumber";
			try
			{
				long balance = _currencyProvider.toNativeNumber(@base);

				LogMrthodeInforamation(methodeName, $"@base {@base} :::::: рузультат {balance}");

				return balance;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр @base={@base}");
				throw;
			}
		}

		[JsonRpcMethod]
		public decimal fromNativeNumber(string @base)
		{
			string methodeName = "fromNativeNumber";
			try
			{
				decimal result = _currencyProvider.fromNativeNumber(@base);

				LogMrthodeInforamation(methodeName, $"@base {@base} :::::: рузультат {result}");

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				logger.LogCritical(e, $"Упал метод {methodeName} параметр @base={@base}");
				throw;
			}
		}

		[JsonRpcMethod]
		public string getProviderAPIHost()
	    {
			string methodeName = "getProviderAPIHost";
		    try
		    {
			    string result = _currencyProvider.getProviderAPIHost();

			    LogMrthodeInforamation(methodeName, $":::::: рузультат {result}");

			    return result;
		    }
		    catch (Exception e)
		    {
			    Console.WriteLine(e);
			    logger.LogCritical(e, $"Упал метод {methodeName}");
			    throw;
		    }
		}

		/*
		 * ICurrencyProvider 
		 *
		 * конец описания интерфейса
		 */
	}
}