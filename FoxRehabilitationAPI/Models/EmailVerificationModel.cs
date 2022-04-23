using FOX.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxRehabilitationAPI.Models
{
    public class EmailVerificationModel
    {
        public string Email { get; set; }
        public string EncodedResponse { get; set; }
    }
    public class EmailValidationRes
    {
        public string EncryptEmailWithTicks { get; set; } = string.Empty;
        public bool IsValidate { get; set; } = false;
    }
    public class UpdatePasswordModel
    {
        public bool IsReset { get; set; }
        public int status { get; set; }
        public string Message { get; set; }
    }
    public class ValidatePassword: BaseModel
    {
        public string password { get; set; }
        public bool isTalkRehab { set; get; }
    }
}