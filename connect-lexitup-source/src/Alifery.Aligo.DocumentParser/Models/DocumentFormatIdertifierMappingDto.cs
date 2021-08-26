namespace Alifery.Aligo.DocumentParser.Models
{
    public class DocumentFormatIdertifierMappingDto
    {
        public int Id { get; set; }
        public int DocumentFormatIdentifierId { get; set; }
        public MapDto Map { get; set; }
        public int MapId { get; set; }
        public int ExecutionOrder { get; set; }
        public bool IsActive { get; set; }
    }
}