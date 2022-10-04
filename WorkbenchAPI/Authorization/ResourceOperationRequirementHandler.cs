using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;

namespace WorkbenchAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Shop>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ResourceOperationRequirement requirement, Shop shop)
        {
            if(requirement.ResourceOperation==ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }
            var clientId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var clientWithSearchId = shop.Clients.FirstOrDefault(x => x.Id == int.Parse(clientId));
            if(clientWithSearchId is not null)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
           
        }
    }
}
