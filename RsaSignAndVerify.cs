using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionItems
{
    public class RsaSignAndVerify
    {

		public AsymmetricCipherKeyPair GenerateRandomKeyPair()
		{
			var rsaKeyPairGen = new RsaKeyPairGenerator();
			rsaKeyPairGen.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
			return rsaKeyPairGen.GenerateKeyPair(); ;
		}

		public bool ClientValidateSignature(string sourceData, byte[] signature, RsaKeyParameters publicKey)
		{
			byte[] tmpSource = Encoding.ASCII.GetBytes(sourceData);

			ISigner signClientSide = SignerUtilities.GetSigner(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id);
			signClientSide.Init(false, publicKey);
			signClientSide.BlockUpdate(tmpSource, 0, tmpSource.Length);

			return signClientSide.VerifySignature(signature);
		}

		public byte[] ServerGenerateSignature(string sourceData, RsaKeyParameters privateKey)
		{
			byte[] tmpSource = Encoding.ASCII.GetBytes(sourceData);

			ISigner sign = SignerUtilities.GetSigner(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id);
			sign.Init(true, privateKey);
			sign.BlockUpdate(tmpSource, 0, tmpSource.Length);
			return sign.GenerateSignature();
		}

		public void PrintKeys(AsymmetricCipherKeyPair keyPair)
		{
			using (TextWriter textWriter1 = new StringWriter())
			{
				var pemWriter1 = new PemWriter(textWriter1);
				pemWriter1.WriteObject(keyPair.Private);
				pemWriter1.Writer.Flush();

				string privateKey = textWriter1.ToString();
				Console.WriteLine(privateKey);
			}

			using (TextWriter textWriter2 = new StringWriter())
			{
				var pemWriter2 = new PemWriter(textWriter2);
				pemWriter2.WriteObject(keyPair.Public);
				pemWriter2.Writer.Flush();
				string publicKey = textWriter2.ToString();
				Console.WriteLine(publicKey);
			}
		}

		private byte[] ConvertHexString(string hexString)
		{
			byte[] data = new byte[hexString.Length / 2];
			for (int index = 0; index < data.Length; index++)
			{
				string byteValue = hexString.Substring(index * 2, 2);
				data[index] = byte.Parse(byteValue, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
			}

			return data;
		}
	}
}
