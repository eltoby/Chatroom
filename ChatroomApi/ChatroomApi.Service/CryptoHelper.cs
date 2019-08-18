namespace ChatroomApi.Service
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class CryptoHelper
    {
        public static string GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return BitConverter.ToString(salt);
        }

        public static string EncodePassword(string pass, string salt) //encrypt password    
        {
            var bytes = Encoding.Unicode.GetBytes(pass);
            var src = Encoding.Unicode.GetBytes(salt);
            var dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            using (var algorithm = HashAlgorithm.Create("SHA1"))
            {
                byte[] inArray = algorithm.ComputeHash(dst);
                return EncodePasswordMd5(Convert.ToBase64String(inArray));
            }
        }

        public static string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            byte[] originalBytes;
            byte[] encodedBytes;
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                originalBytes = ASCIIEncoding.Default.GetBytes(pass);
                encodedBytes = md5.ComputeHash(originalBytes);
            }
            return BitConverter.ToString(encodedBytes);
        }
    }
}
