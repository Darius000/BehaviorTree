using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace AIBehaviorTree
{
    public enum EResult { Running, Failure, Success, Abort };

    [DisplayName("Node")]
    public abstract class BTNode : BTNodeBase
    {

        private EResult m_State = EResult.Running;

        public EResult State { get { return m_State; } }

        [HideInInspector] public bool m_BeganExecution = false;

        [HideInInspector] public BehaviorTree m_Tree;

        [HideInInspector] public NavMeshAgent m_Agent;

        [HideInInspector] public BehaviorTree Tree { get { return m_Tree; } set { m_Tree = value; } }


        public Action<EResult> OnCompletedEvent;

        public Action<bool> OnBreakPointSet;

#if UNITY_EDITOR
        [HideInInspector]
        public bool m_BreakPoint = false;
#endif
        public BTNode()
        {
            
        }

       

        protected void SetState(EResult state)
        {
            m_State = state;

            if(OnCompletedEvent != null)
            {
                OnCompletedEvent.Invoke(m_State);
            }
        }
#if UNITY_EDITOR
        public bool ToggleBreakPoint()
        {
           m_BreakPoint = !m_BreakPoint;

            OnBreakPointSet?.Invoke(m_BreakPoint);

            return m_BreakPoint;
        }
#endif
        protected void Abort()
        {
            m_State = EResult.Abort;
        }

        public EResult Execute()
        {
#if UNITY_EDITOR
            if (m_BreakPoint)
            {
                Debug.Break();
                Debug.DebugBreak();
            }
#endif

            if(m_State == EResult.Abort)
            {
                return Execute();
            }

            if (!m_BeganExecution)
            {
                OnBeginExecute();
                m_BeganExecution = true;
            }

            SetState(OnExecute());

            if (m_State == EResult.Success || m_State == EResult.Failure)
            {
                OnEndExecute();
                m_BeganExecution = false;
            }

            return m_State;
        }

        protected virtual EResult OnExecute()
        {
            return EResult.Failure;
        }

        protected virtual void OnBeginExecute()
        {
            return;
        }

        protected virtual void OnEndExecute()
        {
            return;
        }

        protected override void OnDelete()
        {
            base.OnDelete();
        }


       
        public virtual bool AddChild(BTNode node) { return true; }

        public virtual void RemoveChild(BTNode node) { }

        public virtual List<BTNode> GetChildren() { return new List<BTNode>(); }


        public override BTNodeBase Clone()
        {
            var node = Instantiate(this);

            return node;
        }

        public BlackBoard GetBlackBoard()
        {
            return m_Tree ? m_Tree.m_BlackBoard: null;
        }

        internal void InitializeRuntimeNode(NavMeshAgent agnet, BehaviorTree tree)
        {
            m_Tree = tree;
            m_Agent = agnet;

        }

        public bool ContainsChild(BTNode b)
        {
            return GetChildren().Contains(b);
        }

        public int GetChildIndex(BTNode b)
        {
            if (ContainsChild(b))
                return 0;

            return -1;
        }
    }
}