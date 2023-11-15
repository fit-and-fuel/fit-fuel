using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
	public class SendEmailService : IEmailSender
	{
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<ApplicationUser> _userManager;
		public SendEmailService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
		{
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
		}

		public async Task EmailSenderAsync(string email, string subject, string HtmlMessage)
		{

			string password = _configuration["Email:Key"];
			string SenderEmail = _configuration["Email:DefaultFromEmailAddress"];
			string FromName = _configuration["Email:DefaultFromName"];




			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(FromName, SenderEmail));
			message.To.Add(new MailboxAddress("hi", email));
			message.Subject = subject;
			message.Body = new TextPart("plain")
			{
				Text = HtmlMessage
			};

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
				client.Authenticate(SenderEmail, password);
				client.Send(message);
				client.Disconnect(true);
			}
			//  return RedirectToAction("Index");
		}

		public async Task EmailToAdmin(string subject, string HtmlMessage)
		{
			string password = _configuration["Email:Key"];
			string SenderEmail = _configuration["Email:DefaultFromEmailAddress"];
			string FromName = _configuration["Email:DefaultFromName"];




			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(FromName, SenderEmail));
			message.To.Add(new MailboxAddress("To Admin", SenderEmail));
			message.Subject = subject;
			message.Body = new TextPart("plain")
			{
				Text = HtmlMessage
			};

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
				client.Authenticate(SenderEmail, password);
				client.Send(message);
				client.Disconnect(true);
			}
		}

		public async Task EmailToUser(string userEmail,string Name)
		{
			string subject = "register";
			string HtmlMessage = "send your CV";
			string password = _configuration["Email:Key"];
			string SenderEmail = _configuration["Email:DefaultFromEmailAddress"];
			string FromName = _configuration["Email:DefaultFromName"];
			//string userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;



			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(FromName, SenderEmail));
			message.To.Add(new MailboxAddress(Name, userEmail));
			message.Subject = subject;
			message.Body = new TextPart("plain")
			{
				Text = HtmlMessage
			};

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
				client.Authenticate(SenderEmail, password);
				client.Send(message);
				client.Disconnect(true);
			}
		}

		public async Task EmailToUserRole(string userEmail, string Name)
		{

			string subject = "welcom nutristion";
			string HtmlMessage = $"Welcome {Name}, you are now a Nutritionist.\r\nYou can now create your profile by clicking on this" +
				$" https://fit-and-fuel20231024140058.azurewebsites.net/nutritionist/CreateProfile ";
			string password = _configuration["Email:Key"];
			string SenderEmail = _configuration["Email:DefaultFromEmailAddress"];
			string FromName = _configuration["Email:DefaultFromName"];
			//string userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;



			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(FromName, SenderEmail));
			message.To.Add(new MailboxAddress(Name, userEmail));
			message.Subject = subject;
			message.Body = new TextPart("plain")
			{
				Text = HtmlMessage
			};

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
				client.Authenticate(SenderEmail, password);
				client.Send(message);
				client.Disconnect(true);
			}
		}
	}
}
