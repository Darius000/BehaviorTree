using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace AIBehaviorTree
{
    [Category("Utility AI")]
    public abstract class UtilityNode : BTNode
    {
        
        public float Score { get { return score; } set { score = Mathf.Clamp01(value); } }

        private float score;

        public abstract float EvaluateScore(AIController controller);
    }
}
