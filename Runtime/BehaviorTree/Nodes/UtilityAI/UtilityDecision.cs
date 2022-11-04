using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

namespace AIBehaviorTree
{

    [Input(Type = typeof(UtilityDecision))]
    [Category("Utility AI")]
    public class UtilityDecision : BTNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(UtilityAction))]
        [HideInInspector]
        public List<UtilityAction> Actions = new List<UtilityAction>();

        private EState CurrentState = EState.Decide;

        private UtilityAction BestAction = null;

        public override bool AddChild(BTNode node)
        {
            if(node is UtilityAction utilityAction)
            {
                if(!Actions.Contains(utilityAction))
                {
                    Actions.Add(utilityAction);

                    utilityAction.OnFinishedEvent += OnActionCompleted;

                    return true;
                }
            }

            return false;
        }

        private void OnActionCompleted()
        {
            CurrentState = EState.Decide;
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
                    utilityAction.OnFinishedEvent -= OnActionCompleted;

                    Actions.Remove(utilityAction);
                }
            }
        }

        protected override void OnBeginExecute(NavMeshAgent agent, AIController controller)
        {
            CurrentState = EState.Decide;
        }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            if (Actions.Count == 0) return EResult.Failure;

            if(CurrentState == EState.Decide)
            {
                BestAction = DecideBestAction();

                if(Vector3.Distance(BestAction.GetRequiredLocation(), agent.transform.position) < 2f)
                {
                    CurrentState = EState.Execute;
                }
                else
                {
                    CurrentState = EState.Move;
                }
            }
            else if(CurrentState == EState.Move)
            {
                if (Vector3.Distance(BestAction.GetRequiredLocation(), agent.transform.position) < 2f)
                {
                    CurrentState = EState.Execute;
                }
                else
                {
                    agent.SetDestination(BestAction.GetRequiredLocation());
                }
            }
            else if(CurrentState == EState.Execute)
            {
                return BestAction.Execute(agent, controller);
            }

            return EResult.Running;
        }

        private UtilityAction DecideBestAction()
        {
            float score = 0f;
            int bestActionIndex = 0;

            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].EvaluateScore() > score)
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
