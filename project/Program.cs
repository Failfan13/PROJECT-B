using System.Globalization;
// Set system US Culture
System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
System.Threading.Thread.CurrentThread.CurrentCulture = ci;
// Show unicode characters
Console.OutputEncoding = System.Text.Encoding.Unicode;

//Menu.Start();

//DbAccess.TryMe();

var sussy = await DbAccess.LoadAll<MovieModel>();

Console.WriteLine(sussy.First().Title);


// var sussy = DbAccess.LoadAll<MovieModel>();
// Console.WriteLine(sussy);
// Console.WriteLine(sussy.Result);
// Console.WriteLine(sussy.GetType().Name);
