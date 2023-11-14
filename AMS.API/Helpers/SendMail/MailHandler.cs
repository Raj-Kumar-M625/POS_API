using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace ProjectOversight.API.Helpers.SendMail
{
    public class MailHandler
    {
        private string apiKey;

        public MailHandler(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<bool> SendMail(string toEmailAddress, string message, string subject)
        {
            try
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("manojbj@epicmindsit.com");
                var to = new EmailAddress(toEmailAddress);
                var htmlContent = "<strong> " + message + " </strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, message, htmlContent);
                var response = await client.SendEmailAsync(msg);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
