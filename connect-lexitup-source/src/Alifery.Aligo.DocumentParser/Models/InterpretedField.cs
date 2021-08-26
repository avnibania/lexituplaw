namespace Alifery.Aligo.DocumentParser.Models
{
    public class InterpretedField
    {
        public InterpretedField()
        {
        }

        public InterpretedField(string fieldName, string value)
        {
            FieldName = fieldName;
            Value = value;
        }

        public string FieldName { get; set; }
        public string Value { get; set; }
    }
}