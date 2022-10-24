using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    

    [DisplayName("CompareBBEntries")]
    [NodeIcon("CompareBlackBoard")]
    public class BTCompareBBEntries : BTDecorator
    {
        [SerializeReference]
        public BlackBoardKeySelector BlackBoardKeyA = new BlackBoardKeySelector();

        [SerializeReference]
        public BlackBoardKeySelector BlackBoardKeyB = new BlackBoardKeySelector();

        public EEqualOperation Operator;

        protected override bool PerformConditionCheck(NavMeshAgent agent, GameObject agentGameObject)
        {
            var a = Tree.GetBlackBoard().GetKey(BlackBoardKeyA.GetName());
            var b = Tree.GetBlackBoard().GetKey(BlackBoardKeyB.GetName());

            if(Operator == EEqualOperation.IsEqualTo)
            {
                if(a.GetObjectValue() == b.GetObjectValue())
                {
                    return true;
                }
            }
            else if(Operator == EEqualOperation.IsNotEqualTo)
            {
                if(a.GetObjectValue() == b.GetObjectValue())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
