using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBehaviorTree
{
    public enum EEqualOperation
    {
        IsEqualTo,
        IsNotEqualTo
    }

    public enum EKeyOperation
    {
        IsSet,
        IsNotSet
    }

    public enum EArithmeticOperation
    {
        Less,
        LessThanOrEqual,
        Equal,
        GreaterThan,
        GreaterThanOrEqual,
    }

    public enum ETextOperation
    {
        Equal,
        NotEqual,
        Contain,
        NotContain
    }

    public enum EOperationType
    {
        Basic,
        Arithmetic,
        Text
    }
}
