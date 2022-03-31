using System.Threading.Tasks;

namespace Sample_Unit_Testing.Application.Services.Interfaces
{
    public interface IEncryptionService
    {
        Task<string> PasswordEncryption(string password);
    }
}