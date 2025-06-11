namespace Playground.Tests.Repositories;

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
    public async Task GetUsersAsync_With_No_Parameters_Returns_Ten_Users()
    {
        // Arrange
        const int expectedDefaultQuantity = 10;
        _httpTest.RespondWithJson(GetMockUsersResponse(expectedDefaultQuantity));

        // Act
        var result = await CreateSubjectUnderTest().GetUsersAsync();

        // Assert
        result.Count.ShouldBe(expectedDefaultQuantity);
        _httpTest.ShouldHaveCalled(FakerApiUrl)
            .WithQueryParam("_quantity", expectedDefaultQuantity)
            .Times(1);
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(0, 10)]
    [InlineData(1001, 1000)]
    public async Task GetUsersAsync_With_Invalid_Quantity_Returns_Correct_Values(int requestedQuantity, int expectedQuantityReturned)
    {
        // Arrange
        _httpTest.RespondWithJson(GetMockUsersResponse(requestedQuantity));

        // Act
        var result = await CreateSubjectUnderTest().GetUsersAsync(requestedQuantity);

        // Assert
        result.Count.ShouldBe(expectedQuantityReturned);
        _httpTest.ShouldHaveCalled(FakerApiUrl)
            .WithQueryParam("_quantity", requestedQuantity)
            .Times(1);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(1000)]
    public async Task GetUsersAsync_With_Parameters_Returns_That_Many_Users(int expectedQuantity)
    {
        // Arrange
        _httpTest.RespondWithJson(GetMockUsersResponse(expectedQuantity));

        // Act
        var result = await CreateSubjectUnderTest().GetUsersAsync(expectedQuantity);

        // Assert
        result.Count.ShouldBe(expectedQuantity);
        _httpTest.ShouldHaveCalled(FakerApiUrl)
            .WithQueryParam("_quantity", expectedQuantity)
            .Times(1);
    }

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