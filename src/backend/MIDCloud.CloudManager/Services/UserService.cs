using Ardalis.GuardClauses;
using Ardalis.Result;
using DAL.Database.Repositories.Base;
using Microsoft.Extensions.Configuration;
using MIDCloud.CloudManager.Helpers;
using MIDCloud.GlobalInterfaces.Models;
using MIDCloud.GlobalInterfaces.Responses;
using MIDCloud.GlobalInterfaces.Services;
using MIDCloud.GlobalInterfaces.Users;

namespace MIDCloud.CloudManager.Services
{
    internal class UserService : IUserService
    {

        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _usersRepository;

        public UserService(IConfiguration configuration, IRepository<User> usersRepository)
        {
            _configuration = Guard.Against.Null(configuration, nameof(configuration));
            _usersRepository = Guard.Against.Null(usersRepository, nameof(usersRepository));
        }

        public bool AddPermissions(IUser user, string folder)
        {
            if (user is User userEntity)
            {
                userEntity.PermissionsToFolder.Add(folder);

                _usersRepository.Update(userEntity);

                return true;
            }

            return false;
        }

        public bool AddPermissionsAsRoot(IUser user, string folder)
        {
            user.RootFolderPath = folder;

            return AddPermissions(user, folder);
        }

        public bool RemovePermissions(IUser user, string folder)
        {
            if (user is User userEntity)
            {
                userEntity.PermissionsToFolder.Remove(folder);

                _usersRepository.Update(userEntity);

                return true;
            }

            return false;
        }

        public User AddToDatabase(IUser user)
        {
            //todo: check sames (login f e)

            if (user is User userEntity)
            {
                return _usersRepository.Add(userEntity);
            }

            throw new Exception("User do not implicit as IUser");
        }

        public (string, string) GenerateTokens(IUser user)
        {
            var token = _configuration.GenerateJwtToken(user);
            var refreshToken = _configuration.GenerateJwtRefreshToken(user);

            user.RefreshToken = refreshToken;

            if (user is User userEntity)
            {
                _usersRepository.Update(userEntity);
            }
            else
            {
                throw new Exception("User do not implicit as IUser");
            }

            return (token, refreshToken);
        }

        public User Get(int id)
        {
            return _usersRepository.Get(id);
        }

        public User Get(IMinimalUser user)
        {
            return _usersRepository.Items.First(x => x.Login == user.Login && x.Password == user.Password);
        }

        public User Get(string refreshToken)
        {
            return _usersRepository.Items.First(x => x.RefreshToken == refreshToken);
        }
    }
}
