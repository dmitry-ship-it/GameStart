using AutoMapper;
using GameStart.IdentityService.Api.Controllers;
using GameStart.IdentityService.Common;
using GameStart.IdentityService.Common.ViewModels;
using GameStart.IdentityService.Data.Models;
using GameStart.Shared.MessageBus;
using GameStart.Shared.MessageBus.Models.EmailModels;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.IdentityService.Api.Tests
{
    public class AccountControllerTests
    {
        private readonly AccountManager accountManagerMock;
        private readonly AccountController sut;

        public AccountControllerTests()
        {
            var userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            accountManagerMock = Substitute.For<AccountManager>(
                Substitute.For<IMapper>(),
                Substitute.For<IMessagePublisher<EmailTemplate>>(),
                userManager,
                Substitute.For<SignInManager<User>>(
                    userManager,
                    Substitute.For<IHttpContextAccessor>(),
                    Substitute.For<IUserClaimsPrincipalFactory<User>>(),
                    null, null, null, null),
                Substitute.For<IdentityServerTools>(null, null, null));

            sut = new AccountController(accountManagerMock);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnOkResult()
        {
            // Arrange
            var loginViewModel = new LoginViewModel();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await sut.LoginAsync(loginViewModel, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
            await accountManagerMock.Received().LoginAsync(
                loginViewModel, Arg.Any<HttpContext>(), cancellationToken);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnOkResult()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await sut.RegisterAsync(registerViewModel, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
            await accountManagerMock.Received().RegisterAsync(
                registerViewModel, Arg.Any<HttpContext>(), cancellationToken);
        }

        [Fact]
        public async Task SendVerificationEmailAsync_ShouldReturnAcceptedResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await sut.SendVerificationEmailAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<AcceptedResult>();
            await accountManagerMock.Received().SendEmailVerificationRequestAsync(
                Arg.Any<HttpContext>(), cancellationToken);
        }

        [Fact]
        public async Task VerifyEmailAsync_WhenEmailVerifiedSuccessfully_ShouldReturnOkResult()
        {
            // Arrange
            const string token = "test token";
            var cancellationToken = CancellationToken.None;
            accountManagerMock.VerifyEmailAsync(token, Arg.Any<HttpContext>(), cancellationToken)
                .Returns(true);

            // Act
            var result = await sut.VerifyEmailAsync(token, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
            await accountManagerMock.Received().VerifyEmailAsync(
                token, Arg.Any<HttpContext>(), cancellationToken);
        }

        [Fact]
        public async Task VerifyEmailAsync_WhenEmailVerificationFailed_ShouldReturnBadRequestResult()
        {
            // Arrange
            const string token = "test token";
            var cancellationToken = CancellationToken.None;
            accountManagerMock.VerifyEmailAsync(token, Arg.Any<HttpContext>(), cancellationToken)
                .Returns(false);

            // Act
            var result = await sut.VerifyEmailAsync(token, cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
            await accountManagerMock.Received().VerifyEmailAsync(
                token, Arg.Any<HttpContext>(), cancellationToken);
        }

        [Fact]
        public void Challenge_ShouldReturnChallengeResult()
        {
            //Arrange
            const string scheme = "Test scheme";
            const string returnUrl = "https://example.com";

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("example.com");

            accountManagerMock.CreateAuthenticationProperties(
                    scheme, returnUrl, Arg.Any<string>(), Arg.Any<HttpContext>())
                .Returns(new AuthenticationProperties());

            sut.Url = Substitute.For<IUrlHelper>();
            sut.ControllerContext = new() { HttpContext = httpContext };

            //Act
            var result = sut.Challenge(scheme, returnUrl) as ChallengeResult;

            //Assert
            result.Should().NotBeNull();
            result!.Properties.Should().NotBeNull();
            result.Properties.Should().BeOfType<AuthenticationProperties>();
            result.AuthenticationSchemes.Should().BeEquivalentTo(new[] { scheme });

            accountManagerMock.Received().CreateAuthenticationProperties(
                scheme, returnUrl, Arg.Any<string>(), httpContext);
        }

        [Fact]
        public async Task CallbackAsync_ShouldReturnRedirectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var httpContext = new DefaultHttpContext();
            var returnUrl = new Uri("https://example.com/some-page");
            accountManagerMock.ExternalAuthenticateAsync(httpContext, cancellationToken)
                .Returns(returnUrl);

            sut.ControllerContext = new() { HttpContext = httpContext };

            // Act
            var result = await sut.CallbackAsync(cancellationToken) as RedirectResult;

            // Assert
            result.Should().NotBeNull();
            result!.Url.Should().Be(returnUrl.ToString());

            await accountManagerMock.Received()
                .ExternalAuthenticateAsync(httpContext, cancellationToken);
        }

        [Fact]
        public async Task LogoutAsync_ShouldReturnOkResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var httpContext = new DefaultHttpContext();

            sut.ControllerContext = new() { HttpContext = httpContext };

            // Act
            var result = await sut.LogoutAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
            await accountManagerMock.Received()
                .ClearCookiesAsync(httpContext, cancellationToken);
        }
    }
}
