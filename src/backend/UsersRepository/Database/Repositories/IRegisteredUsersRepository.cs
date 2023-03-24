using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.API.Services.Interfaces
{
    public interface IRegisteredUsersRepository
    {
        List<IUser> Users { get; }
        List<IUser> GetAll();
        IUser GetById(int id);
        void Add(IUser user);
        void Update(IUser user);
        void Delete(int id);
    }
}
