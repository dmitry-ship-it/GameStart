using AutoMapper;
using GameStart.IdentityService.Common.ViewModels;
using GameStart.IdentityService.Data.Models;
using GameStart.Shared;
using GameStart.Shared.MessageBus;
using GameStart.Shared.MessageBus.Models.EmailModels;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Security.Principal;

namespace GameStart.IdentityService.Common.Tests
{
    public class AccountManagerTests
    {
        private readonly HttpContext httpContextMock;

        private readonly IMapper mapperMock;
        private readonly IMessagePublisher<EmailTemplate> messagePublisherMock;
        private readonly UserManager<User> userManagerMock;
        private readonly SignInManager<User> signInManagerMock;
        private readonly IdentityServerTools toolsMock;
        private readonly AccountManager sut;

        public AccountManagerTests()
        {
            httpContextMock = Substitute.For<HttpContext>();
            httpContextMock.RequestServices
                .GetService(typeof(IAuthenticationService))
                .Returns(Substitute.For<IAuthenticationService>());

            mapperMock = Substitute.For<IMapper>();
            messagePublisherMock = Substitute.For<IMessagePublisher<EmailTemplate>>();
            userManagerMock = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);
            signInManagerMock = Substitute.For<SignInManager<User>>(
                userManagerMock,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);
            toolsMock = Substitute.For<IdentityServerTools>(null, null, null);

            sut = new AccountManager(
                mapperMock,
                messagePublisherMock,
                userManagerMock,
                signInManagerMock,
                toolsMock);
        }

        [Fact]
        public async Task LoginAsync_ShouldSignInUser_WhenValidCredentials()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var loginModel = new LoginViewModel
            {
                Username = "John_Doe",
                Password = "qwerty123"
            };

            var user = new User { UserName = loginModel.Username };
            userManagerMock.FindByNameAsync(loginModel.Username)
                .Returns(user);

            signInManagerMock.CheckPasswordSignInAsync(user, loginModel.Password, false)
                .Returns(SignInResult.Success);

            // Act
            await sut.LoginAsync(loginModel, httpContextMock, cancellationToken);

            // Assert
            await signInManagerMock.Received().SignInAsync(user, true);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowArgumentException_WhenUserNotFound()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var loginModel = new LoginViewModel
            {
                Username = "John_Doe",
                Password = "qwerty123"
            };

            userManagerMock.FindByNameAsync(loginModel.Username).Returns((User)null!);

            // Act
            var act = async () => await sut.LoginAsync(
                loginModel, httpContextMock, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                    .WithMessage(Constants.IdentityService.ExceptionMessages.UserNotFound);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowArgumentException_WhenInvalidCredentials()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var loginModel = new LoginViewModel
            {
                Username = "John_Doe",
                Password = "qwerty123"
            };

            var user = new User { UserName = loginModel.Username };
            userManagerMock.FindByNameAsync(loginModel.Username).Returns(user);

            signInManagerMock.CheckPasswordSignInAsync(user, loginModel.Password, false)
                .Returns(SignInResult.Failed);

            // Act
            var act = async () => await sut.LoginAsync(
                loginModel, httpContextMock, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage(Constants.IdentityService.ExceptionMessages.InvalidCredentials);
        }

        [Fact]
        public async Task LoginAsync_ShouldWriteJwtToCookie_WhenLoggedInSuccessfully()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            const string token = "Test token";
            var loginModel = new LoginViewModel
            {
                Username = "John_Doe",
                Password = "qwerty123"
            };

            var user = new User { UserName = loginModel.Username };
            userManagerMock.FindByNameAsync(loginModel.Username)
                .Returns(user);

            signInManagerMock.CheckPasswordSignInAsync(user, loginModel.Password, false)
                .Returns(SignInResult.Success);

            httpContextMock.User.Claims.Returns(Identity.Anonymous.Claims);
            toolsMock.IssueJwtAsync(
                Constants.IdentityService.TokenLifetimeSeconds,
                httpContextMock.User.Claims).Returns(token);

            // Act
            await sut.LoginAsync(loginModel, httpContextMock, cancellationToken);

            // Assert
            httpContextMock.Received().Response.Cookies
                .Append(Constants.AuthCookieName, token);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUser()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var registerModel = new RegisterViewModel
            {
                Email = "example@mail.com",
                Username = "John_Doe",
                Password = "qwerty123"
            };

            var user = new User
            {
                UserName = registerModel.Username,
                Email = registerModel.Email
            };

            userManagerMock.FindByNameAsync(registerModel.Username).Returns(user);
            signInManagerMock.CheckPasswordSignInAsync(user, registerModel.Password, Arg.Any<bool>())
                .Returns(SignInResult.Success);

            // Act
            await sut.RegisterAsync(registerModel, httpContextMock, cancellationToken);

            // Assert
            await userManagerMock.Received().CreateAsync(Arg.Is<User>(user =>
                user.UserName == registerModel.Username
                && user.Email == registerModel.Email
            ), registerModel.Password);
        }

