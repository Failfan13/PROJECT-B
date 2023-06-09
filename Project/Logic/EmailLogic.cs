using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Text.RegularExpressions;

public class EmailLogic
{
    public EmailLogic()
    {
        CheckReleasedMovie();
    }

    public void SendAllEmail(string subject, string body) => this.SendEmail("", subject, body);
    public async void SendEmail(string to, string subject, string body)
    {
        MimeMessage message = new MimeMessage();
        List<MailboxAddress> emailAddresses = new List<MailboxAddress>();

        if (!to.Contains("@"))
        {
            foreach (AccountModel email in await GetAllAccounts())
            {
                emailAddresses.Add(MailboxAddress.Parse(email.EmailAddress));
            }
        }
        else
        {
            emailAddresses.Add(MailboxAddress.Parse(to));
        }

        message.From.Add(new MailboxAddress("Project-B Team-E", "Hsr.ProjectB.TeamE@gmail.com"));

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
                    // client.Authenticate("Hsr.ProjectB.TeamE@gmail.com", "fbfmimakrvojgpvs");
                    // client.Send(message);

                    // Console.WriteLine($"Email sent: {email.Address}");
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

    public async Task<List<AccountModel>> GetAllAccounts()
    {
        AccountsLogic AccountsLogic = new AccountsLogic();
        return (await AccountsLogic.GetAllAccounts()).FindAll(a => a.AdMails == true);
    }

    public bool ValidateEmail(string email)
    {
        var regexItem = new Regex("^[a-zA-Z0-9@. ]*$");

        if (regexItem.IsMatch(email))
        {
            if (email.Contains("@.") || email.Contains(".@")) return false;
            else if (email.Contains("@") && email.Contains(".")) return true;
        }

        return false;
    }

    public void CheckReleasedMovie()
    {
        MoviesLogic ML = new MoviesLogic();

        // Every movie more then 0 follower
        // foreach (MovieModel movie in ML.AllMovies(true).Where(m => m.Followers.Count > 0))
        // {
        //     // send notice when date reached and date within 7 days
        //     if (movie.ReleaseDate.Date >= DateTime.Now.AddDays(-7) &&
        //     movie.ReleaseDate.Date < DateTime.Now)
        //     {
        //         NotifyFollowers(movie);
        //     }
        // }
    }

    private async void NotifyFollowers(MovieModel movie)
    {
        MoviesLogic ML = new MoviesLogic();
        AccountsLogic AccountsLogic = new AccountsLogic();
        AccountModel account = null!;
        Tuple<string, string> message = null!;

        foreach (int AccountId in movie.Followers)
        {
            try
            {
                account = await AccountsLogic.GetById(AccountId)!;
                message = NewMovieMessage(movie, account);

                SendEmail(account.EmailAddress, message.Item1, message.Item2);
            }
            catch (System.Exception)
            {
                continue;
            }
            ML.RemoveFollower(movie, AccountId);
        }
    }

    public Tuple<string, string> NewMovieMessage(MovieModel movie, AccountModel account)
    {
        string subject = $"The movie {movie.Title} has been released";
        string body = @$"Hey {account.FirstName},

We saw you were interested in the movie {movie.Title}, and we thought you might be pleased
to know that the movie has been released on {movie.ReleaseDate}.

You are now able to order the tickets for this movie.

Regards,
Project-B Team-E";

        return new Tuple<string, string>(subject, body);
    }

    public void SubscribeAds(AccountModel accountId)
    {
        AccountsLogic AccountsLogic = new AccountsLogic();

        accountId.AdMails = true;

        AccountsLogic.UpdateList(accountId);
    }
}
