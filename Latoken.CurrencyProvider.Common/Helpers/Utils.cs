using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Latoken.CurrencyProvider.Common.Helpers.Crypto
{
	public static class Utils
	{
		public static string ToHexString2(this byte[] bytes)
		{
			var hex = new StringBuilder(bytes.Length * 2);
			foreach (var b in bytes)
			{
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString().ToUpper();
		}


		public static byte[] FromHexToByteArray2(this string input)
		{
			var numberChars = input.Length;
			var bytes = new byte[numberChars / 2];
			for (var i = 0; i < numberChars; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
			}
			return bytes;
		}
	}
}