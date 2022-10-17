using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [BlackBoardKeyColor(0 , 1 , 1)]
    [DisplayName("GameObject")]
    public class GameObjectBlackBoardKey : TBlackBoardKeyType<GameObject>
    {
        protected override void OnDrawObjectDebugInfo()
        {
            var text = GetObjectValue() == null ? "None" : GetValue<GameObject>().name;
            GizmoUtils.DrawString(text, Color.yellow);
        }
    }
}
