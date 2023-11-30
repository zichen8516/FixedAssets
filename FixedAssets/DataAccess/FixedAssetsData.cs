using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FixedAssets.Models;

namespace FixedAssets.DataAccess
{
    public class FixedAssetsData:DbContext
    {
        public FixedAssetsData() : base("fixedassetsdata")
        {
            //this.Database.CommandTimeout = 6000000;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetMaintain>().ToTable("AssetMaintain");
            modelBuilder.Entity<AssetMainData>().ToTable("AssetMainData");
            modelBuilder.Entity<AssetDepartment>().ToTable("AssetDepartment");
            modelBuilder.Entity<AssetChangeRecord>().ToTable("AssetChangeRecord");
            modelBuilder.Entity<UserInfo>().ToTable("UserInfo");
            modelBuilder.Entity<AssetOS>().ToTable("AssetOS");
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<AssetMaintain> AssetMaintains { get; set; }
        public DbSet<AssetMainData> AssetMainData { get; set; }
        public DbSet<AssetDepartment> AssetDepartments { get; set; }
        public DbSet<AssetChangeRecord> AssetChangeRecords { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<AssetOS> AssetOSs { get; set; }
    }
}