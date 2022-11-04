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

        protected override void OnBeginExecute(NavMeshAgent agent, AIController controller)
        {
            if(!Animator)
            {
                Animator = controller.GetComponent<Animator>();
            }
        }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if (!Animator) return EResult.Failure;

            Animator.SetTrigger(Trigger);

            return EResult.Success;
        }
    }
}
