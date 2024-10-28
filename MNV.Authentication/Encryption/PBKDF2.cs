using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MNV.Infrastructure.Encryption
{
    public class PBKDF2
    {
        public static string Encrypt(string password )
        {
            byte[] salt;
            byte[] bytes;
            if ( password == null )
            {
                throw new ArgumentNullException( "password" );
            }
            using ( Rfc2898DeriveBytes rfc2898DeriveByte = new Rfc2898DeriveBytes( password, 16, 1000 ) )
            {
                salt = rfc2898DeriveByte.Salt;
                bytes = rfc2898DeriveByte.GetBytes( 32 );
            }
            byte[] numArray = new byte[49];
            Buffer.BlockCopy( salt, 0, numArray, 1, 16 );
            Buffer.BlockCopy( bytes, 0, numArray, 17, 32 );
            return Convert.ToBase64String( numArray );
        }
    }
}
