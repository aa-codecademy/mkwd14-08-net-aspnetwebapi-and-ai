namespace NotesApp.Services.Configuration;

public class JwtSettings
{
    // SecretKey => used to sign the JWT token, and it should be kept secret. It is used to create a digital signature for the token, which allows the server to verify that the token has not been tampered with. The secret key should be long, random, and stored securely (e.g., in environment variables or a secure vault).
    // You can use this command in Git Bash to generate a random key: `openssl rand -base64 32`
    public string SecretKey { get; set; } = string.Empty;
    // Issuer => identifies the principal that issued the JWT. It is used to verify that the token was issued by a trusted source. The issuer is typically the URL or name of the application or service that generated the token.
    public string Issuer { get; set; } = string.Empty;
    // Audience => identifies the recipients that the JWT is intended for. It is used to verify that the token is being used by the correct recipient.
    public string Audience { get; set; } = string.Empty;
    // ExpirationInMinutes => specifies the duration for which the JWT token will be valid.
    public int ExpirationInMinutes { get; set; } 
}
