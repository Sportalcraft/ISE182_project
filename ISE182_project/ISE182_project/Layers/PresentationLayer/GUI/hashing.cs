using System;
using System.Text;
using System.Security.Cryptography;
using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using System.Reflection;

namespace ISE182_project.Layers.PresentationLayer.GUI
{
    public class hashing
    {
        private const int MIN_PASSWORD_LENGTH = 4; //Minimum password lenfth
        private const int MAX_PASSWORD_LENGTH = 16; //Maximum password lenfth
        private const string SALT = "1337";
        /// <summary>
        /// Compute hash as bytes from input string
        /// </summary>
        /// <param name="inputString">string to hash</param>
        /// <returns>hashed string represented as byte array</returns>
        private static byte[] GetHash(string inputString1)
        {
            string inputString = SALT + inputString1;
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

            /// <summary>
            ///
            /// Convert string to hash string
            ///A sha256 is 256 bits long -- as its name indicates. 
            ///If you are using an hexadecimal representation, each digit codes for 4 bits ;
            ///so you need 64 digits to represent 256 bits -- so, you need a varchar(64) ,
            ///or a char(64) , as the length is always the same, not varying at all.
            /// </summary>
            /// <param name="inputString">the string to hash</param>
            /// <returns>64 long string representation of the hashed input string</returns>
        public static string GetHashString(string inputString)
        {
            if (isValidPassword(inputString))
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in GetHash(inputString))
                    sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
            string error = "A user tried to use an invalid password!";
            Logger.Log.Error(Logger.Maintenance(error));
            throw new InvalidOperationException(error);
        }
        //check passwod validity
        private static bool isValidPassword(string Password)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return Password != null && //Not null
                Password.Length >= MIN_PASSWORD_LENGTH && // Not too short
                Password.Length <= MAX_PASSWORD_LENGTH; //Not too long
        }
    }
}
