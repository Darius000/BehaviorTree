using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [Input(Type = typeof(UtilityAction))]
    public class UtilityAction : UtilityNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(UtilityConsideration))]
        public List<UtilityConsideration> Considerations = new List<UtilityConsideration>();

        public override bool AddChild(BTNode node)
        {
            if(node is UtilityConsideration utilityConsideration)
            {
                if(!Considerations.Contains(utilityConsideration))
                {
                    Considerations.Add(utilityConsideration);

                    return true;
                }
            }

            return false;
        }

        public override IEnumerable<BTNode> GetChildren()
        {
            return Considerations;
        }

        public override void RemoveChild(BTNode node)
        {
            if (node is UtilityConsideration utilityConsideration)
            {
                if (Considerations.Contains(utilityConsideration))
                {
                    Considerations.Remove(utilityConsideration);
                }
            }
        }

        public override float EvaluateScore()
        {          
            if (Considerations.Count == 0)
            {
                return Score;
            }

            float score = 1f;
            foreach (var consideration in Considerations)
            {
                float considerationScore = consideration.EvaluateScore();
                score *= considerationScore;

                if(score == 0)
                {
                    Score = 0;
                    return Score; //no use computing further
                }
            }

            //averaging scheme of overall score
            float originalScore = score;
            float modFactor = 1 - (1 / Considerations.Count);
            float makeUpValue = (1 - originalScore) * modFactor;
            Score = originalScore + (makeUpValue * originalScore);

            return Score;
        }
    }
}
