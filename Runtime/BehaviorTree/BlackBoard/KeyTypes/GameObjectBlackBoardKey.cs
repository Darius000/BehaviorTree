using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [BlackBoardKeyColor(0 , 1 , 1)]
    [DisplayName("GameObject")]
    [System.Serializable]
    public class GameObjectBlackBoardKey : TBlackBoardKeyType<GameObject>
    {
        public GameObjectBlackBoardKey() : base() { }

        public GameObjectBlackBoardKey(GameObjectBlackBoardKey other) : base(other) { }

        protected override BlackBoardKey OnClone()
        {
            return new GameObjectBlackBoardKey(this);
        }
    }
}
