using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [Input(Type = typeof(UtilityAction))]
    public class UtilityAction : UtilityNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(UtilityConsideration))]
        [HideInInspector]
        public List<UtilityConsideration> Considerations = new List<UtilityConsideration>();

        [Output(Type = typeof(BTNode))]
        public BTNode Action;

        protected override void OnAddChild(BTNode node)
        {
            base.OnAddChild(node);

            if (node is UtilityConsideration utilityConsideration)
            {
                Considerations.Add(utilityConsideration);
            }
            else
            {
                Action = node;
            }
        }

        protected override void OnRemoveChild(BTNode node)
        {
            base.OnRemoveChild(node);

            if (node is UtilityConsideration utilityConsideration)
            {
                Considerations.Remove(utilityConsideration);
            }
            else if (Action == node)
            {
                Action = null;
            }
        }

        public override IEnumerable<BTNode> GetChildren()
        {
           var children = new List<BTNode>() { Action };
           children.AddRange(Considerations);
           return children;
        }

        public override int GetChildIndex(BTNode b)
        {
            if(b is UtilityConsideration utilityConsideration)
            {
                if (Considerations.Contains(utilityConsideration)) return 0;
            }
            else if(Action == b)
            {
                return 1;
            }

            return -1;
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

        public virtual Vector3 GetRequiredLocation()
        {
            return Vector3.zero;
        }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if(Action)
            {
                return Action.Execute(agent, controller);
            }

            return EResult.Success;
        }
    }
}
