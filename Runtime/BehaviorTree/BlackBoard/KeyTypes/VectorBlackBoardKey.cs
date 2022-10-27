using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [BlackBoardKeyColor(1, 1 ,0)]
    [DisplayName("Vector3")]
    [System.Serializable]
    public class VectorBlackBoardKey : TBlackBoardKeyType<Vector3>
    {
        public VectorBlackBoardKey() : base() { }

        public VectorBlackBoardKey(VectorBlackBoardKey other) : base(other) { }

        protected override BlackBoardKey OnClone()
        {
            return new VectorBlackBoardKey(this);
        }
    }
}

