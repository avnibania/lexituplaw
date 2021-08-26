using System.Collections.Generic;
using System.Threading.Tasks;
using Alifery.Aligo.Search.Models;

namespace Alifery.Aligo.Search.Services
{
    public interface IContentSearchService
    {
        Task<ICollection<SearchResultModel>> SearchCandidates(string searchString,
            List<CandidateSearchInput> input = null);
    }
}