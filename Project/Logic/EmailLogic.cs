/* Gmail info
    E-mail: HSR.ProjectB.TeamE@gmail.com
    Pass: Tim&Max~Makayru~Emir&Erik
    smpt address: smpt.gmail.com
    smpt pass: igtmcrhmlfymuagb
    smpt port: 465
*/
using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
public class EmailLogic
{
    public void SendAllEmail(string subject, string body) => this.SendEmail("", subject, body);
    public void SendEmail(string to, string subject, string body)
    {
        MimeMessage message = new MimeMessage();
        List<MailboxAddress> emailAddresses = new List<MailboxAddress>();

        if (!to.Contains("@"))
        {
            foreach (AccountModel email in GetAllAccounts())
            {
                emailAddresses.Add(MailboxAddress.Parse(email.EmailAddress));
            }
        }
        else
        {
            emailAddresses.Add(MailboxAddress.Parse(to));
        }

        message.From.Add(new MailboxAddress("tester", "Hsr.ProjectB.TeamE@gmail.com"));

        foreach (MailboxAddress email in emailAddresses)
        {
            message.To.Clear();

            message.To.Add(email);

            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                try //uncomment to send email
                {
                    // client.Connect("smtp.gmail.com", 465, true);
                    // client.Authenticate("Hsr.ProjectB.TeamE@gmail.com", "igtmcrhmlfymuagb");
                    // client.Send(message);

                    Console.WriteLine($"Email sent: {email.Address}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // client.Disconnect(true);
                    // client.Dispose();
                }
            }
        }
    }

    public List<AccountModel> GetAllAccounts()
    {
        AccountsLogic AccountsLogic = new AccountsLogic();
        return AccountsLogic.GetAllAccounts().FindAll(a => a.AdMails == true);
    }

    public bool ValidateEmail(string email)
    {
        if (email.Contains("@") && email.Contains(".")) return true;
        return false;
    }
}