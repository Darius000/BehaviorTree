using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    /// <summary>
    /// Keeps executing unitl a child returns success
    /// </summary>
    [DisplayName("Fallback")]
    public class BTFallback : BTComposite
    {
        private int m_CurrentNode = 0;
        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if (m_Children.Count == 0) return EResult.Success;

            var child = m_Children[m_CurrentNode];
            switch (child.Execute(agent, controller))
            {
                case EResult.Running:
                    return EResult.Running;
                case EResult.Failure:
                    m_CurrentNode++;
                    break;
                case EResult.Success:
                    return EResult.Success;
            }

            return m_CurrentNode == m_Children.Count ? EResult.Success : EResult.Running;
        }
    }
}
