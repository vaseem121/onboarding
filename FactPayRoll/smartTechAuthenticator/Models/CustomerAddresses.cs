using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartTechAuthenticator.Models
{
    [Table("CustomerAddresses")]
    public class CustomerAddresses
    {
        [Key]
        public int Id { get; set; }
        public int StateId { get; set; }
        public int DistricttId { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }

    }
}