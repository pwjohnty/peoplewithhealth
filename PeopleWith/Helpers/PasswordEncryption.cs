using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class PasswordEncryption
    {
        public async Task<string> GetHashAsHex(string data)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    // Compute MD5 hash
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] hash = md5.ComputeHash(dataBytes);

                    // Convert hash to hex string
                    var hex = new StringBuilder(hash.Length * 2);
                    foreach (byte b in hash)
                    {
                        hex.AppendFormat("{0:x2}", b);
                    }

                    return hex.ToString();
                }
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        private async Task<string> HashPasswordAsync(string password)
        {
            try
            {
#pragma warning disable CS8603 // Possible null reference return.
                return await Task.Run(() =>
            {
    
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                        byte[] hashBytes = md5.ComputeHash(inputBytes);

                        // Convert the byte array to a hexadecimal string
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < hashBytes.Length; i++)
                        {
                            sb.Append(hashBytes[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }
                
            });
#pragma warning restore CS8603 // Possible null reference return.
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
    }
}
