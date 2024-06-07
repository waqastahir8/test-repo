﻿using Microsoft.Extensions.Logging;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Api.Services;


namespace AmeriCorps.Users.Api.Tests;

public sealed partial class UsersControllerServiceTests : BaseTests<UsersControllerService>
{
    private Mock<IUserRepository>? _repositoryMock;
    private Mock<IRequestMapper>? _reqMapperMock;
    private Mock<IResponseMapper>? _respMapperMock;
    private Mock<IValidator>? _validatorMock;

    [Fact]
    public async Task GetUserAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();
        var userId =
            Fixture
            .Create<int>();
        _repositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Create<User>());

        // Act
        var (status, _) = await sut.GetAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }
    [Fact]
    public async Task GetUserAsync_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();
        var userId =
            Fixture
            .Create<int>();
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
            .Setup(x => x.GetByExternalAcctId(externalAccountId))
            .ReturnsAsync(() => Fixture.Create<User>());

        // Act
        var (status, _) = await sut.GetByExternalAccountId(externalAccountId);

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
            .Setup(x => x.GetByExternalAcctId(externalAccountId))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetByExternalAccountId(externalAccountId);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
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

    [Fact]
    public async Task CreateSearchAsync_UserRepositoryThrowsException_UnknownErrorStatus()
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
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.CreateSearchAsync(userId, model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }
    [Fact]
    public async Task CreateSearchAsync_SearchRepositoryThrowsException_UnknownErrorStatus()
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

    [Fact]
    public async Task GetUserSearchesAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();
        var userId =
            Fixture
            .Create<int>();
        _repositoryMock!
            .Setup(x => x.GetUserSearchesAsync(userId))
            .ReturnsAsync(() => Fixture.Create<List<SavedSearch>>());

        // Act
        var (status, _) = await sut.GetUserSearchesAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }
    [Fact]
    public async Task GetUserSearchesAsync_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();
        var userId =
            Fixture
            .Create<int>();
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
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ThrowsAsync(new Exception());
        _repositoryMock!
            .Setup(x => x.SaveAsync(search))
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
        _reqMapperMock!
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
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(search);
        _respMapperMock!
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
        _reqMapperMock!
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
        _reqMapperMock!
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
        _reqMapperMock!
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
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _respMapperMock!
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
        _reqMapperMock!
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
        _reqMapperMock!
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
        _reqMapperMock!
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

    [Fact]
    public async Task UpdateReference_ReferenceSaved_ReturnsMappedObject()
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
        var expected =
            Fixture
            .Create<ReferenceResponseModel>();
        _validatorMock!
            .Setup(x => x.Validate(model))
            .Returns(true);
        _reqMapperMock!
            .Setup(x => x.Map(model))
            .Returns(reference);
        _respMapperMock!
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
            .Setup(x => x.SaveAsync<Reference>(reference))
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
        _reqMapperMock!
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
        _reqMapperMock!
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