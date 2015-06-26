namespace NeuronExportedDocuments.Models
{
    public class FormaterKey
    {
        public string Key { get; set; }

        public string Description { get; set; }

        public FormaterKey(string key, string description)
        {
            Key = key;
            Description = description;
        }
    }
}