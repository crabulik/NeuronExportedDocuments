using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeuronExportedDocuments.Models
{
    public class NLogError
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime RecordTime { get; set; }
        [Required]
        [MaxLength]
        public string Host { get; set; }
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
        [Required]
        [MaxLength]
        public string Source { get; set; }
        [Required]
        [MaxLength]
        public string Message { get; set; }
        [Required]
        [MaxLength(50)]
        public string Level { get; set; }
        [Required]
        [MaxLength]
        public string Logger { get; set; }
        [Required]
        [MaxLength]
        public string Stacktrace { get; set; }
        [Required]
        [Column(TypeName="ntext")] 
        public String AllXml {get;set;}

        [Timestamp] 
        public Byte[] TimeStamp { get; set; }

        public NLogError()
        {
            RecordTime = DateTime.Now;
        }
    }
}