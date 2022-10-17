using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditor;


namespace AIBehaviorTree
{
    [DisplayName("Decorator")]
    public abstract class BTDecorator : BTNode
    {

        [HideInInspector] public BTNode Child;

        public override bool AddChild(BTNode node)
        {
            Undo.RecordObject(this, "Behavior Tree (Add Child");

            Child = node;

            EditorUtility.SetDirty(this);

            return true;
        }

        public override void RemoveChild(BTNode node)
        {
            Undo.RecordObject(this, "Behavior Tree (Remove Child");

            Child = null;

            EditorUtility.SetDirty(this);
        }

        public override List<BTNode> GetChildren()
        {
            return new List<BTNode>{ Child};
        }

        public override BTNodeBase Clone()
        {
            BTDecorator node = Instantiate(this);
            node.Child = Child.Clone() as BTNode;

            return node;
        }

        protected virtual bool PerformConditionCheck(NavMeshAgent agent, GameObject agentGameObject) { return true; }

        protected override EResult OnExecute()
        {
            bool condition = PerformConditionCheck(m_Agent, m_Agent.gameObject);
            if (Child != null && condition)
            {
                return Child.Execute();
            }
            else if(!condition)
            {
                return EResult.Failure;
            }

            return EResult.Success;
        }

    }
}