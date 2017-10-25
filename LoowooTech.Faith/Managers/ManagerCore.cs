using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class ManagerCore
    {
        public static readonly ManagerCore Instance = new ManagerCore();

        private UserManager _userManager { get; set; }
        public UserManager UserManager { get { return _userManager == null ? _userManager = new UserManager() : _userManager; } }
        private LawyerManager _lawyerManager { get; set; }
        public LawyerManager LawyerManager { get { return _lawyerManager == null ? _lawyerManager = new LawyerManager() : _lawyerManager; } }
        private DailyManager _dailyManager { get; set; }
        public DailyManager DailyManager { get { return _dailyManager == null ? _dailyManager = new DailyManager() : _dailyManager; } }
        private EnterpriseManager _enterpriseManager { get; set; }
        public EnterpriseManager EnterpriseManager { get { return _enterpriseManager == null ? _enterpriseManager = new EnterpriseManager() : _enterpriseManager; } }
        private StandardManager _standardManager { get; set; }
        public StandardManager StandardManager { get { return _standardManager == null ? _standardManager = new StandardManager() : _standardManager; } }
        private ConductManager _conductManager { get; set; }
        public ConductManager ConductManager { get { return _conductManager == null ? _conductManager = new ConductManager() : _conductManager; } }
        private ConductStandardManager _conductStandardManager { get; set; }
        public ConductStandardManager ConductStandardManager { get { return _conductStandardManager == null ? _conductStandardManager = new ConductStandardManager() : _conductStandardManager; } }
        private RollManager _rollManager { get; set; }
        public RollManager RollManager { get { return _rollManager == null ? _rollManager = new RollManager() : _rollManager; } }
        private FlowNodeManager _flowNodeManager { get; set; }
        public FlowNodeManager FlowNodeManager { get { return _flowNodeManager == null ? _flowNodeManager = new FlowNodeManager() : _flowNodeManager; } }
        private FlowNodeConductManager _flowNodeConductManager { get; set; }
        public FlowNodeConductManager FlowNodeConductManager { get { return _flowNodeConductManager == null ? _flowNodeConductManager = new FlowNodeConductManager() : _flowNodeConductManager; } }
        private RollViewManager _rollViewManager { get; set; }
        public RollViewManager RollViewManager { get { return _rollViewManager == null ? _rollViewManager = new RollViewManager() : _rollViewManager; } }
        private GradeManager _gradeManager { get; set; }
        public GradeManager GradeManager { get { return _gradeManager == null ? _gradeManager = new GradeManager() : _gradeManager; } }
        private FlowNodeViewManager _flowNodeViewManager { get; set; }
        public FlowNodeViewManager FlowNodeViewManager { get { return _flowNodeViewManager == null ? _flowNodeViewManager = new FlowNodeViewManager() : _flowNodeViewManager; } }
        private OffendManager _offendManager { get; set; }
        public OffendManager OffendManager { get { return _offendManager == null ? _offendManager = new OffendManager() : _offendManager; } }
        private LandRecordManager _landRecordManager { get; set; }
        public LandRecordManager LandRecordManager { get { return _landRecordManager == null ? _landRecordManager = new LandRecordManager() : _landRecordManager; } }
        private LandRecordViewManager _landRecordViewManager { get; set; }
        public LandRecordViewManager LandRecordViewManager { get { return _landRecordViewManager == null ? _landRecordViewManager = new LandRecordViewManager() : _landRecordViewManager; } }
        private LandManager _landManager { get; set; }
        public LandManager LandManager { get { return _landManager == null ? _landManager = new LandManager() : _landManager; } }
        private ScoreManager _scoreManager { get; set; }
        public ScoreManager ScoreManager { get { return _scoreManager == null ? _scoreManager = new ScoreManager() : _scoreManager; } }
        private FeedManager _feedManager { get; set; }
        public FeedManager FeedManager { get { return _feedManager == null ? _feedManager = new FeedManager() : _feedManager; } }
        private CityManager _cityManager { get; set; }
        public CityManager CityManager { get { return _cityManager == null ? _cityManager = new CityManager() : _cityManager; } }

        private FaithFileManager _fileManager { get; set; }
        public FaithFileManager FileManager { get { return _fileManager == null ? _fileManager = new FaithFileManager() : _fileManager; } }

        private GradeHistoryManager _gradeHistoryManager { get; set; }
        public GradeHistoryManager GradeHistoryManager { get { return _gradeHistoryManager == null ? _gradeHistoryManager = new GradeHistoryManager() : _gradeHistoryManager; } }
        private ScoresHistoryManager _scoresHistoryManager { get; set; }
        public ScoresHistoryManager ScoresHistoryManager { get { return _scoresHistoryManager == null ? _scoresHistoryManager = new ScoresHistoryManager() : _scoresHistoryManager; } }

    }
}