using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    /// <summary>
    /// Triggers an animation on an animator controller
    /// </summary>
    [DisplayName("Trigger Animation")]
    public class BTTriggerAnimation : BTTaskNode
    {
        public string Trigger;
        private Animator Animator;

        protected override void OnBeginExecute(NavMeshAgent agent)
        {
            if(!Animator)
            {
                Animator = agent.GetComponent<Animator>();
            }
        }

        protected override EResult OnExecute(NavMeshAgent agent)
        {
            if (!Animator) return EResult.Failure;

            Animator.SetTrigger(Trigger);

            return EResult.Success;
        }
    }
}
