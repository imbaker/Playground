namespace Playground.Middleware;

using System.Net;
using Microsoft.AspNetCore.Http;

public class ResponseLocationDomainTransformMiddleware
{

    #region Constants

    private const string LocationKey = "Location";
    private const string XForwardedHostKey = "X-Forwarded-Host";
    private const string XafdRoutePatternToMatch = "XAFD-Route-Pattern-To-Match";

    #endregion

    private readonly RequestDelegate _next;

    public ResponseLocationDomainTransformMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ctx = (HttpContext) context;
        var hasForwardedHostHeader = ctx.Request.Headers.ContainsKey(XForwardedHostKey);
        var hasLocationHeader = ctx.Response.Headers.ContainsKey(LocationKey);
        var responseStatusCode = ctx.Response.StatusCode;
        if ((responseStatusCode == (int)HttpStatusCode.Created || responseStatusCode == (int)HttpStatusCode.Accepted)
            && hasForwardedHostHeader && hasLocationHeader)
        {
            if (ctx.Response.Headers.TryGetValue(LocationKey, out var locationHeaderValue)
                && ctx.Request.Headers.TryGetValue(XForwardedHostKey, out var forwardedHostHeaderValue))
            {
                var optionalPathExists = ctx.Request.Headers.TryGetValue(XafdRoutePatternToMatch, out var optionalPath);
                var callingClientHostname = $"{forwardedHostHeaderValue.First()}{(optionalPathExists ? $"/{optionalPath}" : string.Empty)}";
                var transformedLocationHeaderValue = locationHeaderValue.First().Replace(ctx.Request.Host.Value, callingClientHostname);
                ctx.Response.Headers.Remove(LocationKey);
                ctx.Response.Headers.Append(LocationKey, transformedLocationHeaderValue);
            }
        }
    }
}