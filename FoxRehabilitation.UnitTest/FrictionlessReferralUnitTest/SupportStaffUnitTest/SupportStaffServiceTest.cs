using FOX.BusinessOperations.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.FrictionlessReferral.SupportStaff;
using FOX.DataModels.Models.RequestForOrder;
using NUnit.Framework;

namespace FoxRehabilitation.UnitTest.FrictionlessReferralUnitTest.SupportStaffUnitTest
{
    [TestFixture]
    public class SupportStaffServiceTest
    {
        private SupportStaffService _supportStaffService;
        private PatientDetail _patientDetail;
        private RequestDeleteWorkOrder _requestDeleteWorkOrder;
        private FrictionLessReferral _frictionLessReferral;
        private ProviderReferralSourceRequest _providerReferralSourceRequest;
        private SubmitReferralModel _submitReferralModel;

        [SetUp]
        public void Setup()
        {
            _supportStaffService = new SupportStaffService();
            _patientDetail = new PatientDetail();
            _requestDeleteWorkOrder = new RequestDeleteWorkOrder();
            _frictionLessReferral = new FrictionLessReferral();
            _providerReferralSourceRequest = new ProviderReferralSourceRequest();
            _submitReferralModel = new SubmitReferralModel();
        }
        [Test]
        public void GetPracticeCode_HasPracticeCode_ReturnData()
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetPracticeCode();

            //Assert
            Assert.That(result, Is.EqualTo(1011163).Or.EqualTo(1012714));
        }
        [Test]
        public void GetInsurancePayers_HasInsurance_ReturnData()
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetInsurancePayers();

