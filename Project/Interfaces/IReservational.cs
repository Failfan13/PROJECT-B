interface IReservational<T>
{
    void UpdateList(T model);

    T? GetById(int id);

    int GetNewestId();

    List<T> AllModel();
}