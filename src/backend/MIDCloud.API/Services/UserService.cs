//using Ardalis.GuardClauses;
//using MIDCloud.API.Services.Interfaces;
//using DAL.Entities;
//using DAL.Interfaces;
//using DAL.Database.Repositories.Base;
//using Ardalis.Result;
//using MIDCloud.API.Helpers;
//using MIDCloud.API.Models.UserModels;

//namespace MIDCloud.API.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly IConfiguration _configuration;
//        private readonly IRepository<RegisteredUser> _registeredUsersRepository;

//        public UserService(IConfiguration configuration, IRepository<RegisteredUser> registeredUsersRepository)
//        {
//            _configuration = 
//                Guard.Against.Null(configuration, nameof(configuration));
//            _registeredUsersRepository = 
//                Guard.Against.Null(registeredUsersRepository, nameof(registeredUsersRepository));
//        }

//        public IRegisteredUser GetUserById(int id)
//        {
//            return _registeredUsersRepository.Items.FirstOrDefault(user => user.Id == id);
//        }

//        public IRegisteredUser GetUserByRefreshToken(string refreshToken)
//        {
//            return _registeredUsersRepository.Items.FirstOrDefault(item => item.RefreshToken == refreshToken);
//        }

//        public AuthResponse Authenticate(string login, string password)
//        {
//            var user = _registeredUsersRepository.Items.FirstOrDefault(x => x.Login == login && x.Password == password);

//            if (user == null)
//            {
//                // todo: need to add logger
//                return null;
//            }

//            var token = _configuration.GenerateJwtToken(user);
//            var refreshToken = _configuration.GenerateJwtRefreshToken(user);

//            user.RefreshToken = refreshToken;
//            _registeredUsersRepository.Update(user);

//            return new AuthResponse(token, refreshToken);
//        }

//        public Result Register(string name, string login, string password, string rootFolderPath)
//        {
//            try
//            {
//                var user = new RegisteredUser
//                {
//                    Name = name,
//                    Login = login,
//                    Password = password,
//                    RootFolderPath = rootFolderPath,
//                    PermissionsToFolder = new List<string>() { rootFolderPath },
//                    RefreshToken = string.Empty
//                };

//                var registeredUser = _registeredUsersRepository.Add(user);

//                if (registeredUser is null)
//                {
//                    return Result.Error("Can't create this user, trouble with database");
//                }

//                SetPermissionsForFolder(registeredUser, registeredUser.RootFolderPath);

//                return Result.Success();
//            }
//            catch (Exception ex)
//            {
//                return Result.Error(ex.Message);
//            }
//        }

//        public bool IsHavePermissionToFolder(IRegisteredUser user, string folderPath)
//        {
//            var userPermissions = user.PermissionsToFolder;

//            if (userPermissions.Contains(folderPath))
//            {
//                return true;
//            }

//            return false;
//        }

//        public Result SetPermissionsForFolder(IRegisteredUser user, string folderPath)
//        {
//            Guard.Against.Null(user, nameof(user));
//            Guard.Against.NullOrEmpty(folderPath, nameof(folderPath));

//            try
//            {
//                user.PermissionsToFolder.Add(folderPath);

//                _registeredUsersRepository.Update(user as RegisteredUser);

//                return Result.Success();
//            }
//            catch(Exception ex)
//            {
//                return Result.Error(ex.Message);
//            }
            
//        }

//        public Result SetPermissionsForFolder(int userId, string folderPath)
//        {
//            var user = _registeredUsersRepository.Get(userId);

//            var resultSetPermission = SetPermissionsForFolder(user, folderPath);

//            if (resultSetPermission.IsSuccess is false)
//            {
//                return Result.Error(resultSetPermission.Errors.FirstOrDefault());
//            }

//            return Result.Success();
//        }
//    }
//}
