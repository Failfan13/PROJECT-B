using System.Globalization;
using System.Collections.Generic;
// Set system US Culture
System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
System.Threading.Thread.CurrentThread.CurrentCulture = ci;
// Show unicode characters
Console.OutputEncoding = System.Text.Encoding.Unicode;

Menu.Start();
