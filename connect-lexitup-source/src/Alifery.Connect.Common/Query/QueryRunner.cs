using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Alifery.Connect.Common.Query
{
    public class QueryRunner : IQueryRunner
    {
        private readonly IQueryHandler[] _handlers;
        private static readonly Type HandlerInterfaceType = typeof (IQueryHandler<,>);
        private const BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public;

        public QueryRunner(IQueryHandler[] handlers)
        {
            _handlers = handlers;
        }

        public Task<T> RunAsync<T, TQuery>(TQuery query) where TQuery : IQuery<T>
        {
            var queryHandler = GetQueryHandler<TQuery, T>();
            return queryHandler.Handle(query);
        }

        public async Task<T> RunAsync<T>(IQuery<T> query)
        {
            var queryType = query.GetType();
            var handlerRequired = HandlerInterfaceType.MakeGenericType(queryType, typeof (T));
            var handler = GetQueryHandler<T>(handlerRequired);
            var result = await
                (Task<T>) handlerRequired.InvokeMember("Handle", bindingFlags, null, handler, new object[] {query});
            return result;
        }

        private IQueryHandler GetQueryHandler<T>(Type handlerRequired)
        {
            var handler = _handlers.SingleOrDefault(x => x.GetType().GetInterfaces().Contains(handlerRequired));
            if (handler == null)
            {
                throw new QueryHandlerNotImplementedException(typeof (T));
            }
            return handler;
        }

        private IQueryHandler<TQuery, T> GetQueryHandler<TQuery, T>()
        {
            var handler = _handlers.OfType<IQueryHandler<TQuery, T>>().SingleOrDefault();
            if (handler == null)
            {
                throw new QueryHandlerNotImplementedException(typeof(T));
            }

            return handler;
        }
    }
}