using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;

namespace AIBehaviorTree
{
    public class BlackBoardView : VisualElement
    {

        protected ListView KeyListView { get; private set; }

        protected IMGUIContainer KeyInspector { get; private set; }

        protected BlackBoard BlackBoard { get; private set; }

        private SerializedObject m_BlackBoardObject = null;


        public BlackBoardView()
        {

        }

        ~BlackBoardView()
        {
            if (BlackBoard)
            {
                BlackBoard.OnKeyRemovedEvent -= OnKeyRemovedFromBlackBoard;
                BlackBoard.OnKeyAddedEvent -= OnKeyAddedToBlackBoard;
                BlackBoard.OnKeysChangedEvent -= OnKeysChangedInBlackBoard;
                BlackBoard.OnBlackBoardKeyUpdated -= OnKeyUpdated;
            }
        }

        public void Initialize(BlackBoard blackBoard)
        {
            BlackBoard = blackBoard;

            Clear();

            if (BlackBoard == null)
            {

                return;
            }

            m_BlackBoardObject = new SerializedObject(BlackBoard);

            CreateView();

            PopulateKeyList();

            BlackBoard.OnKeyRemovedEvent += OnKeyRemovedFromBlackBoard;
            BlackBoard.OnKeyAddedEvent += OnKeyAddedToBlackBoard;
            BlackBoard.OnKeysChangedEvent += OnKeysChangedInBlackBoard;
            BlackBoard.OnBlackBoardKeyUpdated += OnKeyUpdated;
        }

        private void OnKeyUpdated(int index)
        {
            KeyListView.RefreshItem(index);
        }

        private void OnKeysChangedInBlackBoard(List<BlackBoardKey> obj)
        {
            KeyListView.RefreshItems();
        }

        private void OnKeyAddedToBlackBoard(BlackBoardKey key)
        {
            KeyListView.itemsSource.Add(key);
            KeyListView.selectedIndex = KeyListView.itemsSource.Count - 1;
            KeyListView.RefreshItems();
        }

        private void OnKeyRemovedFromBlackBoard(int index)
        {
            if(index == KeyListView.selectedIndex)
            {
                KeyInspector.onGUIHandler = null;
            }

            KeyListView.selectedIndex = -1;
            KeyListView.itemsSource.RemoveAt(index);

            KeyListView.RefreshItems();
        }

        #region View
        private void CreateView()
        {
            var CreateKeyButton = new Button { text = "Add Key", name = "Add-Key", style = { alignContent = Align.Stretch} };
            CreateKeyButton.clicked += DisplayDropDown;

            KeyListView = new ListView { showFoldoutHeader = true, showBoundCollectionSize = false, headerTitle = "Keys", name = "KeyList" };

            KeyInspector = new IMGUIContainer { name = "KeyInspector" };


            var splitView = new SplitView();
            splitView.orientation = TwoPaneSplitViewOrientation.Horizontal;
            splitView.fixedPaneInitialDimension = 300.0f;


            var toolBar = CreateToolBar("ToolBar", new VisualElement[] { CreateKeyButton });

            var leftPanel = CreatePanel("Left-Panel", new VisualElement[] { CreateLabel("BlackBoard Keys", 12.0f), toolBar, KeyListView });

            var rightPanel = CreatePanel("Right-Panel", new VisualElement[] { CreateLabel("BlackBoard Key Details", 12.0f), KeyInspector });

            splitView.Add(leftPanel);
            splitView.Add(rightPanel);

            this.Add(splitView);

        }

        private Label CreateLabel(string text, float size)
        {
            var label = new Label { text = text };
            label.style.fontSize = size;

            return label;
        }

        private VisualElement CreatePanel(string name, VisualElement[] children)
        {
            var panel = new VisualElement();
            panel.name = name;
            panel.style.flexGrow = 1;
            panel.style.paddingBottom = 5f;
            panel.style.paddingLeft = 5f;
            panel.style.paddingRight = 5f;
            panel.style.paddingTop = 5f;

            AddChildrenToElement(panel, children);

            return panel;
        }

