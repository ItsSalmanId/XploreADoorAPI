using FOX.DataModels.Context;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.SettingsService.AnnouncementService
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly DbContextSettings _settingsContext = new DbContextSettings();
        private readonly GenericRepository<FoxRoles> _foxRolesRepository;

        public AnnouncementService()
        {
            _foxRolesRepository = new GenericRepository<FoxRoles>(_settingsContext);
        }
        // Description: This function is trigger to get role names
        public List<FoxRoles> GetFoxRoles(UserProfile userProfile)
        {
            List<FoxRoles> result = new List<FoxRoles>();
            if (userProfile != null && userProfile.PracticeCode != 0)
            {
                result = _foxRolesRepository.GetMany(r => r.PRACTICE_CODE == userProfile.PracticeCode && !r.DELETED);
            }
            return result;
        }
    }
}
