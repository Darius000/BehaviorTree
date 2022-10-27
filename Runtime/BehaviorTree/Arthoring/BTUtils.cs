using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace AIBehaviorTree
{
    public static class BTUtils
    {
        public static object GetObjectFromKey(BTNode node, BlackBoardKeySelector selector)
        {
            return node.Tree?.GetBlackBoard()?.GetKey(selector.GetName())?.GetObjectValue();
        }


    }

    public static class GizmoUtils
    {
        
        private static Vector3 CurrentScreenPos;
        private static SceneView CurrentView;


        public static void DrawString(string text, Vector3 pos, Color? color = null, bool IsScreenPos = false)
        {

            UnityEditor.Handles.BeginGUI();

            //get current screen view to get screen pos
            CurrentView = UnityEditor.SceneView.currentDrawingSceneView;
            CurrentScreenPos = IsScreenPos ? pos : CurrentView.camera.WorldToScreenPoint(pos);

            if (color.HasValue) GUI.color = color.Value;

            
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));

            GUI.Label(new Rect(CurrentScreenPos.x, -CurrentScreenPos.y + CurrentView.position.height , size.x, size.y), text);

            UnityEditor.Handles.EndGUI();
        }
    }
}
