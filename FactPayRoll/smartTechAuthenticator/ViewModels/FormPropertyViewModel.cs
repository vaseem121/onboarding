using Newtonsoft.Json;
using smartTechAuthenticator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class FormPropertyViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid FormId { get; set; }
        public string Name { get; set; }
        public FieldType InputType { get; set; }
        public int RowNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public bool Captcha { get; set; }
        public bool NumberSlider { get; set; }
        public int NumberSliderValue { get; set; }
        public int Type { get; set; }
        public string fId { get; set; }
        public string FormName { get; set; }
        public string NameValue { get; set; }
        public bool isFile { get; set; }
        public string File { get; set; }
        public CheckBoxViewModel CheckBoxView { get; set; }
        public List<CheckBoxViewModel> CheckBoxViewList { get; set; }
        public MultipleChoiceViewModel MultipleChoiceView { get; set; }
        public List<MultipleChoiceViewModel> MultipleChoiceViewList { get; set; }
        public DropdownViewModel DropdownView { get; set; }
        public FormView formview { get; set; }
        public List<DropdownViewModel> DropdownViewList { get; set; }
        public List<FormPropertyViewModel> FormPropertyViewList { get; set; }
        public FormPropertyViewModel FormPropertyView { get; set; }
        public Guid TestId { get; set; }
        public string ResponceText { get; set; }
        public string MultipleChoiceValue { get; set; }
        public FormRes formres { get; set; }
        public string userid { get; set; }
    }

    public class FormRes
    {
        public Guid FormId { get; set; }
        public Guid FormResponseId { get; set; }
        public string FormName { get; set; }
        public List<FormPropertyViewModel> FormPropertyViewList { get; set; }
    }



    public class CheckBoxViewModel
    {
        public Guid Id { get; set; }
        public Guid FormPropertyId { get; set; }
        [Display(Name = "Checkbox Name")]
        public string[] CheckboxName { get; set; }
        public bool CheckboxValue { get; set; }
        public string CheckboxText { get; set; }
        public int RowNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class MultipleChoiceViewModel
    {
        public Guid Id { get; set; }
        public Guid FormPropertyId { get; set; }
        [Display(Name = "Multiple Choice Name")]
        public string[] ChoiceName { get; set; }
        public bool ChoiceNameValue { get; set; }
        public string ChoiceText { get; set; }
        public int RowNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class FormView
    {
        public Guid FormId { get; set; }
        public string FormName { get; set; }
    }


    public class DropdownViewModel
    {
        public Guid Id { get; set; }
        public Guid FormPropertyId { get; set; }
        [Display(Name = "Dropdown Option Name")]
        public string[] OptionName { get; set; }
        public string OptionText { get; set; }
        public string OptionValue { get; set; }
        public int RowNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success
        {
            get;
            set;
        }
        [JsonProperty("error-codes")]
        public List<string> ErrorMessage
        {
            get;
            set;
        }
    }


    public class FormAPIViewModel
    {
        public Guid Id { get; set; }
        //public Guid CustomerId { get; set; }
       // public Guid FormId { get; set; }
        public string Name { get; set; }
        public FieldType InputType { get; set; }
        public int RowNumber { get; set; }
        //public DateTime CreatedDate { get; set; }
       // public DateTime UpdatedDate { get; set; }
        //public Guid CreatedBy { get; set; }
       // public bool Captcha { get; set; }
        //public bool NumberSlider { get; set; }
        //public int NumberSliderValue { get; set; }
       // public int Type { get; set; }
       // public string fId { get; set; }
       // public string FormName { get; set; }
       // public string NameValue { get; set; }
       // public bool isFile { get; set; }
       // public string File { get; set; }
       // public CheckBoxViewModel CheckBoxView { get; set; }
        public List<CheckBoxViewModel> CheckBoxViewList { get; set; }
        //public MultipleChoiceViewModel MultipleChoiceView { get; set; }
        public List<MultipleChoiceViewModel> MultipleChoiceViewList { get; set; }
       // public DropdownViewModel DropdownView { get; set; }
       public FormView FormHeader { get; set; }
        public List<DropdownViewModel> DropdownViewList { get; set; }
        public List<FormAPIViewModel> FormPropertyViewList { get; set; }
        //public FormAPIViewModel FormPropertyView { get; set; }
       // public Guid TestId { get; set; }
        public string ResponseText { get; set; }
       // public string MultipleChoiceValue { get; set; }
       // public FormRes formres { get; set; }
    }



}
