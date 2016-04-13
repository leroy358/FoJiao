using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FoJiao.Models
{
    public class FoJiaoDbContext : DbContext
    {
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Articles> Articles { get; set; }
        public DbSet<AudioCollections> AudioCollections { get; set; }
        public DbSet<Audios> Audios { get; set; }
        public DbSet<ConfusionCate> ConfusionCate { get; set; }
        public DbSet<Confusions> Confusions { get; set; }
        public DbSet<ConfusionSecCate> ConfusionSecCate { get; set; }
        public DbSet<DailyWords> DailyWords { get; set; }
        public DbSet<VideoCollections> VideoCollections { get; set; }
        public DbSet<Videos> Videos { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<ConfusionGuest> ConfusionGuest { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Answers { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// 1、用户提问；2、后台编辑内容
        /// </summary>
        public int Category { get; set; }
        public int ConfusionSecId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 0、未处理1、已回答2、删除
        /// </summary>
        public int StateId { get; set; }
        //public bool IsAnswered { get; set; }
        //public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public FoJiaoDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<FoJiaoDbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}