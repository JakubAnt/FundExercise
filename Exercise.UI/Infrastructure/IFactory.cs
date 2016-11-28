namespace Exercise.UI.Infrastructure
{
    public interface IFactory<out T>
    {
        T Create();
    }
}