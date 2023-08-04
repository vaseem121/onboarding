﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace smartTechAuthenticator.Models
{
    public partial class TestKits
    {
        public Guid Id { get; set; }
        public string Product { get; set; }
        public string Qrcode { get; set; }
        public int Status { get; set; }
        public Guid? CustId { get; set; }
        public DateTime CreatedTs { get; set; }
    }
}
