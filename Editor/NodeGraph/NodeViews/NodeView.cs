using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using System.Runtime.InteropServices;

namespace AIBehaviorTree
{
    public class NodeView : Node
    {
        public BTNode m_Node;


        private SerializedObject m_SerializedObject;

        public Action<NodeView> OnSelectedEvent;

        public Image BreakPointIcon = new Image() { style = { position = Position.Absolute } };


        public List<Port> Inputs { get; set; } = new List<Port>();

        public List<Port> Outputs { get; set; } = new List<Port>();

        public NodeView(BTNode node, Action<NodeView> OnSelectedCallback = null) 
            : base(AssetDatabase.GetAssetPath(BehaviorTreeSettings.GetOrCreateSettings().m_NodeUXML))
        {
            m_Node = node;

            style.left = node.m_Position.x;
            style.top = node.m_Position.y;

            CreatePins(node);
            CreateBreakPoint();

            //set serailized object and bind to this visual element
            m_SerializedObject = new SerializedObject(node);
            this.Bind(m_SerializedObject);

            SetNodeIcon(node);
            SetTitle(node);
            
            viewDataKey = node.m_GUID;

            OnSelectedEvent = OnSelectedCallback;

            node.OnCompletedEvent = OnNodeCompleteExecution;

            SetUpClasses();
            SetUpDescription(node);

            capabilities |= ~UnityEditor.Experimental.GraphView.Capabilities.Copiable | UnityEditor.Experimental.GraphView.Capabilities.Renamable |
                ~UnityEditor.Experimental.GraphView.Capabilities.Resizable;
        }

        private void SetNodeIcon(BTNode node)
        {
            var icon = this.Q<VisualElement>("Icon");
            if (icon != null)
            {
                string path = Utils.BehaviorTreeUtils.GetIcon(node.GetType());
                if(path != string.Empty)
                {
                    icon.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>(path));
                    icon.style.display = DisplayStyle.Flex;
                }
            }
        }

        private void SetTitle(BTNode node)
        {
            title = node.m_DisplayName;
        }

        //creates the node pins from attributes
        private void CreatePins(BTNode node)
        {
            var type = node.GetType();

            //create inputs from attributes
            ParseAttribute<InputAttribute>(type, Direction.Input, inputContainer, Inputs);

            //create outputs from attibutes
            ParseAttribute<OutputAttribute>(type, Direction.Output, outputContainer, Outputs);
        }

        private void CreateBreakPoint()
        {
            var error_icon = EditorGUIUtility.IconContent("console.erroricon").image;
            BreakPointIcon.image = error_icon;
            BreakPointIcon.style.left = -error_icon.width / 2.0f;
            BreakPointIcon.style.top = -error_icon.height / 2.0f;
            BreakPointIcon.style.visibility = Visibility.Hidden;

            base.hierarchy.Add(BreakPointIcon);

            //initilaize breakpoint visibility
            ToggleBreakPoint(m_Node.m_BreakPoint);
            m_Node.OnBreakPointSet += ToggleBreakPoint;
        }

        private void ParseAttribute<T>(Type type, Direction direction, VisualElement container, List<Port> ports) where T : PinAttribute
        {
            //get attributes
            var classAttribute = type.GetCustomAttribute<T>();
            var fields = type.GetFields();
            var properties = type.GetProperties();
            Port.Capacity capacity = Port.Capacity.Single;

            if (classAttribute != null)
            {
                var classinput = InstantiatePort(Orientation.Vertical, direction, Port.Capacity.Single, type);
                container.Add(classinput);
                ports.Add(classinput);
                
            }

            foreach (var field in fields)
            {
                var fieldAttribute = field.GetCustomAttribute<T>();
                if(fieldAttribute != null)
                {
                    
                    var fieldType = ParseType(field.FieldType, out capacity);
                    var fieldinput = InstantiatePort(Orientation.Vertical, direction, capacity, fieldType);
                    
                    container.Add(fieldinput);
                    ports.Add(fieldinput);

                }
            }

            foreach(var property in properties)
            {
                var propertyAttribute = property.GetCustomAttribute<T>();
                if(propertyAttribute != null)
                {
                    var propertyType = ParseType(property.PropertyType, out capacity);
                    var propertyInput = InstantiatePort(Orientation.Vertical, direction, capacity, propertyType);
                    container.Add(propertyInput);
                    ports.Add(propertyInput);
                }
            }
        }

        public Edge ConnectEdgeToChildren(NodeView childView, int index)
        {
            if(childView == null) return null;
            if(childView.Inputs.Count == 0 || Outputs.Count == 0) return null;
            return this.Outputs[index].ConnectTo(childView.Inputs[0]);
        }

        private void ToggleBreakPoint(bool hasbreakpoint)
        {
            BreakPointIcon.style.visibility = hasbreakpoint ? Visibility.Visible : Visibility.Hidden;
        }

        private Type ParseType(Type type, out Port.Capacity capacity)
        {
            bool IsArray = type.GetInterfaces().Contains(typeof(IEnumerable));
            capacity = IsArray ? Port.Capacity.Multi : Port.Capacity.Single;
            return type;
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
            var breakpoint_text = m_Node.m_BreakPoint ? "Remove Breakpoint" : "Add Breakpoint";
            evt.menu.AppendAction(breakpoint_text, (action) => { m_Node.ToggleBreakPoint(); });
        }

        public override void OnSelected()
        {
            base.OnSelected();

            if (OnSelectedEvent != null)
            {
                OnSelectedEvent.Invoke(this);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            //store position in node
            m_Node.SetPosition(newPos.min);
        }

        //sorts children in the composite node when moved in graph
        public void SortChildren()
        {
            BTComposite composite = m_Node as BTComposite;
            if (composite)
            {
                composite.m_Children.Sort(SortByHorizontalPosition);
            }
        }

        //sort nodes from left to right
        private int SortByHorizontalPosition(BTNode lhs, BTNode rhs)
        {
            return lhs.m_Position.x < rhs.m_Position.x ? -1 : 1;
        }
    }
}