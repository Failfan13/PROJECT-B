/*
List<TimeSlotModel> slots = new List<TimeSlotModel>();

for (int i = 0; i < 10; i++)
{
    slots.Add(new TimeSlotModel(i, i % 4, (DateTime.Now.AddDays(i)), new TheatherLogic().GetById(0)));
}


foreach (var item in slots)
{
    new TimeSlotsLogic().UpdateList(item);
}
*/


Menu.Start();