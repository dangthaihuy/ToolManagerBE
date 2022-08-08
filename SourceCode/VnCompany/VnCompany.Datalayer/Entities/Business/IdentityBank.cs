﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnCompany.DataLayer.Entities.Business
{
    public class IdentityBank : CommonIdentity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }
}
