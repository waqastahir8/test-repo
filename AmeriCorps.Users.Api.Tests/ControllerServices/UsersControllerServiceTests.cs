using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class UsersControllerServiceTests : BaseTests<UsersControllerService>
{
    private Mock<IUserRepository>? _repositoryMock;
    private Mock<IRequestMapper>? _requestMapperMock;
    private Mock<IResponseMapper>? _responseMapperMock;
    private Mock<IValidator>? _validatorMock;
    private Mock<IProjectRepository>? _projectRepository;
    private Mock<IRoleRepository>? _roleRepository;
    private Mock<IAccessRepository>? _accessRepository;

    private Mock<IUserHelperService>? _userHelperService;

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task GetUserAsync_Successful_Status(int userId)
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Without(u => u.EncryptedSocialSecurityNumber)
            .Create());

        // Act
        var (status, _) = await sut.GetAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task GetUserAsync_NonExistent_InformationMissing_Status(int userId)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task GetByExternalAccountId_Successful_Status()
    {
        // Arrange
        var sut = Setup();

        var externalAccountId =
            Fixture
            .Create<string>();
        _repositoryMock!
            .Setup(x => x.GetByExternalAccountIdAsync(externalAccountId))
            .ReturnsAsync(() => Fixture.Build<User>()
             .Without(u => u.Roles)
             .Without(u => u.UserProjects)
             .Without(u => u.EncryptedSocialSecurityNumber)
             .Create());

        // Act
        var (status, _) = await sut.GetByExternalAccountIdAsync(externalAccountId);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task GetByExternalAccountId_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();
        var externalAccountId =
            Fixture
            .Create<string>();
        _repositoryMock!
            .Setup(x => x.GetByExternalAccountIdAsync(externalAccountId))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetByExternalAccountIdAsync(externalAccountId);

        // Assert
        Assert.Equal(ResponseStatus.NotFound, status);
    }

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
    public async Task CreateAsync_GetByExternalIdThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<UserRequestModel>();
        var user =
            Fixture
            .Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(user);
        _repositoryMock!
            .Setup(x => x.GetUserIdByExternalAccountIdAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception());
        // Act
        var (status, _) = await sut.CreateOrPatchAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
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
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
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
    public async Task CreateAsync_SaveCalled_WhenUserIsNew()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<UserRequestModel>();
        var user =
            Fixture
            .Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(user);
        _repositoryMock!
            .Setup(x => x.GetUserIdByExternalAccountIdAsync(It.IsAny<string>()))
            .ReturnsAsync(0);

        // Act
        var (status, _) = await sut.CreateOrPatchAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
        _repositoryMock?.Verify(x => x.SaveAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_UpdateCalled_WhenUserIsNotNew()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<UserRequestModel>();
        var user =
            Fixture
            .Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(user);
        _repositoryMock!
            .Setup(x => x.GetUserIdByExternalAccountIdAsync(It.IsAny<string>()))
            .ReturnsAsync(1);

        // Act
        var (status, _) = await sut.CreateOrPatchAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
        _repositoryMock?.Verify(x => x.UpdateUserAsync(It.IsAny<User>()), Times.Once);
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
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
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
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create();
        var expected =
            Fixture
            .Create<UserResponse>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(user);
        _responseMapperMock!
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
    public async Task CreateSearchAsync_InvalidUserId_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<SavedSearchRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _repositoryMock?
            .Setup(x => x.ExistsAsync<User>(u => u.Id == Fixture.Create<int>()))
            .ReturnsAsync(false);

        //Act
        var (status, _) = await sut.CreateSearchAsync(Fixture.Create<int>(), model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task CreateSearchAsync_UserRepositoryThrowsException_UnknownErrorStatus(int userId)
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.CreateSearchAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task CreateSearchAsync_SearchRepositoryThrowsException_UnknownErrorStatus(int userId)
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync<SavedSearch>(search))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.CreateSearchAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task CreateAsync_SearchSaved_SuccessfulStatus(int userId)
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(search))
            .ReturnsAsync(search);

        // Act
        var (status, _) = await sut.CreateSearchAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task CreateAsync_SearchSaved_ReturnsMappedObject(int userId)
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
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
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _responseMapperMock!
            .Setup(x => x.Map(search))
            .Returns(expected);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(search))
            .ReturnsAsync(search);

        // Act
        var (_, actual) = await sut.CreateSearchAsync(userId, model);

        // Assert
        Assert.Equal(actual, expected);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task GetUserSearchesAsync_Successful_Status(int userId)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetUserSearchesAsync(userId))
            .ReturnsAsync(() => Fixture.Create<List<SavedSearch>>());

        // Act
        var (status, _) = await sut.GetUserSearchesAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task GetUserSearchesAsync_NonExistent_InformationMissing_Status(int userId)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetUserSearchesAsync(userId))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetUserSearchesAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateAsync_NullSearch_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();

        //Act
        var (status, _) = await sut.UpdateSearchAsync(userId, searchId, null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateAsync_InvalidSearch_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();
        var model = Fixture.Create<SavedSearchRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(false);

        //Act
        var (status, _) = await sut.UpdateSearchAsync(userId, searchId, model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateSearchAsync_InvalidUserId_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();
        var model = Fixture.Create<SavedSearchRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _repositoryMock?
            .Setup(x => x.ExistsAsync<User>(u => u.Id == Fixture.Create<int>()))
            .ReturnsAsync(false);

        //Act
        var (status, _) = await sut.UpdateSearchAsync(userId, searchId, model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateSearchAsync_RepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.UpdateSearchAsync(userId, searchId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task UpdateAsync_SearchSaved_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<SavedSearch>(s => s.UserId == userId &&
                                                   s.Id == searchId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(search))
            .ReturnsAsync(search);

        // Act
        var (status, _) = await sut.UpdateSearchAsync(userId, searchId, model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task UpdateAsync_SearchSaved_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<SavedSearchRequestModel>();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();
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
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _responseMapperMock!
            .Setup(x => x.Map(search))
            .Returns(expected);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<SavedSearch>(s => s.UserId == userId &&
                                                   s.Id == searchId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(search))
            .ReturnsAsync(search);

        // Act
        var (_, actual) = await sut.UpdateSearchAsync(userId, searchId, model);

        // Assert
        Assert.Equal(actual, expected);
    }

    [Fact]
    public async Task Delete_SavedSearch_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        var expected = true;
        _repositoryMock!
            .Setup(x => x.ExistsAsync<SavedSearch>(s => s.UserId == userId &&
                                                   s.Id == searchId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.DeleteAsync<SavedSearch>(searchId))
            .ReturnsAsync(true);

        // Act
        var (_, actual) = await sut.DeleteSearchAsync(userId, searchId);

        // Assert
        Assert.Equal(actual, expected);
    }

    [Fact]
    public async Task Delete_SavedSearch_InvalidSearch_MissingInformationStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _repositoryMock!
            .Setup(x => x.ExistsAsync<SavedSearch>(s => s.UserId == userId &&
                                                   s.Id == searchId))
            .ReturnsAsync(false);

        // Act
        var (status, _) = await sut.DeleteSearchAsync(userId, searchId);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task Delete_SavedSearch_RepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();
        var search =
            Fixture
            .Build<SavedSearch>()
            .Create();
        _repositoryMock!
            .Setup(x => x.ExistsAsync<SavedSearch>(s => s.UserId == userId &&
                                                   s.Id == searchId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.DeleteAsync<SavedSearch>(searchId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.DeleteSearchAsync(userId, searchId);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task CreateAsync_NullReference_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        //Act
        var (status, _) = await sut.CreateReferenceAsync(Fixture.Create<int>(), null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateAsync_InvalidReference_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ReferenceRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(false);

        //Act
        var (status, _) = await sut.CreateReferenceAsync(Fixture.Create<int>(), model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateReferenceAsync_InvalidUserId_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ReferenceRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _repositoryMock?
            .Setup(x => x.ExistsAsync<User>(u => u.Id == Fixture.Create<int>()))
            .ReturnsAsync(false);

        //Act
        var (status, _) = await sut.CreateReferenceAsync(Fixture.Create<int>(), model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateReferenceAsync_UserRepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var userId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ThrowsAsync(new Exception());
        _repositoryMock!
            .Setup(x => x.SaveAsync(reference))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.CreateReferenceAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task CreateReferenceAsync_ReferenceRepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var userId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(reference))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.CreateReferenceAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task CreateAsync_Reference_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var userId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(reference))
            .ReturnsAsync(reference);

        // Act
        var (status, _) = await sut.CreateReferenceAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task CreateAsync_Reference_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var userId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        var expected =
            Fixture
            .Create<ReferenceResponseModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _responseMapperMock!
            .Setup(x => x.Map(reference))
            .Returns(expected);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(reference))
            .ReturnsAsync(reference);

        // Act
        var (_, actual) = await sut.CreateReferenceAsync(userId, model);

        // Assert
        Assert.Equal(actual, expected);
    }

    [Fact]
    public async Task GetUserReferencesAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();
        var userId =
            Fixture
            .Create<int>();
        _repositoryMock!
            .Setup(x => x.GetUserReferencesAsync(userId))
            .ReturnsAsync(() => Fixture.Create<List<Reference>>());

        // Act
        var (status, _) = await sut.GetReferencesAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task GetUserReferencesAsync_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();
        var userId =
            Fixture
            .Create<int>();
        _repositoryMock!
            .Setup(x => x.GetUserReferencesAsync(userId))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetReferencesAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task GetReferencesAsync_RepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var userId =
            Fixture
            .Create<int>();
        _repositoryMock!
            .Setup(x => x.GetUserReferencesAsync(userId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.GetReferencesAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task UpdateReference_NullReference_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();

        //Act
        var (status, _) = await sut.UpdateReferenceAsync(userId, referenceId, null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateReference_InvalidReference_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var model = Fixture.Create<ReferenceRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(false);

        //Act
        var (status, _) = await sut.UpdateReferenceAsync(userId, referenceId, model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateReference_InvalidUserId_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var model = Fixture.Create<ReferenceRequestModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _repositoryMock?
            .Setup(x => x.ExistsAsync<User>(u => u.Id == Fixture.Create<int>()))
            .ReturnsAsync(false);

        //Act
        var (status, _) = await sut.UpdateReferenceAsync(userId, referenceId, model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateReference_UserRepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task UpdateReference_ReferenceRepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync<Reference>(reference))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateReference_ReferenceSaved_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<Reference>(r => r.UserId == userId &&
                                                   r.Id == referenceId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync<Reference>(reference))
            .ReturnsAsync(reference);

        // Act
        var (status, _) = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(52)]
    [InlineData(35)]
    public async Task UpdateReference_ReferenceSaved_ReturnsMappedObject(int userId)
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var referenceId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        var expected =
            Fixture
            .Create<ReferenceResponseModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _responseMapperMock!
            .Setup(x => x.Map(reference))
            .Returns(expected);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<Reference>(r => r.UserId == userId &&
                                                   r.Id == referenceId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.SaveAsync(reference))
            .ReturnsAsync(reference);

        // Act
        var (_, actual) = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        Assert.Equal(actual, expected);
    }

    [Fact]
    public async Task UpdateReference_UserExistsThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task UpdateReference_ReferenceExistsThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Create<ReferenceRequestModel>();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<Reference>(r => r.UserId == userId &&
                                                   r.Id == referenceId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task Delete_Reference_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        var expected = true;
        _repositoryMock!
            .Setup(x => x.ExistsAsync<Reference>(r => r.UserId == userId &&
                                                   r.Id == referenceId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.DeleteAsync<Reference>(referenceId))
            .ReturnsAsync(true);

        // Act
        var (_, actual) = await sut.DeleteReferenceAsync(userId, referenceId);

        // Assert
        Assert.Equal(actual, expected);
    }

    [Fact]
    public async Task Delete_Reference_InvalidReference_MissingInformationStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _repositoryMock!
            .Setup(x => x.ExistsAsync<Reference>(r => r.UserId == userId &&
                                                   r.Id == referenceId))
            .ReturnsAsync(false);

        // Act
        var (status, _) = await sut.DeleteReferenceAsync(userId, referenceId);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task Delete_Reference_RepositoryThrowsException_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();
        var reference =
            Fixture
            .Build<Reference>()
            .Create();
        _repositoryMock!
            .Setup(x => x.ExistsAsync<Reference>(r => r.UserId == userId &&
                                                   r.Id == referenceId))
            .ReturnsAsync(true);
        _repositoryMock!
            .Setup(x => x.DeleteAsync<Reference>(referenceId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.DeleteReferenceAsync(userId, referenceId);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Theory]
    [InlineData("  ")]
    [InlineData("")]
    [InlineData("\n\t")]
    [InlineData(null)]
    public async Task GetAsync_NoAttributeType_MissingInformationResponseStatus(string? value)
    {
        // Arrange
        var attributeValue = Fixture.Create<string>();
        var sut = Setup();

        // Act
        var (actual, _) = await sut.GetAsync(value!, attributeValue);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, actual);
    }

    [Theory]
    [InlineData("  ")]
    [InlineData("")]
    [InlineData("\n\t")]
    [InlineData(null)]
    public async Task GetAsync_NoAttributeValue_MissingInformationResponseStatus(string? value)
    {
        // Arrange
        var attributeType = Fixture.Create<string>();
        var sut = Setup();

        // Act
        var (actual, _) = await sut.GetAsync(attributeType, value!);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, actual);
    }

    [Fact]
    public async Task GetAsync_RaisedException_UnknownErrorResponseStatus()
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetByAttributeAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        // Act
        var (actual, _) = await sut.GetAsync(Fixture.Create<string>(), Fixture.Create<string>());

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, actual);
    }

    [Fact]
    public async Task GetAsync_NullFromRepository_UnknownErrorResponseStatus()
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetByAttributeAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(null as IEnumerable<User>);

        // Act
        var (actual, _) = await sut.GetAsync(Fixture.Create<string>(), Fixture.Create<string>());

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, actual);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(3)]
    [InlineData(156)]
    public async Task GetAsync_SuccessfulCallToRepository_MapsEveryUser(int amount)
    {
        // Arrange
        var sut = Setup();

        var users =
            Fixture
            .Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .CreateMany(amount);

        _repositoryMock!
            .Setup(x => x.GetByAttributeAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(users);

        // Act
        var (_, response) = await sut.GetAsync(Fixture.Create<string>(), Fixture.Create<string>());
        _ = response.ToList(); // To force enumeration

        // Assert
        users
            .ToList()
            .ForEach(x => _responseMapperMock!.Verify(y => y.Map(x), Times.Once));
    }

    [Fact]
    public async Task GetAsync_SuccessfulCallToRepository_SuccessResponseStatus()
    {
        // Arrange
        var sut = Setup();

        var users =
            Fixture
            .Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .CreateMany(10);

        _repositoryMock!
            .Setup(x => x.GetByAttributeAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(users);

        // Act
        var (actual, _) = await sut.GetAsync(Fixture.Create<string>(), Fixture.Create<string>());

        // Assert
        Assert.Equal(ResponseStatus.Successful, actual);
    }

    [Theory]
    [InlineData(1, "proj")]
    public async Task AddUserToProjectAsync_Successful_Status(int userId, string projCode)
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Build<ProjectRequestModel>()
                              .With(p => p.ProjectCode, projCode)
                              .Create();

        var project =
            Fixture
            .Build<Project>()
            .With(p => p.ProjectCode, projCode)
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(project);

        _projectRepository!
            .Setup(x => x.GetProjectByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(project);

        _repositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create());

        // Act
        var (status, _) = await sut.AddUserToProjectAsync(userId, projCode);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData(1, "proj")]
    public async Task AddUserToProjectAsync_InformationMissing_Status(int userId, string projCode)
    {
        // Arrange
        var sut = Setup();

        var (status, _) = await sut.AddUserToProjectAsync(userId, projCode);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);

        _repositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create());

        // Act
        var (status2, _) = await sut.AddUserToProjectAsync(userId, projCode);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status2);
    }

    [Theory]
    [InlineData("org")]
    public async Task FetchUserListByOrgCodeAsync_Successful_Status(string orgCode)
    {
        // Arrange
        var sut = Setup();

        var userId =
            Fixture
            .Create<int>();

        _repositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Build<User>()
            .With(o => o.OrgCode, orgCode)
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create());

        // Act
        var (status, _) = await sut.FetchUserListByOrgCodeAsync(orgCode);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("")]
    public async Task FetchUserListByOrgCodeAsync_Missing_Status(string orgCode)
    {
        // Arrange
        var sut = Setup();

        // Act
        var (status, _) = await sut.FetchUserListByOrgCodeAsync(orgCode);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Theory]
    [InlineData(1)]
    public async Task UpdateUserProjectAndRoleDataAsync_Successful_Status(int userId)
    {
        // Arrange
        var sut = Setup();

        var roles = Fixture.Build<List<UserRoleRequestModel>>()
                        .Create();

        var projs = Fixture.Build<List<UserProjectRequestModel>>()
                        .Create();

        var model = Fixture.Build<UserProjectRoleUpdateRequestModel>()
                        .With(p => p.Id, userId)
                        .With(p => p.UserRoles, roles)
                        .With(p => p.UserProjects, projs)
                        .Create();

        _repositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create());

        // Act
        var (status, _) = await sut.UpdateUserProjectAndRoleDataAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task UpdateUserProjectAndRoleDataAsync_Missing_Status()
    {
        // Arrange
        var sut = Setup();

        var user = Fixture.Build<UserProjectRoleUpdateRequestModel>()
                        .With(u => u.Id, 0)
                        .Create();

        // Act
        var (status, _) = await sut.UpdateUserProjectAndRoleDataAsync(user);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task InviteUserAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<UserRequestModel>()
                        .Create();

        model.Email = "email";

        var user =
            Fixture
            .Build<User>()
            .Without(u => u.Roles)
            .Without(u => u.UserProjects)
            .Create();

        var userEmail =
            Fixture
            .Build<CommunicationMethod>()
            .Create();

        var modelEmail =
            Fixture
            .Build<CommunicationMethodRequestModel>()
            .With(p => p.Type, "email")
            .With(p => p.Value, "test")
            .With(p => p.IsPreferred, true)
            .Create();

        user.FirstName = "test";
        user.LastName = "name";
        user.CommunicationMethods.Add(userEmail);

        model.FirstName = "test";
        model.LastName = "name";
        var modelEmailList =
            Fixture
            .Build<List<CommunicationMethodRequestModel>>()
            .Create();

        modelEmailList.Add(modelEmail);
        model.CommunicationMethods = modelEmailList;

        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(user);

        _repositoryMock!
            .Setup(x => x.SaveAsync(user))
            .ReturnsAsync(user);

        var (status, _) = await sut.InviteUserAsync(model);

        // Assert
        //TODO FIX TEST
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task InviteUserAsync_Missing_Status()
    {
        // Arrange
        var sut = Setup();

        var user = Fixture.Build<UserRequestModel>()
                .With(p => p.Email, "")
                .Create();

        // Act
        var (status, _) = await sut.InviteUserAsync(user);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task InviteUserAsync_Unknown_Error()
    {
        // Arrange
        var sut = Setup();

        var user = Fixture.Build<UserRequestModel>()
                .Create();

        // Act
        var (status, _) = await sut.InviteUserAsync(user);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }


    [Fact]
    public async Task LinkNewAccountToExistingUserAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<ExistingUserSearchModel>()
                        .Create();

        model.UserEmail = "email";

        var user =
            Fixture
            .Build<UserRequestModel>()
            .Create();

        model.NewUser = user;

        var (status, _) = await sut.LinkNewAccountToExistingUserAsync(model);

        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task LinkNewAccountToExistingUserAsync_Missing_Status()
    {
        // Arrange
        var sut = Setup();

        var toLink = Fixture.Build<ExistingUserSearchModel>()
                .Create();

        toLink.UserEmail = "";

        // Act
        var (status, _) = await sut.LinkNewAccountToExistingUserAsync(toLink);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task LinkNewAccountToExistingUserAsync_Unknown_Error()
    {
        // Arrange
        var sut = Setup();

        var toLink = Fixture.Build<ExistingUserSearchModel>()
                .Create();

        _repositoryMock!
            .Setup(x => x.FindInvitedUserInfo(It.IsAny<string>(),It.IsAny<string>()))
            .ThrowsAsync(new Exception());
            
        // Act
        var (status, _) = await sut.LinkNewAccountToExistingUserAsync(toLink);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    protected override UsersControllerService Setup()
    {
        _repositoryMock = new();
        _requestMapperMock = new();
        _responseMapperMock = new();
        _validatorMock = new();
        _projectRepository = new();
        _roleRepository = new();
        _accessRepository = new();
        _userHelperService = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<UsersControllerService>>(),
            _requestMapperMock.Object,
            _responseMapperMock.Object,
            _validatorMock.Object,
            _repositoryMock.Object,
            _projectRepository.Object,
            _roleRepository.Object,
            _accessRepository.Object,
            _userHelperService.Object);
    }
}