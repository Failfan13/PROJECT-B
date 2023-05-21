using System.Globalization;
// Set system US Culture
System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
System.Threading.Thread.CurrentThread.CurrentCulture = ci;
// Show unicode characters
Console.OutputEncoding = System.Text.Encoding.Unicode;

Menu.Start();

// await DbAccess.ExecuteNonQuery().ConfigureAwait(false);

// Console.WriteLine(DbLogic.GetNewestId());