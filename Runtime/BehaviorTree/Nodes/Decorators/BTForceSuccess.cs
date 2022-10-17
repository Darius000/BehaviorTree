using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    [DisplayName("Force Success")]
    public class BTForceSuccess : BTDecorator
    {
        protected override EResult OnExecute()
        {
            Child.Execute();
            return EResult.Success;
        }
    }
}
