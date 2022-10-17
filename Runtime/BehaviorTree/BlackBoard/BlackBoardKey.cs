using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    public abstract class BlackBoardKey : ScriptableObject
    {
        public static string GetDefaultName() { return "New Key"; }

        public string m_KeyName = "";

        public string m_Description = "";

        public bool m_IsInstance = false;

        public object _object = null;

        private static BlackBoardKey s_Instance = null;

        public BlackBoardKey()
        {

        }

        public abstract Type GetKeyType();

        public virtual T GetValue<T>()
        {
            return (T)_object;
        }

        public virtual void SetValue<T>(T v)
        {
            _object = v;
        }

        public object GetObjectValue()
        {
            return _object;
        }

        public Color GetKeyColor()
        {
            var attributes = GetType().GetCustomAttributes(typeof(BlackBoardKeyColorAttribute), true);
            if (attributes.Length > 0)
            {
                var colorAttribute = attributes[0] as BlackBoardKeyColorAttribute;
                return colorAttribute.Tint;
            }

            return Color.white;
        }

        public static string GetDisplayNameAttribute(System.Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if (attributes.Length > 0)
            {
                var nameAttribute = attributes[0] as DisplayNameAttribute;
                return nameAttribute.DisplayName;
            }

            return "";
        }

        internal BlackBoardKey Clone()
        {
            if (m_IsInstance)
            {
                if (!s_Instance)
                {
                    s_Instance = OnClone();
                }

                return s_Instance;
            }
            else
            {
                return OnClone();
            }
        }

        protected abstract BlackBoardKey OnClone();

        public void DrawDebugInfo()
        {
            GizmoUtils.DrawString(m_KeyName + ": ", Color.white);
            GizmoUtils.SameLine();
            OnDrawObjectDebugInfo();
        }

        protected virtual void OnDrawObjectDebugInfo()
        {
            var text = GetObjectValue() == null ? "(Invalid)" : GetObjectValue().ToString();
            GizmoUtils.DrawString(text, Color.yellow);
        }
    }
}
