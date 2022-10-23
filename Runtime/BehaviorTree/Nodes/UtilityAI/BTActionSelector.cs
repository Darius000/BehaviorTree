using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace AIBehaviorTree.UtilityAI
{
    [DisplayName("ActionSelector")]
    public class BTActionSelector : BTNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(BTActionSelector))]
        [HideInInspector] public List<BTAction> Actions;

        protected override EResult OnExecute()
        {
            float score = 0f;

            int nextBestActionIndex = 0;
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].Score > score)
                {
                    nextBestActionIndex = i;
                    score = Actions[i].Score;
                }
            }

            if (nextBestActionIndex > Actions.Count - 1) return EResult.Failure;

            return Actions[nextBestActionIndex].Execute();
        }

        public override bool AddChild(BTNode node)
        {
            if(node is BTAction action)
            {
                if(!Actions.Contains(action))
                {
                    Actions.Add(action);

                    return true;
                }

                return false;
            }

            return false;
        }

        public override void RemoveChild(BTNode node)
        {
            if(node is BTAction action)
            {
                if (Actions.Contains(action))
                {
                    Actions.Remove(action);
                }
            }
        }

        public override IEnumerable<BTNode> GetChildren()
        {
            return Actions;
        }
    }
}
