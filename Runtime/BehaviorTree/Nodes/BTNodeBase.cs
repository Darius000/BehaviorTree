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

        [TextArea]  public string m_Description;

        //text displayed on node title
        [TextArea] public string m_DisplayName;

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

        public virtual BTNodeBase Clone()
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
    }
}
