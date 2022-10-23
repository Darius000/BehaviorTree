using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Events;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using UnityEngine.AI;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace AIBehaviorTree
{
    [CreateAssetMenu(menuName = "BehaviorTree/BehaviourTree")]
    public class BehaviorTree : ScriptableObject
    {
        //Root node
        [SerializeField]
        [HideInInspector] private BTNode m_Root;

        [SerializeField]
        [HideInInspector] private List<BTNode> m_Nodes = new List<BTNode>();

        [SerializeField]
        [HideInInspector] private List<BTGroup> m_Groups = new List<BTGroup>();

        [HideInInspector] private EResult m_TreeState = EResult.Running;

        [HideInInspector] public BTNode Root { get { return m_Root; } set { m_Root = value; } }

        [HideInInspector] public List<BTNode> Nodes { get { return m_Nodes; } }

        [HideInInspector] public List<BTGroup> Groups {  get { return m_Groups; } }


        [HideInInspector] public BlackBoard m_BlackBoard;



        // Update is called once per frame
        public EResult Update()
        {
            if(m_Root.State == EResult.Running)
            {
                m_TreeState = m_Root.Execute();
            }

            return m_TreeState;
        }

        public BTNode PasteNode(BTNode pastedNode, Vector2 pos = new Vector2())
        {
            var newNode = Instantiate(pastedNode);
            CreateNode(newNode, pos);
            return newNode;
        }

        public BTNode CreateNode(BTNode node, Vector2 pos = new Vector2())
        {
            node.m_Position = pos;
            node.m_GUID = GUID.Generate().ToString();
            node.OnDeletedEvent = DeleteNode;

            Undo.RecordObject(this, "Behavior Tree (Create Node)");

            if (node is BTNode)
            {
                BTNode bt_node = (BTNode)node;
                bt_node.m_Tree = this;
                m_Nodes.Add(bt_node);
            }
            else if (node is BTGroup)
            {
                BTGroup group = (BTGroup)node;
                m_Groups.Add(group);
            }

            Undo.RegisterCreatedObjectUndo(node, "Behavior Tree (Create Node)");

            AssetDatabase.AddObjectToAsset(node, this);

            return node;
        }

        public BTNode CreateNode(System.Type type, Vector2 pos = new Vector2())
        {
            BTNode node = ScriptableObject.CreateInstance(type) as BTNode;
            node.name = type.Name;
            return CreateNode(node, pos);
        }

        public void DeleteNode(BTNode node)
        {
 
            Undo.RecordObject(this, "Behavior Tree (Delete Node)");

            if(node is BTNode)
            {
                m_Nodes.Remove(node as BTNode);
            }
            else if(node is BTGroup)
            {
                m_Groups.Remove(node as BTGroup);
            }

            AssetDatabase.RemoveObjectFromAsset(node);

        }

        public bool AddChild(BTNode parent, BTNode child)
        {
            
            var canAdd = parent.AddChild(child);
            
            return canAdd;
        }

        public void RemoveChild(BTNode parent, BTNode child)
        {
            parent.RemoveChild(child);
 
        }

        public List<BTNode> GetChildren(BTNode parent)
        {
            return parent.GetChildren().ToList();
        }

        public void Traverse(BTNode node, System.Action<BTNode> visitor)
        {
            if (node)
            {
                visitor.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => Traverse(n, visitor));
            }
        }

        public BehaviorTree Clone()
        {
            BehaviorTree tree = Instantiate(this);
            tree.m_Root = tree.m_Root.Clone() as BTNode;
            tree.m_Nodes = new List<BTNode>();
            tree.m_BlackBoard = tree.m_BlackBoard.Clone();
            Traverse(tree.m_Root, (n) => tree.m_Nodes.Add(n));

            return tree;
        }

       

        public void Bind(NavMeshAgent agent)
        {
            Traverse(m_Root, node =>
            {
                node.InitializeRuntimeNode(agent, this);
            });
        }

        public void Save()
        {
            AssetDatabase.SaveAssetIfDirty(this);

            foreach(var node in m_Nodes)
            {
                AssetDatabase.SaveAssetIfDirty(node);
            }
        }

        internal void DrawDebugInfo(BehaviorTreeComponent behaviourTreeComponent)
        {
            if (!Application.isPlaying) return;
            
            //BlackBoard Data
            var gameobject = behaviourTreeComponent.gameObject;
            GizmoUtils.BeginStringGUI(gameobject.transform.position);

            m_BlackBoard.DrawDebugInfo();
            GizmoUtils.EndStringGUI();

            //AI Data
            var pos = new Vector2(0f, Screen.height - 100f);
            GizmoUtils.BeginStringGUI(pos, true);
            GizmoUtils.DrawString("[Category : AI]", Color.green);

            GizmoUtils.DrawString("GameObject Name:", Color.white);
            GizmoUtils.SameLine();
            GizmoUtils.DrawString(gameobject.name, Color.yellow);

            
            GizmoUtils.DrawString("NavData :"  + behaviourTreeComponent.GetAgent()?.navMeshOwner +
                ",Path Status : " + behaviourTreeComponent.GetAgent()?.pathStatus, new Color[] { Color.white, Color.yellow });

            GizmoUtils.DrawString("Behavior : " + m_TreeState, new Color[] { Color.white, Color.yellow });


            //Behaviour Tree Data
            GizmoUtils.DrawString("[Category : Behavior Tree]", Color.green);
            GizmoUtils.DrawString("Behavior Tree : " + name.Replace("(Clone)", ""), new Color[] { Color.white, Color.yellow });
            
           
            GizmoUtils.EndStringGUI();
        }
    }
}