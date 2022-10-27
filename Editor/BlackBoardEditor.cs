using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AIBehaviorTree
{
	public class BlackBoardEditor : EditorWindow
	{
		private BlackBoard m_EditedAsset = null;

		private BlackBoardView m_BlackBoardView;

		private static string m_LastCreationPath = "Assets";

		[MenuItem("Window/BlackBoard")]
		public static void OpenWindow()
		{
			var path = EditorUtility.SaveFilePanelInProject("Create new BlackBoard", "BlackBoard", "asset", "", m_LastCreationPath);
			if (!string.IsNullOrEmpty(path))
			{
				m_LastCreationPath = Path.GetDirectoryName(path);

				var bb = ScriptableObject.CreateInstance<BlackBoard>();

				AssetDatabase.CreateAsset(bb, path);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				var window = CreateWindow<BlackBoardEditor>(bb.name);
				window.Initialize(bb);
			}
		}

		public static void OpenBlackBoardAsset(BlackBoard asset)
		{
			BlackBoardEditor wnd = GetWindow<BlackBoardEditor>();
			wnd.minSize = new Vector2(800, 600);
			wnd.Initialize(asset);
		}

		public void Initialize(BlackBoard blackBoard)
		{
			titleContent = new GUIContent(blackBoard.name);
			m_EditedAsset = blackBoard;
			m_BlackBoardView.Initialize(m_EditedAsset);
		}

		public void CreateGUI()
		{
			// Each editor window contains a root VisualElement object
			VisualElement root = rootVisualElement;
			// Import UXML
			//var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Extras/BehaviorTree/UI/BlackBoard/BlackBoardEditor.uxml");
			//visualTree.CloneTree(root);

			// A stylesheet can be added to a VisualElement.
			// The style will be applied to the VisualElement and all of its children.

			m_BlackBoardView = new BlackBoardView();
			m_BlackBoardView.style.flexGrow = 1;
			root.Add(m_BlackBoardView);
		}
	}
}