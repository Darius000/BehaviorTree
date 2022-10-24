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
        private static float LineHeight = 0f;
        private static float PreviousLineHeight= 0f;
        private static float OffsetX = 0f;
        private static float PreviousSizeX = 0f;
        private static Vector3 CurrentScreenPos;
        private static SceneView CurrentView;
        private static string[] Delimiters = new string[] { ",", ":" };


        public static void BeginStringGUI(Vector3 pos, bool screenPos = false)
        {
            UnityEditor.Handles.BeginGUI();

            //get current screen view to get screen pos
            CurrentView = UnityEditor.SceneView.currentDrawingSceneView;
            CurrentScreenPos = screenPos ? pos : CurrentView.camera.WorldToScreenPoint(pos);
        }

        public static void SameLine()
        {
            LineHeight = PreviousLineHeight;
            OffsetX = PreviousSizeX;
        }

        public static void DrawString(string text, Color? color = null)
        {
            
            if (color.HasValue) GUI.color = color.Value;

            
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));

            var screenPos = CurrentScreenPos;
            GUI.Label(new Rect(screenPos.x + OffsetX, -screenPos.y + CurrentView.position.height +  LineHeight , size.x, size.y), text);

            //set next line height
            PreviousLineHeight = LineHeight;
            PreviousSizeX = size.x;

            LineHeight += EditorGUIUtility.singleLineHeight;
            OffsetX = 0;
        }

        public static void DrawString(string text, Color[] colors = null)
        {
            if (text.Contains(Delimiters[0]))
            {
                var groups = Regex.Split(text, @"(?<=[,])");

                for(int j = 0; j < groups.Length; j++)
                {
                    var labels = Regex.Split(groups[j], @"(?<=[:])");

                    for (int i = 0; i < labels.Length; i++)
                    {
                        var label = labels[i];
                        DrawString(label, colors[i]);
                        if(i != labels.Length - 1) SameLine();
                    }
                }
            }
            else
            {
                var labels = Regex.Split(text, @"(?<=[:])");
                for (int i = 0; i < labels.Length; i++)
                {
                    var label = labels[i];
                    DrawString(label, colors[i]);
                    if (i != labels.Length - 1) SameLine();
                }
            }

        }

        public static void EndStringGUI()
        {
            UnityEditor.Handles.EndGUI();

            LineHeight = 0f;
            OffsetX = 0f;
            PreviousLineHeight = 0f;
            PreviousSizeX = 0f;
        }
    }
}