        private VisualElement CreateToolBar(string name, VisualElement[] children)
        {
            var toolbar = new VisualElement()
            {
                name = name,
                style =
                {
                    flexDirection = FlexDirection.Row,
                    borderBottomWidth = 1,
                    borderTopWidth = 1,
                    borderTopColor = new StyleColor(new Color32(110, 110, 110, 255)),
                    borderBottomColor = new StyleColor(new Color32(110, 110, 110, 255)),
                }
            };

            AddChildrenToElement(toolbar, children);

            return toolbar;
        }

        private void AddChildrenToElement(VisualElement parent, VisualElement[] children)
        {
            foreach (var child in children)
            {
                parent.Add(child);
            }

        }

        #endregion

        internal void PopulateKeyList()
        {
            KeyListView.Clear();

            var items = new List<BlackBoardKey>();
            for (int i = 0; i < BlackBoard.GetKeyCount(); i++)
            {
                items.Add(BlackBoard.GetKey(i));
            }

            KeyListView.itemsSource = items;

            KeyListView.makeItem = MakeItem;
            KeyListView.bindItem = BindItem;
            KeyListView.selectionType = SelectionType.Single;
            KeyListView.onSelectedIndicesChange += OnKeySelected;
            KeyListView.RegisterCallback<KeyDownEvent>(OnKeyDown);
            KeyListView.itemsRemoved += DeleteKey;
            KeyListView.itemIndexChanged += KeyIndexChanged;
        }

        internal void DisplayDropDown()
        {
            var keytypes = TypeCache.GetTypesDerivedFrom<BlackBoardKey>();

            GenericMenu menu = new GenericMenu();

            foreach (var keytype in keytypes)
            {
                var displayname = BlackBoardKey.GetDisplayNameAttribute(keytype);
                menu.AddItem(new GUIContent(displayname), false, () => AddNewKey(keytype));
            }

            menu.ShowAsContext();
        }

        internal void AddNewKey(System.Type type)
        {
            var keyexists = BlackBoard.ContainsKey(BlackBoardKey.GetDefaultName());
            if (!keyexists)
            {
                var key = ScriptableObject.CreateInstance(type) as BlackBoardKey;
              
                BlackBoard.AddNewKey(key);


                return;
            }

            Debug.LogWarning("Key already Exists! Please Rename Key.");
        }

        internal void DeleteKey(object data)
        {
            int index = (int)data;

            BlackBoard.RemoveKey(index);

        }

        private VisualElement MakeItem()
        {
            var element = new BlackBoardKeyView();

            //var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Extras/BehaviorTree/UI/BlackBoard/BlackBoardKeyItem.uxml");
            //visualTree.CloneTree(element);
            return element;
        }

        private void BindItem(VisualElement element, int index)
        {
            var vs = (element as BlackBoardKeyView);
            if (vs != null)
            {
                var key = BlackBoard.GetKey(index);

                vs.Update(key);
            }
        }

        internal void KeyIndexChanged(int old, int newIndex)
        {
            BlackBoard.SwitchKeys(old, newIndex);


        }

        internal void OnKeyDown(KeyDownEvent e)
        {
            if (e.keyCode == KeyCode.Delete)
            {
                var selectedIndex = KeyListView.selectedIndex;
                if (selectedIndex != -1)
                {
                    DeleteKey(selectedIndex);
                }
            }
        }

        internal void OnKeySelected(object data)
        {
            if (data == null) return;

            IEnumerable<int> indices = data as IEnumerable<int>;

            int index = indices.Count() == 0 ? -1 : indices.First();

            if (index == -1 || index < 0 || index > BlackBoard.GetKeyCount())
            {
                KeyInspector.onGUIHandler = null;

                return;
            }

            KeyInspector.onGUIHandler = () =>
            {
                BlackBoard.UpdateBlackBoardObject(m_BlackBoardObject, index);
            };
        }
    }
}