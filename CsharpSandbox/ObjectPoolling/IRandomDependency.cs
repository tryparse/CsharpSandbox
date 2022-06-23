namespace CsharpSandbox.ObjectPooling
{
    public interface IRandomDependency
    {
        int Next(int minValue, int maxValue);
    }
}
