using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace MyWebAPI.Security;


public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    )
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? username = null;
        try
        {
            string basicUsername = "catlover";
            string basicPasswordHashed = "AQAAAAEAACcQAAAAEPUxqGhmyduux+R2LeluVV9wUw66a3TazQxFyQ4yACKRBiBpz8LiEtBRFoMcX0xqQw==";
            string authorizationHeader = Request.Headers[HeaderNames.Authorization];
            AuthenticationHeaderValue authenticationHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeader);
            byte[] authenticationByte = Convert.FromBase64String(authenticationHeaderValue.Parameter!);
            string[] credentials = Encoding.UTF8.GetString(authenticationByte).Split(':');
            username = credentials.FirstOrDefault();
            string? password = credentials.LastOrDefault();
            var passwordValid = new PasswordHasher<string>().VerifyHashedPassword(null, basicPasswordHashed, password) == PasswordVerificationResult.Success;
            if (username != basicUsername || !passwordValid)
            {
                throw new ArgumentException("Invalid credentials");
            }
        }
        catch (Exception ex)
        {
            return await Task.FromResult(AuthenticateResult.Fail($"Authentication failed: {ex.Message}"));
        }
        var claims = new[] {
            new Claim(ClaimTypes.Name, username!),
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }
}