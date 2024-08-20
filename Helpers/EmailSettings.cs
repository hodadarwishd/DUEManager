using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client=new SmtpClient("smtp.gmail.com",587);
			Client.EnableSsl = false;
			Client.Credentials = new NetworkCredential("ahmed@Linkdev.com", "Ahmed@123");
			Client.Send("ahmed@Linkdev.com", email.Recipients, email.Body, email.Subject);
		}
	}
}
