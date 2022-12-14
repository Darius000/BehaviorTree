using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

namespace AIBehaviorTree
{
    [Serializable]
    public class CopyPasteCache
    {

        [SerializeField]
        public List<BTNode> m_CopiedNodes = new List<BTNode>();

        public void Add(BTNode node)
        {
            if(!m_CopiedNodes.Contains(node))
            {
                m_CopiedNodes.Add(node);
            }
        }

        public void Clear()
        {
            m_CopiedNodes.Clear();
        }
    }

    //Behaviour Tree Graph View
    public class BehaviorTreeView : GraphView
    {
        public Action<NodeView> m_OnNodeSelected;


        private MiniMap m_MiniMap;

        private BehaviorTreeSearchWindow m_SearchWindow;
        private BehaviorTreeEditor m_EditorWindow;


        //current tree being edited
        public BehaviorTree m_Tree;

        //stores contextual menu mouse position
        private Vector2 m_MousePosition;

        private static CopyPasteCache Cache = new CopyPasteCache();

        public BehaviorTreeView(BehaviorTreeEditor editorWindow)
        {
            m_EditorWindow = editorWindow;


            this.AddManipulator(new ContentZoomer() { maxScale = 3f });
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var stylesheet = Utils.BehaviorTreeUtils.FindAsset<StyleSheet>("Grid");

            styleSheets.Add(stylesheet);

            Undo.undoRedoPerformed += OnUndoRedo;

            AddMiniMap();
            AddMiniMapStyles();
            AddSearchWindow();
            AddGridbackground();

            canPasteSerializedData = AllowPaste;
            unserializeAndPaste += OnPaste;
            serializeGraphElements += CutPasteOperation;
        }

        private string CutPasteOperation(IEnumerable<GraphElement> elements)
        {
            Cache.Clear();
            foreach (var element in elements)
            {
                if (element is NodeView)
                {
                    var node = ((NodeView)element).m_Node;
                    Cache.Add(node);
                }
            }
            string json = JsonUtility.ToJson(Cache);
            return json;
        }

        private void OnPaste(string operationName, string data)
        {
            Cache = JsonUtility.FromJson<CopyPasteCache>(data);
            foreach (var node in Cache.m_CopiedNodes)
            {
                PasteNode(node, m_MousePosition);
            }
        }

        private bool AllowPaste(string data)
        {
            return data != string.Empty;
        }

        void AddGridbackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }


        #region ToolBar


        #region MiniMap
        //add mini map to graph
        private void AddMiniMap()
        {
            m_MiniMap = new MiniMap()
            {
                anchored = true
            };

            m_MiniMap.SetPosition(new Rect(15, 50, 200, 200));

            Add(m_MiniMap);
        }

