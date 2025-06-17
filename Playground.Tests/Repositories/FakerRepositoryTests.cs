namespace Playground.Tests.Repositories;

using System.Diagnostics.CodeAnalysis;
using Flurl.Http.Testing;
using Playground.Models;
using Shouldly;

public class FakerRepositoryTests
{
    #region Constants

    private const string FakerApiUrl = "https://fakerapi.it/api/v2/users";

    #endregion

    #region Private fields

    private readonly HttpTest _httpTest = new();

    #endregion

    #region GetUsersAsync Tests

    [Fact]
    public async Task GetUsersAsync_When_No_Parameters_Are_Provided_Returns_Ten_Users()
    {
        // Arrange
        const int expectedDefaultQuantity = 10;
        _httpTest.RespondWithJson(GetMockUsersResponse(expectedDefaultQuantity));

        // Act
        var actualListOfUser = await CreateSubjectUnderTest().GetUsersAsync();

        // Assert
        actualListOfUser.Count.ShouldBe(expectedDefaultQuantity);
        _httpTest.ShouldHaveCalled(FakerApiUrl)
            .WithQueryParam("_quantity", expectedDefaultQuantity)
            .Times(1);
    }

    [Theory, MemberData(nameof(EdgeCaseUserResponseData))]
    [SuppressMessage("Usage", "xUnit1045:Avoid using TheoryData type arguments that might not be serializable")]
    public async Task GetUsersAsync_When_Invalid_Quantity_Parameter_Is_Supplied_Returns_Corrected_Values(Response<User> userResponse, int expectedQuantity)
    {
        // Arrange
        _httpTest.RespondWithJson(userResponse);

        // Act
        var actualListOfUser = await CreateSubjectUnderTest().GetUsersAsync(expectedQuantity);

        // Assert
        actualListOfUser.Count.ShouldBe(expectedQuantity);
        _httpTest.ShouldHaveCalled(FakerApiUrl)
            .WithQueryParam("_quantity", expectedQuantity)
            .Times(1);
    }

    [Theory, MemberData(nameof(HappyUserResponseData))]
    [SuppressMessage("Usage", "xUnit1045:Avoid using TheoryData type arguments that might not be serializable")]
    public async Task GetUsersAsync_When_Valid_Quantity_Parameter_Is_Supplied_Returns_That_Many_Users(Response<User> userResponse, int expectedQuantity)
    {
        // Arrange
        _httpTest.RespondWithJson(userResponse);

        // Act
        var result = await CreateSubjectUnderTest().GetUsersAsync(expectedQuantity);

        // Assert
        result.Count.ShouldBe(expectedQuantity);
        _httpTest.ShouldHaveCalled(FakerApiUrl)
            .WithQueryParam("_quantity", expectedQuantity)
            .Times(1);
    }

    #endregion

    #region Public properties

    public static TheoryData<Response<User>, int> HappyUserResponseData =>
        new()
        {
            { GetMockUsersResponse(1), 1 },
            { GetMockUsersResponse(10), 10 },
            { GetMockUsersResponse(1000), 1000 },
        };
    
    public static TheoryData<Response<User>, int> EdgeCaseUserResponseData =>
        new()
        {
            { GetMockUsersResponse(-1), 10 },
            { GetMockUsersResponse(0), 10 },
            { GetMockUsersResponse(1001), 1000 },
        };

    #endregion
    
    #region Private methods
    
    private static FakerRepository CreateSubjectUnderTest() => new();

    private static List<User> GetMockUsers(int quantity = 10)
    {
        quantity = (quantity < 1) ? 10 : quantity;
        return Enumerable.Range(1, Math.Min(quantity, 1000))
            .Select(i => new User
            {
                Id = i,
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}",
                UserName = $"UserName{i}",
                Password = $"Password{i}",
                Ip = $"Ip{i}",
                MacAddress = $"MacAddress{i}",
                Website = $"Website{i}",
                Image = $"Image{i}",
            })
            .ToList();
    }

    private static Response<User> GetMockUsersResponse(int userQuantity) =>
        new() { Status = "OK", Locale = "en_GB", Seed = null!, Data = GetMockUsers(userQuantity) };

    #endregion
}
