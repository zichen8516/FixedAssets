using FixedAssets.Filter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FixedAssets.DataAccess;
using FixedAssets.Models;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using System.Web.UI.WebControls;

namespace FixedAssets.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            FixedAssetsData fixedassetsdata = new FixedAssetsData();
            var user = fixedassetsdata.UserInfos.Where(u => u.userName == User.Identity.Name).FirstOrDefault();
            if (user?.role == "Admin")
            {
                ViewBag.IsAdmin = UserLever.UserAdmin;
                ViewBag.plant = user.plant;
            }
            else if (user != null && user.role == "general")
            {
                ViewBag.IsAdmin = UserLever.UserGeneral;
                ViewBag.plant = user.plant;
            }
            else if (user != null && user.role == "normal")
            {
                ViewBag.IsAdmin = UserLever.UserNormal;
                ViewBag.plant = user.plant;
            }
            else
            {
                ViewBag.IsAdmin = UserLever.UserAdministrator;
                ViewBag.plant = user.plant;
            }
            return View();
        }
        public ActionResult FixedAssetsMainData()
        {
            FixedAssetsData fixedassetsdata = new FixedAssetsData();
            var user = fixedassetsdata.UserInfos.Where(u => u.userName == User.Identity.Name).FirstOrDefault();
            if (user?.role == "Admin")
            {
                ViewBag.IsAdmin = UserLever.UserAdmin;
                ViewBag.plant = user.plant;
            }
            else if (user != null && user.role == "general")
            {
                ViewBag.IsAdmin = UserLever.UserGeneral;
                ViewBag.plant = user.plant;
            }
            else if (user != null && user.role == "normal")
            {
                ViewBag.IsAdmin = UserLever.UserNormal;
                ViewBag.plant = user.plant;
            }
            else
            {
                ViewBag.IsAdmin = UserLever.UserAdministrator;
                ViewBag.plant = user.plant;
            }
/*            var assetnumber1 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "正常使用" && n.plant == user.plant ).Count();
            ViewBag.ComputerStatus1 = assetnumber1;
            var assetnumber2 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "等待报废" && n.plant == user.plant).Count();
            ViewBag.ComputerStatus2 = assetnumber2;
            var assetnumber3 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "已经报废" && n.plant == user.plant).Count();
            ViewBag.ComputerStatus3 = assetnumber3;*/
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        [AjaxFilter]
        //固定资产主数据
        public ActionResult getFixedData(string Departmentss,string AssetNumberss, string ComputerTypes,string plantss,string AssetUserss, string ComputerNamess, string ComputerStatusss,string IPAddresss, string computerUseTimes,string field, string order, int page, int limit)
        {
            string sql = "select A.ID,A.plant,A.Department,A.AssetUser,A.AssetNumber,A.ComputerName,A.ComputerType,A.ComputerModel,A.SerialNumber,A.IPAddress,A.MACAddress,A.BuyTime,(datediff(Year,A.BuyTime,GETDATE())) UseTime,A.Place,A.OS,A.USBPort,A.Internet,A.ComputerStatus,B.ChangeRecord01,B.ChangeRecord02,B.ChangeRecord03,B.Remark01,B.Remark02,A.createPeople,A.createTime,A.modifyPeople,A.modifyTime,A.MarkerColor FROM[FixedAssets].[dbo].[AssetMaintain] A left outer join[FixedAssets].[dbo].[AssetChangeRecord] B on A.ID = B.ID ";
            string sql1 = order == null ? sql + "order by A.modifyTime desc" : (order == "" ? sql + "order by A.modifyTime desc" : sql + "order by " + field + " " + order + " ,A.modifyTime desc");
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            if ((Departmentss == null && AssetNumberss == null && ComputerTypes == null && plantss == null && AssetUserss == null && ComputerNamess == null && ComputerStatusss == null && IPAddresss ==null && computerUseTimes == null) ||(Departmentss == "" && AssetNumberss == "" && ComputerTypes == "" && plantss == "" && AssetUserss == "" && ComputerNamess == "" && ComputerStatusss == "" && IPAddresss == "" && computerUseTimes == ""))
            {
                var info = fixedAssetsData.Database.SqlQuery<AssetMainData>(sql1).AsQueryable();
                var infoResult = info.Skip(limit * (page - 1)).Take(limit).ToList();
                var result = new
                {
                    code = 0,
                    msg = "",
                    count = info.Count(),
                    data = infoResult
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string Department;
                string AssetNumber;
                string ComputerType;
                string plant;
                string AssetUser;
                string ComputerName;
                string ComputerStatus;
                string IPAddress;
                string UseTime;
                string sql2;
                if (ComputerTypes == null || ComputerTypes == "")
                {
                    ComputerType = " like '%' or A.ComputerType is null ";
                }
                else
                {
                    ComputerType = " like '%" + ComputerTypes + "%'";
                }
                if (computerUseTimes == null || computerUseTimes == "")
                {
                    UseTime = " like '%' or (datediff(Year,A.BuyTime,GETDATE())) is null ";
                }
                else
                {
                    if (computerUseTimes == "5")
                    {
                        UseTime = " >= " + computerUseTimes + "";
                    }
                    else
                    {
                        UseTime = " >= " + computerUseTimes + "";
                    }
                }
                if (IPAddresss == null || IPAddresss == "")
                {
                    IPAddress = " like '%' or A.IPAddress is null ";
                }
                else
                {
                    IPAddress = " like '%" + IPAddresss + "%'";
                }
                if (Departmentss == null || Departmentss == "")
                {
                    Department = " like '%' or A.Department is null ";
                }
                else
                {
                    Department = " like '%" + Departmentss + "%'";
                }
                if (AssetNumberss == null || AssetNumberss == "")
                {
                    AssetNumber = " like '%' or A.AssetNumber is null ";
                }
                else
                {
                    if (AssetNumberss.Contains('\n'))
                    {
                        string[] list = AssetNumberss.Split('\n');
                        int num = list.Count();
                        AssetNumber = " in (";
                        for (int i = 0; i < num; i++)
                        {
                            if (list[i].ToString().Trim() != "")
                            {
                                AssetNumber += "'" + list[i].ToString().Trim() + "',";

                            }
                            if (i == num - 1)
                            {
                                AssetNumber += "'') ";
                            }
                        }
                    }
                    else
                    {
                        AssetNumber = " like '%" + AssetNumberss + "%'";
                    }
                }
                    if (plantss == null || plantss == "")
                    {
                        plant = " like '%' or A.plant is null ";
                    }
                    else
                    {
                        plant = " like '%" + plantss + "%'";
                    }

                    if (AssetUserss == null || AssetUserss == "")
                    {
                        AssetUser = " like '%' or A.AssetUser is null ";
                    }
                    else
                    {
                    if (AssetUserss.Contains(" "))
                    {
                        string s1_1 = AssetUserss.Replace(" ", "','");
                        string s3 = string.Format("'{0}'", s1_1);
                        AssetUser = "in (" + s3 + ")";
                    }
                    else
                    {
                        AssetUser = " like '%" + AssetUserss + "%'";
                    }
                    }

                if (ComputerNamess == null || ComputerNamess == "")
                    {
                        ComputerName = " like '%' or A.ComputerName is null ";
                    }
                    else
                    {
                        ComputerName = " like '%" + ComputerNamess + "%'";
                    }

                    if (ComputerStatusss == null || ComputerStatusss == "")
                    {
                        ComputerStatus = "like '%'";
                    }
                    else
                    {
                    string s1_1 = ComputerStatusss.Replace(",","','");
                    string s2 = string.Format("'{0}'", s1_1);
                    ComputerStatus = "in ("+s2+")";
                    }
                //sql2 = order == null ? sql + " where A.Department  " + Department + "and A.AssetNumber " + AssetNumber + " and A.plant " + plant + " and A.AssetUser " + AssetUser + "and A.ComputerName " + ComputerName + " and A.ComputerStatus "+ComputerStatus+" order by A.Department desc" : (order == "" ? sql + "where A.Department " + Department + "and A.AssetNumber " + AssetNumber + " and A.plant " + plant + " and A.AssetUser " + AssetUser + "and A.ComputerName " + ComputerName + " and A.ComputerStatus "+ComputerStatus+" order by A.Department desc" : sql + "where A.Department" + Department + "and A.AssetNumber " + AssetNumber + " and A.plant " + plant + " and A.AssetUser " + AssetUser + "and A.ComputerName " + ComputerName + " and A.ComputerStatus " + ComputerStatus+ " order by " + field + " " + order + " ");
                sql2 = order == null ? sql + " where (A.ComputerType  " + ComputerType + ')' + "and(A.Department  " + Department + ')'+ "and ((datediff(Year,A.BuyTime,GETDATE()))  " + UseTime + ')' + " and(A.IPAddress " + IPAddress + ')' + "and (A.AssetNumber " + AssetNumber +')'+ " and (A.plant " + plant + ')'+" and (A.AssetUser " + AssetUser + ')'+" and (A.ComputerName " + ComputerName +')'+ " and (A.ComputerStatus "+ComputerStatus+')'+" order by A.Department desc" : (order == "" ? sql + " where (A.ComputerType  " + ComputerType + ')' + "and(A.Department  " + Department + ')' + " and ((datediff(Year,A.BuyTime,GETDATE()))  " + UseTime + ')' + " and (A.IPAddress " + IPAddress + ')' + "and (A.AssetNumber " + AssetNumber + ')' + " and (A.plant " + plant + ')' + " and (A.AssetUser " + AssetUser + ')' + " and (A.ComputerName " + ComputerName + ')' + " and (A.ComputerStatus " + ComputerStatus + ')' + " order by A.Department desc" : sql + " where (A.ComputerType  " + ComputerType + ')' + "and(A.Department  " + Department + ')' + " and ((datediff(Year,A.BuyTime,GETDATE()))  " + UseTime + ')' + " and (A.IPAddress " + IPAddress + ')' + "and (A.AssetNumber " + AssetNumber + ')' + " and (A.plant " + plant + ')' + " and (A.AssetUser " + AssetUser + ')' + " and (A.ComputerName " + ComputerName + ')' + " and (A.ComputerStatus " + ComputerStatus + ')' + " order by " + field + " " + order + " ");
                var assets2 = fixedAssetsData.Database.SqlQuery<AssetMainData>(sql2).AsQueryable();
                    var infoResult2 = assets2.Skip(limit * (page - 1)).Take(limit).ToList();
                    var result2 = new
                    {
                        code = 0,
                        msg = "",
                        count = assets2.Count(),
                        data = infoResult2
                    };
                    return Json(result2, JsonRequestBehavior.AllowGet);
            }
        }
        [AjaxFilter]
        //固定资产主数据表格
        public ActionResult getFixedDataExt(string Departmentss, string AssetNumberss, string ComputerTypes, string plantss, string AssetUserss, string ComputerNamess, string ComputerStatusss,string IPAddresss, string computerUseTimes, string field, string order)
        {
            string sql = "select A.ID,A.plant,A.Department,A.AssetUser,A.AssetNumber,A.ComputerName,A.ComputerType,A.ComputerModel,A.SerialNumber,A.IPAddress,A.MACAddress,A.BuyTime,(datediff(Year,A.BuyTime,GETDATE())) UseTime,A.Place,A.OS,A.USBPort,A.Internet,A.ComputerStatus,B.ChangeRecord01,B.ChangeRecord02,B.ChangeRecord03,B.Remark01,B.Remark02,A.createPeople,A.createTime,A.modifyPeople,A.modifyTime,A.MarkerColor FROM[FixedAssets].[dbo].[AssetMaintain] A left outer join[FixedAssets].[dbo].[AssetChangeRecord] B on A.ID = B.ID ";
            string sql1 = order == null ? sql + "order by A.modifyTime desc" : (order == "" ? sql + "order by A.modifyTime desc" : sql + "order by " + field + " " + order + " ,A.modifyTime desc");
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            if ((Departmentss == null && AssetNumberss == null && ComputerTypes  == null && plantss == null && AssetUserss == null && ComputerNamess == null && ComputerStatusss == null) || (Departmentss == "" && AssetNumberss == "" && ComputerTypes =="" && plantss == "" && AssetUserss == "" && ComputerNamess == "" && ComputerStatusss == ""))
            {
                var info = fixedAssetsData.Database.SqlQuery<AssetMainData>(sql1).AsQueryable();
               // var infoResult = info.Skip(limit * (page - 1)).Take(limit).ToList();
                var result = new
                {
                    code = 0,
                    msg = "",
                    count = info.Count(),
                    data = info
                };
                return new JsonResult() { MaxJsonLength = Int32.MaxValue, Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                string Department;
                string AssetNumber;
                string ComputerType;
                string plant;
                string AssetUser;
                string ComputerName;
                string ComputerStatus;
                string IPAddress;
                string UseTime;
                string sql2;
                if (ComputerTypes == null || ComputerTypes == "")
                {
                    ComputerType = " like '%' or A.ComputerType is null ";
                }
                else
                {
                    ComputerType = " like '%" + ComputerTypes + "%'";
                }
                if (computerUseTimes == null || computerUseTimes == "")
                {
                    UseTime = " like '%' or (datediff(Year,A.BuyTime,GETDATE())) is null ";
                }
                else
                {
                    if (computerUseTimes == "5")
                    {
                        UseTime = " >= " + computerUseTimes + "";
                    }
                    else
                    {
                        UseTime = " >= " + computerUseTimes + "";
                    }
                }
                if (IPAddresss == null || IPAddresss == "")
                {
                    IPAddress = " like '%' or A.IPAddress is null ";
                }
                else
                {
                    IPAddress = " like '%" + IPAddresss + "%'";
                }
                if (Departmentss == null || Departmentss == "")
                {
                    Department = " like '%' or A.Department is null ";
                }
                else
                {
                    Department = " like '%" + Departmentss + "%'";
                }
                if (AssetNumberss == null || AssetNumberss == "")
                {
                    AssetNumber = " like '%' or A.AssetNumber is null ";
                }
                else
                {
                    if (AssetNumberss.Contains('\n'))
                    {
                        string[] list = AssetNumberss.Split('\n');
                        int num = list.Count();
                        AssetNumber = " in (";
                        for (int i = 0; i < num; i++)
                        {
                            if (list[i].ToString().Trim() != "")
                            {
                                AssetNumber += "'" + list[i].ToString().Trim() + "',";

                            }
                            if (i == num - 1)
                            {
                                AssetNumber += "'') ";
                            }
                        }
                    }
                    else
                    {
                        AssetNumber = " like '%" + AssetNumberss + "%'";
                    }
                }
                if (plantss == null || plantss == "")
                {
                    plant = " like '%' or A.plant is null ";
                }
                else
                {
                    plant = " like '%" + plantss + "%'";
                }
/*                if (ComputerStatusss.Contains(" "))
                {
                    string s1_1 = ComputerStatusss.Replace(",", "','");
                    string s2 = string.Format("'{0}'", s1_1);
                    ComputerStatus = "in (" + s2 + ")";
                }*/
                if (AssetUserss == null || AssetUserss == "")
                {
                    AssetUser = " like '%' or A.AssetUser is null ";
                }
                else
                {
                    AssetUser = " like '%" + AssetUserss + "%'";
                }
                if (ComputerNamess == null || ComputerNamess == "")
                {
                    ComputerName = " like '%' or A.ComputerName is null ";
                }
                else
                {
                    ComputerName = " like '%" + ComputerNamess + "%'";
                }

                if (ComputerStatusss == null || ComputerStatusss == "")
                {
                    ComputerStatus = "like '%'";
                }
                else
                {
                    string s1_1 = ComputerStatusss.Replace(",", "','");
                    string s2 = string.Format("'{0}'", s1_1);
                    ComputerStatus = "in (" + s2 + ")";
                }
                sql2 = order == null ? sql + " where (A.ComputerType  " + ComputerType + ')' + "and(A.Department  " + Department + ')' + " and ((datediff(Year,A.BuyTime,GETDATE()))  " + UseTime + ')' + " and (A.IPAddress " + IPAddress + ')' + "and (A.AssetNumber " + AssetNumber + ')' + " and (A.plant " + plant + ')' + " and (A.AssetUser " + AssetUser + ')' + " and (A.ComputerName " + ComputerName + ')' + " and (A.ComputerStatus " + ComputerStatus + ')' + " order by A.Department desc" : (order == "" ? sql + " where (A.ComputerType  " + ComputerType + ')' + "and(A.Department  " + Department + ')' + "and ((datediff(Year,A.BuyTime,GETDATE()))  " + UseTime + ')' + " and (A.IPAddress " + IPAddress + ')' + " and (A.AssetNumber " + AssetNumber + ')' + " and (A.plant " + plant + ')' + " and (A.AssetUser " + AssetUser + ')' + " and (A.ComputerName " + ComputerName + ')' + " and (A.ComputerStatus " + ComputerStatus + ')' + " order by A.Department desc" : sql + " where (A.ComputerType  " + ComputerType + ')' + "and(A.Department  " + Department + ')' + " and ((datediff(Year,A.BuyTime,GETDATE()))  " + UseTime + ')' + " and (A.IPAddress " + IPAddress + ')' + "and (A.AssetNumber " + AssetNumber + ')' + " and (A.plant " + plant + ')' + " and (A.AssetUser " + AssetUser + ')' + " and (A.ComputerName " + ComputerName + ')' + " and (A.ComputerStatus " + ComputerStatus + ')' + " order by " + field + " " + order + " ");
                var assets2 = fixedAssetsData.Database.SqlQuery<AssetMainData>(sql2).AsQueryable();
                //var infoResult2 = assets2.Skip(limit * (page - 1)).Take(limit).ToList();
                var result2 = new
                {
                    code = 0,
                    msg = "",
                    count = assets2.Count(),
                    data = assets2
                };
                return new JsonResult() { MaxJsonLength = Int32.MaxValue, Data = result2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public ActionResult FixedAssetsMainTain(int? ID)
       {
            FixedAssetsData fixedassetsdata = new FixedAssetsData();
            var user = fixedassetsdata.UserInfos.Where(u => u.userName == User.Identity.Name).FirstOrDefault();
            if (user?.role == "Admin")
            {
                ViewBag.IsAdmin = UserLever.UserAdmin;
                ViewBag.plant = user.plant;
            }
            else if (user != null && user.role == "general")
            {
                ViewBag.IsAdmin = UserLever.UserGeneral;
                ViewBag.plant = user.plant;
            }
            else if (user != null && user.role == "normal")
            {
                ViewBag.IsAdmin = UserLever.UserNormal;
                ViewBag.plant = user.plant;
            }
            else
            {
                ViewBag.IsAdmin = UserLever.UserAdministrator;
                ViewBag.plant = user.plant;
            }
            /*            var assetnumber1 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "正常使用" && n.plant == user.plant ).Count();
                        ViewBag.ComputerStatus1 = assetnumber1;
                        var assetnumber2 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "等待报废" && n.plant == user.plant).Count();
                        ViewBag.ComputerStatus2 = assetnumber2;
                        var assetnumber3 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "已经报废" && n.plant == user.plant).Count();
                        ViewBag.ComputerStatus3 = assetnumber3;*/
            ViewBag.UserName = User.Identity.Name;
            //return View();
            if (ID == null)
            {
                return View();
            }
            else
            {
                FixedAssetsData fixedAssetsData = new FixedAssetsData();
                var assects = fixedAssetsData.Database.SqlQuery<AssetMainData>("SELECT A.ID,A.plant,A.Department,A.AssetUser,A.AssetNumber,A.ComputerName,A.ComputerType,A.ComputerModel,A.SerialNumber,A.IPAddress,A.MACAddress,A.BuyTime,(datediff(Year,A.BuyTime,GETDATE())) UseTime,A.Place,A.OS,A.USBPort,A.Internet,A.ComputerStatus,B.ChangeRecord01,B.ChangeRecord02,B.ChangeRecord03,B.Remark01,B.Remark02,A.createPeople,A.createTime,A.modifyPeople,A.modifyTime,A.MarkerColor FROM [FixedAssets].[dbo].[AssetMaintain] A left outer join[FixedAssets].[dbo].[AssetChangeRecord] B on A.AssetNumber = B.AssetNumber where A.ID = '" + ID + "'").FirstOrDefault();
                if (assects != null)
                {
                    //指定Newtonsoft.Json序列化时间的格式;
                    IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" };
                    ViewBag.datas = Newtonsoft.Json.JsonConvert.SerializeObject(assects, timeConverter);
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }
        [AjaxFilter]
        //固定资产标准维护
        public ActionResult getAssetsMainTain()
        {
            return View();
        }
        //检查模具
        // 加载固定资产数据
/*        [AjaxFilter]
        public ActionResult getAssetStandardData(string assetNumbers, string plant)
        {
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            var assects = fixedAssetsData.Database.SqlQuery<AssetMainData>("SELECT A.ID,A.plant,A.Department,A.AssetUser,A.AssetNumber,A.ComputerName,A.ComputerType,A.ComputerModel,A.SerialNumber,A.IPAddress,A.MACAddress,A.BuyTime,(datediff(Year,A.BuyTime,GETDATE())) UseTime,A.Place,A.OS,A.USBPort,A.Internet,A.ComputerStatus,B.ChangeRecord01,B.ChangeRecord02,B.ChangeRecord03,B.Remark01,B.Remark02,A.createPeople,A.createTime,A.modifyPeople,A.modifyTime FROM [FixedAssets].[dbo].[AssetMaintain] A left outer join[FixedAssets].[dbo].[AssetChangeRecord] B on A.AssetNumber = B.AssetNumber where A.AssetNumber = '" + assetNumbers + "' and A.plant = '"+plant+"'" ).FirstOrDefault();
            if (assects != null)
            {
                return Json(new
                {
                    Success = true,
                    assetDatas = assects
                });
            }
            return Json(new
            {
                Success = false,
                assetDatas = ""
            });
        }*/
        // 保存更新固定资产标准数据
        [AjaxFilter]
        public ActionResult saveAssetStandardData(AssetMaintain assetData,AssetChangeRecord assetDatas)
        {
            int result = 0;
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            //var assects = fixedAssetsData.Database.SqlQuery<AssetMainData>("SELECT *FROM [FixedAssets].[dbo].[AssetMaintain] A left outer join[FixedAssets].[dbo].[AssetChangeRecord] B on A.AssetNumber = B.AssetNumber where A.AssetNumber = '" + assetData.AssetNumber + "' and A.plant = '" + assetData.plant + "'").FirstOrDefault();
             // var assects = fixedAssetsData.Database.SqlQuery<AssetMaintain>("SELECT *FROM [FixedAssets].[dbo].[AssetMaintain] where AssetNumber = '" + assetData.AssetNumber + "'and plant = '" + assetData.plant + "'").FirstOrDefault();
            var assects = fixedAssetsData.AssetMaintains.Where(u => u.ID == assetData.ID).FirstOrDefault();
            var user = fixedAssetsData.UserInfos.Where(u => u.userName == User.Identity.Name.ToUpper()).FirstOrDefault();
            if (assects != null)
            {
                //更新
                assects.AssetNumber = assetData.AssetNumber;
                assects.plant = assetData.plant;
                assects.AssetUser = assetData.AssetUser;
                assects.Department = assetData.Department;
                assects.ComputerName = assetData.ComputerName;
                assects.ComputerType = assetData.ComputerType;
                assects.ComputerModel = assetData.ComputerModel;
                assects.SerialNumber = assetData.SerialNumber;
                assects.IPAddress = assetData.IPAddress;
                assects.MACAddress =  assetData.MACAddress;
                assects.BuyTime =  assetData.BuyTime;
                assects.Place =  assetData.Place;
                assects.OS =  assetData.OS;
                assects.ComputerStatus =  assetData.ComputerStatus;
                assects.USBPort =  assetData.USBPort;
                assects.Internet =  assetData.Internet;
                //assects.ChangeRecord01 =  assetData.ChangeRecord01;
                //assects.ChangeRecord02 = assetData.ChangeRecord02;
                //assects.ChangeRecord03 = assetData.ChangeRecord03;
               // assects.Remark01 =  assetData.Remark01;
                //assects.Remark02 =  assetData.Remark02;
                assects.modifyPeople = User.Identity.Name;
                assects.modifyTime = DateTime.Now;
                //标记颜色
                assects.MarkerColor = assetData.MarkerColor;
                result = fixedAssetsData.SaveChanges();
            }
            else
            {
                //新增
                assetData.modifyPeople = User.Identity.Name;
                assetData.modifyTime = DateTime.Now;
                assetData.createPeople = User.Identity.Name;
                assetData.createTime = DateTime.Now;
                fixedAssetsData.AssetMaintains.Add(assetData);
                result = fixedAssetsData.SaveChanges();
            }
            //添加ChangeRecord表数据
            //var assectss = fixedAssetsData.Database.SqlQuery<AssetMainData>("SELECT *FROM [FixedAssets].[dbo].[AssetMaintain] A left outer join[FixedAssets].[dbo].[AssetChangeRecord] B on A.AssetNumber = B.AssetNumber where A.AssetNumber = '" + assetData.AssetNumber + "' and plant = '" + assetData.plant + "'").FirstOrDefault();
            // var assectss = fixedAssetsData.AssetChangeRecords.Where(u => u.AssetNumber == assetData.AssetNumber && u.plant == assetData.plant).FirstOrDefault();
            var assects2 = fixedAssetsData.AssetChangeRecords.Where(u => u.ID == assetData.ID).FirstOrDefault();
            if(assects2 == null)
            {
                AssetChangeRecord assetChangeRecord = new AssetChangeRecord();
                assetChangeRecord.AssetNumber = assetDatas.AssetNumber;
                assetChangeRecord.plant = assetDatas.plant;
                assetChangeRecord.ChangeRecord01 = assetDatas.ChangeRecord01;
                assetChangeRecord.ChangeRecord02 = assetDatas.ChangeRecord02;
                assetChangeRecord.ChangeRecord03 = assetDatas.ChangeRecord03;
                assetChangeRecord.Remark01 = assetDatas.Remark01;
                assetChangeRecord.Remark02 = assetDatas.Remark02;
                fixedAssetsData.AssetChangeRecords.Add(assetChangeRecord);
                fixedAssetsData.SaveChanges();
            }
            else
            {
                assects2.AssetNumber = assetDatas.AssetNumber;
                assects2.plant = assetDatas.plant;
                assects2.ChangeRecord01 = assetDatas.ChangeRecord01;
                assects2.ChangeRecord02 = assetDatas.ChangeRecord02;
                assects2.ChangeRecord03 = assetDatas.ChangeRecord03;
                assects2.Remark01 = assetDatas.Remark01;
                assects2.Remark02 = assetDatas.Remark02;
                fixedAssetsData.SaveChanges();
            }
            return Json(new
            {
                res = result
            });

        }
        // 删除模具标准数据
        [AjaxFilter]
        public ActionResult deleteAssetStandardData(string assetNumber, string plant,int ID)
        {
            int result = 0;
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            //var assects = fixedAssetsData.AssetMaintains.Where(u => u.AssetNumber == assetNumber && u.plant == plant).FirstOrDefault();
            var assects = fixedAssetsData.AssetMaintains.Where(u => u.ID == ID).FirstOrDefault();
            var userInfo = fixedAssetsData.UserInfos.Where(u => u.userName == User.Identity.Name).FirstOrDefault();
            //var assect2 = fixedAssetsData.AssetChangeRecords.Where(u => u.AssetNumber == assetNumber && u.plant == plant).FirstOrDefault();
            var assect2 = fixedAssetsData.AssetChangeRecords.Where(u => u.ID == ID).FirstOrDefault();
            if (userInfo.plant == plant)
            {
                if (assects != null)
                {
                    fixedAssetsData.AssetMaintains.Attach(assects);
                    fixedAssetsData.AssetMaintains.Remove(assects);
                    fixedAssetsData.Entry<AssetMaintain>(assects).State = System.Data.Entity.EntityState.Deleted;
                    result = fixedAssetsData.SaveChanges();
                    if (assect2 != null)
                    {
                        fixedAssetsData.AssetChangeRecords.Attach(assect2);
                        fixedAssetsData.AssetChangeRecords.Remove(assect2);
                        fixedAssetsData.Entry<AssetChangeRecord>(assect2).State = System.Data.Entity.EntityState.Deleted;
                        result = fixedAssetsData.SaveChanges();
                    }
                    if (result > 0)
                    {
                        return Json(new
                        {
                            Success = true,
                            Message = "固定资产数据删除成功！"
                        });
                    }
                }
                return Json(new
                {
                    Success = false,
                    Message = "删除失败！"
                });
            }
            else
            {
                return Json(new
                {
                    Success = false,
                    Message = "请勿删除其它工厂数据！"
                });
            }
        }
        public ActionResult getAssetsPlant(string ComputerName, string plant)
        {
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            if(ComputerName == null || ComputerName == "")
            {
                return Json(new
                {
                    Success = false,
                });

            }
            var assects = fixedAssetsData.AssetMaintains.Where(u => u.ComputerName == ComputerName && u.plant == plant).FirstOrDefault();
            if(assects != null)
            {
                return Json(new
                {
                    Success = true,
                    Message = "固定资产名称重复！"
                });
            }
            else
            {
                return Json(new
                {
                    Success = false,
                });
            }
        }
        public ActionResult UserInfo()
        {
            FixedAssetsData fixedassetsdata = new FixedAssetsData();
            var user = fixedassetsdata.UserInfos.Where(u => u.userName == User.Identity.Name).FirstOrDefault();
            if (user?.role == "Admin")
            {
                ViewBag.IsAdmin = UserLever.UserAdmin;
                ViewBag.plant = user.plant;
            }
            else if (user != null && user.role == "general")
            {
                ViewBag.IsAdmin = UserLever.UserGeneral;
                ViewBag.plant = user.plant;
            }
            else if (user != null && user.role == "normal")
            {
                ViewBag.IsAdmin = UserLever.UserNormal;
                ViewBag.plant = user.plant;
            }
            else
            {
                ViewBag.IsAdmin = UserLever.UserAdministrator;
                ViewBag.plant = user.plant;
            }
            /*            var assetnumber1 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "正常使用" && n.plant == user.plant ).Count();
                        ViewBag.ComputerStatus1 = assetnumber1;
                        var assetnumber2 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "等待报废" && n.plant == user.plant).Count();
                        ViewBag.ComputerStatus2 = assetnumber2;
                        var assetnumber3 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "已经报废" && n.plant == user.plant).Count();
                        ViewBag.ComputerStatus3 = assetnumber3;*/
            ViewBag.UserName = User.Identity.Name;
            return View();
        }
        /// <summary>
        /// 新建更新用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord1">密码</param>
        /// <param name="role">角色权限</param>
        /// <returns></returns>
        [AjaxFilter]
        public ActionResult saveUserInfo(string userName, string passWord1, string role,string Department, string plant)
        {
            int result = 0;
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            var user = fixedAssetsData.UserInfos.Where(u => u.userName == userName).FirstOrDefault();
            if (user != null)
            {
                if (user.role == "Admin" && user.userName.ToUpper() != User.Identity.Name.ToUpper())
                {
                    result = 99;
                }
                else
                {
                    //更新
                    user.userName = userName;
                    user.passWord = passWord1;
                    user.role = role;
                    user.department = Department;
                    user.plant = plant;
                    result = fixedAssetsData.SaveChanges();
                }
            }
            else
            {
                //新增
                UserInfo users = new UserInfo();
                users.userName = userName;
                users.passWord = passWord1;
                users.role = role;
                users.department = Department;
                users.plant = plant;
                users.remark = DateTime.Now.ToString();
                fixedAssetsData.UserInfos.Add(users);
                result = fixedAssetsData.SaveChanges();
            }
            return Json(new
            {
                res = result
            });
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="UserID">用户名</param>
        /// <returns></returns>
        [AjaxFilter]
        public ActionResult deleteUserInfo(string UserID)
        {
            int result = 0;
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            var user = fixedAssetsData.UserInfos.Where(u => u.userName == UserID).FirstOrDefault();
            if (user != null && user.role != "Admin")
            {
                fixedAssetsData.UserInfos.Attach(user);
                fixedAssetsData.UserInfos.Remove(user);
                fixedAssetsData.Entry<UserInfo>(user).State = System.Data.Entity.EntityState.Deleted;
                result = fixedAssetsData.SaveChanges();
                if (result > 0)
                {
                    return Json(new
                    {
                        Success = true
                    });
                }
            }
            return Json(new
            {
                Success = false
            });
        }
        //登录信息
        public JsonResult getUserJson()
        {
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            var user = fixedAssetsData.UserInfos.Where(u => u.userName == User.Identity.Name).FirstOrDefault();

            var result = new
            {
                UserID = user.userName,
                role = user.role,
                plant = user.plant,
                department = user.department,
            };
            var jsonresult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonresult;
        }
        //加载部门数据
        [AjaxFilter]
        public ActionResult SelectPlantStaff(string plant)
        {
            FixedAssetsData fixedAssetsData = new FixedAssetsData();
            var user = fixedAssetsData.UserInfos.Where(u => u.userName == User.Identity.Name).FirstOrDefault();
            List<AssetDepartment> department = null;
/*            if (plant == "" || plant == null)
            {
                department = fixedAssetsData.AssetDepartments.Where(u => u.plant != "").ToList();
            }
            else
            {
               
            }*/
            department = fixedAssetsData.AssetDepartments.Where(u => u.plant == plant).ToList();
            return Json(new
            {
                data = department
            });
        }
        //加载电脑状态数据
        [AjaxFilter]
        public ActionResult SelectComputerStatus(string plant)
        {
            FixedAssetsData fixedassetsdata = new FixedAssetsData();
            var assetnumber1 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "正常使用" && n.plant == plant).Count();
            int ComputerStatus1 = assetnumber1;
            var assetnumber2 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "等待报废" && n.plant == plant).Count();
            int ComputerStatus2 = assetnumber2;
            var assetnumber3 = fixedassetsdata.AssetMaintains.Where(n => n.ComputerStatus == "已经报废" && n.plant == plant).Count();
            int ComputerStatus3 = assetnumber3;
            return Json(new
            {
                ComputerStatus1 = ComputerStatus1,
                ComputerStatus2 = ComputerStatus2,
                ComputerStatus3 = ComputerStatus3
            });
        }
    }
}