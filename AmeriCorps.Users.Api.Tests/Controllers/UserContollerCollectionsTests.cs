using System.Net;
using AmeriCorps.Users.Controllers;
using AmeriCorps.Users.Data.Core;
using Microsoft.AspNetCore.Mvc;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class UsersControllerTests 
{
    [Fact]
    private async Task GetUserCollectionAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();

        foreach (var collectionType in CollectionTypes.Types)
        {
            _serviceMock!
                .Setup(x => x.GetCollectionAsync(userId, collectionType))
                .ReturnsAsync((ResponseStatus.MissingInformation, null));
    
            // Act
            var actual = await sut.GetUserCollectionsAsync(userId, collectionType);

            // Assert
            var response = actual as StatusCodeResult;
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }
        
        
    }
    
    
    [Fact]
    private async Task GetUserCollectionAsync_Invalid_Collection_Type_UnknownError_500StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var collectionType = Fixture.Create<string>();
       
            _serviceMock!
                .Setup(x => x.GetCollectionAsync(userId, collectionType))
                .ReturnsAsync((ResponseStatus.MissingInformation, null));
    
            // Act
            var actual = await sut.GetUserCollectionsAsync(userId, collectionType);

            // Assert
            var response = actual as StatusCodeResult;
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
        
        
    }

    [Fact]
    
    public async Task CreateUserCollectionAsync_UnknownError_500StatusCode()
    {

        //Arrange
        var sut = Setup();
        var model = Fixture.Create<CollectionRequestModel>();
        foreach (var collectionType in CollectionTypes.Types)
        {
            model.Type = collectionType;
            _serviceMock!
                .Setup(controller => controller.CreateCollectionAsync(model))
                .ReturnsAsync((ResponseStatus.UnknownError, null));
            
            //Act
            var actual = await sut.CreateCollectionAsync(model);

            //Assert
            var response = actual as StatusCodeResult;
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }

    [Fact]
    public async Task CreateUserCollectionAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<CollectionRequestModel>();
        foreach (var collectionType in CollectionTypes.Types)
        {
            model.Type = collectionType;
            _serviceMock!
                .Setup(controller => controller.CreateCollectionAsync(model))
                .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<CollectionResponseModel
                >()));

            // Act
            var actual = await sut.CreateCollectionAsync(model);

            // Assert
            var response = actual as OkObjectResult;
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);

        }
    }
    [Fact]
    public async Task DeleteUserCollectionAsync_UnknownError_500StatusCode()
    {

        //Arrange
        var sut = Setup();
        var model = Fixture.Create<CollectionListRequestModel>();
        foreach (var collectionType in CollectionTypes.Types)
        {
            model.Type = collectionType;
            _serviceMock!
                .Setup(controller => controller.DeleteCollectionAsync(model))
                .ReturnsAsync((ResponseStatus.UnknownError, false));
            
            //Act
            var actual = await sut.DeleteUserCollectionsAsync(model);

            //Assert
            var response = actual as StatusCodeResult;
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
    



}