using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System;
using System.Text;

namespace IMS.Application.Secutiry
{
    public class Cryptography : ICryptography
    {
        private readonly CryptographyConfig _cryptographyConfig;

        public Cryptography(IOptions<CryptographyConfig> cryptographyConfig)
        {
            _cryptographyConfig = cryptographyConfig.Value;
        }

        public string DoCrypto(string password)
        {
            var salt = Encoding.UTF8.GetBytes(_cryptographyConfig.Salt);
            var crypto = KeyDerivation.Pbkdf2(password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA1, iterationCount: 10000, numBytesRequested: 256 / 8);
            return Convert.ToBase64String(crypto);
        }
    }
}
