using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public abstract class JwtAuthentificationService<T>
    {
        protected IConfiguration _configuration;

        protected JwtAuthentificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public abstract ClaimsIdentity CreateIdentity(T user);
        public abstract string CreateToken(ClaimsIdentity identity);
    }
}
