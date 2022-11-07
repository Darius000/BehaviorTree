using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditor;
using Unity.Plastic.Antlr3.Runtime.Tree;
using UnityEngine.Diagnostics;

namespace AIBehaviorTree
{
    public enum EResult { Running, Failure, Success, Abort };

    [DisplayName("Node")]
    [Input]
    public abstract class BTNode : ScriptableObject
    {
        [HideInInspector] public string m_GUID;

        [HideInInspector] public Vector2 m_Position;

        [HideInInspector] public Action<BTNode> OnDeletedEvent;

        /*[HideInInspector]*/ public string m_Description = "";

        //text displayed on node title
        /*[HideInInspector]*/ public string m_DisplayName = "";

        private EResult m_State = EResult.Running;

        public EResult State { get { return m_State; } }

        [HideInInspector] public bool m_BeganExecution = false;

        public Action<EResult> OnCompletedEvent;

        public Action<bool> OnBreakPointSet;

        public BehaviorTree Tree { get; set; }

#if UNITY_EDITOR
        [HideInInspector]
        public bool m_BreakPoint = false;
#endif
       

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

        protected virtual void OnDelete() { }

        public void Initialize(Vector2 position, string id, Action<BTNode> OnDelete = null)
        {
            m_Position = position;
            m_GUID = id;
            OnDeletedEvent = OnDelete;
            m_Description = m_DisplayName = GetDisplayName(GetType());
        }

        /// <summary>
        /// Get display name attribute from given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetDisplayName(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length > 0)
            {
                var nameAttribute = attributes[0] as DisplayNameAttribute;
                return nameAttribute.DisplayName;
            }

            return type.Name;
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
            if(!ContainsChild(node))
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

        public virtual IDictionary<int, IEnumerable<BTNode>> GetChildren() { return new Dictionary<int, IEnumerable<BTNode>>(); }

        internal void InitializeRuntimeNode(BehaviorTree tree)
        {
            Tree = tree;
        }

        public bool ContainsChild(BTNode b)
        {
            foreach(var pair in GetChildren())
            {
                if (pair.Value.Contains(b)) return true;
            }

            return false;
        }

        public virtual int GetChildIndex(BTNode b)
        {
            if (ContainsChild(b))
                return 0;

            return -1;
        }
    }
}