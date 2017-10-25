using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class ScoresHistoryManager:ManagerBase
    {
        public  void Scores(int gradeId,int cityId)
        {
            ScoresEnterprise(gradeId, cityId);
            ScoresLawyer(gradeId, cityId);
        }

        public void ScoresEnterprise(int gradeId,int cityId)
        {
            var list = Db.EnterpriseScores.Where(e=>e.CityID==cityId&&e.Deleted==false).ToList().Select(e => new ScoresHistory
            {
                SystemData = SystemData.Enterprise,
                ELID = e.ID,
                Times = e.Times,
                ScoreValue = e.ScoreValue,
                Record = e.Record,
                GradeID = gradeId,
                ScoreTexts=new List<ScoreText>()
            }).ToList();
            foreach(var item in list)
            {
                item.ScoreTexts.AddRange(Db.LandRecords.Where(e => e.State == LandRecordState.Enter && e.SystemData == SystemData.Enterprise && e.ELID == item.ELID).ToList().Select(e => new ScoreText
                {
                    Scores = e.Score,
                    CLID = e.ID,
                    Type = FileType.LandRecord,
                    Name = string.Format("[违法用地]：违法用地面积：{0}【亩】；合法用地面积：{1}【亩】；查出文号：{2}", Math.Round(e.IllegalArea, 4), Math.Round(e.Area, 4), e.Code)
                }));
                item.ScoreTexts.AddRange(Db.ConductStandards.Where(e => e.SystemData == SystemData.Enterprise && e.State == BaseState.Argee && e.ELID == item.ELID).ToList().Select(e => new ScoreText
                {
                    Scores = e.Score,
                    CLID = e.ID,
                    Type = FileType.Conduct,
                    Name = string.Format("[诚信行为]：项目名称：{0}；环节：{1}；行为：{2}", e.LandName, e.Credit.GetDescription(), e.StandardName)
                }));
            }
            Db.ScoresHistorys.AddRange(list);
            Db.SaveChanges();
        }
        public void ScoresLawyer(int gradeId,int cityId)
        {
            var list = Db.LawyerScores.Where(e => e.CityID == cityId&&e.Deleted==false).ToList().Select(e => new ScoresHistory
            {
                SystemData = SystemData.Lawyer,
                ELID = e.ID,
                Times = e.Times,
                ScoreValue = e.ScoreValue,
                Record = e.Record,
                GradeID = gradeId,
                ScoreTexts=new List<ScoreText>()
            }).ToList();
            foreach (var item in list)
            {
                item.ScoreTexts.AddRange(Db.LandRecords.Where(e => e.State == LandRecordState.Enter && e.SystemData == SystemData.Lawyer && e.ELID == item.ELID).ToList().Select(e => new ScoreText
                {
                    Scores = e.Score,
                    CLID = e.ID,
                    Type = FileType.LandRecord,
                    Name = string.Format("[违法用地]：违法用地面积：{0}【亩】；合法用地面积：{1}【亩】；查出文号：{2}", Math.Round(e.IllegalArea, 4), Math.Round(e.Area, 4), e.Code)
                }));
                item.ScoreTexts.AddRange(Db.ConductStandards.Where(e => e.SystemData == SystemData.Lawyer && e.State == BaseState.Argee && e.ELID == item.ELID).ToList().Select(e => new ScoreText
                {
                    Scores = e.Score,
                    CLID = e.ID,
                    Type = FileType.Conduct,
                    Name = string.Format("[诚信行为]：项目名称：{0}；环节：{1}；行为：{2}", e.LandName, e.Credit.GetDescription(), e.StandardName)
                }));
            }
            Db.ScoresHistorys.AddRange(list);
            Db.SaveChanges();
        }
      
        public ScoresHistory Get(int id)
        {
            return Db.ScoresHistorys.Find(id);
        }

        public List<ScoresHistory> GetList(int elId,SystemData systemData)
        {
            return Db.ScoresHistorys.Where(e => e.Grade.Delete == false && e.SystemData == systemData && e.ELID == elId).OrderBy(e=>e.GradeID).ToList();
        }
    }
}