using Blogosphere.Web.Models.DTOs;
using Blogosphere.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogosphere.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository,UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        [HttpGet]   
        public async Task<IActionResult>  List()
        {
            var users = await userRepository.GetAll();
            var usersViewModel = new UsersViewModel();
            usersViewModel.Users=new List<User>();
            foreach (var user in users) 
            {
                usersViewModel.Users.Add(new User
                {
                    Id=Guid.Parse(user.Id),
                    EmailAddress=user.Email,
                    UserName=user.UserName
                });
            }
            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UsersViewModel usersViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName=usersViewModel.UserName,
                Email=usersViewModel.Email,

            };
          var identityResult=  await userManager.CreateAsync(identityUser,usersViewModel.Password);
            if (identityResult != null) 
            {
                if (identityResult.Succeeded) 
                { 
                    //Assign role to this User
                    var roles=new List<string>();
                    if (usersViewModel.AdminRoleCheckBox)
                    {
                        roles.Add("Admin");
                    }
                   identityResult= await userManager.AddToRolesAsync(identityUser, roles);
                    if (identityResult.Succeeded && identityResult != null) 
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user=await userManager.FindByIdAsync(id.ToString());
            if (user !=null)
            {
               var identityResult= await userManager.DeleteAsync(user);
                if (identityResult != null && identityResult.Succeeded) 
                {
                    //Show Success Message
                    return RedirectToAction("List", "AdminUsers");
                }
            }
            //Show failure Message
            return RedirectToAction("List", "AdminUsers");
        }
    }
}
