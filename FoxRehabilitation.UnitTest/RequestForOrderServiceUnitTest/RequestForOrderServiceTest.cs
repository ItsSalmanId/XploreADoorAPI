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
    [TestFixture]
    public class RequestForOrderServiceTest
    {
        private RequestForOrderService _requestForOrderService;
        private UserProfile _userProfile;
        private RequestSendEmailModel _requestSendEmailModel;
        private ResponseModel _result;
        [SetUp]
        public void Setup()
        {
            _requestForOrderService = new RequestForOrderService();
            _userProfile = new UserProfile();
            _result = new ResponseModel();
        }

        [Test]
        public void SendEmail_CheckRehabUser_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 5110459;
            _userProfile.isTalkRehab = false;
            _requestSendEmailModel.WorkId = 123;
            _requestSendEmailModel.AttachmentHTML = "";
            _requestSendEmailModel.EmailAddress = "faheemjaved@carecloud.com";

            //Act            
            _result = _requestForOrderService.SendEmail(_requestSendEmailModel, _userProfile);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }

        [Test]
        public void SendEmail_CheckEmail_ResponseFalse()
        {
            //Arrange
            _userProfile.PracticeCode = 5110459;
            _userProfile.isTalkRehab = true;
            _requestSendEmailModel.WorkId = 123;
            _requestSendEmailModel.AttachmentHTML = "";
            _requestSendEmailModel.EmailAddress = "";

            //Act
            _result = _requestForOrderService.SendEmail(_requestSendEmailModel, _userProfile);

            //Assert
            Assert.AreEqual(false, _result.Success);
        }
        [TearDown]
        public void Teardown()
        {
            // Optionally dispose or cleanup objects
            _requestForOrderService = null;
            _userProfile = null;
            _requestSendEmailModel = null;
            _result = null;
        }
    }
}
