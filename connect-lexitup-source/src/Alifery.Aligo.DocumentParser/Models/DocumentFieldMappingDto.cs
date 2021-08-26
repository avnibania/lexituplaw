namespace Alifery.Aligo.DocumentParser.Models
{
    public class DocumentFieldMappingDto
    {
        public int Id { get; set; }
        public int DocumentFieldId { get; set; }
        public MapDto Map { get; set; }
        public int MapId { get; set; }
        public int ExecutionOrder { get; set; }
        public bool IsActive { get; set; }
    }
}