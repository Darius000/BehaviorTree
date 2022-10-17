using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    [DisplayName("Selector")]
    internal class BTSelector : BTComposite
    {
        private int m_CurrentNode = 0;

        protected override void OnBeginExecute()
        {
            m_CurrentNode = 0;
        }

        protected override EResult OnExecute()
        {
            if (m_Children.Count == 0) return EResult.Success;

            var child = m_Children[m_CurrentNode];
            switch (child.Execute())
            {
                case EResult.Running:
                    return EResult.Running;
                case EResult.Success:
                    return EResult.Success;
                case EResult.Failure:
                    m_CurrentNode++;
                    break;

            }

            return m_CurrentNode == m_Children.Count ? EResult.Failure : EResult.Running;
        }
    }
}
