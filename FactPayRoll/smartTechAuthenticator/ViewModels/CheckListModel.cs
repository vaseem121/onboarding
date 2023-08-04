using smartTechAuthenticator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class CheckListModel
    {
        public ProductMaster Data { get; set; }
        public IEnumerable<TestkitCheckList>   TestkitChecks { get; set; }
        
    }
}