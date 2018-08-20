using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Numerics;
using HashLib;
using Latoken.CurrencyProvider.Common.Helpers.Crypto.Sha;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Latoken.CurrencyProvider.Common.Helpers.Crypto
{
	public class ECKeySign
	{
		public static bool VerifySignature(string message, string publicKey, string signature)
		{
			var curve = SecNamedCurves.GetByName("secp256k1");
			var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

			var publicKeyBytes = publicKey.FromHexToByteArray2();

			var q = curve.Curve.DecodePoint(publicKeyBytes);

			var keyParameters = new
				Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters(q,
					domain);

			ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

			signer.Init(false, keyParameters);
			signer.BlockUpdate(Encoding.ASCII.GetBytes(message), 0, message.Length);

			var signatureBytes = Base58.Decode(signature);

			return signer.VerifySignature(signatureBytes);
		}


		public static bool VerifySignature(byte[] message, byte[] publicKey, byte[] signature)
		{
			var curve = SecNamedCurves.GetByName("secp256k1");
			var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

			var publicKeyBytes = publicKey;

			var q = curve.Curve.DecodePoint(publicKeyBytes);

			var keyParameters = new
				Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters(q,
					domain);

			ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

			signer.Init(false, keyParameters);
			signer.BlockUpdate(message, 0, message.Length);

			var signatureBytes = signature;

			return signer.VerifySignature(signatureBytes);
		}


		public static string GetSignature(string privateKey, string message)
		{
			var curve = SecNamedCurves.GetByName("secp256k1");
			var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

			var keyParameters = new ECPrivateKeyParameters(new Org.BouncyCastle.Math.BigInteger(privateKey),domain);

			ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

			signer.Init(true, keyParameters);
			signer.BlockUpdate(Encoding.ASCII.GetBytes(message), 0, message.Length);
			var signature = signer.GenerateSignature();
			

			return Base58.Encode(signature);
		}

		public static string GetPublicKeyFromPrivateKeyEx(string privateKey)
		{
			var curve = SecNamedCurves.GetByName("secp256k1");
			var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

			var d = new Org.BouncyCastle.Math.BigInteger(privateKey, 16);
			var q = domain.G.Multiply(d);

			var publicKey = new ECPublicKeyParameters(q, domain);
			return Base58.Encode(publicKey.Q.GetEncoded());
		}

		public static byte[] GetPubAddress(string privateKey)
		{

			//	ECKey ecKey = new ECKey();
			//	ECPrivateKeyParameters ecPrivateKeyParameters = ecKey._privateKey;
			//	byte[] bytes = ecPrivateKeyParameters.D.ToByteArray();
			ECKeyPair ecKeyPair = new ECKeyPair(privateKey);

			byte[] bytesPub64 = ecKeyPair.ECPublicKey.Q.GetEncoded().SubArray(1,64);
			DZen.Security.Cryptography.SHA3 sha3 = DZen.Security.Cryptography.SHA3.Create("sha3-256");
			sha3.UseKeccakPadding = true;
			byte[] computeHash = sha3.ComputeHash(bytesPub64);

			//byte[] computeHash = Sha3.Sha3256().ComputeHash(bytesPub65,1,64);//.ComputeHash(bytes, 12, 20);
			computeHash.ToHexString2();

			byte[] computeHash20 = new byte[20];
			computeHash20 = computeHash.SubArray(11, 21);
			computeHash20.ToHexString2();

			computeHash20[0] = 65;
			byte[] address41 = computeHash20;

		//	byte[] hexStringToByteArray = StringHelper.HexStringToByteArray("415a523b449890854c8fc460ab602df9f31fe4293f");
		//	address41 = hexStringToByteArray;
			string hexString2 = address41.ToHexString2();

			byte[] SecondSHA = Sha256.HashTwice(address41);
			byte[] checkSum = SecondSHA.SubArray(0, 4);
			string SecondSHAhexString2 = SecondSHA.ToHexString2();
		
			byte[] address = new byte[address41.Length + 4];
			Array.Copy(address41, 0, address, 0, address41.Length);
			Array.Copy(checkSum, 0, address, 21, 4);

			string hexString = address.ToHexString2();
			string encode = Base58.Encode(address);
			//415a523b449890854c8fc460ab602df9f31fe4293f
			return address;
		}

		public static string GetPublicKeyFromPrivateKey(string privateKey)
		{
			var p = BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F",
				NumberStyles.HexNumber);
			var b = (BigInteger) 7;
			var a = BigInteger.Zero;
			var Gx = BigInteger.Parse("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798",
				NumberStyles.HexNumber);
			var Gy = BigInteger.Parse("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8",
				NumberStyles.HexNumber);

			CurveFp curve256 = new CurveFp(p, a, b);
			Point generator256 = new Point(curve256, Gx, Gy);

			var secret = BigInteger.Parse(privateKey, NumberStyles.HexNumber);
			var pubkeyPoint = generator256 * secret;
			return pubkeyPoint.X.ToString("X") + pubkeyPoint.Y.ToString("X");
		}
	}
}