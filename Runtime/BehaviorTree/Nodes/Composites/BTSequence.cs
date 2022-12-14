using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    /// <summary>
    /// Runs though all children until a failure is returned
    /// </summary>
    [DisplayName("Sequence")]
    [NodeIcon("Sequence")]
    public class BTSequence : BTComposite
    {
        private int m_CurrentNode = 0;

        protected override void OnBeginExecute(NavMeshAgent agent, AIController controller)
        {
            m_CurrentNode = 0;
        }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if(m_Children.Count == 0) return EResult.Success;

            var child = m_Children[m_CurrentNode];
            switch(child.Execute(agent, controller))
            {
                case EResult.Running:
                    return EResult.Running;
                case EResult.Failure:
                    return EResult.Failure;
                case EResult.Success:
                    m_CurrentNode++;
                    break;

            }

            return m_CurrentNode == m_Children.Count ? EResult.Success : EResult.Running;
        }
    }
}