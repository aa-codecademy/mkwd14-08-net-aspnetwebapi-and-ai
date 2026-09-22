using System.Security.Claims;

namespace NotesApp.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        // 1) The same constant AuthService wrote the claim with. It travels as
        // "nameid" inside the token and comes back as ClaimTypes.NameIdentifier -
        // .NET shortens it on the way out and expands it again on the way in.
        Claim? userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        // 2) Every claim value is a string - the token is JSON, it has no int.
        if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new InvalidOperationException("The token does not contain a valid user id.");
        }

        return userId;
    }
}
