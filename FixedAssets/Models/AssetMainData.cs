using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FixedAssets.Models
{
    public class AssetMainData
    {
        [Key]
       //ID
        public int ID { get; set; }
        //工厂
        public string plant { get; set; }
        //部门
        public string Department { get; set; }
        //使用人
        public string AssetUser { get; set; }
        //资产编号
        public string AssetNumber { get; set; }
        //电脑名称
        public string ComputerName { get; set; }
        //电脑类型
        public string ComputerType { get; set; }
        //电脑型号
        public string ComputerModel { get; set; }
        //序列号
        public string SerialNumber { get; set; }
        //IP 地址
        public string IPAddress { get; set; }
        //MAC 地址
        public string MACAddress { get; set; }
        //购买时间
        public DateTime? BuyTime { get; set; }
        //使用时间
        public int? UseTime { get; set; }
        //使用地点
        public string Place { get; set; }
        //操作系统
        public string OS { get; set; }
        //USBPort 开放状态
        public string USBPort { get; set; }
        //是否联网
        public string Internet { get; set; }
        //电脑状态
        public string ComputerStatus { get; set; }
        //电脑变更记录01
        public string ChangeRecord01 { get; set; }
        //电脑变更记录02
        public string ChangeRecord02 { get; set; }
        //电脑变更记录03
        public string ChangeRecord03 { get; set; }
        //备注01
        public string Remark01 { get; set; }
        //备注02
        public string Remark02 { get; set; }
        //创建人
        public string createPeople { get; set; }
        //创建时间
        public DateTime? createTime { get; set; }
        //修改人
        public string modifyPeople { get; set; }
        //修改时间
        public DateTime? modifyTime { get; set; }
        //标记颜色
        public string MarkerColor { get; set; }
    }
}