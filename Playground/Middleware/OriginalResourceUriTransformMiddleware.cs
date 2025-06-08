namespace Playground.Middleware;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class OriginalResourceUriTransformMiddleware
{
    public Task InvokeAsync(HttpContext context)
    {
        return Task.CompletedTask;
    }

    public string Transform(string body)
    {
        var inBody = JObject.Parse(body);
        var originalResourceUri = inBody["OriginalResourceUri"]?.ToString();
        
        inBody["OriginalResourceUri"] = TransformOriginalResourceUri(originalResourceUri!);

        return inBody.ToString(Formatting.None);
    }

    private string TransformOriginalResourceUri(string uri)
    {
        if (uri == null)
        {
            return string.Empty;
        }
        
        var context = new
        {
            Api = new { Path = "/rdt-documentstorage/service" },
            Request = new
            {
                OriginalUrl = new Uri("https://test.api.landscape.rdttest.co/rdt-document-storage/service/rdtproductionfactory/iad/api/v2/documents/157/artifacts/attachment/123/456/Claims-Complaint2.pdf6"),
            },
        };

        var servicePath = context.Api.Path;
        const string tenant = "/rdtproductionfactory";
        const string environment = "/iad";
        var incomingUri = new Uri("https://test.api.landscape.rdttest.co/rdt-document-storage/service/rdtproductionfactory/iad/api/v2/documents/157/artifacts/attachment/123/456/Claims-Complaint2.pdf6");
        var locationAbsolutePath = new Uri(uri).AbsolutePath;
        return incomingUri.Scheme + "://" + "test.api.landscape.rdttest.co" + servicePath + tenant + environment + locationAbsolutePath;
    }
}