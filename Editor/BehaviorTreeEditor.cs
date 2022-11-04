using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;
using UnityEditor.Events;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace AIBehaviorTree
{

	[System.Serializable]
	public class BehaviorTreeEditor : EditorWindow
	{
		[SerializeField]
		private BehaviorTreeView m_TreeView;

		[SerializeField]
		private InspectorView m_InspectorView;

		[SerializeField]
		private BlackBoardView m_BlackBoardView;

		[SerializeField]
		protected SerializedObject m_TreeObject;

		[SerializeField]
		private SerializedProperty m_BlackBoardProperty;

		[SerializeField]
		private IMGUIContainer m_BlackBoardField;

		[SerializeField]
		private ListView m_BlackBoardKeyList;

		private PropertyField m_BlackBoardPropertyField;

        [SerializeField]
        private BehaviorTreeToolbar m_Toolbar;


		[SerializeField]
		private BehaviorTree m_CurrentSelectedTree = null;

		private static string m_LastCreationPath = "Assets";

		[MenuItem("Window/BehaviorTree")]
		public static void OpenWindow()
		{
			var path = EditorUtility.SaveFilePanelInProject("Save Behavior Tree", "BehaviorTree", "asset", "", m_LastCreationPath);


			if (!string.IsNullOrEmpty(path))
			{
				m_LastCreationPath = Path.GetDirectoryName(path);

				var tree = ScriptableObject.CreateInstance<BehaviorTree>();

				AssetDatabase.CreateAsset(tree, path);
				//AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				var window = CreateWindow<BehaviorTreeEditor>(tree.name);
				window.Initialize(tree);
				window.minSize = new Vector2(600, 800);
				window.m_CurrentSelectedTree = tree;
			}
		}

		public static void OpenWindowAsset(BehaviorTree tree)
		{
			BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>(tree.name);

			if (wnd != null)
			{
				wnd.Initialize(tree);
				wnd.m_CurrentSelectedTree = tree;
			}
		}


		public void Initialize(BehaviorTree tree)
		{

			if (tree == null) return;

			SetBlackBoard(tree.m_BlackBoard);

			titleContent = new GUIContent("BehaviorTreeGraph (" + tree.name + ")");
			m_TreeObject = new SerializedObject(tree);
			m_BlackBoardProperty = m_TreeObject.FindProperty("m_BlackBoard");

			m_BlackBoardPropertyField.BindProperty(m_BlackBoardProperty);

			m_BlackBoardPropertyField.RegisterValueChangeCallback((property) =>
			{
				var blackboard = property.changedProperty.objectReferenceValue as BlackBoard;


                m_TreeView.m_Tree.Root.m_Description = blackboard ? blackboard.name : "";


                SetBlackBoard(blackboard);
			});

			if (m_TreeView != null)
			{
				m_TreeView.PopulateView(tree);
			}
		}

		private void OnAISelected(AIController obj)
		{
			var tree = obj.Tree;

			Selection.activeGameObject = obj.gameObject;

			Initialize(tree);
		}

		private void OnEnable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}


		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		public void CreateGUI()
		{
			m_BlackBoardView = new BlackBoardView();

			//Add toolbar
			m_Toolbar = new BehaviorTreeToolbar(GetSelectedTree);
			m_Toolbar.SaveButton.clicked += Save;
			m_Toolbar.MiniMapButton.clicked += ToggleMiniMap;
			m_Toolbar.AIListMenu.OnAISelectedEvent += OnAISelected;

            rootVisualElement.Add(m_Toolbar);

            AddTabs();

			if (m_CurrentSelectedTree != null)
			{
				Initialize(m_CurrentSelectedTree);

			}
		}

        #region Toolbar

		private BehaviorTree GetSelectedTree()
		{
			return m_CurrentSelectedTree;
		}

		private void Save()
        {
			m_CurrentSelectedTree.Save();
        }

        private void ToggleMiniMap()
        {
			m_TreeView.ToggleMiniMap();
        }

        #endregion

        #region Tabs

		private void AddTabs()
        {
			var treeContainer = CreateBlackBoardGUISection();

			var inspector = CreateInspectorView("Bottom-Panel");

			var treeElement = CreateTreeView("Right-Panel");


			var horizontalSplitView = new SplitView { fixedPaneInitialDimension = 1000.0f, orientation = TwoPaneSplitViewOrientation.Horizontal };

			var verticalplitView = new SplitView { fixedPaneInitialDimension = 300f, orientation = TwoPaneSplitViewOrientation.Vertical };

			verticalplitView.Add(inspector);
			verticalplitView.Add(treeContainer);


			horizontalSplitView.Add(treeElement);
			horizontalSplitView.Add(verticalplitView);

			var behaviourTreeTab = new Tab("BehaviourTree", horizontalSplitView);
			var blackboardTab = new Tab("BlackBoard", m_BlackBoardView);
			var tabView = new TabView() { name = "Tree Tabs", style = { flexGrow = 1 } };
			tabView.Add(behaviourTreeTab);
			tabView.Add(blackboardTab);
			tabView.Activate(behaviourTreeTab);

			rootVisualElement.Add(tabView);
		}

        #endregion

        public static void AddChildrenToElement(VisualElement parent, VisualElement[] toolBarElements = null)
		{
			if (toolBarElements != null)
			{
				foreach (VisualElement element in toolBarElements)
				{
					parent.Add(element);
				}
			}
		}




		#region GUI
		


        #region MiniMap
        

        #endregion

        private VisualElement CreateBlackBoardGUISection()
		{
			float flexGrow = 1;

			var treeContainer = new VisualElement { name = "tree-container" };
			treeContainer.style.flexDirection = FlexDirection.Column;
			treeContainer.style.flexGrow = flexGrow;
			treeContainer.style.paddingBottom = 5f;
			treeContainer.style.paddingLeft = 5f;
			treeContainer.style.paddingRight = 5f;
			treeContainer.style.paddingTop = 5f;

			var label = new Label() {  text = "Tree Inspector", style = { backgroundColor = new StyleColor(new Color32(78, 78, 78, 255)) } };

			m_BlackBoardPropertyField = new PropertyField { label = "BlackBoard", name = "BlackBoard" };
			m_BlackBoardPropertyField.style.paddingTop = 5f;


			m_BlackBoardKeyList = new ListView
			{
				name = "KeysView",
				headerTitle = "Keys",
				showBorder = true,
				showFoldoutHeader = true,
				showBoundCollectionSize = false,
			};

			m_BlackBoardKeyList.style.paddingTop = 10f;
			m_BlackBoardKeyList.style.fontSize = 12f;
			m_BlackBoardKeyList.style.flexGrow = flexGrow;

			m_BlackBoardKeyList.makeItem = MakeListItem;
			m_BlackBoardKeyList.bindItem = BindItem;

			treeContainer.Add(label);
			treeContainer.Add(m_BlackBoardPropertyField);
			treeContainer.Add(m_BlackBoardKeyList);

			rootVisualElement.Add(treeContainer);

			return treeContainer;
		}

		private VisualElement CreateInspectorView(string panelName)
		{
			var root = new ScrollView { name = panelName };
			root.style.flexGrow = 1;
			root.style.flexDirection = FlexDirection.Column;
			root.style.borderRightWidth = 5;
			root.style.borderLeftWidth = 5;

			var label = new Label() { text = "Inspector", style = { backgroundColor = new StyleColor(new Color32(78, 78, 78, 255)) } };
			m_InspectorView = new InspectorView { name = "Inspector" };
			m_InspectorView.style.paddingTop = 10f;

			root.Add(label);
			root.Add(m_InspectorView);

			rootVisualElement.Add(root);

			return root;
		}

		private VisualElement CreateTreeView(string panelName)
		{
			var root = new VisualElement { name = panelName };
			root.style.flexGrow = 1;
			root.style.flexDirection = FlexDirection.Column;


			m_TreeView = new BehaviorTreeView(this) { name = "BehaviourTreeView" };
			m_TreeView.style.flexGrow = 1;

			m_TreeView.m_OnNodeSelected = OnNodeSelectionChanged;
			


			root.Add(m_TreeView);

			rootVisualElement.Add(root);

			return root;
		}

		#endregion

		#region ListView
		internal VisualElement MakeListItem()
		{
			var keyView = new BlackBoardKeyView();

			return keyView;
		}

		private void BindItem(VisualElement element, int index)
		{
			var vs = (element as BlackBoardKeyView);
			if (vs != null)
			{
				var key = m_BlackBoardKeyList.itemsSource[index] as BlackBoardKey;
				vs.Update(key);
			}
		}

		#endregion

		private void SetBlackBoard(BlackBoard blackBoard)
		{
			if (blackBoard != null)
			{
				blackBoard.OnKeyAddedEvent = (key) => { UpdateBlackBoardKeys(blackBoard); };
				blackBoard.OnKeyRemovedEvent = (key) => { UpdateBlackBoardKeys(blackBoard); };
			}

			UpdateBlackBoardKeys(blackBoard);

            m_BlackBoardView.Initialize(blackBoard);
		}

		internal void UpdateBlackBoardKeys(BlackBoard blackboard)
		{
			var names = new List<BlackBoardKey>();

			if (blackboard && blackboard.GetKeyCount() > 0)
			{
				foreach (var key in blackboard.GetAllKeys())
				{
					names.Add(key);

				}

				m_BlackBoardKeyList.itemsSource = names;
				m_BlackBoardKeyList.RefreshItems();
			}
			else
			{
				if (m_BlackBoardKeyList.itemsSource != null)
					m_BlackBoardKeyList.itemsSource.Clear();
				m_BlackBoardKeyList.RefreshItems();
			}
		}


		private void OnPlayModeStateChanged(PlayModeStateChange obj)
		{

			switch (obj)
			{
				case PlayModeStateChange.EnteredEditMode:
					Initialize(m_CurrentSelectedTree);
					break;
				default:
					break;
			}
		}

		void OnNodeSelectionChanged(NodeView view)
		{
			view.m_Node.OnDeletedEvent += (node) => { m_InspectorView.UpdateSelection(null); };
			m_InspectorView.UpdateSelection(view);
		}
	}
}
