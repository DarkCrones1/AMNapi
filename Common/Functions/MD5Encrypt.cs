using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AMNApi.Common.Functions;

public class MD5Encrypt
{
    public static string GetMD5(string str)
    {
        byte[] input = Encoding.ASCII.GetBytes(str);
        byte[] hash = MD5.HashData(input);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
