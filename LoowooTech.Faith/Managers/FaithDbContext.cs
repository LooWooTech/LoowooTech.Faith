using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class FaithDbContext:DbContext
    {
        public FaithDbContext() : base("name=DbConnection")
        {
            Database.SetInitializer<FaithDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Conduct>().HasRequired(e => e.Standard).WithMany().HasForeignKey(e => e.StandardId);
            modelBuilder.Entity<FlowNode>().HasRequired(e => e.Conduct).WithMany().HasForeignKey(e => e.InfoID);

            //modelBuilder.Entity<FlowStep>().HasRequired(e => e.Prev).WithRequiredPrincipal(e => e.Prev);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lawyer> Lawyers { get; set; }
        public DbSet<Daily> Dailys { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<Standard> Standards { get; set; }
        public DbSet<Conduct> Conducts { get; set; }
        public DbSet<ConductStandard> ConductStandards { get; set; }
        public DbSet<Roll> Rolls { get; set; }
        public DbSet<FlowNode> FlowNodes { get; set; }
        public DbSet<FlowNodeConduct> FlowNodeConducts { get; set; }
        public DbSet<RollView> RollViews { get; set; }
        public DbSet<ConductView> ConductView { get; set; }//打算作废
        public DbSet<FlowNodeView> FlowNodeView { get; set; }//打算作废
        public DbSet<Offend> Offends { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<LandRecord> LandRecords { get; set; }
        public DbSet<LandRecordView> LandRecordViews { get; set; }
        public DbSet<Land> Lands { get; set; }
        public DbSet<LandView> LandViews { get; set; }

        public DbSet<Feed> Feeds { get; set; }
        public DbSet<FeedView> FeedViews { get; set; }
        public DbSet<EnterpriseScore> EnterpriseScores { get; set; }
        public DbSet<LawyerScore> LawyerScores { get; set; }
        public DbSet<City> Citys { get; set; }//城市

        public DbSet<FaithFile> Files { get; set; }//文件

        public DbSet<GradeHistory> GradeHistorys { get; set; }

        public DbSet<ScoresHistory> ScoresHistorys { get; set; }
        public DbSet<ScoreText> ScoreTexts { get; set; }
       // public DbSet<FlowStep> FlowSteps { get; set; }
    }

    
}