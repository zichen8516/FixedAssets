using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FixedAssets.Models
{
    public class AssetDepartment
    {
        [Key]
        public int ID { get; set; }
        //工厂
        public string plant { get; set; }
        //部门
        public string department { get; set; }
    }
}