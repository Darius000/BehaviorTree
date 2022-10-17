using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [DisplayName("Fallback")]
    public class BTFallback : BTComposite
    {
        private int m_CurrentNode = 0;
        protected override EResult OnExecute()
        {
            if (m_Children.Count == 0) return EResult.Success;

            var child = m_Children[m_CurrentNode];
            switch (child.Execute())
            {
                case EResult.Running:
                    return EResult.Running;
                case EResult.Failure:
                    m_CurrentNode++;
                    break;
                case EResult.Success:
                    return EResult.Success;
            }

            return m_CurrentNode == m_Children.Count ? EResult.Success : EResult.Running;
        }
    }
}
