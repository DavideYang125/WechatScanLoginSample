using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Wiwi.Sample.Common.Extensions
{
    public static class SHA1Extensions
    {
        public static string GetSHA1Hash(this string value, string encode = "UTF-8")
        {
            var sha1 = new SHA1Managed();
            var sha1bytes = Encoding.GetEncoding(encode).GetBytes(value);
            byte[] resultHash = sha1.ComputeHash(sha1bytes);
            string sha1String = BitConverter.ToString(resultHash).ToLower();
            sha1String = sha1String.Replace("-", "");
            return sha1String;
        }
    }
}
