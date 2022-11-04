using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    /// <summary>
    /// Plays a legacy Animation
    /// </summary>
    [DisplayName("Play Animation")]
    public class BTPlayAnimation : BTTaskNode
    {
        public AnimationClip AnimationClip;
        private Animation Animation;
        private float ElaspedTime = 0.0f;

        protected override void OnBeginExecute(NavMeshAgent agent, AIController controller)
        {
            if (!Animation)
            {
                Animation = controller.GetComponent<Animation>();
            }

            if(Animation.clip != AnimationClip)
            {
                Animation.clip = AnimationClip;
                
            }

        }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if (!Animation) return EResult.Failure;

            

            if(!Animation.isPlaying)
            {
                Animation.Play();
            }
            
            if(Animation.isPlaying)
            {

                ElaspedTime += Time.deltaTime;
                if (AnimationClip.length > ElaspedTime)
                {
                    return EResult.Running;  
                }

                Animation.Stop();
                ElaspedTime = 0.0f;
            }

            return EResult.Success;
        }
    }
}
