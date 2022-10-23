using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree.UtilityAI
{
    [DisplayName("Action")]
    [Input(Type = typeof(BTActionSelector))]
    public class BTAction : BTNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(BTConsideration))]
        [HideInInspector] public List<BTConsideration> Considerations;

        public float score;

        public float Score
        {
            get { return score; }
            set { score = Mathf.Clamp01(value); }
        }

        public float EvaluateScore()
        {
            float score = 1f;

            for (int i = 0; i < Considerations.Count; i++)
            {
                float considerationScore = Considerations[i].EvaluateScore();
                score *= considerationScore;

                if (score == 0)
                {
                    Score = 0;
                    return Score;//no point in computing further
                }
            }

            if (Considerations.Count == 0) return Score = 1;

            //Average scheme of overall score
            float originalScore = score;
            float modFactor = 1 - (1 / (Considerations.Count));
            float makeUpValue = (1 - originalScore) * modFactor;
            Score = originalScore + (makeUpValue * originalScore);

            Debug.Log(GetDisplayName() + ", Scored : " + Score);

            return Score;
        }

        public override bool AddChild(BTNode node)
        {
            if (node is BTConsideration consideration)
            {
                if (!Considerations.Contains(consideration))
                {
                    Considerations.Add(consideration);

                    EvaluateScore();

                    return true;
                }

                return false;
            }

            return false;
        }

        public override void RemoveChild(BTNode node)
        {
            if (node is BTConsideration consideration)
            {
                if (Considerations.Contains(consideration))
                {
                    Considerations.Remove(consideration);

                    EvaluateScore();
                }
            }
        }

        public override IEnumerable<BTNode> GetChildren()
        {
            return Considerations;
        }
    }
}
