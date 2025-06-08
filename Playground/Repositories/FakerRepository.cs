namespace Playground.Models;

using Flurl;
using Flurl.Http;

public class FakerRepository
{
    private const string DefaultDomainName = "https://fakerapi.it";
    private const string DefaultLocale = "en-GB";
    private const int DefaultQuantity = 5;
    private readonly string _domainName;

    public FakerRepository(string domainName = DefaultDomainName) => _domainName = domainName;

    public async Task<List<User>> GetUsersAsync(int quantity = DefaultQuantity, string locale = DefaultLocale)
    {
        var result = await _domainName
            .AppendPathSegment("/api/v2/users")
            .AppendQueryParam("_quantity", quantity)
            .AppendQueryParam("_locale", locale)
            .GetJsonAsync<Response<User>>();

        return result.Data;
    }
}