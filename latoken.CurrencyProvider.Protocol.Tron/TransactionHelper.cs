using Google.Protobuf;
using Latoken.CurrencyProvider.Common.Helpers.Crypto.Sha;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Core;

namespace Latoken.CurrencyProvider.Protocol.Tron
{
    public class TransactionHelper
    {
	    public byte[] GetTransactionHash(Transaction transaction)
	    {
			byte[] hashTransaction = Sha256.Hash(transaction.RawData.ToByteArray());

		    return hashTransaction;
	    }
    }
}