using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace Latoken.CurrencyProvider.Common.Helpers.Crypto
{
	public class ECKey
	{
		private const string CurveName = "secp256k1";

		public static readonly ECDomainParameters Curve;
		public static readonly SecureRandom SecureRandom;
		public static readonly X9ECParameters Params;

		public ECPoint Pub { get; }
		public readonly ECPrivateKeyParameters _privateKey;


		static ECKey()
		{

			Params = SecNamedCurves.GetByName(CurveName);
			Curve = new ECDomainParameters(Params.Curve, Params.G, Params.N, Params.H);
			SecureRandom = new SecureRandom();
		}

		public ECKey()
		{
			var parameters = new ECDomainParameters(Params.Curve, Params.G, Params.N, Params.H);
			var generator = new ECKeyPairGenerator();
			generator.Init(new ECKeyGenerationParameters(parameters, SecureRandom));
			var pair = generator.GenerateKeyPair();
			Pub = ((ECPublicKeyParameters)pair.Public).Q;
			_privateKey = (ECPrivateKeyParameters)pair.Private;
		}


		public ECKey(BigInteger privateKey, ECPoint publicPoint)
		{
			_privateKey = new ECPrivateKeyParameters(new BigInteger(privateKey.ToString()), Curve);
			Pub = publicPoint;

		}

		public byte[] GetSignature(byte[] messageBytes)
		{
			var curve = SecNamedCurves.GetByName("secp256k1");
			var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

			var keyParameters = new ECPrivateKeyParameters(_privateKey.D, domain);

			ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");

			signer.Init(true, keyParameters);
			signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
			var signature = signer.GenerateSignature();

			return signature;
		}

		public static ECKey FromPrivate(BigInteger privKey)
		{
			return new ECKey(privKey, Curve.G.Multiply(privKey));
		}

		public static ECKey FromPrivate(byte[] privKeyBytes)
		{
			return FromPrivate(new BigInteger(1, privKeyBytes));
		}

		public static ECKey FromPrivateHexString(string privateKeyStr)
		{
			var bytes =  StringHelper.HexStringToByteArray(privateKeyStr);
			return FromPrivate(new BigInteger(1, bytes));
		}
	}
}
