using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    [DisplayName("Sequencer")]
    public class BTSequencer : BTComposite
    {
        private int m_CurrentNode = 0;

        protected override void OnBeginExecute()
        {
            m_CurrentNode = 0;
        }

        protected override EResult OnExecute()
        {
            if(m_Children.Count == 0) return EResult.Success;

            var child = m_Children[m_CurrentNode];
            switch(child.Execute())
            {
                case EResult.Running:
                    return EResult.Running;
                case EResult.Failure:
                    return EResult.Failure;
                case EResult.Success:
                    m_CurrentNode++;
                    break;

            }

            return m_CurrentNode == m_Children.Count ? EResult.Success : EResult.Running;
        }
    }
}