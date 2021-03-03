using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationService.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.ApiCollection.Interfaces;
using Shopping.Entities;

namespace Shopping.Pages
{
    public class RegistrationModel : PageModel
    {
        private IUsersApi _usersApi;
        public RegistrationModel(IApiFactory factory)
        {
            _usersApi = factory.UsersApi ?? throw new ArgumentNullException(nameof(_usersApi));
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostRegisterUserAsync(RegistrationUser regUser)
        {
            if (!ModelState.IsValid) return RedirectToPage(new { registrationError = "Fill all fields" });
            User user = new User
            {
                Firstname = regUser.Firstname,
                Lastname = regUser.Lastname,
                Password = regUser.Password,
                Username = regUser.Username,
                Email = regUser.Email,
                Role = regUser.Role,
                Id = Guid.NewGuid()
            };
            string responseString = await _usersApi.RegistrationToken(user);
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var claims = handler.ReadJwtToken(responseString).Claims;
                string id = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                if(!string.IsNullOrEmpty(id)) HttpContext.Session.SetString("id", id);
                string username = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;
                if (!string.IsNullOrEmpty(username)) HttpContext.Session.SetString("username", username);
                string role = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
                if (!string.IsNullOrEmpty(role)) HttpContext.Session.SetString("role", role);
            } catch (Exception e)
            {
                if (responseString.Length < 50)
                {
                    var errorResponse = new { registrationError = responseString };
                    return RedirectToPage(errorResponse);
                } else
                {
                    throw;
                }
            }
                return RedirectToPage("Product", new { pageNumber = 1});
        }
    }
}
