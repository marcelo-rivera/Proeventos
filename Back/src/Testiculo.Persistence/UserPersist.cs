
using Microsoft.EntityFrameworkCore;
using Testiculo.Domain.Identity;
using Testiculo.Persistence.Contexto;
using Testiculo.Persistence.Contratos;

namespace Testiculo.Persistence
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly TesticuloContext _context;
        public UserPersist(TesticuloContext context) : base(context)
        {
            _context = context;
        }

        async Task<IEnumerable<User>> IUserPersist.GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        async Task<User> IUserPersist.GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        async Task<User> IUserPersist.GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                                .SingleOrDefaultAsync(user => user.UserName == userName.ToLower());
        }

    }
}