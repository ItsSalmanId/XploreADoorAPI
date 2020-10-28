using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FoxRehabilitationAPI.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }
    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class RegisterBindingModel
    {
        [Display(Name = "NPI")]
        [MaxLength(10, ErrorMessage = "The {0} must be {2} characters long.")]
        [MinLength(10, ErrorMessage = "The {0} must be {2} characters long.")]
        public string NPI { get; set; }

        [Required]
        [Display(Name = "First name")]
        [MaxLength(100)]
        public string FIRST_NAME { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [MaxLength(100)]
        public string LAST_NAME { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PASSWORD { get; set; }

        [Required]
        [Display(Name = "Mobile no")]
        public string PHONE_NO { get; set; }

        [Required]
        [Display(Name = "Sender type")]
        public long FOX_TBL_SENDER_TYPE_ID { get; set; }

        [Display(Name = "Gender")]
        public string GENDER { get; set; }

        [Display(Name = "Address")]
        public string ADDRESS_1 { get; set; }

        [Display(Name = "Practice organization name")]
        public string CITY { get; set; }

        [Display(Name = "State")]
        public string STATE { get; set; }

        [Display(Name = "ZIP Code")]
        public string ZIP { get; set; }

        [Display(Name = "Time zone")]
        public string TIME_ZONE { get; set; }

        [Display(Name = "Work phone")]
        public string WORK_PHONE { get; set; }

        [Display(Name = "Mobile phone")]
        public string MOBILE_PHONE { get; set; }
        public string ACO_TEXT { get; internal set; }
        public string HHH_TEXT { get; internal set; }
        public string PRACTICE_ORGANIZATION_TEXT { get; internal set; }
        public string SNF_TEXT { get; internal set; }
        public string HOSPITAL_TEXT { get; internal set; }
        public string SPECIALITY_TEXT { get; internal set; }
        public string THIRD_PARTY_REFERRAL_SOURCE { get; internal set; }
    }
    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }
    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
