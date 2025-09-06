using FluentValidation.TestHelper;
using Library.Application.Common.Interfaces;
using Library.Application.Features.Writers.Commands.Add;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Moq;
using TS.Result;
namespace Library.Test;

public class WriterCommandsTests
{
    // ------------- Validator Tests -------------

    [Fact]
    public void AddWriter_Validator_Should_Pass_On_Valid_Input()
    {
        var validator = new AddWriterCommandValidator();

        var cmd = new AddWriterCommand
        {
            FirstName = "Eric",
            LastName = "Evans",
            Biography = "DDD pioneer",
            Nationality = "US",
            BirthDate = new DateTimeOffset(1965, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Website = "https://example.com",
            Email = "eric@example.com"
        };

        var result = validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddWriter_Validator_Should_Fail_When_FirstName_Is_Empty(string? firstName)
    {
        var validator = new AddWriterCommandValidator();

        var cmd = new AddWriterCommand { FirstName = firstName! };

        var result = validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void AddWriter_Validator_Should_Fail_On_Invalid_Website()
    {
        var validator = new AddWriterCommandValidator();

        var cmd = new AddWriterCommand
        {
            FirstName = "Eric",
            Website = "not-a-valid-url"
        };

        var result = validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Website);
    }

    [Fact]
    public void AddWriter_Validator_Should_Fail_On_Invalid_Email()
    {
        var validator = new AddWriterCommandValidator();

        var cmd = new AddWriterCommand
        {
            FirstName = "Eric",
            Email = "wrong@@mail"
        };

        var result = validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    // ------------- Handler Tests -------------
    // Not: Handler internal olduğundan Library.Application'a
    // [assembly: InternalsVisibleTo("Library.Test")] eklenmiş olmalı.

    [Fact]
    public async Task AddWriter_Handler_Should_Map_Add_And_Commit_Success()
    {
        // Arrange
        var repo = new Mock<IWriterRepository>(MockBehavior.Strict);
        var uow  = new Mock<IUnitOfWorkWithTransaction>(MockBehavior.Strict);

        // Repository AddAsync çağrıldığında Writer özelliklerini doğruluyoruz
        repo.Setup(r => r.AddAsync(It.Is<Writer>(w =>
                    w.FirstName == "Eric" &&
                    w.LastName == "Evans" &&
                    w.Nationality == "US"
                ), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var succeed = Result<bool>.Succeed(true);
        succeed.StatusCode = 200;

        uow.Setup(u => u.SaveChangesAndReturnSuccessAsync(It.IsAny<CancellationToken>()))
           .ReturnsAsync(true);

        var handler = new AddWriterCommandHandler(repo.Object, uow.Object);

        var cmd = new AddWriterCommand
        {
            FirstName = "Eric",
            LastName = "Evans",
            Nationality = "US",
            Biography = "DDD pioneer",
            BirthDate = new DateTimeOffset(1965, 1, 1, 0, 0, 0, TimeSpan.Zero),
            Website = "https://example.com",
            Email = "eric@example.com"
        };

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.True(result.Data);
        Assert.Equal(200, result.StatusCode);

        repo.Verify(r => r.AddAsync(It.IsAny<Writer>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAndReturnSuccessAsync(It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
        uow.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AddWriter_Handler_Should_Bubble_Failure_From_UnitOfWork()
    {
        // Arrange
        var repo = new Mock<IWriterRepository>(MockBehavior.Strict);
        var uow  = new Mock<IUnitOfWorkWithTransaction>(MockBehavior.Strict);

        repo.Setup(r => r.AddAsync(It.IsAny<Writer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // UnitOfWork bool döndüğü için başarısız senaryoda false dönüyoruz
        uow.Setup(u => u.SaveChangesAndReturnSuccessAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new AddWriterCommandHandler(repo.Object, uow.Object);

        var cmd = new AddWriterCommand { FirstName = "Eric" };

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.False(result.Data);                                   // Başarısız olmalı

        repo.Verify(r => r.AddAsync(It.IsAny<Writer>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(u => u.SaveChangesAndReturnSuccessAsync(It.IsAny<CancellationToken>()), Times.Once);
        repo.VerifyNoOtherCalls();
        uow.VerifyNoOtherCalls();
    }
}
