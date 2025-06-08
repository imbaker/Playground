namespace Playground.Middleware;

using System.Net;
using Microsoft.AspNetCore.Http;
using Serilog;

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
        Log.Logger.Debug("ResponseLocationDomainTransformMiddleware.InvokeAsync");
        var hasForwardedHostHeader = context.Request.Headers.ContainsKey(XForwardedHostKey);
        var hasLocationHeader = context.Response.Headers.ContainsKey(LocationKey);
        var responseStatusCode = context.Response.StatusCode;
        if ((responseStatusCode == (int)HttpStatusCode.Created || responseStatusCode == (int)HttpStatusCode.Accepted)
            && hasForwardedHostHeader && hasLocationHeader)
        {
            if (context.Response.Headers.TryGetValue(LocationKey, out var locationHeaderValue)
                && context.Request.Headers.TryGetValue(XForwardedHostKey, out var forwardedHostHeaderValue))
            {
                var optionalPathExists = context.Request.Headers.TryGetValue(XafdRoutePatternToMatch, out var optionalPath);
                var callingClientHostname = $"{forwardedHostHeaderValue.First()}{(optionalPathExists ? $"/{optionalPath}" : string.Empty)}";
                var transformedLocationHeaderValue = locationHeaderValue.First()?.Replace(context.Request.Host.Value, callingClientHostname);
                context.Response.Headers.Remove(LocationKey);
                context.Response.Headers.Append(LocationKey, transformedLocationHeaderValue);
            }
        }

        await _next(context);
    }
}