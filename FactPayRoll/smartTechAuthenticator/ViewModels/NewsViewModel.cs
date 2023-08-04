using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class NewsViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string NewsTitle { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
        public string CreatedBy { get; set; }
        public string Photo { get; set; }

        public List<NewsViewModel> NewsList { get; set; }
    }
}