using System.Collections.Generic;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class PostProcessMapDto
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int ExecutionSequence { get; set; }
        public PostProcessAction PostProcessAction { get; set; }
        public ICollection<PostProcessMapParametersDto> Parameters { get; set; }
    }
}