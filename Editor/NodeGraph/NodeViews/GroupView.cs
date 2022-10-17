using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;

namespace AIBehaviorTree
{
    internal class GroupView : Group
    {
        private BTGroup m_Group;

        public GroupView(BTGroup group) : base()
        {
            m_Group = group;
            style.left = group.m_Position.x;
            style.top = group.m_Position.y;

            title = group.m_Title;

            capabilities |= Capabilities.Resizable | Capabilities.Renamable;
        }

        public override bool IsRenamable()
        {
            return true;
        }

        public override bool IsResizable()
        {
            return true;
        }

        protected override void OnGroupRenamed(string oldName, string newName)
        {
            base.OnGroupRenamed(oldName, newName);

            m_Group.Rename(newName);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            m_Group.SetPosition(newPos.min);
        }

        protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        {
            base.OnElementsAdded(elements);

            foreach(var element in elements)
            {
                NodeView view = element as NodeView;
                BTNode node = view.m_Node;
                m_Group.AddChildToGroup(node);
            }
        }

        protected override void OnElementsRemoved(IEnumerable<GraphElement> elements)
        {
            base.OnElementsRemoved(elements);

            foreach (var element in elements)
            {
                NodeView view = element as NodeView;
                BTNode node = view.m_Node;
                m_Group.RemoveChildFromGroup(node);
            }
        }
    }
}
