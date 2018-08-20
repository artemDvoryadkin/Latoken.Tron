using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Google.Protobuf;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Exceptions;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Latoken.CurrencyProvider.Common.Intefaces;
using Latoken.CurrencyProvider.Common.Interfaces;
using Latoken.CurrencyProvider.Common.Interfaces.Models;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api;
using Latoken.CurrencyProvider.Protocol.Tron.Grpc.Core;
using Latoken.CurrencyProvider.Protocol.Tron.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
    
using Newtonsoft.Json;
using TronToken.BussinesLogic.Model.TronModel;
using TronToken.CurrencyProvider.BussinesLogic.Model;
using RequestHelper = TronToken.BussinesLogic.RequestHelper;

namespace Latoken.CurrencyProvider.Protocol.Tron
{
    public class CurrencyProvider : ICurrencyProvider
    {
        private String _host = "api.tronscan.org";
        private RequestHelper _requestHelper;
        private Wallet.WalletClient _walletClient;
        const long MAX_AMOUNT_PROVIDER = 1000000000000000;
        const long MIN_AMOUNT_PROVIDER = 0;
        const long MAX_AMOUNT_PROVIDER_COIN = 1000000000;
        const long MIN_AMOUNT_PROVIDER_COIN = 0;
        const long RateCoinToSatoshi = 1000000;

       

        private ProtocolConfiguration _protocolConfiguration;

        // todo:: сделать потоко безопасно.
        private static readonly List<TransactionAtom> _cacheTransaction = new List<TransactionAtom>();
        private static long lastCacheBlodkId = 0;

        //Store Stuff in the cache  
        public void UpdateTransactionAtomCache()
        {

            var block = _walletClient.GetNowBlock(new EmptyMessage());
            long lastBlockId = block.BlockHeader.RawData.Number;

                //     lastBlockId = 1000; //test

            if (lastBlockId > lastCacheBlodkId)
            {
                List<TransactionAtom> transactionByRangeBlocks = GetTransactionByRangeBlocks(lastCacheBlodkId, lastBlockId);

                // todo: нужно переписать потоко безопасно.
                _cacheTransaction.AddRange(transactionByRangeBlocks);
                lastCacheBlodkId = lastBlockId;
            }
        }

        public List<TransactionAtom> GetTransactionAtomsFromCache()
        {
            // todo: нужно переписать потоко безопасно.
            UpdateTransactionAtomCache();
            
            return _cacheTransaction;
        }


        public CurrencyProvider(ProtocolConfiguration protocolConfiguration)
        {
            _protocolConfiguration = protocolConfiguration;
            _requestHelper = new RequestHelper(_host);

            _walletClient =
                new Latoken.CurrencyProvider.Protocol.Tron.Grpc.Api.Wallet.WalletClient(_protocolConfiguration.Channel);
        }



        public bool validateAddress(string address)
        {
            return TronAddress.ValidateAddress(address);
        }

        public bool validateReceipt(object any)
        {
            throw new NotImplementedException();
        }

        public bool validateTransactionOptions(TransactionOptions options)
        {
            throw new NotImplementedException();
        }

        public bool txOfflineExternalIdAvailable()
        {
            return true;
        }

        /**
        * Получить значение в минимальных единицах валюты
        * @param {string} base - значение для конвертации в десятичном представлении
        * @returns {BigInt} значение в минимальных единицах валюты
        */
        public TransactionSendResult txTransferRaw(string txHex, string txHash)
        {
            throw new NotImplementedException();
        }

        public TransactionSendResult txTransfer(TransactionOptions options)
        {
            throw new NotImplementedException();
        }

        public long toNativeNumber(string @base)
        {
            IFormatProvider provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string formatAmount = @base.Replace(".", ",").Replace(" ", "").Replace("_", "");

            Decimal resultDecimal;
            bool tryParse = Decimal.TryParse(formatAmount, NumberStyles.Any, provider, out resultDecimal);
            if (!tryParse)
                throw new ParseAmountException(ParseAmountException.CodeEnum.Format, $"Не верный формат суммы {@base}");

            if (resultDecimal < (decimal) 1 / RateCoinToSatoshi)
                throw new ParseAmountException(ParseAmountException.CodeEnum.MinAmount,
                    $"Сумма \"{@base}\" меньше минимального значения 0,000 001");

            long transformAmount = (long) (resultDecimal * RateCoinToSatoshi);

            if (!isValidateAmountInSatoshiByMax(transformAmount))
                throw new ParseAmountException(ParseAmountException.CodeEnum.MaxAmount,
                    $"Сумма \"{@base}\" больше максимального значения ${MAX_AMOUNT_PROVIDER_COIN}.");

            return transformAmount;
        }

