using System.ComponentModel.DataAnnotations;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class MapParameterDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DataType Type { get; set; }
    }
}