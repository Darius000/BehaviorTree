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

        public TBlackBoardKeyType(TBlackBoardKeyType<T> key) : base(key)
        {
            _object = key._object;
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
            var key =  new TBlackBoardKeyType<T>(this);
            return key;
        }
    }
}
