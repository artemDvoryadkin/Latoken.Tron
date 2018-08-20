using System;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Latoken.CurrencyProvider.Common.Helpers.Crypto.Sha;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;

namespace Latoken.CurrencyProvider.UnitTests.Garbage
{
	[TestClass]
	public class TransactionTests : BaseTest
	{

		[TestMethod]
		public void getTransactionFromTestNet2Tesst()
		{
			WalletExtension.WalletExtensionClient wallet = GetMainNetWalletExtensionClient;

			var keyTriple = new KeyTriple("B7E85CA5910922C49DCCB92EE48DFEC13A60674845CB7152C86B88F31DA6DC67");
			AccountPaginated accountPaginated = new AccountPaginated();

			Account account = new Account();
			account.Address = ByteString.CopyFrom(keyTriple.GetAddressWallet(TypeNet.Test));
			accountPaginated.Account = account;
			accountPaginated.Offset = 0;
			accountPaginated.Limit = 10;
			TransactionList transactionList = wallet.GetTransactionsFromThisAsync(accountPaginated).GetAwaiter().GetResult();

			Transaction transaction = new Transaction();
		//	transaction.RawData.

			//wallet.
		}

		[TestMethod]
		public void getTransactionFromTestNetTesst()
		{
			Wallet.WalletClient wallet = GetMainNetWalletClient;

		//	wallet.tra

			byte[] hashTransaction = StringHelper.HexStringToByteArray("d2e635f7c2d85cbcd343721047447b122a3c19e86f30651d8ec6ee76f744d065");
			BytesMessage bytesMessage = new BytesMessage();
			bytesMessage.Value = ByteString.CopyFrom(hashTransaction);
			//				Transaction transactionLoad = wallet.GetTransactionByIdAsync(bytesMessage).GetAwaiter().GetResult();
			//				Transaction transactionLoad = wallet.GetTransactionByIdAsync(bytesMessage).GetAwaiter().GetResult();
			NodeList nodeList = wallet.ListNodes(GetEmptyMessage);
			Block result = wallet.GetNowBlockAsync(GetEmptyMessage).GetAwaiter().GetResult();
			//TK4BAeF72P3gop24RsoqPJynWgBBDv9fXX
			var keyTriple = new KeyTriple("B7E85CA5910922C49DCCB92EE48DFEC13A60674845CB7152C86B88F31DA6DC67");
			byte[] addressWallet = keyTriple.GetAddressWallet(TypeNet.Main);
			string encode = Base58.Encode(addressWallet);
			Account account = new Account();
			account.Address = ByteString.CopyFrom(addressWallet);
			Account account1 = wallet.GetAccountAsync(account).GetAwaiter().GetResult();

		//	new WalletExtension
			


			byte[] hashTransaction1 = StringHelper.HexStringToByteArray("a7e974c6e69fb7741bf5e08de8a2d8f6617826c59422b440de22e0612b03c393");
			BytesMessage bytesMessage1 = new BytesMessage();
			bytesMessage1.Value = ByteString.CopyFrom(hashTransaction1);
			Transaction transactionLoad1 = wallet.GetTransactionByIdAsync(bytesMessage1).GetAwaiter().GetResult();
			TransferContract transferContract = TransferContract.Parser.ParseFrom(transactionLoad1.RawData.Contract[0].Parameter.Value.ToByteArray());
		}

		[TestMethod]
		public void CreateTest()
		{
			//1B7229DC6A062904C0D2B9D9A5795D4E555F5D1FEBC4A37B63EEB0FF4BC82D60 Passw0rdTRON TYYgwqYU9U8PT19bVDYjJckGrEnJoQKGUX
			// hash tran 6e712f3c4a50945973b655fbcaa503369ec4977a294d697823070026ba539110
			// test wallet 2E77481EA8344FF43D5301D55F500CA45A4FEE62AFCDEFF3E8D4F2A4465C8CB2  TMmjx5RbjDwS4JoqKgujbsoa617b7DwfJq

			//D8414F1EFEC490358FEF9C8F6AE3980BD7F4323E0D2F2DE0DB9D590935D12F72

			//	String privateStr = "D95611A9AF2A2A45359106222ED1AFED48853D9A44DEFF8DC7913F5CBA727366";
			String privateKeyHex = "750EB0343C49C9958387737108FB41EB53B6FD1FC074700F30082A7F9D4CC823"; //тестовая сеть TLWY31TNNkqENXNcSctb2mqH1qvRdwbjFL
			//			String privateKeyHex = "74767E0ACC92FF6ED8E6A8C6CDDD61E27AEF20ACA3EC3E6492098C8B96CA23DB";
			String toAddressWalletHes = "TPwJS5eC5BPGyMGtYTHNhPTB89sUWjDSSu";
	//		String toAddressWalletHes = "TSSrETcLX7ZaWuXRF652R4AJhXDWRX6JuV";
			KeyTriple keyTriple = new KeyTriple(privateKeyHex);
			byte[] fromAddressWallet = keyTriple.GetAddressWallet(TypeNet.Main);
			String base58AddressWallet = Base58.Encode(fromAddressWallet);

			long amount = 231L; //100 TRX, api only receive trx in drop, and 1 trx = 1000000 drop

			Wallet.WalletClient wallet = GetMainNetWalletClient;

			//		byte[] hashTransaction = StringHelper.HexStringToByteArray("f5eea816a16b1907f8b5c6394e133f285001585ce07f657240701511436ba836");
			//		BytesMessage bytesMessage = new BytesMessage();
			//		bytesMessage.Value=ByteString.CopyFrom(hashTransaction);
			//		Transaction transactionLoad = wallet.GetTransactionByIdAsync(bytesMessage).GetAwaiter().GetResult();
			/*
						TransferContract transferContract = TransferContract.Parser.ParseFrom(transactionLoad.RawData.Contract[0].Parameter.Value.ToByteArray());
						String ownerAddress = Base58Check.Base58CheckEncoding.Encode(transferContract.OwnerAddress.ToByteArray());
						String toAddress = Base58Check.Base58CheckEncoding.Encode(transferContract.ToAddress.ToByteArray());
						String ownerAddress64 = transferContract.OwnerAddress.ToBase64();
						*/
			//	string sign11 = Base64.ToBase64String(Sha256.Hash(transactionLoad.RawData.ToByteArray()));


			//		NodeList nodeList = wallet.ListNodesAsync().GetAwaiter().GetResult();

			Transaction transaction = createTransaction(wallet, fromAddressWallet, Base58.Decode(toAddressWalletHes), amount);
			byte[] transactionBytes = transaction.ToByteArray();

			Transaction transaction1 = sign(transaction, keyTriple);
			//get byte transaction
			byte[] transaction2 = transaction1.ToByteArray();
			Console.WriteLine("transaction2 ::::: " + Base58.Encode(transaction2));

			//sign a transaction in byte format and return a Transaction object
		//	Transaction transaction3 = Transaction.Parser.ParseFrom(signTransaction2Byte(transactionBytes, keyTriple.PrivateKey));
		//	Console.WriteLine("transaction3 ::::: " + Base58Check.Base58CheckEncoding.Encode(transaction3.ToByteArray()));

		//	wallet.CreateTransactionAsync()
			Return result1 = wallet.BroadcastTransactionAsync(transaction1).GetAwaiter().GetResult();
	//		Return result3 = wallet.BroadcastTransactionAsync(transaction3).GetAwaiter().GetResult();
		}

