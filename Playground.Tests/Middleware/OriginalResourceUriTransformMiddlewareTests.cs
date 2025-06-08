namespace Playground.Tests.Middleware;

using Playground.Middleware;
using Shouldly;
using Xunit;

public class OriginalResourceUriTransformMiddlewareTests
{
    [Fact]
    public void OriginalResourceUriTransformMiddleware_InvokeAsync_Transforms_Successfully()
    {
        const string body =
            "{\"DocumentId\": 157,\"ArtifactPath\": \"attachment/123/456/Claims-Complaint2.pdf6\",\"OriginalResourceUri\": \"http://fctyiadweb01.fcty.rdt-factory.co.uk:8550/api/v2/documents/157/artifacts/attachment/123/456/Claims-Complaint2.pdf6\"}";
    
        var sut = new OriginalResourceUriTransformMiddleware();

        var result = sut.Transform(body);
        
        result.ShouldBe("{\"DocumentId\":157,\"ArtifactPath\":\"attachment/123/456/Claims-Complaint2.pdf6\",\"OriginalResourceUri\":\"https://test.api.landscape.rdttest.co/rdt-documentstorage/service/rdtproductionfactory/iad/api/v2/documents/157/artifacts/attachment/123/456/Claims-Complaint2.pdf6\"}");
    }
}