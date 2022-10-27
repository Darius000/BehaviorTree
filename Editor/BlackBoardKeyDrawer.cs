//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UIElements;


//namespace AIBehaviorTree
//{
//    [CustomPropertyDrawer(typeof(BlackBoardKey), true)]
//    internal class BlackBoardKeyDrawer : PropertyDrawer
//    {
//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            return EditorGUIUtility.singleLineHeight * 3 + 4;
//        }

//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);

//            var key = property.managedReferenceValue as BlackBoardKey;

//            var textrect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

//            key.m_KeyName = EditorGUI.TextField(textrect, "Key Name", key.m_KeyName);

//            textrect.y += EditorGUIUtility.singleLineHeight + 2;
//            key.m_Description = EditorGUI.TextField(textrect, new GUIContent("Description"), key.m_Description);

//            textrect.y += EditorGUIUtility.singleLineHeight + 2;
//            key.m_IsInstance = EditorGUI.Toggle(textrect, new GUIContent("Instance"), key.m_IsInstance);

//            EditorGUI.EndProperty();
//        }
//    }
//}
