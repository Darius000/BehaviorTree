using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace AIBehaviorTree
{
    [DisplayName("Group")]
    public class BTGroup : BTNode
    {
        /*[HideInInspector] */public List<BTNode> m_Children;

        public string m_Title = "New Group";

        public void Rename(string name)
        {
            Undo.RecordObject(this, "Behavior Tree (Add Child)");

            m_Title = name;

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);

                //AssetDatabase.SaveAssets();           
            }
        }

        public void AddChildToGroup(BTNode node)
        {
            if(node && !m_Children.Contains(node))
            {
              
                Undo.RecordObject(this, "Behavior Tree (Add Child)");

                m_Children.Add(node);

                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(this);

                   // AssetDatabase.SaveAssets();                   
                }
            }
        }

        public void RemoveChildFromGroup(BTNode node)
        {
            if (node && m_Children.Contains(node))
            {

                Undo.RecordObject(this, "Behavior Tree (Add Child)");

                m_Children.Remove(node);

                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(this);

                    //AssetDatabase.SaveAssets();
                }
            }
        }

        public override BTNode Clone()
        {
            BTGroup node = Instantiate(this);
            node.m_Children = m_Children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}
