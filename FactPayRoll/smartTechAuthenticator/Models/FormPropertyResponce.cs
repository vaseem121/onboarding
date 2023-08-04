using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartTechAuthenticator.Models
{
    public class FormPropertyResponce
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public Guid FormResponceId { get; set; }
        public Guid FormPropertyId { get; set; }
        public FieldType FieldType { get; set; }
        public string ResponceText { get; set; }
        public int NumberSliderValue { get; set; }
        public string DropdownValue { get; set; }
        public string MultipleChoiceValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }

    }
}