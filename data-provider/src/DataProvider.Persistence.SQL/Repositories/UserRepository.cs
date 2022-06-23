using DataProvider.Domain.Users.Models;
using DataProvider.Domain.Users.Repositories;
using DataProvider.Persistence.SQL.Context;
using Infrastructure.Repositories;

namespace DataProvider.Persistence.SQL.Repositories
{
    public class UserRepository : Repository<User, SqlContext>, IUserRepository
    {
        public UserRepository(SqlContext context) : base(context) { }
    }
}
