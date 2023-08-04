using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class HtmlResponseDataViewModel
    {
        public string FormId { get; internal set; }
        public string UserId { get; internal set; }
        public string HtmlResponse { get; internal set; }

        public class HtmlResponseData
        {
            public Guid Id { get; set; }
            public string FormId { get; set; }
            public string UserId { get; set; }
            public string HtmlResponse { get; set; }

        }
    }
}