using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using TimeTracker.Contract.Requests;

namespace TimeTracker.Contract.Utils
{
    internal class CryptographyHelper
    {
        private readonly ConcurrentDictionary<string, string> _nonces = new ConcurrentDictionary<string, string>();

        private readonly string _apiKey;

        private readonly string _privateKey;

        private readonly MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();

        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private readonly Random _random = new Random((int)DateTime.UtcNow.Ticks);

        private const int SaltLength = 6;

        public CryptographyHelper()
        {
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _privateKey = ConfigurationManager.AppSettings["PrivateKey"];
        }

        public string GetNonce(string clientId)
        {
            var nonce = Guid.NewGuid().ToString();
            _nonces[clientId] = nonce;
            return nonce;
        }

        public bool VerifyPasswordSyntax(string password, out DecryptedPasswordData passwordData)
        {
            passwordData = DecodePassword(password);
            string nonce;
            if (_nonces.TryGetValue(passwordData.ClientId, out nonce))
            {
                return nonce == passwordData.Nonce;
            }
            return false;
        }

        public string XorDecode(string message, string key)
        {
            var data = Convert.FromBase64String(message);
            var text = Encoding.UTF8.GetString(data);
            return Xor(text, key);
        }

        public DecryptedPasswordData DecodePassword(string password)
        {
            var decoded = XorDecode(password, _apiKey).Split('#');
            return new DecryptedPasswordData
            {
                Password = decoded[0],
                ClientId = decoded[1],
                Nonce = decoded[2]
            };
        }

        public DecryptedTicketData DecodeTicket(string ticket)
        {
            var decoded = XorDecode(ticket, _privateKey).Split('#');
            return new DecryptedTicketData
            {
                Id = int.Parse(decoded[0]),
                UserId = int.Parse(decoded[1]),
                ClientId = decoded[2],
                TicketSalt = decoded[3],
                TicketHash = decoded[4]
            };
        }

        public string XorEncode(string message, string key)
        {
            var text = Xor(message, key);
            var data = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(data);
        }

        private static string Xor(string text, string key)
        {
            var result = new StringBuilder();
            for (var c = 0; c < text.Length; c++)
            {
                result.Append((char) ((uint) text[c] ^ key[c%key.Length]));
            }

            return result.ToString();
        }

        private string MD5(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var hashData = _md5.ComputeHash(data);
            return Encoding.UTF8.GetString(hashData);
        }

        public string GetRandomString(int length)
        {
            var stringChars = new char[length];
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = Chars[_random.Next(Chars.Length)];
            }
            var finalString = new string(stringChars);
            return finalString;
        }

        public string GetSessionTicket(int id, int userId, string clientId)
        {
            var ticketSalt = GetRandomString(SaltLength);
            var ticket = string.Format("{0}#{1}#{2}#{3}", id, userId, clientId, ticketSalt);
            var hash = MD5(ticket);
            var ticketWithHash = string.Format("{0}#{1}#{2}#{3}#{4}", id, userId, clientId, hash, ticketSalt);
            return XorEncode(ticketWithHash, _privateKey);
        }

        public bool VerifyTicketSyntax(string ticket, out DecryptedTicketData data)
        {
            data = DecodeTicket(ticket);
            var decoded = string.Format("{0}#{1}#{2}#{3}", data.Id, data.UserId, data.ClientId, data.TicketSalt);
            return data.TicketHash == MD5(decoded);
        }
    }
}
