using System.Security.Cryptography;
using System.Text;

namespace OrderDashboard.Services
{
    public interface IPasswordService
    {
        bool VerifyPasswordHash (string password, string storedHash, string storedSalt);
        void CreatePasswordHash (string password, out string passwordHash, out string passwordSalt);
    }

    public class PasswordService : IPasswordService
    {
        public void CreatePasswordHash (string password, out string passwordHash, out string passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser nula ou vazia.", nameof(password));

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordHash = Convert.ToBase64String(computedHash);
            }
        }

        public bool VerifyPasswordHash (string password, string storedHash, string storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser nula ou vazia.", nameof(password));
            if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
                return false;

            var saltBytes = Convert.FromBase64String(storedSalt);
            var storedHashBytes = Convert.FromBase64String(storedHash);

            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHashBytes);
            }
        }
    }
}
