namespace Books.API; 

public class JwtSettings {
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string AccessKey { get; set; }
    public string RefreshKey { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
}