static class Logger
{
    public static List<string> Types = new List<string>() { "Data Change", "User", "Reservation", "" };

    public static void Log(string LogString, string type)
    {

        try
        {

            if (Types.Contains(type))
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                LogAccess.WriteLine($"{timestamp},{type},{LogString}");
            }
            else
            {
                throw new Exception(searchStr + " is not present in the list.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}