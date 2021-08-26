using System.Collections.Generic;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class DocumentFormatIdentifierDto
    {
        public int Id { get; set; }
        public int DocumentFormatId { get; set; }
        public FormatIdentifierType IdentifierType { get; set; }
        public int ExecutionSequence { get; set; }
        public int GroupKey { get; set; }
        public FormatIdentifierGroupMethod GroupMethod { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MapDto> Mappings { get; set; }
    }
}