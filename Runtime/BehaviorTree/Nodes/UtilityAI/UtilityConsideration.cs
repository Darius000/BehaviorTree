using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [Input]
    public  class UtilityConsideration : UtilityNode
    {
        public override float EvaluateScore(AIController controller)
        {
            return Score;
        }
    }
}
