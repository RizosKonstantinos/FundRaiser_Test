using System.Threading.Tasks;
using FundRaiser.Common.Data;
using FundRaiser.Common.Dto;
using FundRaiser.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace FundRaiser.Common.Services
{
    public interface IUserService
    {
        Task<User> Add(User userModel);
        Task<User> Update(int id, User userModel);
        Task<bool> Delete(int id);
        Task<User> GetUser(int id, bool baseInfo = true);
    }
    
    public class UserService : IUserService
    {
        private readonly FundRaiserDbContext _context;   
        
        public UserService(FundRaiserDbContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User userModel)
        {
            var user = new User() 
            { 
                //Map base info
                FirstName = userModel.FirstName, 
                LastName = userModel.LastName
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        
        public async Task<User> Update(int id, User userModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            //Map base info
            user.FirstName = userModel.FirstName ?? user.FirstName;
            user.LastName = userModel.LastName ?? user.LastName;
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUser(int id, bool baseInfo = true)
        {
            var user = baseInfo 
                ? await _context.Users.FirstOrDefaultAsync(p => p.Id == id)
                : await _context.Users
                    .Include(u => u.Projects)
                    .Include(u => u.Funds)
                    .FirstOrDefaultAsync(u => u.Id == id);

            return user != null ? user : null;          
        }
    }
}