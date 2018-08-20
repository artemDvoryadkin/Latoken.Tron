using System;
using System.Collections.Generic;
using System.Text;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Core;
using TronToken.CurrencyProvider.BussinesLogic.Model;

namespace Latoken.CurrencyProvider.Protocol.Tron.Models
{
    public class TransactionAtom
    {
        public TronAddress From { get; set; }
        public TronAddress To { get; set; }
        public long Amount { get; set; }
        public byte[] HashTransaction {get;set;}

        private Latoken.CurrencyProvider.Protocol.Tron.Grpc.Core.Transaction.Types.raw RawData {get;set;}
        private byte[] fromBytes { get; set; }
        private byte[] toBytes { get; set; }

        public TransactionAtom(TronAddress @from, TronAddress to, long amount, byte[] hashTransaction)
        {
            From = @from;
            To = to;
            Amount = amount;
            HashTransaction = hashTransaction;
        }

        public TransactionAtom( byte[] fromBytes, byte[] bytes, long amount, Transaction.Types.raw rawData)
        {
            Amount = amount;
            RawData = rawData;
            this.fromBytes = fromBytes;
            toBytes = bytes;
        }
    }
}