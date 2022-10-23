using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace AIBehaviorTree
{
    public class NodeView : NodeElementView
    {
        //public Port m_Input;
        //public Port m_Output;
        public Image BreakPointIcon = new Image() { style = { position = Position.Absolute } };

        public NodeView(BTNode node, Action<NodeElementView> OnSelectedCallback = null) : base(node, OnSelectedCallback, 
            AssetDatabase.GetAssetPath(BehaviorTreeSettings.GetOrCreateSettings().m_NodeUXML))
        {
            style.left = node.m_Position.x;
            style.top = node.m_Position.y;
            
            //CreateInputPorts();
            //CreateOutputPorts();

            var error_icon = EditorGUIUtility.IconContent("console.erroricon").image;
            BreakPointIcon.image = error_icon;
            BreakPointIcon.style.left = -error_icon.width / 2.0f;
            BreakPointIcon.style.top = -error_icon.height / 2.0f;
            BreakPointIcon.style.visibility = Visibility.Hidden;

            base.hierarchy.Add(BreakPointIcon);

            //initilaize breakpoint visibility
            ToggleBreakPoint(node.m_BreakPoint);
            node.OnBreakPointSet += ToggleBreakPoint;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            var breakpoint_text = m_Node.m_BreakPoint ?  "Remove Breakpoint" : "Add Breakpoint";
            evt.menu.AppendAction(breakpoint_text, (action) => { m_Node.ToggleBreakPoint(); });
        }

        private void ToggleBreakPoint(bool hasbreakpoint)
        {
            BreakPointIcon.style.visibility = hasbreakpoint ? Visibility.Visible : Visibility.Hidden;
        }

        //private void CreateInputPorts()
        //{
        //    if (m_Node is BTRoot)
        //    {
        //        //no inputs are needed for the root node
        //    }
        //    else
        //    {
        //        m_Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                
        //    }

        //    if (m_Input != null)
        //    {
        //        m_Input.AddManipulator(new EdgeConnector<Edge>(new EdgeConnectorListener()));
                
        //        m_Input.portName = "";
        //        m_Input.style.flexDirection = FlexDirection.Column;
        //        inputContainer.Add(m_Input);
        //    }

        //}

        //private void CreateOutputPorts()
        //{
        //    if (m_Node is BTComposite)
        //    {
        //        m_Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        //    }
        //    else if (m_Node is BTDecorator)
        //    {
        //        m_Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        //    }
        //    else if (m_Node is BTRoot)
        //    {
        //        m_Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        //    }



        //    if (m_Output != null)
        //    {
        //        m_Output.AddManipulator(new EdgeConnector<Edge>(new EdgeConnectorListener()));
        //        m_Output.portName = "";
        //        m_Output.style.flexDirection = FlexDirection.ColumnReverse;
        //        outputContainer.Add(m_Output);
        //    }
        //}

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            m_Node.SetPosition(newPos.min);
        }

        //sorts children in the composite node when moved in graph
        public override void SortChildren()
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


        //public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
        //{
        //    return BTPort.Create<Edge>(orientation, direction, capacity, type);
        //}
    }
}