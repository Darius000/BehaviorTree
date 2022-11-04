using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [Input(Type = typeof(UtilityDecision))]
    [Category("Utility AI")]
    public class UtilityDecision : BTNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(UtilityAction))]
        public List<UtilityAction> Actions = new List<UtilityAction>();

        public override bool AddChild(BTNode node)
        {
            if(node is UtilityAction utilityAction)
            {
                if(!Actions.Contains(utilityAction))
                {
                    Actions.Add(utilityAction);

                    return true;
                }
            }

            return false;
        }

        public override IEnumerable<BTNode> GetChildren()
        {
            return Actions;
        }

        public override void RemoveChild(BTNode node)
        {
            if (node is UtilityAction utilityAction)
            {
                if (Actions.Contains(utilityAction))
                {
                    Actions.Remove(utilityAction);
                }
            }
        }

        protected override EResult OnExecute(NavMeshAgent agent)
        {
            if (Actions.Count == 0) return EResult.Failure;

            float score = 0f;
            int bestActionIndex = 0;

            for(int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].EvaluateScore() > score)
                {
                    bestActionIndex = i;
                    score = Actions[i].Score;
                }
            }

            return Actions[bestActionIndex].Execute(agent);
        }
    }
}
