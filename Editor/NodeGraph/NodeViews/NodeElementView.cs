using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using System.Runtime.InteropServices;

namespace AIBehaviorTree
{
    public class NodeElementView : Node
    {
        internal BTNode m_Node;

        private SerializedObject m_SerializedObject;

        public Action<NodeElementView> OnSelectedEvent;

        protected VisualElement m_Content { get; private set; }

        public List<Port> Inputs { get; set; } = new List<Port>();

        public List<Port> Outputs { get; set; } = new List<Port>();

        public NodeElementView(BTNode node, Action<NodeElementView> OnSelectedCallback = null, string uxml = default) : base(uxml)
        {
            m_Node = node;

            CreatePins(node);

            //set serailized object and bind to this visual element
            m_SerializedObject = new SerializedObject(node);
            this.Bind(m_SerializedObject);


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

        //creates the node pins from attributes
        private void CreatePins(BTNodeBase node)
        {
            var type = node.GetType();

            //create inputs from attributes
            ParseAttribute<InputAttribute>(type, Direction.Input, inputContainer, Inputs);

            //create outputs from attibutes
            ParseAttribute<OutputAttribute>(type, Direction.Output, outputContainer, Outputs);
        }

        private void ParseAttribute<T>(Type type, Direction direction, VisualElement container, List<Port> ports) where T : PinAttribute
        {
            //get attributes
            var classAttribute = type.GetCustomAttribute<T>();
            var fields = type.GetFields();
            var properties = type.GetProperties();

            
            if (classAttribute != null)
            {
                var classinput = InstantiatePort(Orientation.Vertical, direction, (Port.Capacity)classAttribute.Capacity, classAttribute.Type);
                container.Add(classinput);
                ports.Add(classinput);
            }

            foreach (var field in fields)
            {
                var fieldAttribute = field.GetCustomAttribute<T>();
                if(fieldAttribute != null)
                {
                    var fieldinput = InstantiatePort(Orientation.Vertical, direction, (Port.Capacity)fieldAttribute.Capacity, fieldAttribute.Type);
                    container.Add(fieldinput);
                    ports.Add(fieldinput);
                }
            }

            foreach(var property in properties)
            {
                var propertyAttribute = property.GetCustomAttribute<T>();
                if(propertyAttribute != null)
                {
                    var propertyInput = InstantiatePort(Orientation.Vertical, direction, (Port.Capacity)propertyAttribute.Capacity, propertyAttribute.Type);
                    container.Add(propertyInput);
                    ports.Add(propertyInput);
                }
            }
        }

        public Edge ConnectEdgeToChildren(NodeView childView, int index)
        {
            if(childView.Inputs.Count == 0 || Outputs.Count == 0) return null;
            return this.Outputs[index].ConnectTo(childView.Inputs[0]);
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