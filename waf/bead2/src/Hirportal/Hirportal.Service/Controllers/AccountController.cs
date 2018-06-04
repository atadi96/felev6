using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Hirportal.Persistence;
using Hirportal.Persistence.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Hirportal.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private SignInManager<Author> signInManager;
        private UserManager<Author> userManager;
        
        public AccountController(SignInManager<Author> signInManager, UserManager<Author> userManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // POST: api/account/login/username/password
        [HttpGet("login/{username}/{password}")]
        [Produces("application/json")]
        public async Task<AuthorDTO> Login(string username, string password)
        {
            if (signInManager.IsSignedIn(User))
            {
                string userName = User.Identity.Name;
                var user = await userManager.FindByNameAsync(userName);
                return new AuthorDTO()
                {
                    Name = user.Name,
                    Username = user.UserName
                };
            }
            else
            {
                var result = await signInManager.PasswordSignInAsync(username, password, true, false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(username);
                    return new AuthorDTO()
                    {
                        Name = user.Name,
                        Username = user.UserName
                    };
                }
                else
                {
                    return null;
                }
            }
            
        }
        
        // DELETE: api/account/logout
        [HttpGet("logout")]
        [Authorize]
        public async void Logout()
        {
            await signInManager.SignOutAsync();
        }
    }
}
