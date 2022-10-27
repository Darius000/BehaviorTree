using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    [BlackBoardKeyColor(.5f, 1, .5f)]
    [DisplayName("Float")]
    [System.Serializable]
    public class FloatBlackBoardKey : TBlackBoardKeyType<float>
    {
        public FloatBlackBoardKey():base(){}

        public FloatBlackBoardKey(FloatBlackBoardKey other) : base(other) { }

        protected override BlackBoardKey OnClone()
        {
            return new FloatBlackBoardKey(this);
        }
    }
}
