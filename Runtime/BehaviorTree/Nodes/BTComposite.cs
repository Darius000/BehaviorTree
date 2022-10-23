using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;


namespace AIBehaviorTree
{
    [DisplayName("Composite")]
    public abstract class BTComposite : BTNode
    {
        [Output(Capacity = Capacity.Multi, Type = typeof(BTNode))]
        [HideInInspector] public List<BTNode> m_Children = new List<BTNode>();

        public override bool AddChild(BTNode node)
        {
            Undo.RecordObject(this, "Behavior Tree (Add Child");

            m_Children.Add(node);

            EditorUtility.SetDirty(this);

            return true;
        }

        public override void RemoveChild(BTNode node)
        {
            Undo.RecordObject(this, "Behavior Tree (Remove Child");

            m_Children.Remove(node);

            EditorUtility.SetDirty(this);
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