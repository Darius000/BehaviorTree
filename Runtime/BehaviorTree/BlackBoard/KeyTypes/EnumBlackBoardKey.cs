using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    [DisplayName("Enum")]
    [BlackBoardKeyColor(0f, .7f, .1f)]
    [System.Serializable]
    public class EnumBlackBoardKey : TBlackBoardKeyType<Enum>
    {
        public EnumBlackBoardKey() : base() { }

        public EnumBlackBoardKey(EnumBlackBoardKey other) : base(other) { }

        protected override BlackBoardKey OnClone()
        {
            return new EnumBlackBoardKey(this);
        }
    }
}
