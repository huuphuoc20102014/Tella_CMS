using System;
using System.Collections.Generic;

namespace Tella_CMS.Efs.Entities
{
    public partial class Oder_view
    {
        public string Code { get; set; }
        public string TenKH { get; set; }
        public string TenSP { get; set; }
        public DateTime NgayMua { get; set; }
        public double Price { get; set; }
        public int? SoLuong { get; set; }
        public string Age { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
    }
}
