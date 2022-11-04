using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    [DisplayName("Debug")]
    internal class BTDebug : BTTaskNode
    {
        public string m_Message = "";

        [SerializeReference]
        public BlackBoardKeySelector m_KeySelector = new BlackBoardKeySelector();

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            Debug.Log(m_Message);

            var key = Tree?.GetBlackBoard()?.GetKey(m_KeySelector.GetName());
            if (key != null)
            {
                Debug.Log(key.GetObjectValue());
            }

            return EResult.Success;
        }

       
    }
}
