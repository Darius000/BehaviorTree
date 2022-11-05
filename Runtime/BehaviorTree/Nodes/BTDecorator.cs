using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditor;


namespace AIBehaviorTree
{
    [DisplayName("Decorator")]
    [NodeIcon("Decorator")]
    [Category("Decorators")]
    public abstract class BTDecorator : BTNode
    {
        [Output(Capacity = Capacity.Single, Type = typeof(BTNode))]
        [HideInInspector] public BTNode Child = null;

        protected override void OnAddChild(BTNode node)
        {
            base.OnAddChild(node);

            Child = node;
        }

        protected override void OnRemoveChild(BTNode node)
        {
            base.OnRemoveChild(node);

            Child = null;
        }

        public override IDictionary<int, IEnumerable<BTNode>> GetChildren()
        {
            return new Dictionary<int, IEnumerable<BTNode>>{ { 0, new List<BTNode> { Child } } };
        }

        public override BTNode Clone()
        {
            BTDecorator node = Instantiate(this);
            node.Child = Child.Clone();

            return node;
        }

        protected virtual bool PerformConditionCheck(NavMeshAgent agent, GameObject agentGameObject) { return true; }

        protected override EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            bool condition = PerformConditionCheck(agent, agent.gameObject);
            if (Child != null && condition)
            {
                return Child.Execute(agent, controller);
            }
            else if(!condition)
            {
                return EResult.Failure;
            }

            return EResult.Success;
        }

    }
}