Console.OutputEncoding = System.Text.Encoding.Unicode;


// AccountsLogic accountsLogic = new AccountsLogic();
// AccountsLogic.CurrentAccount = accountsLogic.GetById(1);

// Menu.Start();

TimeSlotsLogic TSL = new TimeSlotsLogic();

TimeSlotModel tmsm = TSL.GetById(0);

TheatreLogic theatreLogic = new TheatreLogic();

//theatreLogic.ShowSeats(tmsm, true);
// Theatre.MakeNewTheatre();
theatreLogic.ShowSeats(theatreLogic.GetById(2)!, tmsm!);