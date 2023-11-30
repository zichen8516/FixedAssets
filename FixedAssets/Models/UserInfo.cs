using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FixedAssets.Models
{
    public class UserInfo
    {
        [Key]
        //用户名
        public string userName { get; set; }
        //密码
        public string passWord { get; set; }
        //部门
        public string department { get; set; }
        //工厂
        public string plant { get; set; }
        //权限
        public string role { get; set; }
        //备注
        public string remark { get; set; }
    }
}