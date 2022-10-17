using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;



namespace AIBehaviorTree
{
   

    [DisplayName("Generate Random Location")]
    public class BTGenerateRandomLocation : BTTaskNode
    {
        [SerializeReference]
        public VectorBlackBoardKeySelector m_BlackboardKeySelector = new VectorBlackBoardKeySelector();

        public float m_WalkRadius = 100;

        public Vector3 m_Constraints = new Vector3(1, 1, 1);

        protected override EResult OnExecute()
        {
            Vector3 random = UnityEngine.Random.insideUnitSphere;
            Vector3 randomDirection = Vector3.Scale(random, m_Constraints.normalized) * m_WalkRadius;
            NavMeshHit hit;
            
            if(NavMesh.SamplePosition(randomDirection, out hit, m_WalkRadius, 1))
            {
                if(m_BlackboardKeySelector != null)
                {
                    GetBlackBoard().SetKeyValue(m_BlackboardKeySelector.GetName(), hit.position);

                     return EResult.Success;
                }

               
            }
           

            return EResult.Failure;
        }

      
    }
}
