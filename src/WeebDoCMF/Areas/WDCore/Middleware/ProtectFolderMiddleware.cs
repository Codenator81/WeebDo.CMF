using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WeebDoCMF.WDCore.Middleware
{
    public class ProtectFolderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PathString _path;
        private readonly string _roleName;
        private readonly ILogger _logger;

        public ProtectFolderMiddleware(RequestDelegate next, ProtectFolderOptions options, ILoggerFactory loggerFactory)
        {
            _next = next;
            _path = options.Path;
            _roleName = options.RoleName;
            _logger = loggerFactory.CreateLogger(typeof(ProtectFolderMiddleware).FullName);
        }

        public async Task Invoke(HttpContext httpContext,
                                 IAuthorizationService authorizationService)
        {            
            if (httpContext.Request.Path.StartsWithSegments(_path))
            {
                var authorized = httpContext.User.IsInRole(_roleName);
                if (!authorized)
                {
                    await httpContext.Authentication.ChallengeAsync();
                    return;
                }
            }

            await _next(httpContext);
        }        
    }

    public class ProtectFolderOptions
    {
        public PathString Path { get; set; }
        public string RoleName { get; set; }
    }

    public static class ProtectFolderExtensions
    {
        public static IApplicationBuilder UseProtectFolder(
            this IApplicationBuilder builder,
            ProtectFolderOptions options)
        {
            return builder.UseMiddleware<ProtectFolderMiddleware>(options);
        }
    }

   
}
