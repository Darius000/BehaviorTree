using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    [BlackBoardKeyColor(1, 0, 0)]
    [DisplayName(DisplayName = "Bool")]
    [System.Serializable]
    public class BoolBlackBoardKey : TBlackBoardKeyType<bool>
    {
        public BoolBlackBoardKey() : base() { }

        public BoolBlackBoardKey(BoolBlackBoardKey other) : base(other) { }

        protected override BlackBoardKey OnClone()
        {
            return new BoolBlackBoardKey(this);
        }
    }
}
