using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace LoowooTech.Faith.Common
{
    public static class SystemManager
    {
        private static XmlDocument _configXml { get; set; }
        static SystemManager()
        {
            _configXml = new XmlDocument();
            _configXml.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["Title"] ?? "XMLTitleFile.xml"));
        }
        private static XmlNode GetSingle(string queryString)
        {
            return _configXml.SelectSingleNode(queryString);
        }
        private static XmlNodeList GetList(string queryString)
        {
            return _configXml.SelectNodes(queryString);
        }
        private static string GetTitle()
        {
            var node = GetSingle("/Faiths");
            if (node != null)
            {
                return node.Attributes["Title"].Value;
            }
            return string.Empty;
        }
        private static List<string> GetCredits()
        {
            var list = new List<string>();
            var nodes = GetList("/Faiths/Credits/Credit");
            if (nodes != null && nodes.Count > 0)
            {
                for(var i = 0; i < nodes.Count; i++)
                {
                    list.Add(nodes[i].Attributes["Name"].Value);
                }
            }
            return list;
        }
        private static List<string> GetDegrees()
        {
            var list = new List<string>();
            var nodes = GetList("/Faiths/Degrees/Degree");
            if (nodes != null && nodes.Count > 0)
            {
                for(var i = 0; i< nodes.Count; i++)
                {
                    list.Add(nodes[i].Attributes["Name"].Value);
                }
            }
            return list;
        }
        private static string GetDistrict()
        {
            var node = GetSingle("/Faiths/District");
            if (node != null)
            {
                return node.Attributes["Name"].Value;
            }
            return string.Empty;
        }
        private static XmlNode GetNode()
        {
            var node = GetSingle("/Faiths/District");
            return node;
        }
        private static XmlNode _node { get; set; }
        public static XmlNode Node { get { return _node == null ? _node = GetNode() : _node; } }
        private static string _title { get; set; }
        public static string Title { get { return string.IsNullOrEmpty(_title) ? _title = GetTitle() : _title; } }
        private static string _district { get; set; }
        public static string District { get { return string.IsNullOrEmpty(_district) ? _district = GetDistrict() : _district; } }
        private static string _department { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public static string Department { get { return string.IsNullOrEmpty(_department) ? Node != null ? Node.Attributes["Department"].Value : _department : _department; } }
        private static string _number { get; set; }
        public static string Number { get { return string.IsNullOrEmpty(_number) ? Node != null ? Node.Attributes["Number"].Value : _number : _number; } }

        private static List<string> _credits { get; set; }
        public static List<string> Credits { get { return _credits == null || _credits.Count == 0 ? _credits = GetCredits() : _credits; } }
        private static List<string> _degrees { get; set; }
        public static List<string> Degrees { get { return _degrees == null || _degrees.Count == 0 ? _degrees = GetDegrees() : _degrees; } }
    }
}