        public decimal fromNativeNumber(string @base)
        {
            long resultLong;
            bool tryParse = long.TryParse(@base, out resultLong);

            if (!tryParse)
                throw new ParseAmountException(ParseAmountException.CodeEnum.Format, $"Не верный формат суммы {@base}");

            decimal result = (decimal) ((decimal) resultLong / RateCoinToSatoshi);

            if (!isValidateAmountInCoinByMax(result))
                throw new ParseAmountException(ParseAmountException.CodeEnum.MaxAmount,
                    "Сумма \"{@base}\" больше максимального значения ${MAX_AMOUNT_PROVIDER_COIN}.");

            if (!isValidateAmountInCoinByMin(result))
                throw new ParseAmountException(ParseAmountException.CodeEnum.MinAmount,
                    "Сумма \"{@base}\" меньше минимального значения ${MAX_AMOUNT_PROVIDER_COIN}.");

            return result;
        }

        public bool isValidateAmountInSatoshiByMax(long amount)
        {
            if (MAX_AMOUNT_PROVIDER > amount) return true;

            return false;
        }

        public bool isValidateAmountInSatoshiByMin(long amount)
        {
            if (MAX_AMOUNT_PROVIDER <= amount) return true;

            return false;
        }

        public bool isValidateAmountInCoinByMax(decimal amount)
        {
            if (MAX_AMOUNT_PROVIDER_COIN > amount) return true;

            return false;
        }

        public bool isValidateAmountInCoinByMin(decimal amount)
        {
            if (MIN_AMOUNT_PROVIDER_COIN <= amount) return true;

            return false;
        }

        public long? balanceMetaByAddress(string address)
        {
            return null; // так как нет базовой валюты. Текущая версия протокола перешла с эфира.
        }

        public long? balanceByAddressDB(string address)
        {
            // Со слов Свята "это наследние". 
            // Не реализовано.
            return null;
        }


        public BytesMessage GetByteMessageFromString(string message)
        {
            byte[] txId = StringHelper.GetBytesAdressFromString(message);

            BytesMessage bytesMessage = new BytesMessage();
            bytesMessage.Value = ByteString.CopyFrom(txId);

            return bytesMessage;
        }

        public ITransactionSendResult txGetSendingResult(string txid)
        {

            Transaction transactionLoad = null;
            BytesMessage txidByteMessage = GetByteMessageFromString(txid);
            try
            {

                transactionLoad = _walletClient.GetTransactionByIdAsync(txidByteMessage).GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                return new TransactionSendResult(TransactionStatus.Error, exception.Message, null);
            }

            if (transactionLoad == null)
                return new TransactionSendResult(TransactionStatus.Error, "Транзакция по txid не найдена.", null);

            Block block = _walletClient.GetBlockByIdAsync(txidByteMessage).GetAwaiter().GetResult();

            ByteString rawDataData = transactionLoad.RawData.Data;
            //	var bigInteger = new BigInteger(rawDataData.ToByteArray());
            //	Info info = new Info(txid, transactionLoad.RawData.ToByteString().ToBase64(), DateTime.Now);
            return new TransactionSendResult(TransactionStatus.Sent, null, null);
        }

        public string txGetOfflineExternalID(string txHex)
        {
            /*
            ECKey ecKey = ECKey.FromPrivate(privateKey);
            Transaction transaction1 = Transaction.Parser.ParseFrom(transaction);
            byte[] rawdata = transaction1.RawData.ToByteArray();
            byte[] hash = Sha256Keccak.Hash(rawdata);
            byte[] sign = ecKey.GetSignature(hash);
            Transaction clone = transaction1.Clone();
            clone.Signature.Add(ByteString.CopyFrom(sign));
            return clone.ToByteArray();
            */
            throw new NotImplementedException();
        }

        public TransactionData txCreate(TransactionOptions options)
        {
            throw new NotImplementedException();
        }

        public string GetUrl(string path)
        {
            if (path.Length > 0 && path[0] != '/') path = "/" + path;

            return getProviderAPIHost() + path;
        }

        public String GetUrlApi(String path)
        {
            if (!path.Contains("https://")) path = GetUrl(path);
            return path;
        }

        public object txGetReceipt(string txid)
        {
            HttpWebRequest setupWebRequest = _requestHelper.SetupWebRequest(GetUrlApi($"/transaction/{txid}"));
            string webRequestString = _requestHelper.GetStringFromHttpWebRequest(setupWebRequest);
            TransactionModel2 transaction = JsonConvert.DeserializeObject<TransactionModel2>(webRequestString);

