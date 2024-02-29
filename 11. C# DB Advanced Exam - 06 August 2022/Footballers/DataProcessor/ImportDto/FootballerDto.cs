using Footballers.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class FootballerDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string ContractStartDate { get; set; }

        [Required]
        public string ContractEndDate { get; set; }

        [Required]
        [Range(0, 4)]
        public int BestSkillType { get; set; }

        [Required]
        [Range(0, 3)]
        public int PositionType { get; set; }
    }
}
