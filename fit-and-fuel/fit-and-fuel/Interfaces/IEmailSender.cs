namespace fit_and_fuel.Interfaces
{
	public interface IEmailSender
	{

		Task EmailSenderAsync(string email, string subject, string HtmlMessage);
		Task EmailToUser(string userEmail, string Name);
		Task EmailToUserRole(string userEmail, string Name);
		Task EmailToAdmin(string subject, string HtmlMessage);
	}
}
