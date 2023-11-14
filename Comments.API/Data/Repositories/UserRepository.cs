using Comments.API.Data;
using Comments.API.Data.Models;
using Comments.API.Data.Repository.Interface;
using ProjectOversight.API.Data.Repository;

namespace Comments.API.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CommentsContext commentsContext)
            : base(commentsContext)
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
}