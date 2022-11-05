using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditor;
using Unity.Plastic.Antlr3.Runtime.Tree;

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

        public Action<EResult> OnCompletedEvent;

        public Action OnFinishedEvent;

        public Action<bool> OnBreakPointSet;

        public BehaviorTree Tree { get; set; }

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

        public EResult Execute(NavMeshAgent agent, AIController controller)
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
                return EResult.Abort;
            }

            if (!m_BeganExecution)
            {
                OnBeginExecute(agent , controller);
                m_BeganExecution = true;
            }

            SetState(OnExecute(agent, controller));

            if (m_State == EResult.Success || m_State == EResult.Failure)
            {
                OnEndExecute(agent, controller);
                m_BeganExecution = false;
            }

            OnFinishedEvent?.Invoke();

            return m_State;
        }

        protected virtual EResult OnExecute(NavMeshAgent agent, AIController controller)
        {
            return EResult.Failure;
        }

        protected virtual void OnBeginExecute(NavMeshAgent agent, AIController controller)
        {
            return;
        }

        protected virtual void OnEndExecute(NavMeshAgent agent, AIController controller)
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

            m_Description = m_DisplayName = GetType().Name;
        }

        public void SetPosition(Vector2 pos)
        {
            Undo.RecordObject(this, "Behavior Tree (Set Positon)");

            m_Position = pos;

            EditorUtility.SetDirty(this);


        }

        public virtual BTNode Clone()
        {
            return Instantiate(this);
        }

        

        public bool AddChild(BTNode node)
        {
            var children = GetChildren();
            if(!children.Contains(node))
            {
                Undo.RecordObject(this, "Behavior Tree (Add Child");

                OnAddChild(node);

                EditorUtility.SetDirty(this);

                return true;
            }

            return false;
        }

        public void RemoveChild(BTNode node)
        {
            Undo.RecordObject(this, "Behavior Tree (Remove Child");

            OnRemoveChild(node);

            EditorUtility.SetDirty(this);
        }

        protected virtual void  OnAddChild(BTNode node) { }

        protected virtual void OnRemoveChild(BTNode node) { }

        public virtual IEnumerable<BTNode> GetChildren() { return new List<BTNode>(); }

        internal void InitializeRuntimeNode(BehaviorTree tree)
        {
            Tree = tree;
        }

        public bool ContainsChild(BTNode b)
        {
            return GetChildren().Contains(b);
        }

        public virtual int GetChildIndex(BTNode b)
        {
            if (ContainsChild(b))
                return 0;

            return -1;
        }
    }
}