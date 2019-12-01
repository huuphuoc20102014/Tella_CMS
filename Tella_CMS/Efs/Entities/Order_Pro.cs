using System;
using System.Collections.Generic;

namespace Tella_CMS.Efs.Entities
{
    public partial class Order_Pro
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Fk_Customer_Id { get; set; }
        public string Fk_Product_Id { get; set; }
        public int? Quanlity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
    }
}
