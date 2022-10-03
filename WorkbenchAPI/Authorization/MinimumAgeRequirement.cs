using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkbenchAPI.Authorization
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; init; }
        public MinimumAgeRequirement(int _minimumAge)
        {
            MinimumAge = _minimumAge;
        }   
    }
}
