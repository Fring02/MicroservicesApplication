using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.ApiCollection.Interfaces;
using Shopping.Entities;

namespace Shopping.Pages
{
    public class ProfileModel : PageModel
    {
        private IUsersApi _usersApi;
        public ProfileModel(IApiFactory factory)
        {
            _usersApi = factory.UsersApi ?? throw new ArgumentNullException(nameof(_usersApi));
        }
        public User User { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            User = await _usersApi.GetUserById(Guid.Parse(HttpContext.Session.GetString("id")));
            return Page();
        }
    }
}
