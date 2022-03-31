using Sample_Unit_Testing.Application.Models;
using System.Threading.Tasks;

namespace Sample_Unit_Testing.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserByEmail(string email);
    }
}