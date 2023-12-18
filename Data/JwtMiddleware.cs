namespace Cafe.Data;

using Cafe.Models;


public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, User userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        // If the token is not in the Authorization header, check the cookie
        if (string.IsNullOrEmpty(token) && context.Request.Cookies.TryGetValue("AuthToken", out var cookieToken))
        {
            token = cookieToken;
        }

        var userId = jwtUtils.ValidateToken(token);
        if (userId != null)
        {
            // Attach user to context on successful jwt validation
            context.Items["User"] = userService.GetId(userId.Value);
        }

        await _next(context);
    }
}


