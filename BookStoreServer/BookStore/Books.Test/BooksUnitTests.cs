
using System.Net;
using System.Text;

namespace Books.Test;

public class BooksUnitTests {
    [Fact]
    public async Task GetBooks() {
        var client = new HttpClient();
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/books");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Authorization() {
        var handler = new HttpClientHandler();
        handler.UseCookies = true;

        var client = new HttpClient(handler);
        
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/users");
        var response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        
        request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/auth/login");
        request.Content = new StringContent("{\"email\":\"admin@admin.com\",\"password\":\"admin\"}", Encoding.UTF8, "application/json");

        response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/users");
        var cookies = handler.CookieContainer.GetAllCookies();
        request.Headers.Add("Authorization", $"Bearer {cookies["access-token"]!.Value}");
        response = await client.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}