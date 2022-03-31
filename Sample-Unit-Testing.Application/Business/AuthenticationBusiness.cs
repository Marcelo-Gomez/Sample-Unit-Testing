using Sample_Unit_Testing.Application.Models;
using Sample_Unit_Testing.Application.Repositories.Interfaces;
using Sample_Unit_Testing.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace Sample_Unit_Testing.Application.Business
{
    public class AuthenticationBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        
        public AuthenticationBusiness(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            UserModel userModel = await _userRepository.GetUserByEmail(loginModel.Email);

            if (userModel != null && userModel.Active)
            {
                string encryptedPassword = await _encryptionService.PasswordEncryption(loginModel.Password);

                if (encryptedPassword == userModel.Password)
                {
                    return true;
                }
            }

            return false;
        }
    }
}