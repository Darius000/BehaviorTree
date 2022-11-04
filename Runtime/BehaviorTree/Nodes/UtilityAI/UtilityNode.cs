using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace AIBehaviorTree
{
    [Category("Utility AI")]
    public abstract class UtilityNode : BTNode
    {
        [field : SerializeField]
        public float Score { get; set; }

        public abstract float EvaluateScore();
    }
}
