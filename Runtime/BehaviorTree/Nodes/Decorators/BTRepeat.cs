using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [DisplayName("Repeat")]
    public class BTRepeat : BTDecorator
    {
        public int NumLoops;

        public bool InfiniteLoop = true;

        private int CurrentLoop = 0;

        protected override void OnBeginExecute(NavMeshAgent agent)
        {
            base.OnBeginExecute(agent);

            CurrentLoop = 0;
        }

        protected override EResult OnExecute(NavMeshAgent agent)
        {
            
            if (!InfiniteLoop)
            {
                if(CurrentLoop == NumLoops)
                {
                    return EResult.Success;
                }
                CurrentLoop++;
            }

            Child.Execute(agent);


            return EResult.Running;
        }
    }
}
