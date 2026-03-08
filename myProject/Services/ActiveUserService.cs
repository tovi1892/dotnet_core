using myProject.Interfaces;
using myProject.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace myProject.Services
{
    public class ActiveUserService : IActiveUser
    {
        public User? ActiveUser { get; private set; }

        public ActiveUserService(IHttpContextAccessor context, IUserService userService)
        {
            // שליפת ה-Username מ-Claims של ה-JWT Token
            var usernameClaim = context?.HttpContext?.User?.FindFirst("username")?.Value;
            
            if (!string.IsNullOrEmpty(usernameClaim))
            {
                // חיפוש המשתמש לפי השם מ-Claims
                ActiveUser = userService.Get().FirstOrDefault(u => u.Name == usernameClaim);
            }
        }
    }
}