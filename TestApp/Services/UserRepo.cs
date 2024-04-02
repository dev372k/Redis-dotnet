using TestApp.DTOs;
using TestApp.Entities;

namespace TestApp.Services
{
    public interface IUserRepo
    {
        Task AddUser(AddUserDTO dto);
        Task UpdateUser(UpdateUserDTO dto);
        List<GetUserDTO> GetUsers();
        GetUserDTO? GetUser(int id);
        Task DeleteUser(int id);


    }
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDBContext _context;
        public UserRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task AddUser(AddUserDTO dto)
        {
            await _context.Users.AddAsync(new User
            {
                Name = dto.name,
                Email = dto.email   
            });

            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateUser(UpdateUserDTO dto)
        {
            var user = _context.Users.Where(x => x.Id == dto.id).FirstOrDefault();
            if (user == null) return;

            user.Name = dto.name;
            user.Email = dto.email;
            await _context.SaveChangesAsync();
        }

        public List<GetUserDTO> GetUsers()
        {
            return _context.Users.Select(x => new GetUserDTO
            {
                id = x.Id,
                name = x.Name,
                email = x.Email
            }).ToList();
        }
        
        public GetUserDTO? GetUser(int id)
        {
            return _context.Users.Where(x => x.Id == id).Select(x => new GetUserDTO
            {
                id = x.Id,
                name = x.Name,
                email = x.Email
            }).FirstOrDefault();
        }

        public async Task DeleteUser(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null) return;
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
