using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Latoken.CurrencyProvider.Common.Helpers.Crypto;
using Org.BouncyCastle.Utilities.Encoders;

namespace Latoken.CurrencyProvider.Common.Helpers
{
    public class StringHelper
    {
	    public static byte[] HexStringToByteArray(string hex)
	    {
		    return Enumerable.Range(0, hex.Length)
			    .Where(x => x % 2 == 0)
			    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
			    .ToArray();
	    }

	    static public byte[] GetBytesFromStirng(string @string, StringType stringType)
	    {
		    byte[] bytes = null;
		    switch (stringType)
		    {
				case StringType.Hex:
					bytes = HexStringToByteArray(@string);
					break;
				case StringType.Base58:
					bytes = Base58.Decode(@string);
					break;
				case StringType.Base64:
					bytes = Base64.Decode(@string);
					break;
		    }
		    return bytes;
	    }

	    public static byte[] GetBytesAdressFromString(string addressString)
	    {
		    byte[] addressBytes = null;

		    try
		    {
			    addressBytes = StringHelper.HexStringToByteArray(addressString);
		    }
		    catch (Exception) { }
		    finally { }

		    if (addressBytes != null) return addressBytes;

			try
		    {
			    addressBytes = Base58.Decode(addressString);
		    }
		    catch (Exception){}
		    finally
		    {
		    }

		    if (addressBytes != null) return addressBytes;

		    try
		    {
			    addressBytes = Base64.Decode(addressString);
		    }
		    catch (Exception) { }
			finally {}

		    if (addressBytes != null) return addressBytes;

		   


		    throw new ArgumentOutOfRangeException($"Не возможно создать адресс из строки {addressString}");
	    }
    }

	public enum StringType
	{
		Unknown,
		String,
		Hex,
		Base58,
		Base64
	}
}