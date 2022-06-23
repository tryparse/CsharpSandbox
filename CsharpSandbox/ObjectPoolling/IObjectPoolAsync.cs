using System.Threading.Tasks;

namespace CsharpSandbox.ObjectPooling
{
    public interface IObjectPoolAsync<T> where T : class
    {
        Task<T> GetAsync();
        void Release(T obj);
    }
}
