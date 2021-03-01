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

        public async Task<IActionResult> OnPostUpdateUserAsync(User updateUser)
        {
            if (updateUser.Id.Equals(Guid.Empty))
            {
                ViewData["updateUserError"] = "Id is not specified";
                return Page();
            }
            User user = await _usersApi.GetUserById(updateUser.Id);
            UpdateUserValues(user, updateUser);
            if(await _usersApi.UpdateUser(user))
            {
                return RedirectToPage();
            }
            ViewData["updateUserError"] = "Failed to update user";
            return Page();
        }

        public IActionResult OnPostSignout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("Index");
        }


        private void UpdateUserValues(User user, User updateUser)
        {
            if (!string.IsNullOrEmpty(updateUser.Email)) user.Email = updateUser.Email;
            if (!string.IsNullOrEmpty(updateUser.Username)) user.Username = updateUser.Username;
            if (!string.IsNullOrEmpty(updateUser.Password)) user.Password = updateUser.Password;
        }
    }
}
