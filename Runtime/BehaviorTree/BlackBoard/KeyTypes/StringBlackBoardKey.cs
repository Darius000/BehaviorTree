using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    [DisplayName("String")]
    [BlackBoardKeyColor(1f, .5f, .5f)]
    [System.Serializable]
    public class StringBlackBoardKey : TBlackBoardKeyType<string>
    {
        public StringBlackBoardKey() : base() { }

        public StringBlackBoardKey(StringBlackBoardKey other) : base(other) { }

        protected override BlackBoardKey OnClone()
        {
            return new StringBlackBoardKey(this);
        }
    }
}