            return transaction;
        }

        public List<string> txListHashByAddress(string address)
        {
            return txListTransactionModelByAddress(address).Select(hash => hash.HashTransaction.ToHexString2()).ToList<String>();
        }

        public string getProviderAPIHost()
        {
            return "https://api.tronscan.org/api";
        }



        public Int64 balanceByAddress(String address)
        {
            byte[] addressBytes = StringHelper.GetBytesAdressFromString(address);

            Account account = new Account();
            account.Address = ByteString.CopyFrom(addressBytes.SubArray(0, 21));

            Account result = _walletClient.GetAccountAsync(account).GetAwaiter().GetResult();

            return result.Balance;
        }

        public List<Object> txListByAddress(string address)
        {
            return txListTransactionModelByAddress(address).ToList<Object>();
        }

        public List<TransactionAtom> txListTransactionModelByAddress(string address)
        {
            TronAddress tronAddress = TronAddress.CreateTronAddress(address);

            List<TransactionAtom> result = new List<TransactionAtom>();
            foreach (TransactionAtom transactionByRangeBlock in GetTransactionAtomsFromCache())
            {
                if (transactionByRangeBlock.From.Equals(tronAddress) || transactionByRangeBlock.To.Equals(tronAddress))
                {
                    result.Add(transactionByRangeBlock);
                }

            }

            return result;
        }

        public bool validateTransactionOptions(ITransactionOptions options)
        {
            IOutputAgent optionsAgent = options.agent;

            if (options.value < 0) return false;

            bool isValidTronAddress = TronAddress.ValidateAddress(optionsAgent.address);
            if (!isValidTronAddress) return false;

            byte[] primaryKeyBytes = StringHelper.GetBytesAdressFromString(optionsAgent.privateKey);
            try
            {
                KeyTriple keyTriple = new KeyTriple(primaryKeyBytes.ToHexString2());
            }
            catch (FormatException)
            {
                return false;
            }

            bool isBelongsPublicKeyToPrivateKey =
                TronAddress.isBelongsPublicKeyToPrivateKey(optionsAgent.privateKey, optionsAgent.address);
            if (!isBelongsPublicKeyToPrivateKey) return false;

            ITransactionCredentials credentials = options.credentials;
            bool isValidTronCredentialsAddress = TronAddress.ValidateAddress(credentials.address);
            if (!isValidTronCredentialsAddress) return false;

            if (!isValidateAmountInSatoshiByMax(options.value)) return false;

            long balanceAgent = balanceByAddress(optionsAgent.address);
            if (balanceAgent < options.value) return false;

            return true;
        }


        private List<TransactionAtom> GetTransactionByRangeBlocks(long startBlockId, long endBlockId)
        {
            List<TransactionAtom> transferContractList = new List<TransactionAtom>();

            long startPosition = 1;
            long endPosition = 1600000;
            long step = 100;

            long currentPosition = startBlockId;
            while (currentPosition <= endBlockId)
            {
                long currentStartPosition = currentPosition;
                long currentEndPosition = currentStartPosition + step;

                var blocklimit = new BlockLimit
                {
                    StartNum = currentStartPosition,
                    EndNum = currentEndPosition
                };
                BlockList qqqq = _walletClient.GetBlockByLimitNext(blocklimit);

                foreach (Block block1 in qqqq.Block)
                {
                    foreach (Transaction transaction in block1.Transactions)
                    {
                        foreach (Transaction.Types.Contract contract1 in transaction.RawData.Contract)
                        {
                            if (contract1.Type == Transaction.Types.Contract.Types.ContractType.TransferContract)
                            {
                                ByteString parameterValue = contract1.Parameter.Value;
                                TransferContract transferContract =
                                    TransferContract.Parser.ParseFrom(parameterValue.ToByteArray());

                                TronAddress fromAccount = new TronAddress(transferContract.OwnerAddress.ToByteArray());
                                TronAddress toAccount = new TronAddress(transferContract.ToAddress.ToByteArray());
                                TransactionAtom ta = new TransactionAtom(transferContract.OwnerAddress.ToByteArray(), transferContract.ToAddress.ToByteArray(),
                                    transferContract.Amount,
                                    transaction.RawData);

                                transferContractList.Add(ta);
                            }
                        }
                    }
                }

                currentPosition = currentEndPosition;
            }

            return transferContractList;
        }
    }
}