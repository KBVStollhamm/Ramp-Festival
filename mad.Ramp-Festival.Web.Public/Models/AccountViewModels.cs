using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mad.Ramp_Festival.Web.Public.Models
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}

	public class ExternalLoginListViewModel
	{
		public string ReturnUrl { get; set; }
	}

	public class SendCodeViewModel
	{
		public string SelectedProvider { get; set; }
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
		public string ReturnUrl { get; set; }
		public bool RememberMe { get; set; }
	}

	public class VerifyCodeViewModel
	{
		[Required]
		public string Provider { get; set; }

		[Required]
		[Display(Name = "Code")]
		public string Code { get; set; }
		public string ReturnUrl { get; set; }

		[Display(Name = "Remember this browser?")]
		public bool RememberBrowser { get; set; }

		public bool RememberMe { get; set; }
	}

	public class ForgotViewModel
	{
		[Required]
		[Display(Name = "E-Mail")]
		public string Email { get; set; }
	}

	public class LoginViewModel
	{
		[Required]
		[Display(Name = "E-Mail")]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Passwort")]
		public string Password { get; set; }

		[Display(Name = "Anmeldedaten merken?")]
		public bool RememberMe { get; set; }
	}

	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "E-Mail")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "Das {0} muss mindestens {2} Zeichen lang sein.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Passwort")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Passwort bestätigen")]
		[Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
		public string ConfirmPassword { get; set; }

		public string DisplayName { get; set; }

		[Display(Name="Newsletter abonieren")]
		public bool SubscribedForNewsletters { get; set; }
	}

	public class ResetPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "E-Mail")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "E-Mail")]
		public string Email { get; set; }
	}
}
