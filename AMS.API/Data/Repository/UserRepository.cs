using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Data.Repository.Interface;
namespace ProjectOversight.API.Data.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ProjectOversightContext naturalCalamityContext)
        : base(naturalCalamityContext)
    {
    }

    public async Task<User?> FindByPhoneAsync(string? phoneNumber)
    {
        var userByPhoneNumber = await FindByConditionAsync(u => u.PhoneNumber == phoneNumber);
        var user = userByPhoneNumber.FirstOrDefault();

        return user;
    }
    public async Task<User?> FindByEmailAsync(string? email)
    {
        var userByEmail = await FindByConditionAsync(u => u.Email == email && u.IsActive == true);
        var user = userByEmail.FirstOrDefault();

        return user;
    }
}