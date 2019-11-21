using System;
using System.Collections.Generic;

namespace Tella_CMS.Models
{
    public partial class Product
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SlugName { get; set; }
        public bool AutoSlug { get; set; }
        public string FkCategoryId { get; set; }
        public string ShortDescriptionHtml { get; set; }
        public string LongDescriptionHtml { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
        public string Style { get; set; }
        public double Price { get; set; }
        public double? PriceNew { get; set; }
        public string Ccy { get; set; }
        public string Country { get; set; }
        public string Producer { get; set; }
        public string Status { get; set; }
        public string ImageSlug { get; set; }
        public int? Rating { get; set; }
        public int? CountView { get; set; }
        public bool IsService { get; set; }
        public string Tags { get; set; }
        public string KeyWord { get; set; }
        public string MetaData { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
    }
}
