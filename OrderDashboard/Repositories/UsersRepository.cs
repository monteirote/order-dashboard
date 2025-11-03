using Microsoft.EntityFrameworkCore;
using OrderDashboard.Database;
using OrderDashboard.Database.Entities;

namespace OrderDashboard.Repositories
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByEmailAsync (string email);

        Task AddUserAsync (Users user);

        Task<bool> UserExistsAsync (string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly MainContext _context;

        public UserRepository(MainContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetUserByEmailAsync (string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task AddUserAsync (Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync (string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower()));
        }
    }

}
