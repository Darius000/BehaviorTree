using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    [System.Serializable]
    public class TBlackBoardKeySelector<T> : BlackBoardKeySelector
    {
        public override Type GetKeyType()
        {
            return typeof(T);
        }
    }

    [System.Serializable]
    public class VectorBlackBoardKeySelector : TBlackBoardKeySelector<Vector3> { }

    [System.Serializable]
    public class BoolBlackBoardKeySelector : TBlackBoardKeySelector<bool> { }

    [System.Serializable]
    public class EnumBlackBoardKeySelector : TBlackBoardKeySelector<Enum> { }

    [System.Serializable]
    public class FloatBlackBoardKeySelector : TBlackBoardKeySelector<float> { }

    [System.Serializable]
    public class IntBlackBoardKeySelector : TBlackBoardKeySelector<int> { }

    [System.Serializable]
    public class GameObjectBlackBoardKeySelector : TBlackBoardKeySelector<GameObject> { }

    [System.Serializable]
    public class ObjectBlackBoardKeySelector : TBlackBoardKeySelector<UnityEngine.Object> { }

    [System.Serializable]
    public class StringBlackBoardKeySelector : TBlackBoardKeySelector<string> { }
}
