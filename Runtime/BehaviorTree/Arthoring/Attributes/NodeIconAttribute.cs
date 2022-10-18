using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NodeIconAttribute : Attribute
    {
        private string iconPath;

        public NodeIconAttribute()
        {

        }

        public NodeIconAttribute(string icon)
        {
            this.iconPath = icon;
        }



        public string IconPath { get { return iconPath; } set { iconPath = value; } }
    }
}
