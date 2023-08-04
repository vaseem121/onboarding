using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class FormViewModel
    {
        public Guid ID { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}