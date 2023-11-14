

using ProjectOversight.API.Data.Model;

namespace ProjectOversight.API.Data.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByPhoneAsync(string? phoneNumber);
        Task<User?> FindByEmailAsync(string? email);
    }
}