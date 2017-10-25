using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Parameters
{
    public class LandRecordViewParameter:ParameterBase
    {
        public string Code { get; set; }
        public string ELName { get; set; }
        public double? MinScore { get; set; }
        public double? MaxScore { get; set; }
        public SystemData? SystemData { get; set; }
        public int? ELID { get; set; }
        public LandRecordState? State { get; set; }
    }
}