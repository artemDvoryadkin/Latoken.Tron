using System;
using System.Linq;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Exceptions;
using Latoken.CurrencyProvider.Common.Helpers;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Latoken.CurrencyProvider.Common.Helpers.Crypto.Sha;
using Latoken.CurrencyProvider.Common.Intefaces;
using Latoken.CurrencyProvider.Common.Interfaces;

namespace TronToken.CurrencyProvider.BussinesLogic.Model
{
	public class TronAddress : IAddress
	{

		public byte[] AddressFull25Bytes { get; private set; }
		public byte[] AddressShort21Bytes { get; private set; }

		public TronAddress(string addressString, StringType stringType) :this(StringHelper.GetBytesFromStirng(addressString, stringType))
		{
		}

		public TronAddress(byte[] addressFull25Bytes)
		{
			this.AddressFull25Bytes = addressFull25Bytes;

			if (!(AddressFull25Bytes.Length == 21 || AddressFull25Bytes.Length == 23 || AddressFull25Bytes.Length == 25))
			{
				throw new ApplicationException($"Длинна адреса неверная {AddressFull25Bytes.Length}. Должно быть 21 или 25");
			}

			if (AddressFull25Bytes.Length == 25)
			{
				AddressShort21Bytes = AddressFull25Bytes.SubArray(0, 21);
			}

		    if (addressFull25Bytes.Length == 21)
		    {
		        byte[] twiceHash = Sha256.HashTwice(addressFull25Bytes.SubArray(0, 21));
		        byte[] checkSum = twiceHash.SubArray(0, 4);

                byte[] addrss25 = new byte[25];
                Array.Copy(addressFull25Bytes, 0, addrss25, 0 ,21 ); 
                Array.Copy(checkSum, 0, addrss25, 21 , 4 );

		        this.AddressFull25Bytes = addrss25;
		    }


		}

		public string AddressBase58
		{get { return Base58.Encode(AddressFull25Bytes); }
		}

		public static bool ValidateAddress(string addresString)
		{
			try
			{
				CreateTronAddress(addresString);
			}
			catch (ParseAddressException)
			{
				return false;
			}

			return true;
		}

		public static bool isBelongsPublicKeyToPrivateKey(string privateKey, string publicKey)
		{
			KeyTriple keyTriple = new KeyTriple(privateKey);

			bool isValidPublicAddress = TronAddress.ValidateAddress(publicKey);
			if (!isValidPublicAddress) return false;

			TronAddress tronAddress = TronAddress.CreateTronAddress(publicKey);
			string addressFromPublickKeyBase58 = tronAddress.AddressBase58;

			string addressFromPrivateKeyBase58 = Base58.Encode(keyTriple.GetAddressWallet(TypeNet.Main));
			if (addressFromPublickKeyBase58 != addressFromPrivateKeyBase58) return false;

			return true;
		}

		public static TronAddress CreateTronAddress(string addressString)
		{
			byte[] addressBytes = null;

			try
			{
				addressBytes = StringHelper.GetBytesAdressFromString(addressString);
			}
			catch (ArgumentOutOfRangeException argumentOutOfRangeException)
			{
				throw new ParseAddressException(ParseAddressException.CodeEnum.Format ,"Строка не явялется строкой в формате Base58, Base64, Hex.", argumentOutOfRangeException);
			}

		    return CreateTronAddress(addressBytes);

		}

	    public static TronAddress CreateTronAddress(byte[] addressBytes)
	    {
	        if (!(addressBytes.Length == 21 || addressBytes.Length == 25))
	        {
	            throw new ParseAddressException(ParseAddressException.CodeEnum.Lenght, $"Длинна адреса неверная {addressBytes.Length}. Должно быть 21 или 25");
	        }

	        if (!(addressBytes[0] == 160 || addressBytes[0] == 65))
	        {
	            throw new ParseAddressException(ParseAddressException.CodeEnum.FirstByte, $"Первый байт адресса должен иметь значение 160d mainnet или 65d testnet.");
	        }

	        // есть чексумма будем ее проверять
	        if (addressBytes.Length == 25)
	        {
	            byte[] twiceHash = Sha256.HashTwice(addressBytes.SubArray(0, 21));
	            byte[] checkSum = twiceHash.SubArray(0, 4);

	            bool validateAddressWithCheckSumm = addressBytes[21] == checkSum[0]
	                                                && addressBytes[22] == checkSum[1]
	                                                && addressBytes[23] == checkSum[2]
	                                                && addressBytes[24] == checkSum[3];

	            if (!validateAddressWithCheckSumm) throw new ParseAddressException(ParseAddressException.CodeEnum.CheckSumm, "Чек сумма у адресаа не совпадает");
	        }

	        return new TronAddress(addressBytes);
	    }
	    public override bool Equals(object other)
	    {
	        if (other == null)
	            return false;

	        if (object.ReferenceEquals(this, other))
	            return true;

	        if (this.GetType() != other.GetType())
	            return false;

	        return this.Equals(other as TronAddress);
	    }
	    public bool Equals(TronAddress other)
	    {
	        if (other == null)
	            return false;

	        if (object.ReferenceEquals(this, other))
	            return true;

	        //то проверять на идентичность необязательно и можно переходить сразу к сравниванию полей.
	        if (this.GetType() != other.GetType())
	            return false;

	        if (string.Compare(this.AddressBase58, other.AddressBase58, StringComparison.CurrentCulture) == 0 )
	            return true;
	        else
	            return false;
	    }
    }
}