Console.OutputEncoding = System.Text.Encoding.Unicode;

ReservationLogic RL = new ReservationLogic();
var ress = RL.GetById(1);

Logger.LogReservation(ress);
var list = Logger.GetLogReservation();

foreach (var item in list)
{
    Console.WriteLine(item["id"]);
}