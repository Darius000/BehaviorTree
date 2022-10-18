using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    [DisplayName("Simple Parallel")]
    public class BTSimpleParallel : BTComposite
    {
        private int m_CurrentNode = 1;

        public enum EFinishMode
        {
            Immediate,
            Delayed
        }

        public EFinishMode FinishMode;

        protected override void OnBeginExecute()
        {
            m_CurrentNode = 1;
        }

        protected override EResult OnExecute()
        {
            if (m_Children.Count == 0) return EResult.Success;

            var main_task = RunMainTaskAsync();


            if(m_CurrentNode == m_Children.Count) return EResult.Success;

            var child = m_Children[m_CurrentNode];
            switch (child.Execute())
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

        private async Task<EResult> RunMainTaskAsync()
        {
            return m_Children[0].Execute();
        }
    }
}
