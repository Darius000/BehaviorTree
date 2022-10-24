
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [DisplayName("Wait")]
    [NodeIcon("Wait")]
    public class BTWait : BTTaskNode
    {

        public float m_Duration = 1;

        private float m_StartTime;

        protected override void OnBeginExecute(NavMeshAgent agent)
        {
            base.OnBeginExecute(agent);

            m_StartTime = Time.time;
        }

        protected override EResult OnExecute(NavMeshAgent agent)
        {
            if (Time.time - m_StartTime > m_Duration)
            {
                return EResult.Success;
            }

            return EResult.Running;
        }

    }
}