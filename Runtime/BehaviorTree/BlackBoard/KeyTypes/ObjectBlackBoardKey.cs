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
    public class ObjectBlackBoardKey : TBlackBoardKeyType<UnityEngine.Object>
    {
        protected override void OnDrawObjectDebugInfo()
        {
            var text = GetObjectValue() == null ? "null" : GetValue<UnityEngine.Object>().name; 
            GizmoUtils.DrawString(text, Color.yellow);
        }
    }
}
