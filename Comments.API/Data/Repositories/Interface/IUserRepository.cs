using Comments.API.Data.Models;
using Comments.API.Data.Repository.Interface;

namespace Comments.API.Data.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByPhoneAsync(string? phoneNumber);
        Task<User?> FindByEmailAsync(string? email);
    }
}