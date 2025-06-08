namespace Playground.Tests.Extensions;

using Microsoft.AspNetCore.Http;

public static class HeaderDictionaryExtensions
{
    public static void Append(this IHeaderDictionary headers, IHeaderDictionary? newValues)
    {
        foreach (var header in newValues ?? new HeaderDictionary())
        {
            headers.Append(header.Key, header.Value);
        }
    }
}