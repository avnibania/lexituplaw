using System.ComponentModel.DataAnnotations;
using Alifery.Connect.CoreEntities.AligoEntities.DocumentParser;

namespace Alifery.Aligo.DocumentParser.Models
{
    public class PostProcessMapParametersDto
    {
        public PostProcessMapParametersDto(ParameterKeys key, string value)
        {
            Key = key.ToString();
            Value = value;
        }

        public int Id { get; set; }
        public int PostProcessMapId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DataType Type { get; set; }
    }
}