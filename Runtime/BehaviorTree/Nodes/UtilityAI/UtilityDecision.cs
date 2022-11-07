using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

namespace AIBehaviorTree
{

    [Input]
    [Category("Utility AI")]
    public class UtilityDecision : BTNode
    {
        [Output]
        [HideInInspector]
        public List<UtilityAction> Actions = new List<UtilityAction>();

        private EState CurrentState = EState.Decide;

        private UtilityAction BestAction = null;

        protected override void OnAddChild(BTNode node)
        {
            base.OnAddChild(node);
            if(node is UtilityAction utilityAction)
            {
                Actions.Add(utilityAction);
            }
        }

        protected override void OnRemoveChild(BTNode node)
        {
            base.OnRemoveChild(node);

            if (node is UtilityAction utilityAction)
            {
                if (Actions.Contains(utilityAction))
                {
                    Actions.Remove(utilityAction);
                }
            }
        }

        private void OnActionCompleted()
        {
            CurrentState = EState.Decide;
        }

        public override IDictionary<int, IEnumerable<BTNode>> GetChildren()
        {
            return new Dictionary<int, IEnumerable<BTNode>> { { 0, Actions} };
        }

        protected override void OnBeginExecute(NavMeshAgent agent, AIController controller)
        {
            //CurrentState = EState.Decide;
        }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if (Actions.Count == 0) return EResult.Failure;

            float distance = agent.radius + .1f;
            if (CurrentState == EState.Decide)
            {
                BestAction = DecideBestAction(controller);
                if(BestAction)
                {
                    agent.SetDestination(BestAction.GetRequiredLocation(agent, controller));
                    CurrentState = EState.Move;
                }

            }
            else if(CurrentState == EState.Move)
            {
                if (agent.remainingDistance < agent.radius)
                {
                    CurrentState = EState.Execute;
                }
               
            }
            else if(CurrentState == EState.Execute)
            {
                BestAction.OnFinishedEvent = OnActionCompleted;
                return BestAction.Execute(agent, controller);
            }
            

            return EResult.Running;
        }

        private UtilityAction DecideBestAction(AIController controller)
        {
            float score = 0f;
            int bestActionIndex = 0;

            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].EvaluateScore(controller) > score)
                {
                    bestActionIndex = i;
                    score = Actions[i].Score;
                }
            }

            return Actions[bestActionIndex];
        }

        private enum EState
        {
            Decide,
            Move,
            Execute
        }
    }
}
