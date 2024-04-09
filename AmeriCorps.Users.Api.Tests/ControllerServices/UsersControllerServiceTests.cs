using Microsoft.Extensions.Logging;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Api.Services;


namespace AmeriCorps.Users.Api.Tests;

public sealed class UsersControllerServiceTests : BaseTests<UsersControllerService>
{
    private Mock<IUserRepository>? _repositoryMock;
    private Mock<IRequestMapper>? _reqMapperMock;
    private Mock<IResponseMapper>? _respMapperMock;
    private Mock<IValidator>? _validatorMock;

    [Fact]
    public async Task CreateAsync_NullUser_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        //Act
        var (status, _) = await sut.CreateOrPatchAsync(null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateAsync_InvalidUser_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<UserRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(false);

        //Act
        var (status, _) = await sut.CreateOrPatchAsync(model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateAsync_RepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<UserRequestModel>();
        var user =
            Fixture
            .Build<User>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(user);
        _repositoryMock!
            .Setup(x => x.SaveAsync(user))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.CreateOrPatchAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task CreateAsync_UserSaved_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<UserRequestModel>();
        var user =
            Fixture
            .Build<User>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(user);
        _repositoryMock!
            .Setup(x => x.SaveAsync(user))
            .ReturnsAsync(user);

        // Act
        var (status, _) = await sut.CreateOrPatchAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task CreateAsync_UserSaved_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<UserRequestModel>();
        var user =
            Fixture
            .Build<User>()
            .Create();
        var expected =
            Fixture
            .Create<UserResponse>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(user);
        _respMapperMock!
            .Setup(x => x.Map(user))
            .Returns(expected);
        _repositoryMock!
            .Setup(x => x.SaveAsync(user))
            .ReturnsAsync(user);

        // Act
        var (_, actual) = await sut.CreateOrPatchAsync(model);

        // Assert
        Assert.Equal(actual, expected);
    }

    [Fact]
    public async Task CreateAsync_NullSearch_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        //Act
        var (status, _) = await sut.CreateSearchAsync(Fixture.Create<int>(), null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateAsync_InvalidSearch_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<SavedSearchRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(false);

        //Act
        var (status, _) = await sut.CreateSearchAsync(Fixture.Create<int>(), model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateAsync_InvalidUserId_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<SavedSearchRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _repositoryMock?
            .Setup(x => x.ExistsAsync(Fixture.Create<int>()))
            .ReturnsAsync(false);

        //Act
        var (status, _) = await sut.CreateSearchAsync(Fixture.Create<int>(), model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateSearchAsync_RepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var userId = Fixture.Create<int>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _repositoryMock!
            .Setup(x => x.ExistsAsync(userId))
            .ThrowsAsync(new Exception());
        _repositoryMock!
            .Setup(x => x.SaveAsync(search))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.CreateSearchAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task CreateAsync_SearchSaved_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var userId = Fixture.Create<int>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _repositoryMock!
            .Setup(x => x.ExistsAsync(userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(search))
            .ReturnsAsync(search);

        // Act
        var (status, _) = await sut.CreateSearchAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task CreateAsync_SearchSaved_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var userId = Fixture.Create<int>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        var expected =
            Fixture
            .Create<SavedSearchResponseModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _respMapperMock!
            .Setup(x => x.Map(search))
            .Returns(expected);
        _repositoryMock!
            .Setup(x => x.ExistsAsync(userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(search))
            .ReturnsAsync(search);

        // Act
        var (_, actual) = await sut.CreateSearchAsync(userId, model);

        // Assert
        Assert.Equal(actual, expected);
    }
    protected override UsersControllerService Setup()
    {
        _repositoryMock = new();
        _reqMapperMock = new();
        _respMapperMock = new();
        _validatorMock = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<UsersControllerService>>(),
            _reqMapperMock.Object,
            _respMapperMock.Object,
            _validatorMock.Object,
            _repositoryMock.Object);
    }
}
