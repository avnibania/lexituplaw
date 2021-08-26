using System.Threading.Tasks;

namespace Alifery.Connect.Common.Query
{
    public interface IQueryHandler<in TQuery, TResult> : IQueryHandler
    {
        Task<TResult> Handle(TQuery query);
    }

    public interface IQueryHandler
    {
    }
}