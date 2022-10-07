using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.BusinessOperations.SettingsService.AnnouncementService
{
    public interface IAnnouncementService
    {
        List<FoxRoles> GetFoxRoles(UserProfile userProfile);
        List<Announcements> GetAnnouncement(Announcements objAnnouncement, UserProfile profile);
        Announcements GetAnnouncementDetails(Announcements objAnnouncement, UserProfile profile);
        ResponseModel AddAnnouncementRole(List<AnnouncementRoles> objAnnouncementRoles, UserProfile profile);
        ResponseModel InsertAnnouncement(AddEditFoxAnnouncement objAnnouncement, UserProfile profile);
        ResponseModel DeleteAnnouncement(Announcements objAnnouncement, UserProfile profile);
    }
}
