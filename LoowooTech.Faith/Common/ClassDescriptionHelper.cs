using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Common
{
    public  class ClassDescriptionHelper
    {
        private static List<ClassDescription> _enterprises { get; set; }
        /// <summary>
        /// 企业类相关信息
        /// </summary>
        public static List<ClassDescription> Enterprises { get { return _enterprises == null ? Analyze(typeof(Enterprise)) : _enterprises; } }
        private static List<ClassDescription> _lawyers { get; set; }
        /// <summary>
        /// 自然人类相关信息
        /// </summary>
        public static List<ClassDescription> Lawyers { get { return _lawyers == null ? Analyze(typeof(Lawyer)) : _lawyers; } }
        private static List<ClassDescription> Analyze(Type t)
        {
            var list = new List<ClassDescription>();
            System.Reflection.PropertyInfo[] properties = t.GetProperties();
            foreach(System.Reflection.PropertyInfo property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute;
                list.Add(new ClassDescription
                {
                    Name = property.Name,
                    Description = attribute != null ? attribute.Description : string.Empty
                });
            }
            return list;
        }
    }
}