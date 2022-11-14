using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    public class UtilityAction_Ex : UtilityAction
    {
        [SerializeReference]
        public VectorBlackBoardKeySelector RequiredLocation = new VectorBlackBoardKeySelector();

        public override Vector3 GetRequiredLocation(NavMeshAgent agent, AIController controller)
        {
            if(Tree && RequiredLocation.IsValid())
            {
                return Tree.GetBlackBoard().GetKey(RequiredLocation.GetName()).GetValue<Vector3>();
            }
            return Vector3.zero;
        }
    }
}
