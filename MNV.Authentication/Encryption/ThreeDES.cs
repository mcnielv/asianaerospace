using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MNV.Infrastructure.Encryption
{
    public class ThreeDES
    {
        private static string PassPhrase
        {
            get
            {
                return "key";
            }
        }
        public static string Encrypt( string input )
        {
            byte[] results;
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash( utf8.GetBytes( PassPhrase ) );
            TripleDESCryptoServiceProvider tdesAlgo = new TripleDESCryptoServiceProvider();
            tdesAlgo.Key = tdesKey;
            tdesAlgo.Mode = CipherMode.ECB;
            tdesAlgo.Padding = PaddingMode.PKCS7;
            byte[] dataToEncrypt = utf8.GetBytes( input );
            try
            {
                ICryptoTransform encryptor = tdesAlgo.CreateEncryptor();
                results = encryptor.TransformFinalBlock( dataToEncrypt, 0, dataToEncrypt.Length );
            }
            finally
            {
                tdesAlgo.Clear();
                hashProvider.Clear();
            }
            return Convert.ToBase64String( results );
        }
        public static string Decrypt( string input )
        {
            byte[] results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash( UTF8.GetBytes( PassPhrase ) );
            TripleDESCryptoServiceProvider tdesAlgo = new TripleDESCryptoServiceProvider();
            tdesAlgo.Key = TDESKey;
            tdesAlgo.Mode = CipherMode.ECB;
            tdesAlgo.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String( input );
            try
            {
                ICryptoTransform Decryptor = tdesAlgo.CreateDecryptor();
                results = Decryptor.TransformFinalBlock( DataToDecrypt, 0, DataToDecrypt.Length );
            }
            finally
            {
                tdesAlgo.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString( results );
        }
    }
}
