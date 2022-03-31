using AutoFixture;
using FluentAssertions;
using Moq;
using Sample_Unit_Testing.Application.Business;
using Sample_Unit_Testing.Application.Models;
using Sample_Unit_Testing.Application.Repositories.Interfaces;
using Sample_Unit_Testing.Application.Services.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace Sample_Unit_Testing.UnitTesting.Application.Business
{
    public class AuthenticationBusinessTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEncryptionService> _encryptionServiceMock;
        private readonly AuthenticationBusiness _authenticationBusiness;
        private readonly LoginModel _loginModel;

        public AuthenticationBusinessTest()
        {
            _loginModel = new Fixture().Create<LoginModel>();
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _encryptionServiceMock = new Mock<IEncryptionService>(MockBehavior.Strict);

            _authenticationBusiness = new AuthenticationBusiness
            (
                _userRepositoryMock.Object,
                _encryptionServiceMock.Object
            );
        }

        [Fact]
        public void LoginAsync_WithInvalidEmail_ResponseFalse()
        {
            //Arrange
            UserModel userModel = null;

            //Setup
            _userRepositoryMock.Setup(_ => _.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult(userModel));

            //Act
            var response = _authenticationBusiness.LoginAsync(_loginModel);

            //Asserts
            response.Result.Should().BeFalse();
            _userRepositoryMock.Verify(_ => _.GetUserByEmail(It.IsAny<string>()), Times.Once);
            _encryptionServiceMock.Verify(_ => _.PasswordEncryption(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void LoginAsync_WithInactiveUser_ResponseFalse()
        {
            //Arrange
            UserModel userModel = new Fixture().Build<UserModel>()
                                               .With(p => p.Active, false)
                                               .Create();

            //Setup
            _userRepositoryMock.Setup(_ => _.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult(userModel));

            //Act
            var response = _authenticationBusiness.LoginAsync(_loginModel);

            //Asserts
            response.Result.Should().BeFalse();
            _userRepositoryMock.Verify(_ => _.GetUserByEmail(It.IsAny<string>()), Times.Once);
            _encryptionServiceMock.Verify(_ => _.PasswordEncryption(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void LoginAsync_WithInvalidPassword_ResponseFalse()
        {
            //Arrange
            UserModel userModel = new Fixture().Create<UserModel>();

            //Setup
            _userRepositoryMock.Setup(_ => _.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult(userModel));
            _encryptionServiceMock.Setup(_ => _.PasswordEncryption(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));

            //Act
            var response = _authenticationBusiness.LoginAsync(_loginModel);

            //Asserts
            response.Result.Should().BeFalse();
            _userRepositoryMock.Verify(_ => _.GetUserByEmail(It.IsAny<string>()), Times.Once);
            _encryptionServiceMock.Verify(_ => _.PasswordEncryption(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void LoginAsync_WithValidLogin_ResponseTrue()
        {
            //Arrange
            UserModel userModel = new Fixture().Create<UserModel>();

            //Setup
            _userRepositoryMock.Setup(_ => _.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult(userModel));
            _encryptionServiceMock.Setup(_ => _.PasswordEncryption(It.IsAny<string>())).Returns(Task.FromResult(userModel.Password));

            //Act
            var response = _authenticationBusiness.LoginAsync(_loginModel);

            //Asserts
            response.Result.Should().BeTrue();
            _userRepositoryMock.Verify(_ => _.GetUserByEmail(It.IsAny<string>()), Times.Once);
            _encryptionServiceMock.Verify(_ => _.PasswordEncryption(It.IsAny<string>()), Times.Once);
        }
    }
}