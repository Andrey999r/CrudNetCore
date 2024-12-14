using Smth.Data;
using Smth.Interfaces;

namespace Smth.Services
{
    public class AuthService : IAuth
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Username and password cannot be empty.");

            if (_context.Users.Any(u => u.Username == username))
                throw new InvalidOperationException("Username already exists.");

            var user = new ApplicationUser
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) 
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public ApplicationUser Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }
    }
}


