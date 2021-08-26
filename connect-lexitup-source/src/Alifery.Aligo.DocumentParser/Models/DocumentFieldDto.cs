using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class DocumentFieldDto
    {
        public int Id { get; set; }
        public int DocumentFormatId { get; set; }
        public int ExecutionSequence { get; set; }
        public string FieldName { get; set; }
        public bool PlaceHolderField { get; set; }
        public bool DerivedField { get; set; }
        public DataType DataType { get; set; }
        public bool IsActive { get; set; }
        public ICollection<DocumentFieldMappingDto> Mappings { get; set; }
    }
}