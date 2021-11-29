using System.Threading.Tasks;
using FundRaiser.Common.Data;
using FundRaiser.Common.Dto;
using FundRaiser.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace FundRaiser.Common.Services
{
    public interface IUserService
    {
        Task<UserDto> Add(UserDto dto);
        Task<UserDto> Update(int id, UserDto dto);
        Task<bool> Delete(int id);
        Task<UserDto> GetUser(int id, bool baseInfo = true);
    }
    
    public class UserService : IUserService
    {
        private readonly FundRaiserDbContext _context;   
        
        public UserService(FundRaiserDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Add(UserDto dto)
        {
            var user = new User() 
            { 
                //Map base info
                FirstName = dto.FirstName, 
                LastName = dto.LastName
            };

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserDto(user);
        }
        
        public async Task<UserDto> Update(int id, UserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            //Map base info
            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserDto(user);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

            if (user == null) return false;

            _context.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> GetUser(int id, bool baseInfo = true)
        {
            var user = baseInfo 
                ? await _context.Users.FirstOrDefaultAsync(p => p.Id == id)
                : await _context.Users
                    .Include(u => u.Projects)
                    .Include(u => u.Funds)
                    .FirstOrDefaultAsync(u => u.Id == id);

            return user != null ? new UserDto(user) : null;
        }
    }
}