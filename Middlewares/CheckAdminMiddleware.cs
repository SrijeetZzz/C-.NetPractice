
// using Microsoft.AspNetCore.Http;
// using System.Threading.Tasks;

// public class CheckAdminEmailMiddleware
// {
//     private readonly RequestDelegate _next;

//     public CheckAdminEmailMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }

//     public async Task InvokeAsync(HttpContext context)
//     {
//         var email = context.User?.Identity?.IsAuthenticated == true
//             ? context.User.Claims.FirstOrDefault(c => c.Type == "email")?.Value
//             : null;

//         if (email != null && email.EndsWith("@admin", StringComparison.OrdinalIgnoreCase))
//         {
//             await _next(context);
//         }
//         else
//         {
//             context.Response.StatusCode = StatusCodes.Status403Forbidden;
//             await context.Response.WriteAsync("Access denied. Admins only.");
//         }
//     }
// }
