using System.Globalization;
// Set system US Culture
System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
System.Threading.Thread.CurrentThread.CurrentCulture = ci;
// Show unicode characters
Console.OutputEncoding = System.Text.Encoding.Unicode;

Menu.Start();

// await DbAccess.ExecuteNonQuery().ConfigureAwait(false);

// Console.WriteLine(DbLogic.GetNewestId());

// AccountsLogic accountsLogic = new AccountsLogic();

// accountsLogic.NewAccount("DitIsEmail@example.com", "eerste twde", "DitIsme", "01/01/2001");

// var model = new AccountModel()
// {
//     EmailAddress = "tImail@eaple.com",
//     FirstName = "eete esde",
//     LastName = "DItName",
//     Password = "Ditme",
// };

// var client = DbAccess._supabase;

// await DbAccess.InsertData<AccountModel>(model);