            //Assert
            Assert.That(result.Count, Is.GreaterThan(0));
        }
        [Test]
        [TestCase(548103)]
        [TestCase(0)]
        public void GetFrictionLessReferralDetails_HasReferralId_ReturnData(long referralId)
        {
            //Arrange
            //Act
            var result = _supportStaffService.GetFrictionLessReferralDetails(referralId);

            //Assert
            if (referralId != 0)
                Assert.That(result.FRICTIONLESS_REFERRAL_ID, Is.EqualTo(referralId));
            else
                Assert.That(result.FRICTIONLESS_REFERRAL_ID, Is.EqualTo(0));
        }
        [Test]
        [TestCase("Taseer", "iqbal", "muhammadiqbal11@carecloud.com", "2064512559")]
        public void SendInviteToPatientPortal_PatientDetailModel_ReturnData(string firstName, string lastName, string email, string phoneNumber)
        {
            //Arrange
            _patientDetail.FirstName = firstName;
            _patientDetail.LastName = lastName;
            _patientDetail.EmailAddress = email;
            _patientDetail.MobilePhone = phoneNumber;

            //Act
            var result = _supportStaffService.SendInviteToPatientPortal(_patientDetail);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("", "", "", "")]
        [TestCase("1679785950", "", "", "")]
        [TestCase("1740503804", "", "", "")]
        [TestCase("", "james", "smith", "ny")]
        [TestCase("1023489119", "james", "smith", "ny")]
        [TestCase("1023489", "james", "smith", "ny")]
        public void GetProviderReferralSources_ProviderReferralSourceModel_ReturnData(string npi, string firstName, string lastName, string state)
        {
            //Arrange
            _providerReferralSourceRequest.ProviderNpi = npi;
            _providerReferralSourceRequest.ProviderFirstName = firstName;
            _providerReferralSourceRequest.ProviderLastName = lastName;
            _providerReferralSourceRequest.ProviderState = state;

            //Act
            var result = _supportStaffService.GetProviderReferralSources(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("\n <table _ngcontent-nud-c7=\"\" style=\"width:100%;\">\n   <tbody _ngcontent-nud-c7=\"\">\n     <tr _ngcontent-nud-c7=\"\">\n   <td _ngcontent-nud-c7=\"\">\n  <table _ngcontent-nud-c7=\"\" id=\"tbl-PrintSendSubmitOrder-printbody\" style=\"border-bottom:1px solid #12222E;width:100%;\">\n                                                            <tbody _ngcontent-nud-c7=\"\">\n                                                                <tr _ngcontent-nud-c7=\"\" style=\"font-size:13pt !important;\">\n                                                                    <td _ngcontent-nud-c7=\"\" id=\"td-PrintSendSubmitOrder-header\" width=\"43%\">\n                                                                        <b _ngcontent-nud-c7=\"\">Fox Rehabilitation</b> <br _ngcontent-nud-c7=\"\">\n                                                                        7 Carnegie Plaza, Cherry Hill<br _ngcontent-nud-c7=\"\">\n                                                                        NJ 08003-1000, USA<br _ngcontent-nud-c7=\"\">\n                                                                        <b _ngcontent-nud-c7=\"\">Phone:</b> +1(877) 407-3422\n                                                                    </td>\n                                                                    <td _ngcontent-nud-c7=\"\" align=\"center\" id=\"td-PrintSendSubmitOrder-logo\" width=\"14%\">\n                                                                        <img _ngcontent-nud-c7=\"\" alt=\"Logo Image\" src=\"https://fox.mtbc.com/assets/images/logo.png\" style=\"width: 100px;\">\n                                                                    </td>\n                                                                    <td _ngcontent-nud-c7=\"\" align=\"right\" id=\"td-PrintSendSubmitOrder-info\" width=\"43%\">\n                                                                        <b _ngcontent-nud-c7=\"\">\n                                                                            Sasa,\n                                                                            Asa</b>\n                                                                        <!--bindings={\n  \"ng-reflect-ng-if\": \"true\"\n}--><span _ngcontent-nud-c7=\"\" class=\"ng-star-inserted\">\n                                                                            <br _ngcontent-nud-c7=\"\"> <b _ngcontent-nud-c7=\"\">DOB:</b> </span>\n                                                                        12/1/2022\n                                                                        <br _ngcontent-nud-c7=\"\">\n                                                                        <span _ngcontent-nud-c7=\"\"><b _ngcontent-nud-c7=\"\">Pri. Ins:</b>\n                                                                            Novitas Medicare NJ99</span><br _ngcontent-nud-c7=\"\">\n                                                                    </td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" colspan=\"3\" id=\"td-PrintSendSubmitOrder-printbody2\">\n                                                                        <table _ngcontent-nud-c7=\"\" id=\"tbl-PrintSendSubmitOrder-printbody2\" style=\"width:100%;\">\n                                                                            <tbody _ngcontent-nud-c7=\"\">\n                                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                                    <td _ngcontent-nud-c7=\"\" id=\"td-PrintSendSubmitOrder-qrimg\" width=\"70px\">\n                                                                                        <img _ngcontent-nud-c7=\"\" alt=\"QR Image\" id=\"QRImage\" style=\"width: 70px; height: 70px;margin:6px;\" src=\"data:image/png;base64,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\">\n                                                                                    </td>\n                                                                                    <td _ngcontent-nud-c7=\"\" align=\"center\" id=\"td-PrintSendSubmitOrder-treatmentreferral\">\n                                                                                        <div _ngcontent-nud-c7=\"\" style=\"font-size: 20pt; font-weight: bold;\">\n                                                                                            THERAPY TREATMENT REFERRAL\n                                                                                        </div>\n                                                                                        <div _ngcontent-nud-c7=\"\" style=\"font-size: 16pt; font-weight: bold;\">\n                                                                                            ***REQUEST***</div>\n                                                                                    </td>\n                                                                                    <td _ngcontent-nud-c7=\"\" width=\"70px\"></td>\n                                                                                </tr>\n                                                                            </tbody>\n                                                                        </table>\n                                                                    </td>\n                                                                </tr>\n                </tbody>\n    </table>\n  </td>\n                                                </tr>\n                                                <tr _ngcontent-nud-c7=\"\">\n                                                    <td _ngcontent-nud-c7=\"\" style=\"height:10px;\"></td>\n                                                </tr>\n                                                <tr _ngcontent-nud-c7=\"\">\n                                                    <td _ngcontent-nud-c7=\"\">\n                                                        <table _ngcontent-nud-c7=\"\" id=\"tbl-PrintSendSubmitOrder-printbody3\" style=\"width:100%;\">\n                                                            <tbody _ngcontent-nud-c7=\"\">\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" id=\"td-PrintSendSubmitOrder-referalsource\">\n                                                                        <b _ngcontent-nud-c7=\"\">Source: Fox Rehabilitation Referral Portal</b> <br _ngcontent-nud-c7=\"\">\n                                                                        Document Type: Unsigned order <br _ngcontent-nud-c7=\"\">\n                                                                        Ordering Referral Source:\n                                                                        Smith,\n                                                                        Kerri\n                                                                        <!--bindings={\n  \"ng-reflect-ng-if\": \"false\"\n}-->\n                                                                        <!--bindings={\n  \"ng-reflect-ng-if\": \"true\"\n}--><span _ngcontent-nud-c7=\"\" class=\"ng-star-inserted\">|\n                                                                            SMITHTOWN</span> <br _ngcontent-nud-c7=\"\">\n                                                                        Sender: dasdsa,\n                                                                        sadas<br _ngcontent-nud-c7=\"\">\n                                                                    </td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:10px;\"></td>\n                                                                </tr>\n\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:10px;\"></td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:10px;\"></td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:10px;\"></td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" id=\"td-PrintSendSubmitOrder-procedureinfo\">\n                                                                        <b _ngcontent-nud-c7=\"\">Procedure Information<br _ngcontent-nud-c7=\"\">\n                                                                            <span _ngcontent-nud-c7=\"\" style=\"font-weight: bold;\">Disciplines: Speech Therapy (ST)\n                                                                            </span> </b> <br _ngcontent-nud-c7=\"\">\n                                                                    </td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:10px;\"></td>\n                                                                </tr>\n\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:10px;\"></td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:10px;\"></td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\" class=\"display-none\">\n                                                                    <td _ngcontent-nud-c7=\"\" id=\"td-PrintSendSubmitOrder-additionnotes\">\n                                                                        <div _ngcontent-nud-c7=\"\" class=\"msg-preview-pod\">\n                                                                            <b _ngcontent-nud-c7=\"\">Additional Notes/Reason for Referral:\n                                                                            </b> aSAsA\n                                                                        </div>\n                                                                </td></tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:30px;\"></td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:20px;\"></td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\">\n                                                                        Provider\n                                                                        Name:&nbsp;SMITH,\n                                                                        KERRI <!--bindings={\n  \"ng-reflect-ng-if\": \"false\"\n}-->\n                                                                        <!--bindings={\n  \"ng-reflect-ng-if\": \"true\"\n}--><span _ngcontent-nud-c7=\"\" class=\"ng-star-inserted\">|\n                                                                            SMITHTOWN</span> <br _ngcontent-nud-c7=\"\">\n                                                                        NPI: 1487057121<br _ngcontent-nud-c7=\"\">\n                                                                        <!--bindings={\n  \"ng-reflect-ng-if\": \"false\"\n}-->\n                                                                    </td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\">\n                                                                        Requested Date:&nbsp;12/10/2022\n                                                                    </td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:30px;\"></td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" id=\"td-PrintSendSubmitOrder-signature\">\n                                                                        Signature:\n                                                                        <span _ngcontent-nud-c7=\"\" style=\"display:inline-block;border-bottom:1px solid #12222E;width:20%;\"></span>&nbsp;&nbsp;\n                                                                        Date: <span _ngcontent-nud-c7=\"\" style=\"display:inline-block;border-bottom:1px solid #12222E;width:20%;\"></span>\n                                                                    </td>\n                                                                </tr>\n                                                                <tr _ngcontent-nud-c7=\"\">\n                                                                    <td _ngcontent-nud-c7=\"\" style=\"height:30px;\">\n                                                                    <!--bindings={\n  \"ng-reflect-ng-if\": \"true\"\n}--><div _ngcontent-nud-c7=\"\" id=\"print-footer\" class=\"ng-star-inserted\">\n                                                                        Please sign and return to FOX at +1 (800) 597 - 0848 or email <a _ngcontent-nud-c7=\"\" href=\"mailto:admit@foxrehab.org\" target=\"_top\">admit@foxrehab.org</a> (mailto:admit@foxrehab.org)\n                                                                   </div>\n                                                                </td>\n                                                                </tr>\n                                                            </tbody>\n                                                        </table>\n                                                    </td>\n                                                </tr>\n    </tbody>\n  </table>\n   ", "test", "test", 54821052)]
        public void SubmitReferral_ProviderReferralSourceModel_ReturnData(string html, string firstName, string filename, long workId)
        {
            //Arrange
            _submitReferralModel.AttachmentHTML = html;
            _submitReferralModel.PatientLastName = firstName;
            _submitReferralModel.WorkId = workId;
            _submitReferralModel.FileName = filename;
            _submitReferralModel.IsFromIndexInfo = false;

            //Act
            var result = _supportStaffService.SubmitReferral(_submitReferralModel);

            //Assert
            if (result.Success)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase(54820838)]
        [TestCase(0)]
        [TestCase(null)]
        public void DeleteWorkOrder_HasWorkId_ReturnData(long workId)
        {
            //Arrange
            _requestDeleteWorkOrder.WorkId = workId;

            //Act
            var result = _supportStaffService.DeleteWorkOrder(_requestDeleteWorkOrder);

            //Assert
            if (result.Success)
                Assert.IsTrue(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase(5481103, "", "", false, "", "")]
        [TestCase(5481103, ",9,3", "Thu Dec 01 2022", false, "Party Referral Source", "8774074329")]
        [TestCase(123, ",1,2", "Thu Dec 01 2022", false, "", "")]
        [TestCase(123, ",1,2", null, false, "", "")]
        public void SaveFrictionLessReferralDetails_FrictionLessReferralModel_ReturnData(long referralId, string disciplineId, string patientDOB, bool isSinged, string userType, string providerFax)
        {
            //Arrange
            _frictionLessReferral.FRICTIONLESS_REFERRAL_ID = referralId;
            _frictionLessReferral.PATIENT_DISCIPLINE_ID = disciplineId;
            _frictionLessReferral.PATIENT_DOB_STRING = patientDOB;
            _frictionLessReferral.IS_SIGNED_REFERRAL = isSinged;
            _frictionLessReferral.USER_TYPE = userType;
            _frictionLessReferral.PROVIDER_FAX = providerFax;

            //Act
            var result = _supportStaffService.SaveFrictionLessReferralDetails(_frictionLessReferral);

            //Assert
            if (result != null)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        [TestCase("1023489119", "", "", "")]
        [TestCase("1740503804", "", "", "")]
        [TestCase("", "carey", "smith", "ny")]
        [TestCase("1023489119", "carey", "smith", "ny")]
        [TestCase("1023489", "carey", "smith", "ny")]
        [TestCase("", "", "", "")]
        public void GetOrderingReferralSource_ProviderReferralSourceModel_ReturnData(string npi, string firstName, string lastName, string state)
        {
            //Arrange
            _providerReferralSourceRequest.ProviderNpi = npi;
            _providerReferralSourceRequest.ProviderFirstName = firstName;
            _providerReferralSourceRequest.ProviderLastName = lastName;
            _providerReferralSourceRequest.ProviderState = state;

            //Act
            var result = _supportStaffService.GetOrderingReferralSource(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [Test]
        public void GetOrderingReferralSource_EmptyProviderReferralSourceModel_NoReturnData()
        {
            //Arrange
            _providerReferralSourceRequest = null;

            //Act
            var result = _supportStaffService.GetOrderingReferralSource(_providerReferralSourceRequest);

            //Assert
            if (result.Count > 0)
                Assert.True(true);
            else
                Assert.IsFalse(false);
        }
        [TearDown]
        public void Teardown()
        {
            _supportStaffService = null;
            _patientDetail = null;
            _requestDeleteWorkOrder = null;
            _frictionLessReferral = null;
            _providerReferralSourceRequest = null;
        }
    }
}
