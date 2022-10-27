using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [BlackBoardKeyColor(0, .5f, 0)]
    [DisplayName("Int")]
    [System.Serializable]
    public class IntBlackBoardKey : TBlackBoardKeyType<int>
    {
        public IntBlackBoardKey() : base() { }

        public IntBlackBoardKey(IntBlackBoardKey other) : base(other) { }

        protected override BlackBoardKey OnClone()
        {
            return new IntBlackBoardKey(this);
        }
    }
}
