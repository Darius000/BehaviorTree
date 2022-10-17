using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;



namespace AIBehaviorTree
{
    [DisplayName("BlackBoard Based Condition")]
    public class BTBlackBoardBasedCondition : BTDecorator
    {

        public EOperationType m_OperationType;

        public EKeyOperation m_BasicKeyOperation;

        public EArithmeticOperation m_ArithmeticOperation;

        public ETextOperation m_TextOperation;

        [SerializeReference]
        public BlackBoardKeySelector m_BlackboardKeySelector = new BlackBoardKeySelector();

        public float m_FloatValue = 0f;

        public int m_IntValue = 0;

        public double m_DoubleValue = 0;

        public string m_StringValue = "";

        protected override bool PerformConditionCheck(NavMeshAgent agent, GameObject gameObject)
        {
            
            var obj = GetBlackBoard().GetKey(m_BlackboardKeySelector.GetName()).GetObjectValue();

            if(m_OperationType == EOperationType.Basic)
            {
                switch(m_BasicKeyOperation)
                {
                    case EKeyOperation.IsSet:
                        if (obj is not null)
                        {                         
                            return true;
                        }
                        break;
                    case EKeyOperation.IsNotSet:
                        if(obj is null)
                        {
                            return true;
                        }
                        break;
                }
            }
            else if(m_OperationType == EOperationType.Arithmetic && obj is not null)
            {
                var type = m_BlackboardKeySelector.GetType();

                switch (type)
                {
                    case Type intType when intType == typeof(int):
                        return CompareIntValues((int)obj, m_IntValue);

                    case Type floatType when floatType == typeof(float):
                        return CompareFloatValues((float)obj, m_FloatValue);

                    case Type doubleType when doubleType == typeof(double):
                        return CompareDoubleValues((double)obj, m_DoubleValue);
                }
            }
            else if(m_OperationType == EOperationType.Text)
            {
                CompareStrings((string)obj, m_StringValue);
            }


            return false;
        }

        internal bool CompareIntValues(int a, int b)
        {
            switch (m_ArithmeticOperation)
            {
                case EArithmeticOperation.Less:
                    if (a < b) { return true; }
                    break;
                case EArithmeticOperation.LessThanOrEqual:
                    if (a <= b) { return true; }
                    break;
                case EArithmeticOperation.Equal:
                    if (a == b) { return true; }
                    break;
                case EArithmeticOperation.GreaterThan:
                    if (a > b) { return true; }
                    break;
                case EArithmeticOperation.GreaterThanOrEqual:
                    if (a >= b) { return true; }
                    break;
            }

            return false;
        }
        internal bool CompareFloatValues(float a, float b)
        {
            switch (m_ArithmeticOperation)
            {
                case EArithmeticOperation.Less:
                    if (a < b) { return true; }
                    break;
                case EArithmeticOperation.LessThanOrEqual:
                    if (a <= b) { return true; }
                    break;
                case EArithmeticOperation.Equal:
                    if (a == b) { return true; }
                    break;
                case EArithmeticOperation.GreaterThan:
                    if (a > b) { return true; }
                    break;
                case EArithmeticOperation.GreaterThanOrEqual:
                    if (a >= b) { return true; }
                    break;
            }

            return false;
        }

        internal bool CompareDoubleValues(double a, double b)
        {
            switch (m_ArithmeticOperation)
            {
                case EArithmeticOperation.Less:
                    if (a < b) { return true; }
                    break;
                case EArithmeticOperation.LessThanOrEqual:
                    if (a <= b) { return true; }
                    break;
                case EArithmeticOperation.Equal:
                    if (a == b) { return true; }
                    break;
                case EArithmeticOperation.GreaterThan:
                    if (a > b) { return true; }
                    break;
                case EArithmeticOperation.GreaterThanOrEqual:
                    if (a >= b) { return true; }
                    break;
            }

            return false;
        }

        internal bool CompareStrings(string a, string b)
        {
            switch (m_TextOperation)
            {
                case ETextOperation.Equal:
                    return a == b;
                case ETextOperation.NotEqual:
                    return a != b;
                case ETextOperation.Contain:
                    return a.Contains(b);
                case ETextOperation.NotContain:
                    return !a.Contains(b);
                default: return true;
            }
        }
        public object GetCurrentValue()
        {
            var key = GetBlackBoard().GetKey(m_BlackboardKeySelector.GetName());
            var type =  key ? key.GetKeyType() : null;

            switch(type)
            {
                case Type intType when intType == typeof(int):
                    return m_IntValue;
                case Type floatType when floatType == typeof(float):
                    return m_FloatValue;
                case Type doubleType when doubleType == typeof(double):
                    return m_DoubleValue;
                case Type stringType when stringType == typeof(string):
                    return m_StringValue;
                default:
                    break;
            }

            return 0;
        }

        public void SetCurrentValue(object value)
        {
            var key = GetBlackBoard().GetKey(m_BlackboardKeySelector.GetName());
            var type =  key ? key.GetKeyType() : null;


            switch (type)
            {
                case Type intType when intType == typeof(int):
                    m_IntValue = (int)value;
                    break;
                case Type floatType when floatType == typeof(float):
                    m_FloatValue = (float)value;
                    break;
                case Type doubleType when doubleType == typeof(double):
                    m_DoubleValue = (double)value;
                    break;
                case Type stringType when stringType == typeof(string):
                    m_StringValue = (string)value;
                    break ;
                default:
                    break;
            }
        }
    }
}