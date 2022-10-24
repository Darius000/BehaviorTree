using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace AIBehaviorTree
{
    [CustomEditor(typeof(BTBlackBoardBasedCondition))]
    public class BlackBoardBasedConditionEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            if (!target) return;

            //base.OnInspectorGUI();

            BTBlackBoardBasedCondition node = (BTBlackBoardBasedCondition)target;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.PrefixLabel(new GUIContent("Description"));
            node.m_Description = EditorGUILayout.TextArea(node.m_Description, GUILayout.Height(45));

            EditorGUILayout.LabelField(new GUIContent("Operation"));

            if (EditorGUILayout.PropertyField(serializedObject.FindProperty("m_BlackboardKeySelector")))
            {
                serializedObject.ApplyModifiedProperties();
            }

            var bb = node?.Tree?.GetBlackBoard();
            Type type = null;

            if (bb != null)
            {
                var key = bb.GetKey(node.m_BlackboardKeySelector.GetName());
                if (key != null)
                {
                    type = key.GetKeyType();
                }
            }

            if (type != null)
            {
                node.m_OperationType = GetOperationFromType(type);
            }

            if (node.m_OperationType == EOperationType.Basic)
            {
                node.m_BasicKeyOperation = (EKeyOperation)EditorGUILayout.EnumPopup(node.m_BasicKeyOperation);
            }
            else if (node.m_OperationType == EOperationType.Arithmetic)
            {
                node.m_ArithmeticOperation = (EArithmeticOperation)EditorGUILayout.EnumPopup(node.m_ArithmeticOperation);
                node.SetCurrentValue(DrawFieldForValue(node.GetCurrentValue()));
            }
            else if (node.m_OperationType == EOperationType.Text)
            {
                node.m_TextOperation = (ETextOperation)EditorGUILayout.EnumPopup(node.m_TextOperation);
                node.SetCurrentValue(DrawFieldForValue(node.GetCurrentValue()));
            }

            //if (EditorGUILayout.PropertyField(serializedObject.FindProperty("m_EvaluateCondition")))
            //{
            //    serializedObject.ApplyModifiedProperties();
            //}

            EditorGUILayout.EndVertical();
        }

        private EOperationType GetOperationFromType(Type type)
        {
            if (type == typeof(string))
            {
                return EOperationType.Text;
            }

            if (type == typeof(int) || type == typeof(float) || type == typeof(double))
            {
                return EOperationType.Arithmetic;
            }

            return EOperationType.Basic;
        }

        private object DrawFieldForValue(object value)
        {
            var type = value.GetType();
            switch (type)
            {
                case Type intT when type == typeof(int):
                    return EditorGUILayout.IntField((int)value);
                case Type floatT when type == typeof(float):
                    return EditorGUILayout.FloatField((float)value);
                case Type doubleT when type == typeof(double):
                    return EditorGUILayout.DoubleField((double)value);
                case Type stringType when type == typeof(string):
                    return EditorGUILayout.TextField((string)value);
                default:
                    break;
            }

            return 0;
        }
    }
}