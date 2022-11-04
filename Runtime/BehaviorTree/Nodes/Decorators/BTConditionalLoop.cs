using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [DisplayName("Conditional Loop")]
    public class BTConditionalLoop : BTDecorator
    {
        [SerializeReference]
        public BlackBoardKeySelector BlackBoardKey = new BlackBoardKeySelector();

        public EKeyOperation KeyQuery;

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            var a = BTUtils.GetObjectFromKey(this, BlackBoardKey);

            if(KeyQuery == EKeyOperation.IsSet && a != null )
            {
                Child.Execute(agent, controller);
                return EResult.Running;
            }
            else if(KeyQuery == EKeyOperation.IsNotSet && a == null)
            {
                Child.Execute(agent, controller);
                return EResult.Running;
            }

            return EResult.Failure;
        }
    }
}
