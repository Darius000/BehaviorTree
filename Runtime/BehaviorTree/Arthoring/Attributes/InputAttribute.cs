using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class InputAttribute  : PinAttribute
    {
        public InputAttribute() : base()
        {

        }

        public InputAttribute(Type type, Capacity capacity = Capacity.Single) : base(type, capacity) { }
    }
}
