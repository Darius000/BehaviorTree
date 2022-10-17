using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AIBehaviorTree
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class BlackBoardKeyColorAttribute : Attribute
    {
        public BlackBoardKeyColorAttribute() { }

        public BlackBoardKeyColorAttribute(Color color)
        {
            m_Color = color;
        }

        public BlackBoardKeyColorAttribute(float r, float g, float b)
        {
            m_Color = new Color(r, g, b);
        }

        private Color m_Color = Color.white;

        public Color Tint { get { return m_Color; } set { m_Color = value; } }
    }
}