using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [Input]
    public class UtilityAction : UtilityNode
    {
        [Output]
        [HideInInspector]
        public List<UtilityConsideration> Considerations = new List<UtilityConsideration>();

        [Output]
        [HideInInspector]
        public BTNode ActionToExecute;

        public Action OnFinishedEvent;

        protected override void OnAddChild(BTNode node)
        {
            base.OnAddChild(node);

            if(node is UtilityConsideration consideration)
            {
                Considerations.Add(consideration);
            }
            else
            {
                ActionToExecute = node;
            }
        }

        protected override void OnRemoveChild(BTNode node)
        {
            base.OnRemoveChild(node);

            if (node is UtilityConsideration consideration)
            {
                Considerations.Remove(consideration);
            }
            else
            {
                ActionToExecute = null;
            }
        }

        public override IDictionary<int, IEnumerable<BTNode>> GetChildren()
        {
           return new Dictionary<int, IEnumerable<BTNode>> { {0, Considerations } , { 1, new BTNode[] { ActionToExecute } } };
        }

        public override int GetChildIndex(BTNode node)
        {
            if(node is UtilityConsideration)
            {
                return 0;
            }

            return 1;
        }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            OnExecuteAction(agent, controller);

            if (ActionToExecute)
            {
                var state = ActionToExecute.Execute(agent, controller);
                OnFinishedEvent?.Invoke();
                return state;
            }

            OnFinishedEvent?.Invoke();

            return EResult.Success;
        }

        protected virtual void OnExecuteAction(NavMeshAgent agent, AIController controller)
        {

        }

        public override float EvaluateScore(AIController controller)
        {          
            if (Considerations.Count == 0)
            {
                return Score;
            }

            float score = 1f;
            foreach (var consideration in Considerations)
            {
                float considerationScore = consideration.EvaluateScore(controller);
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

        public virtual Vector3 GetRequiredLocation(NavMeshAgent agent, AIController controller)
        {
            return Vector3.zero;
        }
    }
}
