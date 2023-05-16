using System.Net;

public static class OpenWebsiteLogic
{

    public static bool ValidUrl(string url)
    {
        try
        {


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;
            request.Method = "HEAD";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static void OpenWebBrowser(string url)
    {
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = url
        };
        System.Diagnostics.Process.Start(psi);
    }
}