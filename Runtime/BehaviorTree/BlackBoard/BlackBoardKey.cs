using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    [System.Serializable]
    public abstract class BlackBoardKey
    {
        public static string GetDefaultName() { return "New Key"; }

        public string m_KeyName = "";

        public string ID { get; set; }

        public string m_Description = "";

        public bool m_IsInstance = false;

        public object _object = null;

        private static BlackBoardKey s_Instance = null;

        public BlackBoardKey()
        {

        }

        public BlackBoardKey(BlackBoardKey key)
        {
            m_KeyName = key.m_KeyName;
            ID = key.ID;
            m_Description = key.m_Description;
            m_IsInstance = key.m_IsInstance;
            _object = key._object;
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

        //public static implicit operator bool(BlackBoardKey instance)
        //{
        //    return instance != null;
        //}


        public static bool operator !(BlackBoardKey key)
        {
            return key != null;
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
    }
}
