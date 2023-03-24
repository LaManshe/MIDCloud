using MIDCloud.GlobalInterfaces.Models;

namespace DAL.Database.Repositories
{
    class UsersRepository : Repository<User>
    {
        private readonly AppDbContext _context;

        public UsersRepository(AppDbContext context) : base(context) { }
    }
}
