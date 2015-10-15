using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using WeebDoCMF.WDCore.Models;

namespace WeebDoCMF.WDCore.Middleware
{
    public class ProtectFolderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PathString _path;
        private readonly string _roleName;
        public UserManager<WeebDoCmfUser> _userManager { get; private set; }
        private readonly ILogger _logger;

        public ProtectFolderMiddleware(UserManager<WeebDoCmfUser> userManager, RequestDelegate next, ProtectFolderOptions options, ILoggerFactory loggerFactory)
        {
            _next = next;
            _path = options.Path;
            _roleName = options.RoleName;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger(typeof(ProtectFolderMiddleware).FullName);
        }

        public async Task Invoke(HttpContext httpContext,
                                 IAuthorizationService authorizationService)
        {
            //_logger.LogWarning("Starting");
            //var useridaa = httpContext.User.Identity.IsAuthenticated;
            //_logger.LogWarning("httpContext.User:useridaa" + useridaa);
            
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
