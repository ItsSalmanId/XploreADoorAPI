using FOX.BusinessOperations.RequestForOrder;
using FOX.DataModels.Models.CommonModel;
using FOX.DataModels.Models.RequestForOrder;
using FOX.DataModels.Models.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxRehabilitation.UnitTest.RequestForOrderServiceUnitTest
{
    class RequestForOrderServiceTest
    {
        private RequestForOrderService _requestForOrderService;
        private UserProfile _userProfile;
        RequestSendEmailModel _requestSendEmailModel;
        [SetUp]
        public void Setup()
        {
            _requestForOrderService = new RequestForOrderService();
            _userProfile = new UserProfile();
        }


        [Test]
        public void SendEmailCheckSuccess()
        {
            //Arrange
            _userProfile.PracticeCode = 5110459;
            _userProfile.isTalkRehab = true;
            RequestSendEmailModel userTeamModelobj = new RequestSendEmailModel();
            userTeamModelobj.WorkId = 123;
            userTeamModelobj.AttachmentHTML = "";
            userTeamModelobj.EmailAddress = "faheemjaved@carecloud.com";
            //Act
            ResponseModel result = new ResponseModel();
            result = _requestForOrderService.SendEmail(userTeamModelobj, _userProfile);
            //Assert
            Assert.AreEqual(result.Success,false);
        }

        [Test]
        public void SendEmailCheckFailed()
        {
            //Arrange
            _userProfile.PracticeCode = 5110459;
            _userProfile.isTalkRehab = true;
            RequestSendEmailModel userTeamModelobj = new RequestSendEmailModel();
            userTeamModelobj.WorkId = 123;
            userTeamModelobj.AttachmentHTML = "";
            userTeamModelobj.EmailAddress = "";
            //Act
            ResponseModel result = new ResponseModel();
            result = _requestForOrderService.SendEmail(userTeamModelobj, _userProfile);
            //Assert
            Assert.AreEqual(result.Success, false);
        }
    }
}
