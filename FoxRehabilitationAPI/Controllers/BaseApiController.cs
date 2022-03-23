using FOX.BusinessOperations.Security;
using FOX.DataModels;
using FOX.DataModels.Models.Security;
using FoxRehabilitationAPI.Filters;
using FoxRehabilitationAPI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoxRehabilitationAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    public class BaseApiController : ApiController
    {
        protected UserProfile GetProfile()
        {
            UserProfile profile = ClaimsModel.GetUserProfile(User.Identity as System.Security.Claims.ClaimsIdentity) ?? new UserProfile();
            EntityHelper.isTalkRehab = profile.isTalkRehab;
            return profile;
        }

        ApplicationUserManager _userManager;
        protected ApplicationUserManager UserManager
        {
            get
            {
                _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                return _userManager ?? Request?.GetOwinContext()?.GetUserManager<ApplicationUserManager>() ?? null;
            }
        }

        ApplicationUserManager _talkRehabUserManager;
        protected ApplicationUserManager TalkRehabUserManager
        {
            get
            {
                _talkRehabUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new TalkRehabDBContext()));
                return _talkRehabUserManager ?? Request?.GetOwinContext()?.GetUserManager<ApplicationUserManager>() ?? null;
            }
        }

        protected async System.Threading.Tasks.Task<ApplicationUser> FindProfileAsync(string userName, string password)
        {
            ApplicationUser user = new ApplicationUser();
            UserProfile userProfile = new UserProfile();
            if (EntityHelper.isTalkRehab)
            {
                using (var dbContext = new TalkRehabDBContext())
                {
                    var userService = new FOX.BusinessOperations.SettingsService.UserMangementService.UserManagementService();
                    userProfile = userService.GetUserProfileByName(userName);
                    if (userProfile != null)
                    {
                        user = await TalkRehabUserManager.FindAsync(userProfile.UserName, password);
                        if (user == null)
                        {
                            string encryptedPass = Encrypt.getEncryptedCode(password);
                            user = await dbContext.Users.Where(u => (u.Email.ToLower() == userName.ToLower() || u.UserName.ToLower() == userName.ToLower()) && (u.PASSWORD == encryptedPass)).FirstOrDefaultAsync();
                        }
                        if (user != null)
                        {
                            user.UserName = user.USER_NAME = userProfile.UserName;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }
            }
            else
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var userService = new FOX.BusinessOperations.SettingsService.UserMangementService.UserManagementService();
                    userProfile = userService.GetUserProfileByName(userName);
                    if (userProfile != null)
                    {
                        user = await UserManager.FindAsync(userProfile.UserName, password);
                        if (user == null)
                        {
                            string encryptedPass = Encrypt.getEncryptedCode(password);
                            user = await dbContext.Users.Where(u => (u.Email.ToLower() == userName.ToLower() || u.UserName.ToLower() == userName.ToLower()) && (u.PASSWORD == encryptedPass)).FirstOrDefaultAsync();
                        }
                        if (user != null)
                        {
                            user.UserName = user.USER_NAME = userProfile.UserName;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }
            }
            return user;
        }
    }
}
