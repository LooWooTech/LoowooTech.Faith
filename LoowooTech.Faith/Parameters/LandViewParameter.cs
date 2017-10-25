using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Parameters
{
    public class LandViewParameter:ParameterBase
    {
        public int ? ELID { get; set; }
        public SystemData? SystemData { get; set; }
        public string Name { get; set; }
        public string sName { get; set; }
        public string Number { get; set; }
        public string ContractNumber { get; set; }
        public SoldWay? Way { get; set; }
        public LandOrder? Order { get; set; }

    }

    public enum LandOrder
    {
        [Description("项目名称")]
        Name,
        [Description("受让人")]
        sName,
        [Description("合同编号")]
        ContractNumber,
    }
}