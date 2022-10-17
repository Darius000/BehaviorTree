using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    [DisplayName("Repeat")]
    public class BTRepeat : BTDecorator
    {
        public int NumLoops;

        public bool InfiniteLoop = true;

        private int CurrentLoop = 0;

        protected override void OnBeginExecute()
        {
            base.OnBeginExecute();

            CurrentLoop = 0;
        }

        protected override EResult OnExecute()
        {
            
            if (!InfiniteLoop)
            {
                if(CurrentLoop == NumLoops)
                {
                    return EResult.Success;
                }
                CurrentLoop++;
            }

            Child.Execute();


            return EResult.Running;
        }
    }
}
