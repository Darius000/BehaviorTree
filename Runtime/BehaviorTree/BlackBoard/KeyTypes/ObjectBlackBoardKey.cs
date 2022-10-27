using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    [BlackBoardKeyColor(.4f, .4f , 1f)]
    [DisplayName("Object")]
    [System.Serializable]
    public class ObjectBlackBoardKey : TBlackBoardKeyType<UnityEngine.Object>
    {
        public ObjectBlackBoardKey() : base() { }

        public ObjectBlackBoardKey(ObjectBlackBoardKey other) : base(other) { }

        protected override BlackBoardKey OnClone()
        {
            return new ObjectBlackBoardKey(this);
        }
    }
}
