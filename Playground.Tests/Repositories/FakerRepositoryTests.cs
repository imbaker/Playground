namespace Playground.Tests.Repositories;

using Flurl.Http.Testing;
using Playground.Models;
using Shouldly;

public class FakerRepositoryTests
{
    private const string FakerApiUrl = "https://fakerapi.it/api/v2/users";

    private readonly HttpTest _httpTest = new();

    [Fact]
    public async Task GetUsersAsync_With_No_Parameters_Returns_Five_Users()
    {
        // Arrange
        const int expectedDefaultQuantity = 5;
        _httpTest.RespondWithJson(GetUsersResponse(expectedDefaultQuantity));

        // Act
        var result = await CreateSubjectUnderTest().GetUsersAsync();

        // Assert
        result.Count.ShouldBe(expectedDefaultQuantity);
        _httpTest.ShouldHaveCalled(FakerApiUrl)
            .WithQueryParam("_quantity", expectedDefaultQuantity)
            .Times(1);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1)]
    public async Task GetUsersAsync_With_Parameters_Returns_That_Many_Users(int expectedQuantity)
    {
        // Arrange
        _httpTest.RespondWithJson(GetUsersResponse(expectedQuantity));

        // Act
        var result = await CreateSubjectUnderTest().GetUsersAsync(expectedQuantity);

        // Assert
        result.Count.ShouldBe(expectedQuantity);
        _httpTest.ShouldHaveCalled(FakerApiUrl)
            .WithQueryParam("_quantity", expectedQuantity)
            .Times(1);
    }

    private static FakerRepository CreateSubjectUnderTest() => new();

    private static List<User> GetUsers(int quantity) =>
        Enumerable.Range(0, quantity)
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

    private static Response<User> GetUsersResponse(int userQuantity) =>
        new() { Status = "OK", Locale = "en_GB", Seed = null!, Data = GetUsers(userQuantity) };
}