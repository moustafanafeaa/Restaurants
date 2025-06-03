using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Requirments
{
    public class MinimumAgeRequirment(int minAge) : IAuthorizationRequirement
    {
        public int MinAge { get; } = minAge; 
    }
}
