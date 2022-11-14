using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace AIBehaviorTree
{
    [CustomPropertyDrawer(typeof(BlackBoardKeySelector), true)]

    internal class BlackBoardKeySelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BlackBoardKeySelector selector = property.managedReferenceValue as BlackBoardKeySelector;

            if (selector == null) return;

            //base.OnGUI(position, property, label);

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.LabelField(position, new GUIContent(property.displayName));

            //Debug.Log(property.serializedObject.targetObject.name);

            var keyList = new List<BlackBoardKey> { };

            var node = property.serializedObject.targetObject as BTNode;

            var nameProperty = property.FindPropertyRelative("m_SelectedKeyName");
            string toolTop = "";

            BlackBoard bb = null;

            if (node != null)
            {
                BehaviorTree tree = node.Tree;

                if (tree != null)
                {
                    bb = tree.m_BlackBoard;
                    if (bb != null)
                    {

                        foreach (var key in bb.GetAllKeys())
                        {
                            if(key.GetKeyType() == selector.GetKeyType() || selector.GetKeyType() == null)
                                keyList.Add(key);
                        }

                        if(bb.ContainsKey(nameProperty.stringValue))
                        {
                            toolTop = bb.GetKey(nameProperty.stringValue).m_Description;
                        }
                    }
                }
            }


            position.x = position.width * .5f;
            position.width *= .5f;


            //Dras the drop menu with the available blackbaord keys

            if (EditorGUI.DropdownButton(position, new GUIContent(nameProperty.stringValue, toolTop), FocusType.Passive))
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("None", ""), false, () =>
                {
                    nameProperty.serializedObject.Update();
                    nameProperty.stringValue = "";
                    nameProperty.serializedObject.ApplyModifiedProperties();
                });

                foreach (var key in keyList)
                {
                    menu.AddItem(new GUIContent(key.m_KeyName, key.m_Description), false, () =>
                    {
                        nameProperty.serializedObject.Update();
                        nameProperty.stringValue = key.m_KeyName;
                        nameProperty.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.DropDown(position);
            }

            EditorGUI.EndProperty();
        }
    }
}
