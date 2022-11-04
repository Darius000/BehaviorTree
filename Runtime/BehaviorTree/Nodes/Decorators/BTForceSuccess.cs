using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [DisplayName("Force Success")]
    public class BTForceSuccess : BTDecorator
    {
        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            Child.Execute(agent, controller);
            return EResult.Success;
        }
    }
}
