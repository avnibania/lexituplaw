using Alifery.Aligo.CandidateScoring.Adapters;

namespace Alifery.Aligo.CandidateScoring
{
    public interface IProvider
    {
        ICriteriaEvaluatorAdapter GetAdapter(string adapterName);
    }
}
