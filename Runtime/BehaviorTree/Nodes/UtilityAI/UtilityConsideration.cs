using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [Input(Type = typeof(UtilityConsideration))]
    public class UtilityConsideration : UtilityNode
    {
        public override float EvaluateScore()
        {
            return 0;
        }
    }
}
