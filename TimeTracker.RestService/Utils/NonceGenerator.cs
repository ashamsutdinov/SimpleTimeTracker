using System;
using System.Collections.Concurrent;

namespace TimeTracker.RestService.Utils
{
    public class NonceGenerator
    {
        private readonly ConcurrentDictionary<string, string> _nonces = new ConcurrentDictionary<string, string>(); 

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
    }
}
