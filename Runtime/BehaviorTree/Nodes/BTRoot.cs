using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [DisplayName("Root")]
    [NodeIcon("Root")]
    public class BTRoot : BTNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(BTNode))]
        [HideInInspector] public List<BTNode> m_Children = new List<BTNode>();

        protected override void OnBeginExecute()
        {
            
        }

        protected override EResult OnExecute()
        {
            m_Children.ForEach(child => child.Execute());

            return EResult.Running; 
        }

        public override bool AddChild(BTNode node)
        {
            if(!m_Children.Contains(node))
            {
                m_Children.Add(node);
            }
            return true;
        }

        public override void RemoveChild(BTNode node)
        {
            m_Children.Remove(node);
        }

        public override IEnumerable<BTNode> GetChildren()
        {
            return m_Children;
        }

        public override BTNode Clone()
        {
            BTRoot node = Instantiate(this);
            node.m_Children = m_Children.ConvertAll(child => child.Clone());
            return node;
        }
    }
}