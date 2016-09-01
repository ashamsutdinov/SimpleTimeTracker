using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Utils
{
    internal class CryptographyHelper
    {
        private readonly ConcurrentDictionary<string, string> _nonces = new ConcurrentDictionary<string, string>();

        private readonly string _apiKey;

        private readonly string _privateKey;

        private readonly MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();

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

        public bool VerifyPasswordSyntax(string password, out PasswordData passwordData)
        {
            passwordData = DecodeXorPassword(password);
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

        public string HashPassword(string password, string salt)
        {
            var stringToHash = password + salt;
            HashAlgorithm algorithm = new SHA256Managed();
            var passwordData = Encoding.UTF8.GetBytes(stringToHash);
            var hashed = algorithm.ComputeHash(passwordData);
            var hash = Convert.ToBase64String(hashed);
            return hash;
        }

        public PasswordData DecodeXorPassword(string password)
        {
            var decoded = XorDecode(password, _apiKey).Split('#');
            return new PasswordData
            {
                Password = decoded[0],
                ClientId = decoded[1],
                Nonce = decoded[2]
            };
        }

        public SessionData DecodeXorTicket(string ticket)
        {
            var decoded = XorDecode(ticket, _privateKey).Split('#');
            return new SessionData
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
            return Convert.ToBase64String(hashData);
        }

        public string CreateSalt(int length = SaltLength)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[length];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public string GetSessionTicket(int id, int userId, string clientId)
        {
            var ticketSalt = CreateSalt();
            var ticket = string.Format("{0}#{1}#{2}#{3}", id, userId, clientId, ticketSalt);
            var hash = MD5(ticket);
            var ticketWithHash = string.Format("{0}#{1}#{2}#{3}#{4}", id, userId, clientId,  ticketSalt, hash);
            return XorEncode(ticketWithHash, _privateKey);
        }

        public bool VerifyTicketSyntax(string ticket, out SessionData data)
        {
            data = DecodeXorTicket(ticket);
            var decoded = string.Format("{0}#{1}#{2}#{3}", data.Id, data.UserId, data.ClientId, data.TicketSalt);
            var hashDecoded = MD5(decoded);
            return data.TicketHash == hashDecoded;
        }
    }
}
