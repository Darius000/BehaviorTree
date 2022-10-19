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


        protected override void OnBeginExecute()
        {
            if (m_BlackBoardKeySelector == null) return;

            var target = GetBlackBoard().GetKey(m_BlackBoardKeySelector.GetName()).GetObjectValue();

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

                m_DestinationSet = m_Agent.SetDestination(targetPosition);

            }
        }

        protected override EResult OnExecute()
        {
            if (m_DestinationSet)
            {
                float distance = m_IncludeAgentRadius ? m_Agent.radius + .1f : .1f;
                if (m_Agent.remainingDistance > distance)
                {
                    switch (m_Agent.pathStatus)
                    {
                        case NavMeshPathStatus.PathComplete: return EResult.Running;
                        case NavMeshPathStatus.PathPartial: { m_Agent.SetDestination(targetPosition); return EResult.Running; }
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