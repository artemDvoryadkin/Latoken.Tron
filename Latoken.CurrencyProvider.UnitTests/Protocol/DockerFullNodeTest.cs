using System;
using System.Collections.Generic;
using System.Text;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Google.Protobuf;
using Latoken.CurrencyProvider.Protocol.Tron;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Core;
using Latoken.CurrencyProvider.Protocol.Tron.Models;
using TronToken.CurrencyProvider.BussinesLogic.Model;

namespace Latoken.CurrencyProvider.UnitTests.Protocol
{
	[TestClass]
	public class DockerFullNodeTest
    {
	    [DataTestMethod]
	    
     //   [DataRow("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu", StringType.Base58, TypeNet.Test)]
        [DataRow("TWxezzaF6Lyvh4mNVQRj9okro8hqC3LfJt", StringType.Base58, TypeNet.Main)]
    //    [DataRow("TMy3cNNSZHY5d6F39BUbwUExJR93QDpGeh", StringType.Base58, TypeNet.Main)]
        public void DockerConnectTest(string addressBase58, StringType stringType, TypeNet typeNet)
	    {
		  //  ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet, "172.17.0.2", 50051);
		   // ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet);
		//  ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet, "10.0.75.1", 50041);
		  ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet, "10.0.75.1", 50051);
		  //  ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet, "18.184.238.21", 50051);


		    Wallet.WalletClient walletClient = new Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api.Wallet.WalletClient(protocolConfiguration.Channel);
		    NodeList listNodes = walletClient.ListNodes(new EmptyMessage());


		    CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = new CurrencyProvider.Protocol.Tron.CurrencyProvider(protocolConfiguration);

            var rr = BitConverter.GetBytes(152948);
            var ddd = rr.ToHexString2();

            BytesMessage numberMessa = new BytesMessage();
            numberMessa.Value = Google.Protobuf.ByteString.CopyFrom(ddd.FromHexToByteArray2());
            var dd = walletClient.GetBlockById(numberMessa);


            var block = walletClient.GetNowBlock(new EmptyMessage());

            //    long balance = currencyProvider.balanceByAddress(addressBase58);
            //   Console.WriteLine($"{addressBase58}:{balance}");
            //  Assert.IsTrue(balance > 0);
	        List<TransactionAtom> transferContractList = new List<TransactionAtom>();

            int startPosition = 1;
	        int endPosition = 1600000;
	        int step = 100;
	        int currentPosition = startPosition;
	        while (currentPosition<= endPosition)
	        {
	            int currentStartPosition = currentPosition;
	            int currentEndPosition = currentStartPosition + step;

                var blocklimit = new BlockLimit();
	            blocklimit.StartNum = currentStartPosition;
	            blocklimit.EndNum = currentEndPosition;
	            BlockList qqqq = walletClient.GetBlockByLimitNext(blocklimit);
	          
	            foreach (Block block1 in qqqq.Block)
	            {
	                foreach (Transaction transaction in block1.Transactions)
	                {
	                    foreach (Transaction.Types.Contract contract1 in transaction.RawData.Contract)
	                    {
	                        if (contract1.Type == Transaction.Types.Contract.Types.ContractType.TransferContract)
	                        {
	                            ByteString parameterValue = contract1.Parameter.Value;
	                            TransferContract transferContract = TransferContract.Parser.ParseFrom(parameterValue.ToByteArray());

	                            TronAddress fromAccount = new TronAddress(transferContract.OwnerAddress.ToByteArray());
	                            TronAddress toAccount = new TronAddress(transferContract.ToAddress.ToByteArray());
	                            TransactionAtom ta = new TransactionAtom(fromAccount, toAccount, transferContract.Amount, new TransactionHelper().GetTransactionHash(transaction));

	                            transferContractList.Add(ta);
	                        }
	                    }
	                }
	            }

	            currentPosition = currentEndPosition;
	        }
            
  //          var eeede = qqqq.Block[0].Transactions[0];
	        //        var contract = eeede.RawData.Contract[0].Parameter;
	    }

        [DataTestMethod]

        //   [DataRow("TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu", StringType.Base58, TypeNet.Test)]
        [DataRow("TWxezzaF6Lyvh4mNVQRj9okro8hqC3LfJt", StringType.Base58, TypeNet.Main)]
        //    [DataRow("TMy3cNNSZHY5d6F39BUbwUExJR93QDpGeh", StringType.Base58, TypeNet.Main)]
        public void SolidityDockerConnectTest(string addressBase58, StringType stringType, TypeNet typeNet)
        {
            //  ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet, "172.17.0.2", 50051);
            // ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet);
            //  ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet, "10.0.75.1", 50041);
            ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet, "10.0.75.1", 50052);
            //  ProtocolConfiguration protocolConfiguration = new ProtocolConfiguration(typeNet, "18.184.238.21", 50051);


            var walletClient = new Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api.WalletSolidity.WalletSolidityClient(protocolConfiguration.Channel);
            var sol= new Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api.WalletSolidity.WalletSolidityClient(protocolConfiguration.Channel);

            byte[] addressBytes = StringHelper.GetBytesAdressFromString(addressBase58);

            Account account = new Account();
            account.Address = ByteString.CopyFrom(addressBytes.SubArray(0, 21));

            var dddd = sol.GetAccount(account);
     //       NodeList listNodes = walletClient.GetPaginatedAssetIssueList ListNodes(new EmptyMessage());

            var we = new Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api.WalletExtension.WalletExtensionClient(protocolConfiguration.Channel);

           

            var ap = new AccountPaginated();
            ap.Account = account;
            ap.Offset = 0;
            ap.Limit = 100;

            var t = we.GetTransactionsFromThis2(ap);
            

            CurrencyProvider.Protocol.Tron.CurrencyProvider currencyProvider = new CurrencyProvider.Protocol.Tron.CurrencyProvider(protocolConfiguration);

            var rr = BitConverter.GetBytes(1502948);

            BytesMessage numberMessa = new BytesMessage();
            numberMessa.Value = Google.Protobuf.ByteString.CopyFrom(rr);
        }
    }
}
