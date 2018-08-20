using System;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Latoken.CurrencyProvider.Common.Interfaces.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latoken.CurrencyProvider.UnitTests.ICurrencyProvider
{
	[TestClass]
	public class ValidateTransactionOptionsTests : BaseTest
	{
		[DataTestMethod]
		[DataRow("750EB0343C49C9958387737108FB41EB53B6FD1FC074700F30082A7F9D4CC823", "TLWY31TNNkqENXNcSctb2mqH1qvRdwbjFL", "TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu", 108,true)]
		[DataRow("750EB0343C49C9958387737108FB41EB53B6FD1FC074700F30082A7F9D4CC823", "TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu", "TLWY31TNNkqENXNcSctb2mqH1qvRdwbjFL", 108,true)] //пара ключей не пара, 
		[DataRow("750EB0343C49C9958387737108FB41EB53B6FD1FC074700F30082A7F9D4CC823", "TLWY31TNNkqENXNcSctb2mqH1qvRdwbjFL", "TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu", 108108,false)] // сумма больше чем на балансе 10 000 TRX
		[DataRow("TRb45eJbLbxCp2WQYQeNt9AoR3iiMKooLqs", -20,false)] // сумма отрицательная.
		public void ValidateTransactionOptionsTest(string agentPrivateKey, string agentAddress, string credentialsAddress, long amount)
		{
			ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(TypeNet.Test);
			CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = new CurrencyProvider.Protocol.Tron.CurrencyProvider(protocolConfiguration);

			TransactionOptions generateTransactionOptions = GenerateTransactionOptions(agentPrivateKey, agentAddress, "TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu", 108);

			bool isValidTransactionOptions = currencyProvider.validateTransactionOptions(generateTransactionOptions);
			Assert.IsTrue(isValidTransactionOptions);
		}

		private TransactionOptions GenerateTransactionOptions(string agentPrivateKey, string agentAddress,
			string credentialsAddress, long amount)
		{
			OutputAgent outputAgentModel = new OutputAgent(agentPrivateKey, agentAddress);
			TransactionCredentials transactionCredentialsModels = new TransactionCredentials(agentAddress);
			TransactionOptions transactionOptionsModels = new TransactionOptions(outputAgentModel, transactionCredentialsModels, amount);

			return transactionOptionsModels;
		}
	}
}