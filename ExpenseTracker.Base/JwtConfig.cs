namespace ExpenseTracker.Base;

public class JwtConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessTokenExpiration { get; set; }
    public string SecretKey { get; set; }
}