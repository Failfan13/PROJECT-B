public abstract class Order<T>
{
    public abstract void UpdateList(T model);
    public abstract T? GetById(int id);
    public abstract int GetNewestId();
}