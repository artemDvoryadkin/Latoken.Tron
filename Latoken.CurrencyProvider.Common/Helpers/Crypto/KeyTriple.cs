using System;
using HashLib;
using Latoken.CurrencyProvider.Common.Configuration;
using Latoken.CurrencyProvider.Common.Helpers.Crypto.Sha;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Latoken.CurrencyProvider.Common.Helpers.Crypto
{
	public class KeyTriple
	{
		ECKeyPair _ecKeyPair;
		public KeyTriple(string privateKeyHex)
		{
			_ecKeyPair = new ECKeyPair(privateKeyHex);
		}

		public byte[] PublicKey
		{
			get
			{
				return _ecKeyPair.ECPublicKey.Q.GetEncoded();
			}
		}

		public byte[] PrivateKey
		{
			get { return _ecKeyPair.ECPrivateKey.D.ToByteArray(); }
		}

		public byte[] PublicKeyShort
		{
			get
			{
				return _ecKeyPair.ECPublicKey.Q.GetEncoded().SubArray(1, 64);
			}
		}

		public byte[] GetAddressWallet(TypeNet typeNet)
		{
			byte[] bytesPub64 = PublicKeyShort;

			byte[] computeHash = Sha256Keccak.Hash(bytesPub64);

			//computeHash.ToHexString2();

			byte[] computeHash20 = computeHash.SubArray(11, 21);
			//computeHash20.ToHexString2();
			if(typeNet == TypeNet.Main) computeHash20[0] = 65;
			else computeHash20[0] = 160;

			//string hexString2 = computeHash20.ToHexString2();

			byte[] secondSha = Sha256.HashTwice(computeHash20);
			byte[] checkSum = secondSha.SubArray(0, 4);
			//string SecondSHAhexString2 = secondSha.ToHexString2();

			byte[] address = new byte[computeHash20.Length + 4];
			Array.Copy(computeHash20, 0, address, 0, computeHash20.Length);
			Array.Copy(checkSum, 0, address, 21, 4);

			//string hexString = address.ToHexString2();
			//string encode = Base_58.Encode(address);

			return address;
		}


		

		public static bool ValidateAddressWithCheckSumm(byte[] address)
		{
			if (address.Length == 21) return true;//нет чекссуммы не можем проврить

			byte[] twiceHash = Sha256.HashTwice(address.SubArray(0, 21));
			byte[] checkSum = twiceHash.SubArray(0, 4);

			bool validateAddressWithCheckSumm = address[21] == checkSum[0]
			                                   && address[22] == checkSum[1]
			                                   && address[23] == checkSum[2]
			                                   && address[24] == checkSum[3];
			return validateAddressWithCheckSumm;
		}

		public byte[] GetSignature(byte[] messageBytes)
		{
			var curve = SecNamedCurves.GetByName("secp256k1");
			var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

			var keyParameters = new ECPrivateKeyParameters(_ecKeyPair.ECPrivateKey.D, domain);

			ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

			signer.Init(true, keyParameters);
			signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
			var signature = signer.GenerateSignature();

			return signature;
		}
	}
}