namespace Latoken.CurrencyProvider.Common.Helpers.Crypto.Sha
{
	public static class Sha256Keccak
	{
		public static byte[] Hash(byte[] input)
		{
			DZen.Security.Cryptography.SHA3 sha3 = DZen.Security.Cryptography.SHA3.Create("sha3-256");
			sha3.UseKeccakPadding = true;
			byte[] hashBytes = sha3.ComputeHash(input);

			return hashBytes;
		}

		public static byte[] HashTwice(byte[] input)
		{
			return Hash(Hash(input));
		}
	}
}