		public static Transaction sign(Transaction transaction, KeyTriple myKey)
		{
			byte[] hash = Sha256Keccak.Hash(transaction.RawData.ToByteArray());
				//	Transaction.Types.Contract listContract = transaction.RawData.Contract;
		//	for (int i = 0; i < listContract.size(); i++)
		//	{
				byte[] signature = myKey.GetSignature(hash);
				ByteString bsSign = ByteString.CopyFrom(signature);
			transaction.Signature.Add(bsSign) ;//Each contract may be signed with a different private key in the future.
		//	}
			return transaction;
		}

		private static byte[] signTransaction2Byte(byte[] transaction, byte[] privateKey)
		{
			ECKey ecKey = ECKey.FromPrivate(privateKey);
			Transaction transaction1 = Transaction.Parser.ParseFrom(transaction);
			byte[] rawdata = transaction1.RawData.ToByteArray();
			byte[] hash = Sha256Keccak.Hash(rawdata);
			byte[] sign = ecKey.GetSignature(hash);
			Transaction clone = transaction1.Clone();
			clone.Signature.Add(ByteString.CopyFrom(sign)); 
			return clone.ToByteArray();
		}

	public static Transaction createTransaction(Wallet.WalletClient wallet, byte[] from, byte[] to, long amount)
		{
			Transaction transactionBuilder = new Transaction();
			transactionBuilder.RawData = new Transaction.Types.raw();

			Block newestBlock = wallet.GetNowBlockAsync(new EmptyMessage()).GetAwaiter().GetResult();

			TransferContract transferContract = new TransferContract
			{
				Amount = amount,
				OwnerAddress = ByteString.CopyFrom(@from),
				ToAddress = ByteString.CopyFrom(to)
			};
		//	Transaction transactionNet = wallet.CreateTransactionAsync(transferContract).GetAwaiter().GetResult();


			Transaction.Types.Contract contract = new Transaction.Types.Contract();
			try
			{
				Any any = Any.Pack(transferContract);
				contract.Parameter = any;
			}
			catch (Exception)
			{
				return null;
			}
			
			
			contract.Type = Transaction.Types.Contract.Types.ContractType.TransferContract;

			transactionBuilder.RawData.Contract.Add(contract);
			transactionBuilder.RawData.Timestamp = DateTime.Now.Ticks;
			transactionBuilder.RawData.Expiration = newestBlock.BlockHeader.RawData.Timestamp + 10 * 60 * 60 * 1000;

			Transaction refTransaction = setReference(transactionBuilder, newestBlock);

			return refTransaction;
		}

		public static Transaction setReference(Transaction transaction, Block newestBlock)
		{
			long blockHeight = newestBlock.BlockHeader.RawData.Number;
			byte[] blockHash = GetBlockHash(newestBlock);
			byte[] refBlockNum = fromLong(blockHeight);

			Transaction.Types.raw rawData = transaction.RawData.Clone();
			rawData.RefBlockHash = ByteString.CopyFrom(blockHash, 8, 8);
			rawData.RefBlockBytes = ByteString.CopyFrom(refBlockNum, 6, 2);
//				.setRefBlockHash(ByteString.copyFrom(ByteArray.subArray(blockHash, 8, 16)))
			//			.setRefBlockBytes(ByteString.copyFrom(ByteArray.subArray(refBlockNum, 6, 8)))
			//		.build();
			
			return transaction;
		}

		public static byte[] GetBlockHash(Block block)
		{
			return Sha256Keccak.Hash(block.BlockHeader.RawData.ToByteArray());
		}

		public static byte[] fromLong(long val)
		{
			return BitConverter.GetBytes(val);
		}
	}
}
