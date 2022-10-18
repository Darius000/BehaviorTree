using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisplayNameAttribute : Attribute
    {
        private string m_DisplayName;

        public DisplayNameAttribute()
        {

        }

        public DisplayNameAttribute(string displayName)
        {
            m_DisplayName = displayName;
        }

        public string DisplayName { get { return m_DisplayName; } set { m_DisplayName = value; } }
    }
}