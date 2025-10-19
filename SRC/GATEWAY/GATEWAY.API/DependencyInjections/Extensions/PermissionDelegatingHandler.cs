using System.Net;

namespace GATEWAY.API.Extensions;

public class PermissionDelegatingHandler : DelegatingHandler
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PermissionDelegatingHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var userId = request.Headers.GetValues("X-User-Id").FirstOrDefault(); // hoặc từ JWT

        var function = request.RequestUri.AbsolutePath;
        var method = request.Method.Method.ToUpper(); // GET → READ, POST → CREATE, etc

        var action = MapHttpMethodToAction(method); // You define this

        // Gửi request đến Identity server để check quyền
        var client = _httpClientFactory.CreateClient("Identity");

        var response = await client.GetAsync(
            $"permissions/check?userId={userId}&function={function}&action={action}");

        if (!response.IsSuccessStatusCode || response.Content.ReadAsStringAsync().Result != "true")
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent("Permission denied")
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private string MapHttpMethodToAction(string method) => method switch
    {
        "GET" => "READ",
        "POST" => "CREATE",
        "PUT" => "UPDATE",
        "DELETE" => "DELETE",
        _ => "UNKNOWN"
    };
}
