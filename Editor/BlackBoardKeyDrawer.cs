using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace AIBehaviorTree
{
    [CustomPropertyDrawer(typeof(BlackBoardKey), true)]
    internal class BlackBoardKeyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position , label, property);

            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);

            var name = serializedObject.FindProperty("m_KeyName");
            var description = serializedObject.FindProperty("m_Description");
            var instance = serializedObject.FindProperty("m_IsInstance");

            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            var textrect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            if(name != null)
                name.stringValue = EditorGUI.TextField(textrect, name.displayName, name.stringValue);

            if(description != null)
            {
                textrect.y += EditorGUIUtility.singleLineHeight + 2;
                description.stringValue = EditorGUI.TextField(textrect, new GUIContent(description.displayName), description.stringValue);
            }

            if(instance != null)
            {
                textrect.y += EditorGUIUtility.singleLineHeight + 2;
                instance.boolValue = EditorGUI.Toggle(textrect, new GUIContent(instance.displayName), instance.boolValue);

            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }
    }
}
