using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

namespace AIBehaviorTree
{
    public class NodeElementView : UnityEditor.Experimental.GraphView.Node
    {
        internal BTNode m_Node;


        public Action<NodeElementView> OnSelectedEvent;

        protected VisualElement m_Content { get; private set; }


        public NodeElementView(BTNode node, Action<NodeElementView> OnSelectedCallback = null, string uxml = default) : base(uxml)
        {
            m_Node = node;

            string iconPath = "";
            
            //find icon attribute and set node icon
            var iconattributes = node.GetType().GetCustomAttributes(typeof(NodeIconAttribute), true);
            if (iconattributes.Length > 0)
            {
                var iconAttribute = iconattributes[0] as NodeIconAttribute;
                iconPath = iconAttribute.IconPath;

                var icon = this.Q<VisualElement>("Icon");
                if (icon != null)
                {
                    icon.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>(iconPath));
                    icon.style.display = DisplayStyle.Flex;
                }
            }

            title = node.GetDisplayName();
            viewDataKey = node.m_GUID;

            OnSelectedEvent = OnSelectedCallback;

            node.OnCompletedEvent = OnNodeCompleteExecution;

            SetUpClasses();
            SetUpDescription(node);

            capabilities |= ~UnityEditor.Experimental.GraphView.Capabilities.Copiable | UnityEditor.Experimental.GraphView.Capabilities.Renamable |
                ~UnityEditor.Experimental.GraphView.Capabilities.Resizable;
        }

        //set to false to allow rectangle selection
        public override bool IsStackable()
        {
            return false;
        }

        public override bool IsSelectable()
        {
            return true;
        }

        public override bool IsCopiable()
        {
            return true;
        }

        public override bool IsResizable()
        {
            return false;
        }

        //delete node using menu
        protected void OnDeleteNodeMenu()
        {
            m_Node.Delete();

            RemoveFromHierarchy();
        }
        private void OnNodeCompleteExecution(EResult result)
        {
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");

            if (Application.isPlaying)
            {
                switch (result)
                {
                    case EResult.Running:
                        if (m_Node.m_BeganExecution)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case EResult.Failure:
                        AddToClassList("failure");
                        break;
                    case EResult.Success:
                        AddToClassList("success");
                        break;
                }
            }
        }


        private void SetUpClasses()
        {

            if (m_Node is BTComposite)
            {
                AddToClassList("composite");
            }
            else if (m_Node is BTDecorator)
            {
                AddToClassList("decorator");
            }
            else if (m_Node is BTRoot)
            {
                AddToClassList("root");
            }
            else
            {
                AddToClassList("leaf");
            }
        }

        private void SetUpDescription(BTNode node)
        {
            Label descriptionLabel = this.Q<Label>("description");

            if (descriptionLabel != null)
            {
                descriptionLabel.bindingPath = "m_Description";
                descriptionLabel.Bind(new SerializedObject(node));
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);

            //if(evt.target is NodeElementView)
            //{
            //    evt.menu.AppendAction("Delete", (a) => OnDeleteNodeMenu());
            //    evt.menu.AppendAction("Copy", (a) => { });
            //    evt.menu.AppendAction("Duplicate", (a) => { });
            //}
        }

        public override void OnSelected()
        {
            base.OnSelected();

            if (OnSelectedEvent != null)
            {
                OnSelectedEvent.Invoke(this);
            }
        }

        public virtual void SortChildren()
        {

        }
    }
}