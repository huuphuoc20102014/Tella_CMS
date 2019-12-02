using System;
using System.Collections.Generic;

namespace Tella_CMS.Efs.Entities
{
    public partial class Customer
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Age { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        public int? Fk_Customer_Id { get; set; }
    }
}
