using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Latoken.CurrencyProvider.Common.Helpers.Crypto
{
	public class ECKeyPair
	{
		public ECKeyPair()
			: this(256)
		{
		}

		public ECKeyPair(int keylength)
		{
			ECKeyPairGenerator keyPairGenerator = new ECKeyPairGenerator();
			keyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), keylength));
			AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();

			TextWriter writer1 = (TextWriter)new StringWriter();
			PemWriter pemWriter1 = new PemWriter(writer1);
			pemWriter1.WriteObject((object)keyPair.Private);
			pemWriter1.Writer.Flush();
			this.PrivateKey = writer1.ToString();

			TextWriter writer2 = (TextWriter)new StringWriter();
			PemWriter pemWriter2 = new PemWriter(writer2);
			pemWriter2.WriteObject((object)keyPair.Public);
			pemWriter2.Writer.Flush();
			this.PublicKey = writer2.ToString();
		}


		public ECKeyPair(string privateKeyHex)
		{
			AsymmetricCipherKeyPair keyPair;

			X9ECParameters Params = SecNamedCurves.GetByName("secp256k1");
			ECDomainParameters Curve = new ECDomainParameters(Params.Curve, Params.G, Params.N, Params.H);

			BigInteger bigInteger = new BigInteger(privateKeyHex, 16);
			ECPrivateKeyParameters ecPrivateKeyParameters = new ECPrivateKeyParameters(bigInteger, Curve);
			var q = Curve.G.Multiply(bigInteger);
			ECPublicKeyParameters ecPublicKeyParameters = new ECPublicKeyParameters(q, Curve);
			ECPublicKey = ecPublicKeyParameters;
			ECPrivateKey = ecPrivateKeyParameters;

			keyPair = new AsymmetricCipherKeyPair(
				(AsymmetricKeyParameter)ecPublicKeyParameters,
				(AsymmetricKeyParameter)ecPrivateKeyParameters);


			TextWriter writer1 = (TextWriter)new StringWriter();
			PemWriter pemWriter1 = new PemWriter(writer1);
			pemWriter1.WriteObject((object)keyPair.Private);
			pemWriter1.Writer.Flush();
			this.PrivateKey = writer1.ToString();

			TextWriter writer2 = (TextWriter)new StringWriter();
			PemWriter pemWriter2 = new PemWriter(writer2);
			pemWriter2.WriteObject((object)keyPair.Public);
			pemWriter2.Writer.Flush();
			this.PublicKey = writer2.ToString();
		}

		public ECPublicKeyParameters ECPublicKey { get; set; }
		public ECPrivateKeyParameters ECPrivateKey { get; set; }
		public string PrivateKey { get; set; }

		public string PublicKey { get; set; }
	}
}