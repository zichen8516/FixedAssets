using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FixedAssets.Models
{
    public class AssetOS
    {
        [Key]
        public string osname { get; set; }
    }
}