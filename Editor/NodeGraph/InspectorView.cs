using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace AIBehaviorTree
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        Editor editor;

        public InspectorView()
        {

        }

        internal void UpdateSelection(NodeView view)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);

            if (view == null || view.m_Node == null) return;

            editor = Editor.CreateEditor(view.m_Node);

            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor.target)
                    editor.OnInspectorGUI();
            });
            Add(container);
        }
    }
}