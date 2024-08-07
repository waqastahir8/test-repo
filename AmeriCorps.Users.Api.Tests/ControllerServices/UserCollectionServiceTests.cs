

using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public partial class UsersControllerServiceTests
{
    [Fact]
    public async Task CreateAsync_UserCollection_UnknownError()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
                .Create<CollectionRequestModel>();
        var userCollection =
            Fixture
                .Build<Collection>()
                .Create();
        _validatorMock!
            .Setup(validator => validator.Validate(model))
            .Returns(new ValidationResponse() { IsValid = true });

        _requestMapperMock!
            .Setup(mapper => mapper.Map(model))
            .Returns(userCollection);


        _repositoryMock!
            .Setup(repository => repository.SaveAsync(userCollection))
            .ReturnsAsync(userCollection);


        // Act
        var (status, _) = await sut.CreateCollectionAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task CreateAsync_UserCollection_SuccessfulStatus()
    {

        var sut = Setup();
        var model =
            Fixture
                .Create<CollectionRequestModel>();
        var userCollection =
            Fixture
                .Build<Collection>()
                .Create();
        var userId = Fixture.Create<int>();



        _validatorMock!
            .Setup(validator => validator.Validate(model))
            .Returns(new ValidationResponse() { IsValid = true, ValidationMessage = "Valid" });

        _requestMapperMock!
            .Setup(mapper => mapper.Map(model))
            .Returns(userCollection);
        _repositoryMock!
            .Setup(x => x.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);

        _repositoryMock!
            .Setup(repository => repository.SaveAsync(userCollection))
            .ReturnsAsync(userCollection);

        model.UserId = userId;
        // Act
        var (status, _) = await sut.CreateCollectionAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task GetUserCollectionAsync_UnknownError()
    {
        // Arrange
        var sut = Setup();

        var collection = Fixture.Create<Collection>();
        _repositoryMock!
            .Setup(repo => repo.GetCollectionAsync(collection))
            .ReturnsAsync(new List<Collection>());


        var (status, _) = await sut.GetCollectionAsync(collection.UserId, collection.Type);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task GetUserCollectionAsync_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
                .Create<CollectionRequestModel>();
        var userCollection =
            Fixture
                .Build<Collection>()
                .Create();
        var userId = Fixture.Create<int>();

        _validatorMock!
            .Setup(validator => validator.Validate(model))
            .Returns(new ValidationResponse() { IsValid = true, ValidationMessage = "Valid" });

        _requestMapperMock!
            .Setup(mapper => mapper.Map(model))
            .Returns(userCollection);

        _repositoryMock!.Setup(userRepo => userRepo.ExistsAsync<User>(u => u.Id == userId)).ReturnsAsync(true);


        _repositoryMock!
            .Setup(repository => repository.GetCollectionAsync(userCollection))
            .ReturnsAsync(new List<Collection>());

        model.UserId = userId;
        // Act
        var (status, _) = await sut.GetCollectionAsync(model.UserId, model.Type);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task DeleteCollectionAsync_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var model =
            Fixture
                .Create<CollectionListRequestModel>();
        var collectionReqModel =
            Fixture
                .Create<CollectionRequestModel>();
        var userCollection =
           Fixture
               .Build<List<Collection>>()
               .Create();

        model.UserId = userId;
        collectionReqModel.UserId = userId;

        _validatorMock!
            .Setup(validator => validator.Validate(collectionReqModel))
            .Returns(new ValidationResponse() { IsValid = true, ValidationMessage = "Valid" });

        _requestMapperMock!
            .Setup(mapper => mapper.Map(model))
            .Returns(userCollection);

        _repositoryMock!
            .Setup(userRepo => userRepo.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);

        _repositoryMock!
            .Setup(repository => repository.DeleteCollectionAsync(userCollection))
            .ReturnsAsync(true);

        // Act
        var (status, _) = await sut.DeleteCollectionAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task DeleteCollectionAsync_NullCollectionList_MissingInformationStatus()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        CollectionListRequestModel? model = null;

        // Act
        var (status, _) = await sut.DeleteCollectionAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }


    [Fact]
    public async Task DeleteCollectionAsync_InvalidCollection_UknownError()
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
                .Create<CollectionListRequestModel>();
        var collectionReqModel =
            Fixture
                .Create<CollectionRequestModel>();

        _validatorMock!
            .Setup(validator => validator.Validate(collectionReqModel))
            .Returns(new ValidationResponse() { IsValid = false, ValidationMessage = "Invalid" });

        // Act
        var (status, _) = await sut.DeleteCollectionAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task DeleteCollectionAsync_UnsuccessfulDelete_UnknownError()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var model =
            Fixture
                .Create<CollectionListRequestModel>();
        var collectionReqModel =
            Fixture
                .Create<CollectionRequestModel>();
        var userCollection =
           Fixture
               .Build<List<Collection>>()
               .Create();

        model.UserId = userId;
        collectionReqModel.UserId = userId;

        _validatorMock!
            .Setup(validator => validator.Validate(collectionReqModel))
            .Returns(new ValidationResponse() { IsValid = true, ValidationMessage = "Valid" });

        _requestMapperMock!
            .Setup(mapper => mapper.Map(model))
            .Returns(userCollection);

        _repositoryMock!
            .Setup(userRepo => userRepo.ExistsAsync<User>(u => u.Id == userId))
            .ReturnsAsync(true);

        _repositoryMock!
            .Setup(repository => repository.DeleteCollectionAsync(userCollection))
            .ReturnsAsync(false);

        // Act
        var (status, _) = await sut.DeleteCollectionAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }
}