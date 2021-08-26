using Alifery.Aligo.CandidateScoring.Adapters;
using Autofac;

namespace Alifery.Aligo.CandidateScoring
{
    public class Provider : IProvider
    {
        private readonly ILifetimeScope _lifetimeScope;

        public Provider(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public ICriteriaEvaluatorAdapter GetAdapter(string adapterName)
        {
            return (ICriteriaEvaluatorAdapter)_lifetimeScope.ResolveNamed<ICriteriaEvaluatorAdapter>(adapterName);

        }
    }
}
