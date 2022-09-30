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
        //ResponseModel InsertAnnouncement(Announcements objAnnouncement, UserProfile profile);
        List<Announcements> GetAnnouncement(Announcements objAnnouncement, UserProfile profile);
        //List<Announcement> GetAnnouncement(Announcements objAnnouncement, UserProfile profile);
        //List<AnnouncementRoles> AddAnnouncementRole(AnnouncementRoles announcementRoles, UserProfile profile);
        //ResponseModel AddAnnouncementRole(List<AnnouncementRoles> objAnnouncementRoles, UserProfile profile);
        ResponseModel AddAnnouncementRole(List<AnnouncementRoles> objAnnouncementRoles, UserProfile profile);
        Announcements InsertAnnouncement(Announcements objAnnouncement, UserProfile profile);
        //PSSearchData GetPSSearchData(long practiceCode);
    }
}
