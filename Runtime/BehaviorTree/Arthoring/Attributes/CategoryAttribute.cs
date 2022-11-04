using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    /// <summary>
    /// Attribute to categorize nodes in the node search window
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CategoryAttribute : Attribute
    {
        public string Category { get; set; }

        public CategoryAttribute(string category)
        {
            Category = category;
        }
    }
}
