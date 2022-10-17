using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace AIBehaviorTree
{
    public class BTNodeBase : ScriptableObject
    {
        [HideInInspector] public string m_GUID;

        [HideInInspector] public Vector2 m_Position;

        [HideInInspector] public Action<BTNodeBase> OnDeletedEvent;

        

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

        public void SetPosition(Vector2 pos)
        {
            Undo.RecordObject(this, "Behavior Tree (Set Positon)");

            m_Position = pos;

            EditorUtility.SetDirty(this);

            
        }

        public virtual BTNodeBase Clone()
        {
            var node = Instantiate(this);

            return node;
        }
    }
}
