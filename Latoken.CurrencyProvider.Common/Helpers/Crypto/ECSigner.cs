using System;
using System.IO;
using System.Text;
using HashLib;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Latoken.CurrencyProvider.Common.Helpers.Crypto
{
	public class ECSigner
	{
		private AsymmetricKeyParameter privateKey;
		private AsymmetricKeyParameter publicKey;
		public ECKeyPair _keyPair;

		public ECSigner()
		{
			_keyPair = new ECKeyPair();

			this.PublicKey = _keyPair.PublicKey;
			this.PrivateKey = _keyPair.PrivateKey;
		}

		public ECSigner(string privateKeyHex)
		{
			_keyPair = new ECKeyPair(privateKeyHex);

			this.PublicKey = _keyPair.PublicKey;
			this.PrivateKey = _keyPair.PrivateKey;
		}

		public ECSigner(string publicKey, string privateKey)
		{
			this.PublicKey = publicKey;
			this.PrivateKey = privateKey;
		}

		public ECSigner(ECKeyPair keyPair)
			: this(keyPair.PublicKey, keyPair.PrivateKey)
		{
		}

		public string PrivateKey
		{
			get
			{
				TextWriter writer = (TextWriter)new StringWriter();
				new PemWriter(writer).WriteObject((object)this.privateKey);
				return writer.ToString();
			}
			set
			{
				this.privateKey = ((AsymmetricCipherKeyPair)new PemReader((TextReader)new StringReader(value)).ReadObject()).Private;
			}
		}

		public string PublicKey
		{
			get
			{
				TextWriter writer = (TextWriter)new StringWriter();
				new PemWriter(writer).WriteObject((object)this.publicKey);
				return writer.ToString();
			}
			set
			{
				StringReader stringReader = new StringReader(value);
				PemReader pemReader = new PemReader((TextReader)stringReader);

				this.publicKey = (AsymmetricKeyParameter)pemReader.ReadObject();
			}
		}

		public bool IsSignatureValid(string message, string signatureBase64)
		{
			ISigner signer = SignerUtilities.GetSigner("ECDSA");
			signer.Init(false, (ICipherParameters)this.publicKey);
			byte[] signature1 = Convert.FromBase64String(signatureBase64);
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			signer.BlockUpdate(bytes, 0, bytes.Length);
			return signer.VerifySignature(signature1);
		}

		public bool IsSignatureValidFromBytes(byte[] message, byte[] signature, byte[] publicKeyBytes)
		{
			ISigner signer = SignerUtilities.GetSigner("ECDSA");
			DerObjectIdentifier _curve = SecObjectIdentifiers.SecP256r1;
			X9ECParameters curve = SecNamedCurves.GetByOid(_curve);
			ECPoint point = curve.Curve.DecodePoint(publicKeyBytes);
			ECDomainParameters ecP = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
			ECPublicKeyParameters ecPublicKeyParameters = new ECPublicKeyParameters(point, ecP);
			signer.Init(false, ecPublicKeyParameters);
			signer.BlockUpdate(message, 0, message.Length);
			return signer.VerifySignature(signature);
		}

		public string Sign(string message)
		{
			ISigner signer = SignerUtilities.GetSigner("ECDSA");
			signer.Init(true, (ICipherParameters)this.privateKey);
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			signer.BlockUpdate(bytes, 0, bytes.Length);
			return Convert.ToBase64String(signer.GenerateSignature());
		}

		public byte[] Sign(byte[] message)
		{
			ISigner signer = SignerUtilities.GetSigner("ECDSA");
			signer.Init(true, (ICipherParameters)this.privateKey);
			//	byte[] bytes = Encoding.UTF8.GetBytes(message);
			signer.BlockUpdate(message, 0, message.Length);
			return signer.GenerateSignature();
		}

		[Obsolete("Deprecated: please use IsSignatureValid")]
		public bool Verify(string message, string signature)
		{
			return this.IsSignatureValid(message, signature);
		}
	}
}