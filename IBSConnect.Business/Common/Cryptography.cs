using System;
using System.Security.Cryptography;
using System.Text;

namespace IBSConnect.Business.Common;

public static class Cryptography
{
    /// <summary>
    /// Generates a hash of <see cref="size"/> bytes with a password and a salt using <see cref="Rfc2898DeriveBytes"/>
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static byte[] Hash(string password, byte[] salt, int size = 20)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        return pbkdf2.GetBytes(size);
    }

    /// <summary>
    /// Generates a set of <see cref="size"/> bytes random bytes using <see cref="RNGCryptoServiceProvider"/>
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static byte[] Salt(int size = 16)
    {
        return RandomNumberGenerator.GetBytes(size);
    }

    public static string Sign(string message, RSAParameters privateKey)
    {
        //// The array to store the signed message in bytes
        byte[] signedBytes;
        using (var rsa = new RSACryptoServiceProvider())
        {
            //// Write the message to a byte array using UTF8 as the encoding.
            var encoder = new UTF8Encoding();
            byte[] originalData = encoder.GetBytes(message);

            try
            {
                //// Import the private key used for signing the message
                rsa.ImportParameters(privateKey);

                //// Sign the data, using SHA512 as the hashing algorithm 
                signedBytes = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA512"));
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                //// Set the keycontainer to be cleared when rsa is garbage collected.
                rsa.PersistKeyInCsp = false;
            }
        }
        //// Convert the a base64 string before returning
        return Convert.ToBase64String(signedBytes);
    }

    public static bool Verify(string originalMessage, string signedMessage, RSAParameters publicKey)
    {
        bool success = false;
        using (var rsa = new RSACryptoServiceProvider())
        {
            var encoder = new UTF8Encoding();
            byte[] bytesToVerify = encoder.GetBytes(originalMessage);
            byte[] signedBytes = Convert.FromBase64String(signedMessage);
            try
            {
                rsa.ImportParameters(publicKey);
                success = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
        return success;
    }
}