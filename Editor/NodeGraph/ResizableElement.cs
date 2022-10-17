using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace AIBehaviorTree
{
    public class ResizableVisualElement : ResizableElement
    {
        public new class UxmlFactory : UxmlFactory<ResizableVisualElement, ResizableElement.UxmlTraits> { }

    }
}