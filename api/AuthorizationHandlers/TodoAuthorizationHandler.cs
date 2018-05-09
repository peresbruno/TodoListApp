using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace api.AuthorizationHandlers
{    
    public class TodoAuthorizationHandler : AuthorizationHandler<TodoOwnerRequirement, TodoItem>
    {
        
        private readonly UserManager<IdentityUser> _userManager;

        public TodoAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TodoOwnerRequirement requirement, TodoItem resource)
        {
            var user = await _userManager.FindByIdAsync(resource.UserId.ToString());

            return;

        }
    }

    public class TodoOwnerRequirement : IAuthorizationRequirement
    {
    }
}
