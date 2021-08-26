using System.Collections.Generic;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class DocumentFormatDto
    {
        public int Id { get; set; }
        public DocumentCategory DocumentCategory { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<DocumentFormatIdentifierDto> FormatIdentifiers { get; set; }
        public ICollection<DocumentFieldDto> Fields { get; set; }
    }
}