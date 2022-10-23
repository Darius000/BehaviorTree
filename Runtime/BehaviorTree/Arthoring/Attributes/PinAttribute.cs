using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    public enum Capacity
    {
        Single = 0,
        Multi = 1
    }
    public class PinAttribute  :Attribute
    {
        private Type type;

        private Capacity capacity;

        public PinAttribute()
        {
            type = typeof(bool);
            capacity = Capacity.Single;
        }

        public PinAttribute(Type type, Capacity capacity = Capacity.Single)
        {
            this.type = type;
            this.capacity = capacity;
        }

        public Type Type { get { return type; } set { type = value; } }
        public Capacity Capacity { get { return capacity; } set { capacity = value; } }
    }
}
