// See https://aka.ms/new-console-template for more information

using System.Reflection;
using ServiceSignerBase;
using ServiceSignerBase.Data;
using ServiceSignerBase.Extentions;
using ServiceSignerBase.Signers;
using System.Text.Json;

namespace SignTest
{

    public class Program
    {
        private static void AttributeTest()
        {
            SomeModel model = new SomeModel()
            {
                Name = "farid",
                Surname = "Ismayilzada",
                TestData = "Test",
                InnerModel = new InnerModel()
                {
                    Year = "2022",
                    HidedObject = new ThirdObject { HidedName = "Secret", ModelSam = new InnerModel { Id = "12" } }
                }
            };

           
            

            SrvSignedContainer<SomeModel> srvSignedContainer = new SrvSignedContainer<SomeModel>(model, "name/surname");

            var result = AttributeHelper.GetPropertiesInfo(model);
            var testdata = AttributeHelper.GetPropValue(model, "name");
            var testdata1 = AttributeHelper.GetPropValue(model, "innermodel.Year");
            var testdata2 = AttributeHelper.GetPropValue(model, "innermodel.HidedObject.HidedName");
            var testdata3 = AttributeHelper.GetPropValue(model, "innermodel.HidedObject.ModelSam.id");

            //PropertyInfo info = model.GetType().GetProperty("InnerModel.Year");
           // Object val = info.GetValue(model, null);

            
            AttributeHelper.GetSignableProperties(model);

        }

        private static void TestServiceSignerWithMOdel()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");


            var privstring = keypair.Private.ToPembase64String();
            var pubstring = keypair.Public.ToPembase64String();
            ServiceSigner signer = new ServiceSigner(privstring, pubstring);

            SomeModel model = new SomeModel()
            {
                Name = "farid",
                Surname = "Ismayilzada",
                TestData = "Test",
                InnerModel = new InnerModel()
                {
                    Year = "2022",
                    HidedObject = new ThirdObject { HidedName = "Secret" }
                }
            };

            var rs = signer.SignDataModel(model);

            var text = JsonSerializer.Serialize(rs);

            signer.ValidateSignatureContainer(rs,pubstring);

        }
        private static void TestServiceSigner()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");


            var privstring = keypair.Private.ToPembase64String();
            var pubstring = keypair.Public.ToPembase64String();


            var stringsignature = servicesigner.SignData("test".ToByteArray().ToBase64String(), privstring, signalg);


            Console.WriteLine(stringsignature);

            servicesigner.VerifySignature("test".ToByteArray().ToBase64String(), stringsignature, pubstring, signalg);

            Console.WriteLine("OK");

        }

        [STAThread]
        private static void Main(string[] args)
        {
           TestServiceSignerWithMOdel();
            AttributeTest();
            TestServiceSigner();
            /*
			
			// the 'usual' usage of keypair in "textbook" RSA (which is an asymmetric algorithm) is:
			// public key  = ENCRYPTING / VERIFYING
			// private key = DECRYPTING / SIGNING
			var rsav = new RsaSignAndVerify();
			var keyPair = rsav.GenerateRandomKeyPair();

			// print keys to console 
			rsav.PrintKeys(keyPair);

			// content we like to sign
			var textToSign = "Data to sign. can be a file or anything :-)";

			// display data we are about to sign
			Console.WriteLine($"Text to sign: {textToSign}");

			// server side - generates the signature by using the PRIVATE key
			var signature = rsav.ServerGenerateSignature(textToSign, (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)keyPair.Private);

			// client side - validates the signature by using the PUBLIC key
			var isSignatureValid = rsav.ClientValidateSignature(textToSign, signature, (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)keyPair.Public);
			Console.WriteLine($"Signature isValid: {isSignatureValid}");

			*/

            string algname = "rsa";


            IServiceSigner serviceSigner = Util.GetSigner(algname);

            var kp = (IKeyGenerator)serviceSigner;

            var kpr = kp.GenerateServiceKeyPair(2048);

            string prv = kpr.Private.ToPembase64String();

            string pub = kpr.Public.ToPembase64String();




            string signalg = "SHA-256withRSA";

            string tosign = "testing";


            var privkey = prv.ToPRivateKey();
            var pubkey = pub.ToPublicKey();

            var tosignbytes = tosign.ToByteArray();

            var sng = serviceSigner.Sign(tosignbytes, privkey, signalg);

            var signature1 = sng.ToBase64String();

            Console.WriteLine($"Signature :{signature1}");


            bool verify = serviceSigner.Verify(tosignbytes, sng, pubkey, signalg);

            Console.WriteLine($"Verify Result : {verify}");





            Console.ReadLine();








        }
    }

}