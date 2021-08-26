using System.Threading.Tasks;

namespace Alifery.Connect.Common.Query
{
    public interface IQueryRunner
    {
        Task<T> RunAsync<T, TQuery>(TQuery query) where TQuery : IQuery<T>;
        Task<T> RunAsync<T>(IQuery<T> query);
    }
}