using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    public class TBlackBoardKeyType<T> : BlackBoardKey
    {

        public TBlackBoardKeyType()
        {
            _object = default(T);
        }

        public T GetValue()
        {
            return GetValue<T>();
        }

        public void SetValue(T vector)
        {
            SetValue<T>(vector);
        }

        public override Type GetKeyType()
        {
            return typeof(T);
        }

        protected override BlackBoardKey OnClone()
        {
            return Instantiate(this);
        }
    }
}