        [Fact]
        public async Task RegisterAsync_ShouldAddUserToRole_WhenValidModel()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var registerModel = new RegisterViewModel
            {
                Email = "example@mail.com",
                Username = "John_Doe",
                Password = "qwerty123"
            };

            var user = new User
            {
                UserName = registerModel.Username,
                Email = registerModel.Email
            };

            userManagerMock.FindByNameAsync(registerModel.Username).Returns(user);
            signInManagerMock.CheckPasswordSignInAsync(user, registerModel.Password, Arg.Any<bool>())
                .Returns(SignInResult.Success);

            // Act
            await sut.RegisterAsync(registerModel, httpContextMock, cancellationToken);

            // Assert
            await userManagerMock.Received().AddToRoleAsync(Arg.Is<User>(user =>
                user.UserName == registerModel.Username
                && user.Email == registerModel.Email
            ), nameof(Roles.User));
        }

        [Fact]
        public async Task RegisterAsync_ShouldLoginUser()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var registerModel = new RegisterViewModel
            {
                Email = "example@mail.com",
                Username = "John_Doe",
                Password = "qwerty123"
            };

            var user = new User
            {
                UserName = registerModel.Username,
                Email = registerModel.Email
            };

            userManagerMock.FindByNameAsync(registerModel.Username).Returns(user);
            signInManagerMock.CheckPasswordSignInAsync(user, registerModel.Password, Arg.Any<bool>())
                .Returns(SignInResult.Success);

            // Act
            await sut.RegisterAsync(registerModel, httpContextMock, cancellationToken);

            // Assert
            await signInManagerMock.Received().SignInAsync(Arg.Any<User>(), Arg.Any<bool>());
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowOperationCanceledException_WhenCanceled()
        {
            // Arrange
            var cancellationToken = new CancellationToken(true);
            var registerModel = new RegisterViewModel
            {
                Email = "example@mail.com",
                Username = "John_Doe",
                Password = "qwerty123"
            };

            // Act
            var act = () => sut.RegisterAsync(registerModel, httpContextMock, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
        }

        [Fact]
        public async Task SendEmailVerificationRequestAsync_ShouldSendEmail_WhenUserExists()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var user = new User
            {
                UserName = "John_Doe",
                Email = "example@mail.com"
            };

            userManagerMock.FindByNameAsync(Arg.Any<string>()).Returns(user);

            // Act
            await sut.SendEmailVerificationRequestAsync(httpContextMock, cancellationToken);

            // Assert
            await userManagerMock.Received().GenerateEmailConfirmationTokenAsync(user);
            await messagePublisherMock.Received().PublishMessageAsync(
                Arg.Is<EmailTemplate>(m => m.To == user.Email),
                cancellationToken);
        }

        [Fact]
        public async Task SendEmailVerificationRequestAsync_ShouldThrowArgumentNullException_WhenHttpContextUserIsNull()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            var act = async () => await sut.SendEmailVerificationRequestAsync(
                httpContextMock, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SendEmailVerificationRequestAsync_ShouldThrowOperationCanceledException_WhenCanceled()
        {
            // Arrange
            var cancellationToken = new CancellationToken(true);
            userManagerMock.FindByNameAsync(Arg.Any<string>()).Returns(new User
            {
                UserName = "John_Doe",
                Email = "example@mail.com"
            });

            // Act
            var act = async () => await sut.SendEmailVerificationRequestAsync(
                httpContextMock, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldReturnFalse_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            const string token = "Test token";
            httpContextMock.User.Identity.Returns((IIdentity?)null);

            // Act
            var result = await sut.VerifyEmailAsync(token, httpContextMock, cancellationToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldReturnFalse_WhenTokenIsInvalid()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            const string token = "Test token";
            const string username = "John_Doe";

            httpContextMock.User.Identity!.Name.Returns(username);
            userManagerMock.ConfirmEmailAsync(Arg.Any<User>(), Arg.Any<string>())
                .Returns(IdentityResult.Failed(new IdentityError()));

            // Act
            var result = await sut.VerifyEmailAsync(token, httpContextMock, cancellationToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldWriteJwtToCookieAndReturnTrue_WhenEmailVerifiedSuccessfully()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            const string token = "Test token";
            const string username = "John_Doe";

            httpContextMock.User.Identity!.Name.Returns(username);
            userManagerMock.ConfirmEmailAsync(Arg.Any<User>(), Arg.Any<string>())
                .Returns(IdentityResult.Success);

            // Act
            var result = await sut.VerifyEmailAsync(token, httpContextMock, cancellationToken);

            // Assert
            httpContextMock.Received().Response.Cookies.Append(
                Constants.AuthCookieName, Arg.Any<string>());
            result.Should().BeTrue();
        }

        [Fact]
        public void CreateAuthenticationProperties_ShouldReturnConfiguredAuthenticationProperties()
        {
            // Arrange
            const string scheme = "Test scheme";
            const string returnUrl = "https://example.com";
            const string callbackUri = "https://some-api.com/callback";

            // Act
            var result = sut.CreateAuthenticationProperties(scheme, returnUrl, callbackUri, httpContextMock);

            // Assert
            result.RedirectUri.Should().Be(callbackUri);
            result.Items.Values.Should().Contain(scheme);
            result.Items.Values.Should().Contain(returnUrl);
        }

        [Fact]
        public async Task ExternalAuthenticateAsync_ShouldThrowArgumentException_WhenAuthenticationFails()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            httpContextMock.RequestServices
                .GetRequiredService<IAuthenticationService>()
                .AuthenticateAsync(httpContextMock, Arg.Any<string>())
                .Returns(AuthenticateResult.Fail(string.Empty));

            // Act
            var act = async () => await sut.ExternalAuthenticateAsync(httpContextMock, cancellationToken);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentException>()
                .WithMessage(Constants.IdentityService.ExceptionMessages.ExternalAuthenticationError);
        }

        [Fact]
        public async Task ExternalAuthenticateAsync_ShouldCreateExternalUser_WhenAuthenticationSucceed()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            httpContextMock.RequestServices
                .GetRequiredService<IAuthenticationService>()
                .AuthenticateAsync(httpContextMock, Arg.Any<string>())
                .Returns(AuthenticateResult.Success(new AuthenticationTicket(
                    new ClaimsPrincipal(Identity.Create(
                        "Test", new Claim(ClaimTypes.Email, "example@email.com"))),
                    new AuthenticationProperties() { Items = { ["returnUrl"] = "https://example.com" } },
                    "Test")));

            // Act
            await sut.ExternalAuthenticateAsync(httpContextMock, cancellationToken);

            // Assert
            await userManagerMock.Received().CreateAsync(Arg.Any<User>());
        }

        [Fact]
        public async Task ClearCookiesAsync_ShouldDeleteAuthCookieFromResponseCookies()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            httpContextMock.RequestServices
                .GetRequiredService<IAuthenticationService>()
                .SignOutAsync(httpContextMock, Arg.Any<string>(), Arg.Any<AuthenticationProperties>())
                .Returns(Task.CompletedTask);

            var responseCookies = Substitute.For<IResponseCookies>();
            httpContextMock.Response.Cookies.Returns(responseCookies);

            // Act
            await sut.ClearCookiesAsync(httpContextMock, cancellationToken);

            // Assert
            responseCookies.Received().Delete(Constants.AuthCookieName);
        }
    }
}
