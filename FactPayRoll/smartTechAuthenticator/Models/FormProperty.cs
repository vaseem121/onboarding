using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartTechAuthenticator.Models
{
    public class FormProperty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid FormId { get; set; }
        public string Name { get; set; }
        public FieldType FieldType { get; set; }
        public string RowNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public bool Captcha { get; set; }
        public bool isPhoto { get; set; }
     

    }
    
    public enum FieldType
    {
        SingleLineText,
        ParagraphText,
        Dropdown,
        MultipleChoice,
        Checkboxes,
        Numbers,
        Name,
        Email,
        NumberSlider,
        Captcha,
        Photo,
        TextField,
        DatePicker,
        EIN_No,
        HeadingTextField,
        CheckboxBlack,
        ListTextField,
        MultiLineText,
        LeftRightText
    }
}