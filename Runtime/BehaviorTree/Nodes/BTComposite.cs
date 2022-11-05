﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;


namespace AIBehaviorTree
{
    [DisplayName("Composite")]
    [Category("Composites")]
    public abstract class BTComposite : BTNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(BTNode))]
        [HideInInspector] public List<BTNode> m_Children = new List<BTNode>();

        protected override void OnAddChild(BTNode node)
        {
            base.OnAddChild(node);

            m_Children.Add(node);
        }

        protected override void OnRemoveChild(BTNode node)
        {
            base.OnRemoveChild(node);

            m_Children.Remove(node);
        }

        public override IEnumerable<BTNode> GetChildren()
        {
            return m_Children;
        }

        

        public override BTNode Clone()
        {
            BTComposite node = Instantiate(this);
            node.m_Children = m_Children.ConvertAll(c => c.Clone() as BTNode);
            return node;
        }
    }
}