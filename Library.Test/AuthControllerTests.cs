namespace Library.Test;

using System.Threading;
using System.Threading.Tasks;
using Library.Application.Features.Auth.Login;
using Library.WebAPI.Controllers; // senin AuthController namespace'in
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TS.Result;
using Xunit;

public class AuthControllerTests
{
[Fact]
        public async Task Login_InvalidCredentials_Returns401_And_ResultBody()
        {
            // arrange
            var mediatorMock = new Mock<IMediator>();

            // IMediator.Send(LoginCommand...) çağrıldığında 401 + error body döndür
            mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((StatusCodes.Status401Unauthorized, "Invalid credentials"));

            var controller = new AuthController(mediatorMock.Object);
            var request = new LoginCommand("admin", "wrongpass");

            // act
            var actionResult = await controller.Login(request, CancellationToken.None);

            // assert
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);

            var body = Assert.IsType<Result<LoginCommandResponse>>(objectResult.Value);
            Assert.False(body.IsSuccessful);
            Assert.Equal(401, body.StatusCode);
            Assert.NotNull(body.ErrorMessages);
            Assert.Equal("Invalid credentials", body.ErrorMessages![0]);
        }

        [Fact]
        public async Task Login_ValidCredentials_Returns200_And_Tokens()
        {
            // arrange
            var mediatorMock = new Mock<IMediator>();

            var dto = new LoginCommandResponse(
                Token: "access-token-xyz",
                RefreshToken: "refresh-token-abc",
                RefreshTokenExpires: DateTimeOffset.UtcNow.AddMinutes(180)
            );

            // Başarılı senaryo -> 200 + data
            mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<LoginCommandResponse>.Succeed(dto));

            var controller = new AuthController(mediatorMock.Object);
            var request = new LoginCommand("admin", "correctpass");

            // act
            var actionResult = await controller.Login(request, CancellationToken.None);

            // assert
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var body = Assert.IsType<Result<LoginCommandResponse>>(objectResult.Value);
            Assert.True(body.IsSuccessful);
            Assert.Equal(200, body.StatusCode);
            Assert.NotNull(body.Data);
            Assert.Equal("access-token-xyz", body.Data!.Token);
            Assert.Equal("refresh-token-abc", body.Data.RefreshToken);
        }
}