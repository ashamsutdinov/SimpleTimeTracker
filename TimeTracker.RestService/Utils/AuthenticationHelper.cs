using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Text;

namespace TimeTracker.RestService.Utils
{
    internal class AuthenticationHelper
    {
        private readonly ConcurrentDictionary<string, string> _nonces = new ConcurrentDictionary<string, string>();

        private readonly string _apiKey;

        public AuthenticationHelper()
        {
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
        }

        public string GetNonce(string clientId)
        {
            var nonce = Guid.NewGuid().ToString();
            _nonces[clientId] = nonce;
            return nonce;
        }

        public bool VerifyNonce(string clientId, string testNonce)
        {
            string nonce;
            _nonces.TryRemove(clientId, out nonce);
            return nonce == testNonce;
        }

        public string Xor(string message)
        {
            var data = Convert.FromBase64String(message);
            var text = Encoding.UTF8.GetString(data);
            var result = new StringBuilder();
            for (var c = 0; c < text.Length; c++)
                result.Append((char)((uint)text[c] ^ _apiKey[c % _apiKey.Length]));
           return result.ToString();
        }

        public string GetTicket(int sessionId, string clientId, string apiKey)
        {
            return string.Empty;
        }
    }
}
