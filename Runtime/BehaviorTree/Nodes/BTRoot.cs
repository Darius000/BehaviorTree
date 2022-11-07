using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [DisplayName("Root")]
    [NodeIcon("Root")]
    public class BTRoot : BTNode
    {
        [Output]
        [HideInInspector] public List<BTNode> m_Children = new List<BTNode>();

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            m_Children.ForEach(child => child.Execute(agent, controller));

            return EResult.Running; 
        }

        protected override void OnRemoveChild(BTNode node)
        {
            base.OnRemoveChild(node);

            m_Children.Remove(node);
        }

        protected override void OnAddChild(BTNode node)
        {
            base.OnAddChild(node);

            m_Children.Add(node);
        }

        public override IDictionary<int, IEnumerable<BTNode>> GetChildren()
        {
            return new Dictionary<int, IEnumerable<BTNode>> { { 0, m_Children} };
        }

        public override BTNode Clone()
        {
            BTRoot node = Instantiate(this);
            node.m_Children = m_Children.ConvertAll(child => child.Clone());
            return node;
        }
    }
}