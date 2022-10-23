using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree.UtilityAI
{
    [DisplayName("Consideration")]
    [Input(Type = typeof(BTConsideration))]
    public class BTConsideration : BTNode
    {
        public float score;

        public float Score { get { return score; } set { score = Mathf.Clamp01(value); } }

        [SerializeField] private AnimationCurve ResponseCurve;

        public virtual float EvaluateScore()
        {
            return score;
        }
    }
}
