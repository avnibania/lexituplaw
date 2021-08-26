using System.Collections.Generic;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class MapDto
    {
        public int Id { get; set; }
        public SearchScope SearchScope { get; set; }
        public LineIdentifierMethod LineIdentifierMethod { get; set; }
        public FieldIdentifierMethod FieldIdentifierMethod { get; set; }
        public ICollection<PostProcessMapDto> PostProcessMaps { get; set; }
        public ICollection<MapParametersDto> Parameters { get; set; }
    }
}