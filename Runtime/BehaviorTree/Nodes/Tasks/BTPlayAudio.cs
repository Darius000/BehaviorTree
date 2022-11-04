using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [DisplayName("Play Audio")]
    public class BTPlayAudio : BTTaskNode
    {
        
        public AudioClip Audio;

        public float Volume = 1.0f;

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if (!Audio) return EResult.Failure;

            AudioSource.PlayClipAtPoint(Audio, agent.transform.position, Volume);

            return EResult.Success;
        }
    }
}
