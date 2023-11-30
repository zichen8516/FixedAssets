using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FixedAssets.Models
{
    public class AssetChangeRecord
    {
        [Key]
        //ID
        public int ID { get; set; }
        //资产编号
        public string AssetNumber { get; set; }
        //工厂
        public string plant { get; set; }
        //电脑变更记录01
        public string ChangeRecord01 { get; set; }
        //电脑变更记录02
        public string ChangeRecord02 { get; set; }
        //电脑变更记录03
        public string ChangeRecord03 { get; set; }
        //备注01
        public string Remark01 { get; set; }
        //备注01
        public string Remark02 { get; set; }
    }
}