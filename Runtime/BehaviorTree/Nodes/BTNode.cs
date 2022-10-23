using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditor;

namespace AIBehaviorTree
{
    public enum EResult { Running, Failure, Success, Abort };

    [DisplayName("Node")]
    [Input(Type = typeof(BTNode))]
    public abstract class BTNode : ScriptableObject
    {
        [HideInInspector] public string m_GUID;

        [HideInInspector] public Vector2 m_Position;

        [HideInInspector] public Action<BTNode> OnDeletedEvent;

        [HideInInspector] public string m_Description;

        //text displayed on node title
        [HideInInspector] public string m_DisplayName;

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

        public void Delete()
        {
            OnDelete();

            if (OnDeletedEvent != null)
            {
                OnDeletedEvent.Invoke(this);
            }
        }

        protected virtual void OnDelete()
        {

        }

        protected void OnEnable()
        {

            m_Description = m_DisplayName = GetDisplayName();
        }

        public void SetPosition(Vector2 pos)
        {
            Undo.RecordObject(this, "Behavior Tree (Set Positon)");

            m_Position = pos;

            EditorUtility.SetDirty(this);


        }

        public virtual BTNode Clone()
        {
            var node = Instantiate(this);

            return node;
        }

        //returns the display name if no displayname attribute default name is returned
        public string GetDisplayName()
        {
            var attributes = GetType().GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length > 0)
            {
                var nameAttribute = attributes[0] as DisplayNameAttribute;
                return nameAttribute.DisplayName;
            }

            return name;
        }



        public virtual bool AddChild(BTNode node) { return true; }

        public virtual void RemoveChild(BTNode node) { }

        public virtual IEnumerable<BTNode> GetChildren() { return new List<BTNode>(); }


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