        //set mini map styles
        private void AddMiniMapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(40, 40, 40, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));

            m_MiniMap.style.backgroundColor = backgroundColor;
            m_MiniMap.style.borderTopColor = borderColor;
            m_MiniMap.style.borderBottomColor = borderColor;
            m_MiniMap.style.borderLeftColor = borderColor;
            m_MiniMap.style.borderRightColor = borderColor;
        }

        //toggle the minimap
        public void ToggleMiniMap()
        {
            m_MiniMap.visible = !m_MiniMap.visible;
        }

        #endregion
        #endregion

        #region elements

        private void AddSearchWindow()
        {
            if (m_SearchWindow == null)
            {
                m_SearchWindow = ScriptableObject.CreateInstance<BehaviorTreeSearchWindow>();
                m_SearchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), m_SearchWindow);

        }

        #endregion

        private void OnUndoRedo()
        {
            PopulateView(m_Tree);
            AssetDatabase.SaveAssets();
        }

        NodeView FindNodeElementView(BTNode node)
        {
            if(node == null) return null;

            return GetNodeByGuid(node.m_GUID) as NodeView;
        }

        internal void PopulateView(BehaviorTree tree)
        {
            m_Tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (m_Tree.Root == null)
            {
                m_Tree.Root = m_Tree.CreateNode(typeof(BTRoot)) as BTRoot;
                EditorUtility.SetDirty(m_Tree);
                AssetDatabase.SaveAssetIfDirty(m_Tree);
            }

            //create node view
            m_Tree.Nodes.ForEach(n => {

                CreateNodeView(n);

                if (n.Tree == null)
                {
                    n.Tree = tree;
                }

                if (n.OnDeletedEvent == null)
                {
                    n.OnDeletedEvent = tree.DeleteNode;
                }
            });

            //create edges
            m_Tree.Nodes.ForEach(n =>
            {
                var children = m_Tree.GetChildren(n);
                //children.ForEach(c =>
                //{
                //    if (c == null) return;

                //    CreateEdgeElement(n, c);
                //});

                foreach(var pair in children)
                {
                    foreach(var child in pair.Value )
                    {
                        CreateEdgeElement(n, child, pair.Key);
                    }
                }
            });

            m_Tree.Groups.ForEach(g =>
            {
                if (g.OnDeletedEvent == null)
                {
                    g.OnDeletedEvent = tree.DeleteNode;
                }

                var groupView = CreateGroupView(g);
                var children = g.m_Children;
                children.ForEach(c =>
                {
                    var view = FindNodeElementView(c);
                    if (view != null)
                    {
                        groupView.AddElement(view);
                    }
                });
            });
        }

        //creates an edge element and adds t to the graph, if b is child of a
        private void CreateEdgeElement(BTNode parent, BTNode child, int portIndex)
        {
            var parentView = FindNodeElementView(parent);
            var childView = FindNodeElementView(child);

            Edge edge = parentView.ConnectEdgeToChildren(childView, portIndex);
            if (edge != null)
            {
                AddElement(edge);
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var node = startPort.node as NodeView;
            var compatiblePorts = new List<Port>(); 
            
            foreach(var port in ports)
            {
                bool NotSameDirection = port.direction != startPort.direction;
                bool NotSamePort = port.node != startPort.node;
                bool IsSameType = port.portType == startPort.portType;
                bool IsDerivedType = port.portType.IsSubclassOf(startPort.portType);
                bool IsArray = startPort.portType.GetInterfaces().Contains(typeof(IEnumerable));

                if(NotSameDirection && NotSamePort && (IsSameType || IsDerivedType || 
                    (IsArray && port.portType.IsSubclassOf(startPort.portType.GenericTypeArguments[0]))))
                {
                    compatiblePorts.Add(port);
                }
            }    
           
            return compatiblePorts;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    var nodeView = elem as NodeView;
                    if (nodeView != null)
                    {
                        var node = nodeView.m_Node;

                        node.Delete();

                    }


                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        var input = edge.input;
                        var output = edge.output ;

                        var parentView = output.node as NodeView;
                        var childView = input.node as NodeView;
                        var parent = parentView.m_Node;
                        var child = childView.m_Node;
                        m_Tree.RemoveChild(parent, child);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    var input = edge.input;
                    var output = edge.output;

                    var parentView = output.node as NodeView;
                    var childView = input.node as NodeView;
                    var parent = parentView.m_Node;
                    var child = childView.m_Node;
                    m_Tree.AddChild(parent, child);

                });
            }


            if (graphViewChange.movedElements != null)
            {
                nodes.ForEach((n) =>
                {
                    var view = n as NodeView;
                    view.SortChildren();
                });
            }


            return graphViewChange;
        }


        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            if (evt.target is GraphView)
            {
                var mouseposition = evt.localMousePosition;
                var types = TypeCache.GetTypesDerivedFrom<BTNode>();
                foreach (var type in types)
                {
                    var displayName = type.Name;

                    if (!type.IsAbstract)
                    {
                        var attributes = type.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                        if (attributes.Length > 0)
                        {
                            var nameAttribute = attributes[0] as DisplayNameAttribute;
                            displayName =  nameAttribute.DisplayName;
                        }

                        evt.menu.AppendAction($"Add {displayName}", (a) => CreateNode(type, GetLocalMousePosition(mouseposition)));
                    }
                }

                m_MousePosition = GetLocalMousePosition(mouseposition);
            }
        }

        internal BTNode PasteNode(BTNode pastedNode, Vector2 position)
        {
            var node = m_Tree.PasteNode(pastedNode, position);

            OnNodeCreated(node);

            return node;
        }

        internal BTNode CreateNode(System.Type type, Vector2 position)
        {
            var node = m_Tree.CreateNode(type, position);
            OnNodeCreated(node);
            
            return node;
        }

        private void OnNodeCreated(BTNode node)
        {
            if (node is BTGroup) CreateGroupView(node as BTGroup);
            else if (node is BTNode) CreateNodeView(node);
        }

        private GroupView CreateGroupView(BTGroup group)
        {
            if (group == null) return null;

            var groupview = new GroupView(group);

            AddElement(groupview);

            return groupview;
        }

        private NodeView CreateNodeView(BTNode node)
        {
            if (node == null) return null;

            var nodeview =  new NodeView(node, m_OnNodeSelected);

            AddElement(nodeview);

            return nodeview;
        }


        #region Utilities

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if(isSearchWindow)
            {
                worldMousePosition -= m_EditorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        #endregion
    }
}