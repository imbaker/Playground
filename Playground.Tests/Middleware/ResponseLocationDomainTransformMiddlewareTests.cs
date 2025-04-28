namespace Playground.Tests.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Playground.Middleware;
using Shouldly;

public class ResponseLocationDomainTransformMiddlewareTests
{
    private const string LocationKey = "Location";
    private const string XForwardedHostKey = "X-Forwarded-Host";
    private const string InternalDomainBaseAddress = "https://internal-domain:1234";
    
    [Fact]
    public async Task ResponseLocationDomainTransformMiddleware_InvokeAsync_Transforms_Successfully()
    {
        // Arrange
        var server = await CreateTestServerAsync(InternalDomainBaseAddress);
        var requestHeaders = new HeaderDictionary() { { XForwardedHostKey, "external-domain"} };
        var responseHeaders = new HeaderDictionary() { { LocationKey, "https://internal-domain:1234/optional-path/resource" } };

        // Act
        var context = await CreateSubjectUnderTest(
            server, 
            StatusCodes.Status201Created, 
            requestHeaders: requestHeaders, 
            responseHeaders: responseHeaders);
        
        // Assert
        context.Response.StatusCode.ShouldBe(StatusCodes.Status201Created);
        context.Response.Headers.ContainsKey(LocationKey).ShouldBeTrue();
        context.Response.Headers.Location.Count.ShouldBe(1);
        context.Response.Headers.Location.First().ShouldBe("https://external-domain/optional-path/resource");
    }

    private static async Task<HttpContext> CreateSubjectUnderTest(
        TestServer server, 
        int statusCode, 
        IHeaderDictionary? requestHeaders = null,
        IHeaderDictionary? responseHeaders = null)
    {
        return await server.SendAsync(c =>
        {
            c.Request.Method = HttpMethods.Post;
            c.Response.StatusCode = statusCode;
            foreach (var header in requestHeaders ?? new HeaderDictionary())
            {
                c.Request.Headers.Append(header.Key, header.Value);
            }
            foreach (var header in responseHeaders ?? new HeaderDictionary())
            {
                c.Response.Headers.Append(header.Key, header.Value);
            }
        });
    }

    private static async Task<TestServer> CreateTestServerAsync(string baseAddress)
    {
        var host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddRouting();
                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseMiddleware<ResponseLocationDomainTransformMiddleware>();
                        app.UseEndpoints(e =>
                        {
                            e.MapPost("/optional-path", () => TypedResults.Text("Response"));
                        });
                    });
            })
            .StartAsync();

        var server = host.GetTestServer();
        server.BaseAddress = new Uri(baseAddress);
        
        return server;
    }
}