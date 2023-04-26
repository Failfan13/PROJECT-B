Console.OutputEncoding = System.Text.Encoding.Unicode;


// AccountsLogic accountsLogic = new AccountsLogic();
// AccountsLogic.CurrentAccount = accountsLogic.GetById(1);

// Menu.Start();

TimeSlotsLogic timeSlotsLogic = new TimeSlotsLogic();

TimeSlotModel tmsm = timeSlotsLogic.GetById(0);

TheatreLogic theatreLogic = new TheatreLogic();

theatreLogic.ShowSeats(tmsm);