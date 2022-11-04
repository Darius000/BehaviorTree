using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [DisplayName("Move To Location")]
    [NodeIcon("MoveTo")]
    public class BTMoveToLocation : BTTaskNode
    {
        public bool m_IncludeAgentRadius = true;
    
        private Vector3 targetPosition;

        private bool m_DestinationSet;

        public Vector3 m_TargetPosition { get { return targetPosition; } private set { } }

        [SerializeReference]
        public BlackBoardKeySelector m_BlackBoardKeySelector = new BlackBoardKeySelector();


        protected override void OnBeginExecute(NavMeshAgent agent, AIController controller)
        {
            if (m_BlackBoardKeySelector == null) return;

            var target = Tree.GetBlackBoard().GetKey(m_BlackBoardKeySelector.GetName()).GetObjectValue();

            if (target != null)
            {
                if (target is GameObject)
                {
                    targetPosition = ((GameObject)target).transform.position;
                }
                else if (target is Vector3)
                {
                    targetPosition = (Vector3)target;
                }

                m_DestinationSet = agent.SetDestination(targetPosition);

            }
        }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if (m_DestinationSet)
            {
                float distance = m_IncludeAgentRadius ? agent.radius + .1f : .1f;
                if (agent.remainingDistance > distance)
                {
                    switch (agent.pathStatus)
                    {
                        case NavMeshPathStatus.PathComplete: return EResult.Running;
                        case NavMeshPathStatus.PathPartial: { agent.SetDestination(targetPosition); return EResult.Running; }
                        case NavMeshPathStatus.PathInvalid:
                            return EResult.Failure;
                    }
                }
                else
                {
                    return EResult.Success;
                }
            }
 

            return EResult.Failure;
        }

    }
}